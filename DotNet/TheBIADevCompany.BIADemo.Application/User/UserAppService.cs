// <copyright file="UserAppService.cs" company="TheBIADevCompany">
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
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;
    using TheBIADevCompany.BIADemo.Domain.User.Mappers;

    /// <summary>
    /// The application service used for user.
    /// </summary>
    public class UserAppService : BaseUserAppService<UserDto, User, UserMapper>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="userSynchronizeDomainService">The user synchronize domain service.</param>
        /// <param name="configuration">The configuration of the BiaNet section.</param>
        /// <param name="userDirectoryHelper">The user directory helper.</param>
        /// <param name="userDefaultTeamRepository">The user team default repository.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="identityProviderRepository">The identity provider repository.</param>
        /// <param name="userIdentityKeyDomainService">The user Identity Key Domain Service.</param>
        /// <param name="principal">The principal.</param>
        public UserAppService(
            ITGenericRepository<User, int> repository,
            IUserSynchronizeDomainService<User> userSynchronizeDomainService,
            IOptions<BiaNetSection> configuration,
            IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper,
            ITGenericRepository<UserDefaultTeam, int> userDefaultTeamRepository,
            ILogger<UserAppService> logger,
            IIdentityProviderRepository identityProviderRepository,
            IUserIdentityKeyDomainService<User> userIdentityKeyDomainService,
            IPrincipal principal)
            : base(repository, userSynchronizeDomainService, configuration, userDirectoryHelper, userDefaultTeamRepository, logger, identityProviderRepository, userIdentityKeyDomainService, principal)
        {
        }
    }
}