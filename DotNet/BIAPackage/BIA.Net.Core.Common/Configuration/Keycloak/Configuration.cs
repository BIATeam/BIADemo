// <copyright file="Configuration.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration.Keycloak
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Keycloak Configuration.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Gets or sets IdpHint. Used to tell Keycloak which IDP the user wants to authenticate with.
        /// </summary>
        public string IdpHint { get; set; }

        /// <summary>
        /// Gets or sets the Realm.
        /// </summary>
        public string Realm { get; set; }

        /// <summary>
        /// Gets or sets the authority.
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [require HTTPS metadata].
        /// </summary>
        public bool RequireHttpsMetadata { get; set; }

        /// <summary>
        /// Gets or sets the valid audience.
        /// </summary>
        public string ValidAudience { get; set; }

        /// <summary>
        /// Gets or sets the name of the cert file.
        /// </summary>
        [JsonIgnore]
        public string CertFileName { get; set; }
    }
}
