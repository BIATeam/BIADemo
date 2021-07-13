using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Common.Configuration.ApiFeature
{
    /// <summary>
    /// Api Feature delegate job to worker using Hangfire, configuration.
    /// </summary>
    public class DelegateJobToWorker
    {
        /// <summary>
        /// Boolean to activate the feature HubForClients.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Connexion string name for the database.
        /// </summary>
        public string ConnectionStringName { get; set; }
    }
}
