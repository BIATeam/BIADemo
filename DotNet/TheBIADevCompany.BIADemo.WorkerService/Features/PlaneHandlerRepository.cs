// BIADemo only
// <copyright file="PlaneHandlerRepository.cs" company="TheBIADevCompany">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService.Features
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Example for handler repository: a signalR event is send to client when something change in the Plane Table.
    /// </summary>
    public class PlaneHandlerRepository : DatabaseHandlerRepository<PlaneHandlerRepository>
    {
        private readonly IClientForHubRepository clientForHubService = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneHandlerRepository"/> class.
        /// Constructor Set the trigger request.
        /// </summary>
        /// <param name="configuration">the project configuration.</param>
        /// <param name="clientForHubService">the client hub service.</param>
        public PlaneHandlerRepository(IConfiguration configuration, IClientForHubRepository clientForHubService)
            : base(
            configuration.GetConnectionString("BIADemoDatabase"),
            new SqlCommand("SELECT RowVersion FROM [dbo].[Planes]"),
            new SqlCommand("SELECT TOP (1) [SiteId] FROM [dbo].[Planes] ORDER BY [RowVersion] DESC"),
            usePolling: false,
            pollingInterval: TimeSpan.FromSeconds(10))
        {
            this.OnChange += async (reader) => await this.PlaneChange(reader);
            this.clientForHubService = clientForHubService;
        }

        /// <summary>
        /// Send message to the clients.
        /// </summary>
        /// <param name="reader">the reader use to retrieve info send by th trigger.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task PlaneChange(SqlDataReader reader)
        {
            if (this.clientForHubService == null)
            {
                throw new ConfigurationErrorsException("The ClientForHub feature is not configure before use PlaneChange. Verify your correctly configure PlaneHandlerRepository in Statup.cs.");
            }

            int siteId = reader.GetInt32(0);
            await this.clientForHubService.SendMessage(new TargetedFeatureDto { ParentKey = siteId.ToString(), FeatureName = "planes" }, "refresh-planes", string.Empty);
        }
    }
}
