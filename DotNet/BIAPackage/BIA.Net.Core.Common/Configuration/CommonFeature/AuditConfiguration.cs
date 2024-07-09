// <copyright file="AuditConfiguration.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common.Configuration.CommonFeature
{
    /// <summary>
    /// AuditConfiguration.
    /// </summary>
    public class AuditConfiguration
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
