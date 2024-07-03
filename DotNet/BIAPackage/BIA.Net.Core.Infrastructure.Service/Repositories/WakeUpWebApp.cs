// <copyright file="WakeUpWebApp.cs" company="BIA.Net">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// WorkInstruction Repository.
    /// </summary>
    /// <seealso cref="TheBIADevCompany.BIADemo.Domain.RepoContract.IWorkInstructionRepository" />
    public class WakeUpWebApp : WebApiRepository
    {
        private readonly string baseAddress;
        private readonly string urlWakeUp;

        /// <summary>
        /// Initializes a new instance of the <see cref="WakeUpWebApp"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="baseAddress">The base adresse for urlWakeUp.</param>
        /// <param name="urlWakeUp">The url to wakeup.</param>
#pragma warning disable S6672 // Generic logger injection should match enclosing type
        public WakeUpWebApp(HttpClient httpClient, ILogger logger, IBiaDistributedCache distributedCache, string baseAddress, string urlWakeUp)
#pragma warning restore S6672 // Generic logger injection should match enclosing type
             : base(httpClient, logger, distributedCache)
        {
            this.baseAddress = baseAddress;
            this.urlWakeUp = urlWakeUp;
        }

        /// <summary>
        /// Lanch the wakup process.
        /// </summary>
        /// <returns>The async task.</returns>
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
