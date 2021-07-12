using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Common.Configuration.CommonFeature
{
    /// <summary>
    /// Api Feature Hub for Client Coniguration.
    /// </summary>
    public class DistributedCacheConfiguration
    {
        /// <summary>
        /// Boolean to activate the feature HubForClients.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Type of database engine.
        /// </summary>
        public string DBEngine { get; set; }

        /// <summary>
        /// Connexion string name for the database.
        /// </summary>
        public string ConnectionStringName { get; set; }

        public DistributedCacheConfiguration()
        {
            this.IsActive = false;
        }

        public void Activate(string connectionStringName, string dbEngine)
        {
            this.IsActive = true;
            this.ConnectionStringName = connectionStringName;
            this.DBEngine = dbEngine;
        }
    }
}
