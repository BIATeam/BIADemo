// <copyright file="ClientForHubConfiguration.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common.Configuration.CommonFeature
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ClientForHubConfiguration
    {
        /// <summary>
        /// Boolean to activate the feature HubForClients.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Url to join the SignalR Hub.
        /// </summary>
        public string SignalRUrl { get; set; }
    }
}
