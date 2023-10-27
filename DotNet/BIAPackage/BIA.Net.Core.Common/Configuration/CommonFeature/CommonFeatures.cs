// <copyright file="CommonFeatures.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common.Configuration.CommonFeature
{
    public class CommonFeatures
    {
        /// <summary>
        /// Gets or sets the Distributed Cache feature configuration.
        /// </summary>
        public DistributedCacheConfiguration DistributedCache { get; set; }

        /// <summary>
        /// Gets or sets the HubForClients feature configuration.
        /// </summary>
        public ClientForHubConfiguration ClientForHub { get; set; }

        /// <summary>
        /// Gets or sets the audit configuration.
        /// </summary>
        public AuditConfiguration AuditConfiguration { get; set; }
    }
}
