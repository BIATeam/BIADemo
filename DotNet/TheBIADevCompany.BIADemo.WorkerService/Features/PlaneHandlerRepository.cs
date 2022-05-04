// BIADemo only
// <copyright file="PlaneHandlerRepository.cs" company="TheBIADevCompany">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService.Features
{
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Example for handler repository: a signalR event is send to client when something change in the Plane Table.
    /// </summary>
    public class PlaneHandlerRepository : DatabaseHandlerRepository
    {
        private static IClientForHubRepository clientForHubService = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneHandlerRepository"/> class.
        /// Constructor Set the trigger request.
        /// </summary>
        /// <param name="configuration">the project configuration.</param>
        public PlaneHandlerRepository(IConfiguration configuration)
            : base(
            configuration.GetConnectionString("BIADemoDatabase"),
            "SELECT RowVersion FROM [dbo].[Planes]",
            "SELECT TOP (1) [SiteId] FROM [dbo].[Planes] ORDER BY [RowVersion] DESC",
            r => PlaneChange(r))
        {
        }

        /// <summary>
        /// Configure the services.
        /// </summary>
        /// <param name="pClientForHubService">The client for hub service.</param>
        public static void Configure(IClientForHubRepository pClientForHubService)
        {
            clientForHubService = pClientForHubService;
        }

        /// <summary>
        /// Send message to the clients.
        /// </summary>
        /// <param name="reader">the reader use to retrieve info send by th trigger.</param>
        public static void PlaneChange(SqlDataReader reader)
        {
            if (clientForHubService == null)
            {
                throw new ConfigurationErrorsException("The ClientForHub feature is not configure before use PlaneChange. Verify your correctly configure PlaneHandlerRepository in Statup.cs.");
            }

            int siteId = reader.GetInt32(0);
            _ = clientForHubService.SendMessage(new TargetedFeatureDto { ParentKey = siteId.ToString(), FeatureName = "planes" }, "refresh-planes", string.Empty);
        }
    }
}
