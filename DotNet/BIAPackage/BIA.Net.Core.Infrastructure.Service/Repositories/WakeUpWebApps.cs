// <copyright file="WakeUpWebApps.cs" company="BIA">
//  Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// WorkInstruction Repository.
    /// </summary>
    /// <seealso cref="BIA.BIADemo.Domain.RepoContract.IWorkInstructionRepository" />
    public class WakeUpWebApps : WebApiRepository, IWakeUpWebApps
    {
        private readonly List<WakeUpWebApp> wakeUpWebApps;

        /// <summary>
        /// Initializes a new instance of the <see cref="WakeUpWebApps"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        public WakeUpWebApps(HttpClient httpClient, IConfiguration configuration, ILogger<WakeUpWebApps> logger, IBiaDistributedCache distributedCache)
             : base(httpClient, logger, distributedCache)
        {
            this.wakeUpWebApps = new List<WakeUpWebApp>();
            var webAppToWakeUpSection = configuration.GetSection("WebAppToWakeUp");
            IEnumerable<IConfigurationSection> webAppToWakeUpArray = webAppToWakeUpSection.GetChildren();
            foreach (var configSection in webAppToWakeUpArray)
            {
                this.wakeUpWebApps.Add(new WakeUpWebApp(httpClient, logger, distributedCache, configSection["BaseAddress"], configSection["UrlWakeUp"]));
            }
        }

        /// <inheritdoc/>
        public List<Task<(bool IsSuccessStatusCode, string ReasonPhrase)>> WakeUp()
        {
            List<Task<(bool IsSuccessStatusCode, string ReasonPhrase)>> wakeUpWebAppTasks = new ();
            foreach (WakeUpWebApp wakeUpWebApp in this.wakeUpWebApps)
            {
                wakeUpWebAppTasks.Add(wakeUpWebApp.WakeUp());
            }

            return wakeUpWebAppTasks;
        }
    }
}
