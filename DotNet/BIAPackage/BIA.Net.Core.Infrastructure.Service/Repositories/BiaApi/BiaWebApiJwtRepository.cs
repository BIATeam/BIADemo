// <copyright file="BiaWebApiJwtRepository.cs" company="TheBIADevCompany">
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
    public abstract class BiaWebApiJwtRepository : WebApiRepository
    {
        /// <summary>
        /// The bia web API.
        /// </summary>
        private readonly BiaWebApi biaWebApi;

        /// <summary>
        /// The bia web API repository.
        /// </summary>
        private readonly IBiaWebApiAuthRepository biaWebApiAuthRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaWebApiJwtRepository"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="biaWebApiAuthRepository">The bia web API repository.</param>
        /// <param name="biaWebApi">The bia web API.</param>
        protected BiaWebApiJwtRepository(
            HttpClient httpClient,
            ILogger<BiaWebApiJwtRepository> logger,
            IBiaDistributedCache distributedCache,
            IBiaWebApiAuthRepository biaWebApiAuthRepository,
            BiaWebApi biaWebApi)
             : base(httpClient, logger, distributedCache, new AuthenticationConfiguration() { Mode = AuthenticationMode.Token })
        {
            this.biaWebApi = biaWebApi;
            this.biaWebApiAuthRepository = biaWebApiAuthRepository;
            this.biaWebApiAuthRepository.Init(biaWebApi);
        }

        /// <summary>
        /// Gets the base address.
        /// </summary>
        protected string BaseAddress => this.biaWebApi?.BaseAddress;

        /// <inheritdoc />
        protected override async Task<string> GetBearerTokenAsync()
        {
            string token = null;

            if (this.biaWebApi.UseLoginFineGrained)
            {
                token = await this.biaWebApiAuthRepository.LoginAsync();
            }
            else
            {
                token = await this.biaWebApiAuthRepository.GetTokenAsync();
            }

            return token;
        }
    }
}