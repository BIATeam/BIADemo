// BIADemo only
// <copyright file="PlaneHandlerRepository.cs" company="TheBIADevCompany">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService.Features
{
    using System;
    using System.Configuration;
    using System.Data.Common;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Example for handler repository using Sql broker handler.
    /// A signalR event is send to client when something change in the Plane Table.
    /// </summary>
    public class PlaneHandlerRepository : DatabaseHandlerRepository<PlaneHandlerRepository>
    {
        /// <summary>
        /// The client for hub service.
        /// </summary>
        private readonly IClientForHubService clientForHubService = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneHandlerRepository"/> class.
        /// Constructor Set the trigger request.
        /// </summary>
        /// <param name="configuration">the project configuration.</param>
        /// <param name="clientForHubService">the client hub service.</param>
        /// <param name="serviceProvider">the service provider.</param>
        public PlaneHandlerRepository(IConfiguration configuration, IClientForHubService clientForHubService, IServiceProvider serviceProvider)
            : base(
                  serviceProvider,
                  configuration.GetConnectionString("BIADemoDatabase"),
                  configuration.GetDBEngine("BIADemoDatabase"),
                  "SELECT Id, SiteId, RowVersion FROM [dbo].[Planes]",
                  "Id",
                  useSqlDataBroker: configuration.GetSqlDataBroker("BIADemoDatabase"))
        {
            this.clientForHubService = clientForHubService;
        }

        /// <inheritdoc/>
        protected override async Task OnChange(DataBaseHandlerChangedData changedData)
        {
            if (this.clientForHubService == null)
            {
                throw new ConfigurationErrorsException("The ClientForHub feature is not configure before use PlaneChange. Verify your correctly configure PlaneHandlerRepository in Statup.cs.");
            }

            int? siteId = null;
            if (changedData.ChangeType == DatabaseHandlerChangeType.Delete && changedData.PreviousData.TryGetValue("SiteId", out object oldSiteId))
            {
                siteId = (int)oldSiteId;
            }
            else if (changedData.CurrentData.TryGetValue("SiteId", out object currentSiteId))
            {
                siteId = (int)currentSiteId;
            }

            if (siteId.HasValue)
            {
                await this.clientForHubService.SendMessage(new TargetedFeatureDto { ParentKey = siteId.Value.ToString(), FeatureName = "planes" }, "refresh-planes", string.Empty);
            }
        }
    }
}
