// <copyright file="DelegateJobToWorker.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration.ApiFeature
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Api Feature delegate job to worker using Hangfire, configuration.
    /// </summary>
    public class DelegateJobToWorker
    {
        /// <summary>
        /// Boolean to activate the feature DelegateJobToWorker.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Connexion string name for the database.
        /// </summary>
        public string ConnectionStringName { get; set; }

        /// <summary>
        /// Url for the dashboard Monitoring.
        /// </summary>
        public string MonitoringUrl { get; set; }
    }
}
