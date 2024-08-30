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
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Example for handler repository using Sql broker handler : a signalR event is send to client when something change in the Plane Table.
    /// </summary>
    public class PlaneHandlerRepository : DatabaseHandlerRepository<PlaneHandlerRepository>
    {
        /// <summary>
        /// The client for hub service.
        /// </summary>
        private readonly IClientForHubRepository clientForHubService = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneHandlerRepository"/> class.
        /// Constructor Set the trigger request.
        /// </summary>
        /// <param name="configuration">the project configuration.</param>
        /// <param name="clientForHubService">the client hub service.</param>
        /// <param name="serviceProvider">the service provider.</param>
        public PlaneHandlerRepository(IConfiguration configuration, IClientForHubRepository clientForHubService, IServiceProvider serviceProvider)
            : base(
                  serviceProvider,
                  configuration.GetConnectionString("BIADemoDatabase"),
                  configuration.GetDBEngine("BIADemoDatabase"),
                  "SELECT RowVersion FROM [dbo].[Planes]",
                  "SELECT TOP (1) [SiteId] FROM [dbo].[Planes] ORDER BY [RowVersion] DESC")
        {
            this.OnChange += async (reader) => await this.PlaneChange(reader);
            this.clientForHubService = clientForHubService;
        }

        /// <summary>
        /// Send message to the clients.
        /// </summary>
        /// <param name="reader">the reader use to retrieve info send by the trigger.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task PlaneChange(DbDataReader reader)
        {
            if (this.clientForHubService == null)
            {
                throw new ConfigurationErrorsException("The ClientForHub feature is not configure before use PlaneChange. Verify your correctly configure PlaneHandlerRepository in Statup.cs.");
            }

            if (reader != null)
            {
                int siteId = reader.GetInt32(0);
                await this.clientForHubService.SendMessage(new TargetedFeatureDto { ParentKey = siteId.ToString(), FeatureName = "planes" }, "refresh-planes", string.Empty);
            }
        }
    }
}
