// <copyright file="BiaApiRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration.AuthenticationSection;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// BiaApi Repository.
    /// </summary>
    public abstract class BiaApiRepository : WebApiRepository
    {
        /// <summary>
        /// The bia web API authentication repository.
        /// </summary>
        private readonly IBiaApiAuthRepository biaApiAuthRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaApiRepository" /> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="biaApiAuthRepository">The bia web API authentication repository.</param>
        protected BiaApiRepository(
            HttpClient httpClient,
            ILogger<BiaApiRepository> logger,
            IBiaDistributedCache distributedCache,
            IBiaApiAuthRepository biaApiAuthRepository)
             : base(httpClient, logger, distributedCache, new AuthenticationConfiguration() { Mode = AuthenticationMode.Token })
        {
            this.biaApiAuthRepository = biaApiAuthRepository;
        }

        /// <summary>
        /// The base address.
        /// </summary>
        protected string BaseAddress { get; set; }

        /// <inheritdoc />
        protected override async Task<string> GetBearerTokenAsync()
        {
            string token = await this.biaApiAuthRepository.LoginAsync(baseAddress: this.BaseAddress);
            return token;
        }
    }
}