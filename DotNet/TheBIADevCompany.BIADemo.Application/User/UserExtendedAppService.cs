// <copyright file="UserExtendedAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Bia.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Bia.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.Bia.User;
    using TheBIADevCompany.BIADemo.Domain.Bia.User.Entities;
    using TheBIADevCompany.BIADemo.Domain.Bia.User.Mappers;
    using TheBIADevCompany.BIADemo.Domain.Bia.User.Models;
    using TheBIADevCompany.BIADemo.Domain.Bia.User.Services;
    using TheBIADevCompany.BIADemo.Domain.Bia.User.Specifications;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;
    using TheBIADevCompany.BIADemo.Domain.User.Mappers;
    using static TheBIADevCompany.BIADemo.Crosscutting.Common.Rights;

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