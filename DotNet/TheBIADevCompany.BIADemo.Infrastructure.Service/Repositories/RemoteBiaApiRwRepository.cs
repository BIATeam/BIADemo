// BIADemo only
// <copyright file="RemoteBiaApiRwRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
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
    public class RemoteBiaApiRwRepository : BiaWebApiRepository, IRemoteBiaApiRwRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteBiaApiRwRepository"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        public RemoteBiaApiRwRepository(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<RemoteBiaApiRwRepository> logger,
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
            string url = "/api/Auth/token";
            var result = await this.GetAsync<object>($"{this.BiaWebApi.BaseAddress}{url}");
            return result.IsSuccessStatusCode;
        }
    }
}
