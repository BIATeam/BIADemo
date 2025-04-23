// <copyright file="DistributedCacheConfiguration.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration.CommonFeature
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Api Feature Hub for Client Coniguration.
    /// </summary>
    public class DistributedCacheConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DistributedCacheConfiguration"/> class.
        /// </summary>
        public DistributedCacheConfiguration()
        {
            this.IsActive = false;
        }

        /// <summary>
        /// Boolean to activate the feature HubForClients.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Connexion string name for the database.
        /// </summary>
        public string ConnectionStringName { get; set; }

        /// <summary>
        /// Activates the specified connection string name.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public void Activate(string connectionStringName)
        {
            this.IsActive = true;
            this.ConnectionStringName = connectionStringName;
        }
    }
}
