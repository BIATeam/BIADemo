// <copyright file="BaseAuthAppService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Application.User
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Common.Helpers;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.User.Entities;
    using BIA.Net.Core.Domain.User.Models;
    using BIA.Net.Core.Domain.User.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Auth App Service.
    /// </summary>
    /// <typeparam name="TUserFromDirectoryDto">The type of user from directory dto.</typeparam>
    /// <typeparam name="TUserFromDirectory">The type of user from directory.</typeparam>
    /// <typeparam name="TAdditionalInfoDto">The type of additional info dto.</typeparam>
    /// <typeparam name="TUserDataDto">The type of user data dto.</typeparam>
    public class BaseAuthAppService<TUserFromDirectoryDto, TUserFromDirectory, TAdditionalInfoDto, TUserDataDto> : IBaseAuthAppService
        where TUserFromDirectoryDto : BaseUserFromDirectoryDto, new()
        where TUserFromDirectory : IUserFromDirectory, new()
        where TAdditionalInfoDto : BaseAdditionalInfoDto, new()
        where TUserDataDto : BaseUserDataDto, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAuthAppService{TUserFromDirectoryDto, TUserFromDirectory, TAdditionalInfoDto, TUserDataDto}" /> class.
        /// </summary>
        /// <param name="userAppService">The user application service.</param>
        /// <param name="teamAppService">The team application service.</param>
        /// <param name="roleAppService">The role application service.</param>
        /// <param name="identityProviderRepository">The identity provider repository.</param>
        /// <param name="jwtFactory">The JWT factory.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="userPermissionDomainService">The user permission domain service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="biaNetconfiguration">The bia netconfiguration.</param>
        /// <param name="userDirectoryHelper">The user directory helper.</param>
        /// <param name="ldapRepositoryHelper">The LDAP repository helper.</param>
        public BaseAuthAppService(
            IJwtFactory jwtFactory,
            IPrincipal principal,
            IUserPermissionDomainService userPermissionDomainService,
            ILogger<BaseAuthAppService<TUserFromDirectoryDto, TUserFromDirectory, TAdditionalInfoDto, TUserDataDto>> logger,
            IConfiguration configuration,
            IOptions<BiaNetSection> biaNetconfiguration,
            IUserDirectoryRepository<TUserFromDirectoryDto, TUserFromDirectory> userDirectoryHelper,
            ILdapRepositoryHelper ldapRepositoryHelper)
        {
            this.JwtFactory = jwtFactory;
            this.ClaimsPrincipal = principal as BiaClaimsPrincipal;
            this.UserPermissionDomainService = userPermissionDomainService;
            this.Logger = logger;
            this.UserDirectoryHelper = userDirectoryHelper;
            this.LdapDomains = biaNetconfiguration.Value.Authentication?.LdapDomains;
            this.LdapRepositoryHelper = ldapRepositoryHelper;
        }

        /// <summary>
        /// The logger.
        /// </summary>
        protected ILogger<BaseAuthAppService<TUserFromDirectoryDto, TUserFromDirectory, TAdditionalInfoDto, TUserDataDto>> Logger { get; }

        /// <summary>
        /// The principal.
        /// </summary>
        protected BiaClaimsPrincipal ClaimsPrincipal { get; }

        /// <summary>
        /// The JWT factory.
        /// </summary>
        protected IJwtFactory JwtFactory { get; }

        /// <summary>
        /// The user permission domain service.
        /// </summary>
        protected IUserPermissionDomainService UserPermissionDomainService { get; }

        /// <summary>
        /// The helper used for AD.
        /// </summary>
        protected IUserDirectoryRepository<TUserFromDirectoryDto, TUserFromDirectory> UserDirectoryHelper { get; }

        /// <summary>
        /// The domain section in the BiaNet configuration.
        /// </summary>
        protected IEnumerable<LdapDomain> LdapDomains { get; }

        /// <summary>
        /// The ldap repository service.
        /// </summary>
        protected ILdapRepositoryHelper LdapRepositoryHelper { get; }

        /// <inheritdoc/>
        public async Task<string> LoginAsync()
        {
            // Check if current user is authenticated
            this.CheckIsAuthenticated();

            // Get informations in Claims
            string sid = this.GetSid();
            string login = this.GetLogin();
            string domain = this.GetDomain();

            // Get Global Roles
            List<string> globalRoles = await this.GetGlobalRolesAsync(sid: sid, domain: domain);

            // Get Permissions
            List<string> userPermissions = this.UserPermissionDomainService.TranslateRolesInPermissions(globalRoles);

            // Check User Permissions
            this.CheckUserPermissions(userPermissions);

            // Sort User Permissions
            userPermissions.Sort();

            // Create Token Dto
            TokenDto<TUserDataDto> tokenDto = new TokenDto<TUserDataDto>()
            {
                IdentityKey = login,
                RoleIds = new List<int>(),
                Permissions = userPermissions,
                UserData = this.CreateUserData(null),
            };

            // Create AuthInfo
            AuthInfoDto<TAdditionalInfoDto> authInfo = await this.JwtFactory.GenerateAuthInfoAsync(tokenDto, this.CreateAdditionalInfo(), new LoginParamDto());

            return authInfo?.Token;
        }

        /// <summary>
        /// Checks the user permissions.
        /// </summary>
        /// <param name="userPermissions">The user permissions.</param>
        /// <exception cref="UnauthorizedException">No permission found.</exception>
        protected void CheckUserPermissions(List<string> userPermissions)
        {
            if (!userPermissions.Any())
            {
                this.Logger.LogInformation("Unauthorized because no permission found");
                throw new UnauthorizedException("No permission found");
            }
        }

        /// <summary>
        /// Checks if user is Authenticated.
        /// </summary>
        /// <param name="claimsPrincipal">The identity.</param>
        protected void CheckIsAuthenticated()
        {
            if (this.ClaimsPrincipal.Identity?.IsAuthenticated != true)
            {
                if (this.ClaimsPrincipal.Identity == null)
                {
                    this.Logger.LogInformation("Unauthorized because identity is null");
                }
                else
                {
                    this.Logger.LogInformation("Unauthorized because not authenticated");
                }

                throw new UnauthorizedException();
            }
        }

        /// <summary>
        /// Gets the global roles.
        /// </summary>
        /// <param name="sid">The sid.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="userInfo">The user information.</param>
        /// <param name="withCredentials">Connection with standard authentication.</param>
        /// <returns>Global roles.</returns>
        /// <exception cref="UnauthorizedException">No roles found.</exception>
        protected async Task<List<string>> GetGlobalRolesAsync(string sid, string domain, UserInfoFromDBDto userInfo = default, bool withCredentials = true)
        {
            List<string> globalRoles = await this.UserDirectoryHelper.GetUserRolesAsync(claimsPrincipal: this.ClaimsPrincipal, userInfoDto: userInfo, sid: sid, domain: domain, withCredentials: withCredentials);

            // If the user has no role
            if (globalRoles?.Any() != true)
            {
                this.Logger.LogInformation("Unauthorized because No roles found");
                throw new UnauthorizedException("No roles found");
            }

            return globalRoles;
        }

        /// <summary>
        /// Gets the sid.
        /// </summary>
        /// <returns>The sid.</returns>
        protected string GetSid()
        {
            return this.ClaimsPrincipal.GetClaimValue(ClaimTypes.PrimarySid);
        }

        /// <summary>
        /// Gets the login.
        /// </summary>
        /// <returns>The login.</returns>
        protected string GetLogin()
        {
            var login = this.ClaimsPrincipal.GetUserLogin()?.Split('\\').LastOrDefault()?.ToUpper();
            if (string.IsNullOrEmpty(login))
            {
                this.Logger.LogWarning("Unauthorized because bad login");
                throw new BadRequestException("Incorrect login");
            }

            return login;
        }

        /// <summary>
        /// Gets the domain.
        /// </summary>
        /// <returns>The domain.</returns>
        protected string GetDomain()
        {
            string domain = null;

            if (this.ClaimsPrincipal.Identity.Name?.Contains('\\') == true)
            {
                domain = this.ClaimsPrincipal.Identity.Name.Split('\\').FirstOrDefault();
                if (
                        !this.LdapDomains.Any(ld => ld.Name.Equals(domain))
                        &&
                        !(
                            this.LdapDomains.Any(ld => this.LdapRepositoryHelper.IsLocalMachineName(ld.Name, true))
                            &&
                            this.LdapRepositoryHelper.IsLocalMachineName(domain, false))
                        &&
                        !(
                            this.LdapDomains.Any(ld => this.LdapRepositoryHelper.IsServerDomain(ld.Name, true))
                            &&
                            this.LdapRepositoryHelper.IsServerDomain(domain, false)))
                {
                    this.Logger.LogInformation("Unauthorized because bad domain");
                    throw new UnauthorizedException();
                }
            }

            return domain;
        }

        /// <summary>
        /// Create a new instance of <typeparamref name="TUserDataDto"/> added into login token of <see cref="AuthInfoDto{TAdditionalInfoDto}"/>.
        /// </summary>
        /// <param name="userInfoFromDBDto">User info from db.</param>
        /// <returns>New <typeparamref name="TUserDataDto"/>.</returns>
        protected virtual TUserDataDto CreateUserData(UserInfoFromDBDto userInfoFromDBDto)
        {
            return new TUserDataDto()
            {
                FirstName = userInfoFromDBDto?.FirstName ?? string.Empty,
                LastName = userInfoFromDBDto?.LastName ?? string.Empty,
            };
        }

        /// <summary>
        /// Create a new instance of <typeparamref name="TAdditionalInfoDto"/> added beside login token of <see cref="AuthInfoDto{TAdditionalInfoDto}"/>.
        /// </summary>
        /// <returns>New <typeparamref name="TAdditionalInfoDto"/>.</returns>
        protected virtual TAdditionalInfoDto CreateAdditionalInfo()
        {
            return new TAdditionalInfoDto();
        }
    }
}