// BIADemo only
// <copyright file="AirportHandlerRepository.cs" company="TheBIADevCompany">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService.Features
{
    using System;
    using System.Data.Common;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Example for handler repository using polling: a signalR event is send to client when something change in the Plane Table.
    /// </summary>
    public class AirportHandlerRepository : DatabaseHandlerRepository<AirportHandlerRepository>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AirportHandlerRepository"/> class.
        /// Constructor Set the trigger request.
        /// </summary>
        /// <param name="configuration">the project configuration.</param>
        /// <param name="clientForHubService">the client hub service.</param>
        /// <param name="serviceProvider">the service provider.</param>
        public AirportHandlerRepository(IConfiguration configuration, IServiceProvider serviceProvider)
            : base(
                  serviceProvider,
                  configuration.GetConnectionString("BIADemoDatabase"),
                  configuration.GetDBEngine("BIADemoDatabase"),
                  "SELECT RowVersion FROM [dbo].[Airports]",
                  "SELECT TOP (1) [Id] FROM [dbo].[Airports] ORDER BY [RowVersion] DESC",
                  // We use polling here instead of default Sql broker handler
                  usePolling: true,
                  pollingInterval: TimeSpan.FromSeconds(1))
        {
            this.OnChange += this.AirportChange;
        }

        /// <summary>
        /// React to airport changed.
        /// </summary>
        /// <param name="reader">the reader use to retrieve info send by the trigger.</param>
        public void AirportChange(DbDataReader reader)
        {
            if (reader != null)
            {
                int airportId = reader.GetInt32(0);
            }
        }
    }
}
