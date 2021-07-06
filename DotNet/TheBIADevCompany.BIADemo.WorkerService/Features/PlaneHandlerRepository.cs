// BIADemo only
// <copyright file="PlaneHandlerRepository.cs" company="TheBIADevCompany">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService.Features
{
    using System.Data.SqlClient;
    using BIA.Net.Core.WorkerService.Features.ClientForHub;
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Example for handler repository: a signalR event is send to client when something change in the Plane Table.
    /// </summary>
    public class PlaneHandlerRepository : DatabaseHandlerRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneHandlerRepository"/> class.
        /// Constructor Set the trigger request.
        /// </summary>
        /// <param name="configuration">the project configuration.</param>
        public PlaneHandlerRepository(IConfiguration configuration)
            : base(
            configuration.GetConnectionString("BIADemoDatabase"),
            "SELECT RowVersion FROM [dbo].[Planes]",
            string.Empty /* Information to pass to the reader in Plnane Change:  "SELECT TOP (1) [Id] FROM [dbo].[Planes] ORDER BY [RowVersion] DESC"*/,
            r => PlaneChange(r))
            {
            }

        /// <summary>
        /// Send message to the clients.
        /// </summary>
        /// <param name="reader">the reader use to retrieve info send by th trigger.</param>
        public static void PlaneChange(SqlDataReader reader)
        {
            /* To read information use: int id = reader.GetInt32(0)  */
            _ = ClientForHubService.SendMessage("refresh-planes", string.Empty);
        }
    }
}
