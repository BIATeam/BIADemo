// <copyright file="BiaWebApiAuthRepository.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories.BiaApi
{
    using System.Dynamic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration.BiaWebApi;
    using BIA.Net.Core.Domain.RepoContract.BiaApi;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// Bia WebApi Auth Repository.
    /// </summary>
    public class BiaWebApiAuthRepository : BiaWebApiRepository, IBiaWebApiAuthRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BiaWebApiAuthRepository"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        public BiaWebApiAuthRepository(
            HttpClient httpClient,
            ILogger<BiaWebApiAuthRepository> logger,
            IBiaDistributedCache distributedCache)
            : base(httpClient, logger, distributedCache)
        {
        }

        /// <inheritdoc />
        public virtual void Init(BiaWebApi biaWebApi)
        {
            this.BiaWebApi = biaWebApi;
        }

        /// <inheritdoc />
        public virtual async Task<string> LoginAsync(string url = "/api/Auth/login?lightToken=false")
        {
            var result = await this.GetAsync<object>($"{this.BaseAddress}{url}");
            if (result.IsSuccessStatusCode && result.Result != null)
            {
                dynamic responseObject = JsonConvert.DeserializeObject<ExpandoObject>(result.Result.ToString());
                return responseObject?.token;
            }

            return null;
        }

        /// <inheritdoc />
        public virtual async Task<string> GetTokenAsync(string url = "/api/Auth/token")
        {
            var result = await this.GetAsync<string>($"{this.BaseAddress}{url}");
            return result.IsSuccessStatusCode ? result.Result : null;
        }
    }
}
