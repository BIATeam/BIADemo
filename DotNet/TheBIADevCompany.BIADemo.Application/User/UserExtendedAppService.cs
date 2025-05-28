// <copyright file="UserExtendedAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Bia.User
{
    using System.Security.Principal;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.User.Entities;
    using BIA.Net.Core.Domain.User.Models;
    using BIA.Net.Core.Domain.User.Services;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Domain.Bia.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;
    using TheBIADevCompany.BIADemo.Domain.User.Mappers;

    /// <summary>
    /// The application service used for user.
    /// </summary>
    public class UserExtendedAppService : UserAppService<UserExtendedDto, UserExtended, UserExtendedMapper>
    {
        public UserExtendedAppService(
            ITGenericRepository<UserExtended, int> repository,
            IUserSynchronizeDomainService<UserExtended> userSynchronizeDomainService,
            IOptions<BiaNetSection> configuration,
            IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper,
            ITGenericRepository<UserDefaultTeam, int> userDefaultTeamRepository,
            ILogger<UserExtendedAppService> logger,
            IIdentityProviderRepository identityProviderRepository,
            IUserIdentityKeyDomainService<UserExtended> userIdentityKeyDomainService,
            IPrincipal principal)
            : base(repository, userSynchronizeDomainService, configuration, userDirectoryHelper, userDefaultTeamRepository, logger, identityProviderRepository, userIdentityKeyDomainService, principal)
        {
        }
    }
}