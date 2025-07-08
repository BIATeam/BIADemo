// <copyright file="HangfireServerConfiguration.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common.Configuration.WorkerFeature
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Hangfire Server Configuration.
    /// </summary>
    public class HangfireServerConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HangfireServerConfiguration"/> class.
        /// </summary>
        public HangfireServerConfiguration()
        {
            this.IsActive = false;
            this.SucceededTasksRetentionDays = 7;
        }

        /// <summary>
        /// Boolean to activate the feature DatabaseHandler.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Boolean to render visible logs in dashboard.
        /// </summary>
        public bool LogsVisibleInDashboard { get; set; }

        /// <summary>
        /// Hangfire name of the server.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Connexion string name for the database.
        /// </summary>
        public string ConnectionStringName { get; set; }

        /// <summary>
        /// Number of days to keep succeeded tasks.
        /// </summary>
        public int SucceededTasksRetentionDays { get; set; }
    }
}
