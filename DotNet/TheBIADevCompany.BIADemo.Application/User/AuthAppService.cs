// <copyright file="AuthAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Application.User
{
    using System.Security.Principal;
#if BIA_FRONT_FEATURE
    using System.Threading.Tasks;
#endif
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Application.User;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.User.Entities;
    using BIA.Net.Core.Domain.User.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
#if BIA_FRONT_FEATURE
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.User;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;
    using TheBIADevCompany.BIADemo.Domain.User.Models;
#endif

    /// <summary>
    /// Auth App Service.
    /// </summary>
    public class AuthAppService(
#if BIA_FRONT_FEATURE
        IBaseUserAppService<UserDto, User, UserFromDirectoryDto, UserFromDirectory> userAppService,
        IBaseTeamAppService<TeamTypeId> teamAppService,
        IRoleAppService roleAppService,
        IIdentityProviderRepository<UserFromDirectory> identityProviderRepository,
#endif
        IJwtFactory jwtFactory,
        IPrincipal principal,
        IUserPermissionDomainService userPermissionDomainService,
        ILogger<AuthAppService> logger,
        IConfiguration configuration,
        IOptions<BiaNetSection> biaNetconfiguration,
        IUserDirectoryRepository<UserFromDirectoryDto, UserFromDirectory> userDirectoryHelper,
        ILdapRepositoryHelper ldapRepositoryHelper)
#if BIA_FRONT_FEATURE
        : BaseFrontAuthAppService<UserDto, User, RoleId, TeamTypeId, UserFromDirectoryDto, UserFromDirectory>(userAppService, teamAppService, roleAppService, identityProviderRepository, jwtFactory, principal, userPermissionDomainService, logger, configuration, biaNetconfiguration, userDirectoryHelper, ldapRepositoryHelper), IAuthAppService
#else
        : BaseAuthAppService(jwtFactory, principal, userPermissionDomainService, logger, configuration, biaNetconfiguration, userDirectoryHelper, ldapRepositoryHelper), IAuthAppService
#endif
    {
#if BIA_FRONT_FEATURE
        /// <summary>
        /// Logins the on teams asynchronous.
        /// </summary>
        /// <param name="loginParam">The login parameter.</param>
        /// <returns>
        /// AuthInfo.
        /// </returns>
        public Task<AuthInfoDto<AdditionalInfoDto>> LoginOnTeamsAsync(LoginParamDto loginParam)
        {
            return this.LoginOnTeamsAsync(loginParam, TeamConfig.Config);
        }

        /// <summary>
        /// Gets the identity key.
        /// </summary>
        /// <returns>The identity key.</returns>
        protected override string GetIdentityKey()
        {
            // If you change it parse all other #IdentityKey to align all (Database, Ldap, Idp, WindowsIdentity).
            return this.GetLogin();
        }
#endif
    }
}