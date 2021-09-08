// BIADemo only
// <copyright file="NotificationHandlerRepository.cs" company="TheBIADevCompany">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService.Features
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    /// <summary>
    /// Example for handler repository: a signalR event is send to client when something change in the Notification Table.
    /// </summary>
    public class NotificationHandlerRepository : DatabaseHandlerRepository
    {
        private static IClientForHubRepository clientForHubService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationHandlerRepository"/> class.
        /// Constructor Set the trigger request.
        /// </summary>
        /// <param name="configuration">the project configuration.</param>
        public NotificationHandlerRepository(IConfiguration configuration)
            : base(
            configuration.GetConnectionString("BIADemoDatabase"),
            "SELECT RowVersion FROM [dbo].[Notification]",
            "SELECT TOP 1 [Id], [Title], [Description], (SELECT COUNT(*) FROM dbo.Notification WHERE Notification.[Read] = 0) as UnreadCount FROM [dbo].[Notification] ORDER BY [RowVersion] DESC",
            r => NotificationChange(r),
            new List<SqlNotificationInfo>() { SqlNotificationInfo.Insert })
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
        /// <param name="reader">the reader use to retrieve info send by the trigger.</param>
        public static void NotificationChange(SqlDataReader reader)
        {
            if (clientForHubService == null)
            {
                throw new ConfigurationErrorsException("The ClientForHub feature is not configure before use NotificationChange. Verify your correctly configure NotificationHandlerRepository in Statup.cs.");
            }

            var notification = new
            {
                id = reader.GetInt32(0),
                title = reader.GetString(1),
                description = reader.GetString(2),
                unreadCount = reader.GetInt32(3),
            };

            /* To read information use: int id = reader.GetInt32(0)  */
            _ = clientForHubService.SendMessage("notification-sent", JsonConvert.SerializeObject(notification));
        }
    }
}
