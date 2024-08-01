// <copyright file="AuthApiAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Common.Helpers;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Service;

    /// <summary>
    /// Auth App Service.
    /// </summary>
    /// <seealso cref="TheBIADevCompany.BIADemo.Application.User.IAuthApiAppService" />
    public class AuthApiAppService : IAuthApiAppService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<AuthApiAppService> logger;

        /// <summary>
        /// The principal.
        /// </summary>
        private readonly BiaClaimsPrincipal claimsPrincipal;

        /// <summary>
        /// The JWT factory.
        /// </summary>
        private readonly IJwtFactory jwtFactory;

        /// <summary>
        /// The user permission domain service.
        /// </summary>
        private readonly IUserPermissionDomainService userPermissionDomainService;

        /// <summary>
        /// The helper used for AD.
        /// </summary>
        private readonly IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper;

        /// <summary>
        /// The domain section in the BiaNet configuration.
        /// </summary>
        private readonly IEnumerable<LdapDomain> ldapDomains;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthApiAppService" /> class.
        /// </summary>
        /// <param name="jwtFactory">The JWT factory.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="userPermissionDomainService">The user permission domain service.</param>
        /// <param name="userAppService">The user application service.</param>
        /// <param name="teamAppService">The team application service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="roleAppService">The role application service.</param>
        /// <param name="userProfileRepository">The user profile repository.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="biaNetconfiguration">The BiaNetSection configuration.</param>
        /// <param name="userDirectoryHelper">The user directory helper.</param>
        /// <param name="identityProviderRepository">The identity provider repository.</param>
        public AuthApiAppService(
            IJwtFactory jwtFactory,
            IPrincipal principal,
            IUserPermissionDomainService userPermissionDomainService,
            ITeamAppService teamAppService,
            ILogger<AuthApiAppService> logger,
            IConfiguration configuration,
            IOptions<BiaNetSection> biaNetconfiguration,
            IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper)
        {
            this.jwtFactory = jwtFactory;
            this.claimsPrincipal = principal as BiaClaimsPrincipal;
            this.userPermissionDomainService = userPermissionDomainService;
            this.logger = logger;
            this.userDirectoryHelper = userDirectoryHelper;
            this.ldapDomains = biaNetconfiguration.Value.Authentication.LdapDomains;
        }

        /// <summary>
        /// Logins the on teams asynchronous.
        /// </summary>
        /// <param name="loginParam">The login parameter.</param>
        /// <returns>
        /// AuthInfo.
        /// </returns>
        public async Task<string> LoginAsync()
        {
            this.CheckIsAuthenticated();

            string sid = this.GetSid();
            string login = this.GetLogin();
            string domain = this.GetDomain();

            List<string> roles = await this.GetRolesAsync(sid, domain);
            List<string> permissions = this.GetPermissions(roles);
            List<int> roleIds = this.GetRoleIds(roles);
            UserDataDto userData = new UserDataDto();

            TokenDto<UserDataDto> tokenDto = new TokenDto<UserDataDto>()
            {
                Login = login,
                RoleIds = roleIds,
                Permissions = permissions,
                UserData = userData,
            };

            AuthInfoDto<AdditionalInfoDto> authInfo = await this.jwtFactory.GenerateAuthInfoAsync(tokenDto, default(AdditionalInfoDto), new LoginParamDto());

            return authInfo?.Token;
        }

        private List<int> GetRoleIds(List<string> roles)
        {
            List<int> roleIds = new List<int>();
            foreach (string role in roles)
            {
                if (Enum.TryParse<RoleId>(role, out var roleId) && !roleIds.Contains((int)roleId))
                {
                    roleIds.Add((int)roleId);
                }
            }

            return roleIds;
        }

        private async Task<List<string>> GetRolesAsync(string sid, string domain)
        {
            List<string> globalRoles = await this.userDirectoryHelper.GetUserRolesAsync(claimsPrincipal: this.claimsPrincipal, userInfoDto: null, sid: sid, domain: domain);

            // If the user has no role
            if (globalRoles?.Any() != true)
            {
                this.logger.LogInformation("Unauthorized because No roles found");
                throw new UnauthorizedException("No roles found");
            }

            return globalRoles;
        }

        private List<string> GetPermissions(List<string> roles)
        {
            List<string> userPermissions = this.userPermissionDomainService.TranslateRolesInPermissions(roles, false);

            if (userPermissions?.Any() != true)
            {
                this.logger.LogInformation("Unauthorized because no permission found");
                throw new UnauthorizedException("No permission found");
            }

            return userPermissions;
        }

        /// <summary>
        /// Checks if user is Authenticated.
        /// </summary>
        /// <param name="claimsPrincipal">The identity.</param>
        private void CheckIsAuthenticated()
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
        /// Gets the sid.
        /// </summary>
        /// <returns>The sid.</returns>
        private string GetSid()
        {
            return this.claimsPrincipal.GetClaimValue(ClaimTypes.PrimarySid);
        }

        /// <summary>
        /// Gets the login.
        /// </summary>
        /// <returns>The login.</returns>
        private string GetLogin()
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
        private string GetDomain()
        {
            string domain = null;

            if (this.claimsPrincipal.Identity.Name?.Contains('\\') == true)
            {
                domain = this.claimsPrincipal.Identity.Name.Split('\\').FirstOrDefault();
                if (!this.ldapDomains.Any(ld => ld.Name.Equals(domain)))
                {
                    this.logger.LogInformation("Unauthorized because bad domain");
                    throw new UnauthorizedException();
                }
            }

            return domain;
        }
    }
}
