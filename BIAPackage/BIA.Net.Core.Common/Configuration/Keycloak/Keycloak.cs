// <copyright file="Keycloak.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration.Keycloak
{
    /// <summary>
    /// Keycloak.
    /// </summary>
    public class Keycloak
    {
        /// <summary>
        /// Gets or sets a value indicating whether keycloak is used.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        public Configuration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the API conf.
        /// </summary>
        public Api Api { get; set; }
    }
}
