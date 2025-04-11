// <copyright file="WorkerFeatures.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common.Configuration.WorkerFeature
{
    /// <summary>
    /// WorkerFeatures.
    /// </summary>
    public class WorkerFeatures
    {
        /// <summary>
        /// Gets or sets the DatabaseHandler feature configuration.
        /// </summary>
        public DatabaseHandlerConfiguration DatabaseHandler { get; set; }

        /// <summary>
        /// Gets or sets the Hangfire Server feature configuration.
        /// </summary>
        public HangfireServerConfiguration HangfireServer { get; set; }

        /// <summary>
        /// Gets or sets the archive configuration.
        /// </summary>
        public ArchiveConfiguration Archive { get; set; }
    }
}
