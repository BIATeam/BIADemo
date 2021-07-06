// <copyright file="Authentication.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    /// <summary>
    /// The authentication configuration.
    /// </summary>
    public class Jwt
    {
        /// <summary>
        /// The jwt issuer Name (name of the Api application).
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// The Audience of the token (url of the Angular application).
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// The Secret Key to crypte the token.
        /// </summary>
        public string SecretKey { get; set; }        
    }
}