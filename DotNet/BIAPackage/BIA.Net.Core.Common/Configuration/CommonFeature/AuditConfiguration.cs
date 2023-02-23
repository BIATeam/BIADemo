using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Common.Configuration.CommonFeature
{
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
