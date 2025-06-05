// <copyright file="AuthAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Application.User
{
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Application.User;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.User.Models;
    using BIA.Net.Core.Domain.User.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.User;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

    /// <summary>
    /// Auth App Service.
    /// </summary>
    public class AuthAppService(
        IBaseUserAppService<UserDto, User> userAppService,
        IBaseTeamAppService<TeamTypeId> teamAppService,
        IRoleAppService roleAppService,
        IIdentityProviderRepository identityProviderRepository,
        IJwtFactory jwtFactory,
        IPrincipal principal,
        IUserPermissionDomainService userPermissionDomainService,
        ILogger<AuthAppService> logger,
        IConfiguration configuration,
        IOptions<BiaNetSection> biaNetconfiguration,
        IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper,
        ILdapRepositoryHelper ldapRepositoryHelper) : BaseAuthAppService<UserDto, User, RoleId, TeamTypeId>(userAppService, teamAppService, roleAppService, identityProviderRepository, jwtFactory, principal, userPermissionDomainService, logger, configuration, biaNetconfiguration, userDirectoryHelper, ldapRepositoryHelper), IAuthAppService
    {
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
    }
}