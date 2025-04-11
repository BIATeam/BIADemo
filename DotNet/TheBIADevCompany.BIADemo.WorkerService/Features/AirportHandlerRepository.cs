// BIADemo only
// <copyright file="AirportHandlerRepository.cs" company="TheBIADevCompany">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService.Features
{
    using System;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Example for handler repository using polling each seconds.
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
                  configuration.GetDatabaseConnectionString("ProjectDatabase"),
                  configuration.GetDBEngine("ProjectDatabase"),
                  "SELECT Id, Name, City FROM [dbo].[Airports]",
                  "Id",
                  pollingInterval: TimeSpan.FromSeconds(1))
        {
        }

        /// <inheritdoc/>
        protected override Task OnChange(DataBaseHandlerChangedData changedData)
        {
            this.Logger.LogInformation("Airport changed : {ChangeType}", changedData.ChangeType);
            return Task.CompletedTask;
        }
    }
}
