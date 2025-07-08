// <copyright file="HangfireDashboardConfiguration.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration.ApiFeature
{
    /// <summary>
    /// HangfireDashboardConfiguration.
    /// </summary>
    public class HangfireDashboardConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HangfireDashboardConfiguration"/> class.
        /// </summary>
        public HangfireDashboardConfiguration()
        {
            this.IsActive = false;
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
        /// Path to log files.
        /// </summary>
        public string LogFiles { get; set; }

        /// <summary>
        /// Boolean to activate the Dashboard admin.
        /// </summary>
        public bool DashboardAdmin { get; set; }

        /// <summary>
        /// Permission to acces Dashboard admin.
        /// </summary>
        public string DashboardAdminPermission { get; set; }

        /// <summary>
        /// Boolean to activate the Dashboard read only.
        /// </summary>
        public bool DashboardReadOnly { get; set; }

        /// <summary>
        /// Permission to acces Dashboard read only.
        /// </summary>
        public string DashboardReadOnlyPermission { get; set; }

        /// <summary>
        /// Hangfire name of the server.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Connexion string name for the database.
        /// </summary>
        public string ConnectionStringName { get; set; }
    }
}
