// <copyright file="BaseAuthAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
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
    /// <typeparam name="TUserDto">The type of user dto.</typeparam>
    /// <typeparam name="TUser">The type of user.</typeparam>
    /// <typeparam name="TEnumRoleId">The type for enum Role Id.</typeparam>
    /// <typeparam name="TEnumTeamTypeId">The type for enum Team Type Id.</typeparam>
    public class BaseAuthAppService : IBaseAuthAppService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger<BaseAuthAppService> logger;

        /// <summary>
        /// The principal.
        /// </summary>
        protected readonly BiaClaimsPrincipal claimsPrincipal;

        /// <summary>
        /// The JWT factory.
        /// </summary>
        protected readonly IJwtFactory jwtFactory;

        /// <summary>
        /// The user permission domain service.
        /// </summary>
        protected readonly IUserPermissionDomainService userPermissionDomainService;

        /// <summary>
        /// The helper used for AD.
        /// </summary>
        protected readonly IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper;

        /// <summary>
        /// The domain section in the BiaNet configuration.
        /// </summary>
        protected readonly IEnumerable<LdapDomain> ldapDomains;

        /// <summary>
        /// The ldap repository service.
        /// </summary>
        protected readonly ILdapRepositoryHelper ldapRepositoryHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAuthAppService" /> class.
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
            ILogger<BaseAuthAppService> logger,
            IConfiguration configuration,
            IOptions<BiaNetSection> biaNetconfiguration,
            IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper,
            ILdapRepositoryHelper ldapRepositoryHelper)
        {
            this.jwtFactory = jwtFactory;
            this.claimsPrincipal = principal as BiaClaimsPrincipal;
            this.userPermissionDomainService = userPermissionDomainService;
            this.logger = logger;
            this.userDirectoryHelper = userDirectoryHelper;
            this.ldapDomains = biaNetconfiguration.Value.Authentication.LdapDomains;
            this.ldapRepositoryHelper = ldapRepositoryHelper;
        }

        /// <inheritdoc cref="IAuthAppService.LoginAsync"/>
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
            List<string> userPermissions = this.userPermissionDomainService.TranslateRolesInPermissions(globalRoles);

            // Check User Permissions
            this.CheckUserPermissions(userPermissions);

            // Sort User Permissions
            userPermissions.Sort();

            // Create Token Dto
            TokenDto<UserDataDto> tokenDto = new TokenDto<UserDataDto>()
            {
                Login = login,
                RoleIds = new List<int>(),
                Permissions = userPermissions,
                UserData = new UserDataDto(),
            };

            // Create AuthInfo
            AuthInfoDto<AdditionalInfoDto> authInfo = await this.jwtFactory.GenerateAuthInfoAsync(tokenDto, default(AdditionalInfoDto), new LoginParamDto());

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
                this.logger.LogInformation("Unauthorized because no permission found");
                throw new UnauthorizedException("No permission found");
            }
        }

        /// <summary>
        /// Checks if user is Authenticated.
        /// </summary>
        /// <param name="claimsPrincipal">The identity.</param>
        protected void CheckIsAuthenticated()
        {
            if (this.claimsPrincipal.Identity?.IsAuthenticated != true)
            {
                if (this.claimsPrincipal.Identity == null)
                {
                    this.logger.LogInformation("Unauthorized because identity is null");
                }
                else
                {
                    this.logger.LogInformation("Unauthorized because not authenticated");
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
        protected async Task<List<string>> GetGlobalRolesAsync(string sid, string domain, UserInfoDto userInfo = default, bool withCredentials = true)
        {
            List<string> globalRoles = await this.userDirectoryHelper.GetUserRolesAsync(claimsPrincipal: this.claimsPrincipal, userInfoDto: userInfo, sid: sid, domain: domain, withCredentials: withCredentials);

            // If the user has no role
            if (globalRoles?.Any() != true)
            {
                this.logger.LogInformation("Unauthorized because No roles found");
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
            return this.claimsPrincipal.GetClaimValue(ClaimTypes.PrimarySid);
        }

        /// <summary>
        /// Gets the login.
        /// </summary>
        /// <returns>The login.</returns>
        protected string GetLogin()
        {
            var login = this.claimsPrincipal.GetUserLogin()?.Split('\\').LastOrDefault()?.ToUpper();
            if (string.IsNullOrEmpty(login))
            {
                this.logger.LogWarning("Unauthorized because bad login");
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

            if (this.claimsPrincipal.Identity.Name?.Contains('\\') == true)
            {
                domain = this.claimsPrincipal.Identity.Name.Split('\\').FirstOrDefault();
                if (
                        !this.ldapDomains.Any(ld => ld.Name.Equals(domain))
                        &&
                        !(
                            this.ldapDomains.Any(ld => this.ldapRepositoryHelper.IsLocalMachineName(ld.Name, true))
                            &&
                            this.ldapRepositoryHelper.IsLocalMachineName(domain, false))
                        &&
                        !(
                            this.ldapDomains.Any(ld => this.ldapRepositoryHelper.IsServerDomain(ld.Name, true))
                            &&
                            this.ldapRepositoryHelper.IsServerDomain(domain, false)))
                {
                    this.logger.LogInformation("Unauthorized because bad domain");
                    throw new UnauthorizedException();
                }
            }

            return domain;
        }
    }
}