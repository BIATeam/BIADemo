// <copyright file="Api.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration.Keycloak
{
    /// <summary>
    /// Keycloak Api.
    /// </summary>
    public class Api
    {
        /// <summary>
        /// Gets or sets the token conf.
        /// </summary>
        public TokenConf TokenConf { get; set; }

        /// <summary>
        /// Gets or sets the search user URL.
        /// </summary>
        public string SearchUserRelativeUrl { get; set; }
    }
}
