// <copyright file="HangfireServerConfiguration.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
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
        }

        /// <summary>
        /// Boolean to activate the feature DatabaseHandler.
        /// </summary>
        public bool IsActive { get; set; }

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
