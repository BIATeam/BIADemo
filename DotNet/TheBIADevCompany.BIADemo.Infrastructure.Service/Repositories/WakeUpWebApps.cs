// <copyright file="BIADemoAppRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// WorkInstruction Repository.
    /// </summary>
    /// <seealso cref="TheBIADevCompany.BIADemo.Domain.RepoContract.IWorkInstructionRepository" />
#pragma warning disable S101 // Types should be named in PascalCase
    public class WakeUpWebApps : WebApiRepository, IWakeUpWebApps
#pragma warning restore S101 // Types should be named in PascalCase
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
            };
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
