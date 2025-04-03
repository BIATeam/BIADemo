// BIADemo only
// <copyright file="BiaRemoteRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories
{
    using System.Net.Http;
    using BIA.Net.Core.Common.Configuration.BiaWebApi;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// Bia Remote Repository.
    /// </summary>
    public class BiaRemoteRepository : BiaWebApiRepository, IBiaRemoteRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BiaRemoteRepository"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        public BiaRemoteRepository(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<BiaRemoteRepository> logger,
            IBiaDistributedCache distributedCache)
            : base(
                httpClient,
                logger,
                distributedCache,
                configuration.GetSection("MyBiaWebApi").Get<BiaWebApi>())
        {
        }

        /// <inheritdoc />
        public virtual async Task<bool> PingAsync()
        {
            string url = "/api/shared/healthcheck";
            return (await this.GetAsync<bool>($"{this.BaseAddress}{url}")).Result;
        }
    }
}
