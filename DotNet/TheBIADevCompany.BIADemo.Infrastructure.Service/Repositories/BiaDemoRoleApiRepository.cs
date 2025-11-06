// BIADemo only
// <copyright file="BiaDemoRoleApiRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories
{
    using System.Net.Http;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Domain.Api.RolesForApp;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// Initializes a new instance of the <see cref="BiaDemoRoleApiRepository"/> class.
    /// </summary>
    /// <param name="httpClient">The injected <see cref="HttpClient"/>.</param>
    /// <param name="configuration">The injected <see cref="IConfiguration"/>.</param>
    /// <param name="logger">The injected <see cref="ILogger"/>.</param>
    /// <param name="distributedCache">The injected <see cref="IBiaDistributedCache"/>.</param>
    public class BiaDemoRoleApiRepository(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<RoleApiRepository<ApiRolesForApp>> logger,
        IBiaDistributedCache distributedCache)
        : RoleApiRepository<ApiRolesForApp>(httpClient, configuration, logger, distributedCache), IBiaDemoRoleApiRepository
    {
    }
}
