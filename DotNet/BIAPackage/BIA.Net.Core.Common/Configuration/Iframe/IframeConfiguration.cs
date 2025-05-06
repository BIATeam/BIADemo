// <copyright file="IframeConfiguration.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration.Iframe
{
    using System.Collections.Generic;

    /// <summary>
    /// Configuration when front being displayed inside an Iframe.
    /// </summary>
    public class IframeConfiguration
    {
        /// <summary>
        /// Gets or sets the configuration to allow to keep the front layout while being displayed in a iframe.
        /// </summary>
        public bool KeepLayout { get; set; }

        /// <summary>
        /// Gets or sets the allowed host for iframe communication.
        /// </summary>
        public List<AllowedHost> AllowedIframeHosts { get; set; }
    }
}
