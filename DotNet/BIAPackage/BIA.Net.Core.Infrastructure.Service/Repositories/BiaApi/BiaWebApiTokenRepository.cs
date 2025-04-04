// <copyright file="BiaWebApiTokenRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration.AuthenticationSection;
    using BIA.Net.Core.Common.Configuration.BiaWebApi;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Bia WebApi Token Repository.
    /// </summary>
    public abstract class BiaWebApiTokenRepository : WebApiRepository
    {
        /// <summary>
        /// The bia web API repository.
        /// </summary>
        private readonly IBiaWebApiRepository biaWebApiRepository;

        protected string BaseAddress => this.biaWebApiRepository?.BaseAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaWebApiTokenRepository"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="biaWebApiRepository">The bia web API repository.</param>
        /// <param name="biaWebApi">The bia web API.</param>
        protected BiaWebApiTokenRepository(
            HttpClient httpClient,
            ILogger<BiaWebApiTokenRepository> logger,
            IBiaDistributedCache distributedCache,
            IBiaWebApiRepository biaWebApiRepository,
            BiaWebApi biaWebApi)
             : base(httpClient, logger, distributedCache, new AuthenticationConfiguration() { Mode = AuthenticationMode.Token })
        {
            this.biaWebApiRepository = biaWebApiRepository;
            this.biaWebApiRepository.Init(biaWebApi);
        }

        /// <inheritdoc />
        protected override async Task<string> GetBearerTokenAsync()
        {
            string token = await this.biaWebApiRepository.LoginAsync();
            return token;
        }
    }
}