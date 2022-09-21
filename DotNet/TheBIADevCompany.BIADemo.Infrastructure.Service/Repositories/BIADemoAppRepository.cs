// <copyright file="BIADemoAppRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// WorkInstruction Repository.
    /// </summary>
    /// <seealso cref="TheBIADevCompany.BIADemo.Domain.RepoContract.IWorkInstructionRepository" />
#pragma warning disable S101 // Types should be named in PascalCase
    public class BIADemoAppRepository : WebApiRepository, IBIADemoAppRepository
#pragma warning restore S101 // Types should be named in PascalCase
    {
        private readonly string baseAddress;
        private readonly string urlWakeUp;

        /// <summary>
        /// Initializes a new instance of the <see cref="BIADemoAppRepository"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        public BIADemoAppRepository(HttpClient httpClient, IConfiguration configuration, ILogger<WebApiRepository> logger, IDistributedCache distributedCache)
             : base(httpClient, logger, distributedCache)
        {
            this.baseAddress = configuration["BIADemoApp:baseAddress"];
            this.urlWakeUp = configuration["BIADemoApp:urlWakeUp"];
        }

        /// <inheritdoc/>
        public virtual async Task<(bool IsSuccessStatusCode, string ReasonPhrase)> WakeUp()
        {
            if (string.IsNullOrWhiteSpace(this.baseAddress))
            {
                return (false, "Base adresse not set.");
            }

            var result = await this.GetAsync<string>(this.baseAddress + this.urlWakeUp);
            return (result.IsSuccessStatusCode, result.ReasonPhrase);
        }
    }
}
