// <copyright file="Authentication.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    /// <summary>
    /// The general security configuration.
    /// </summary>
    public class Security
    {
        /// <summary>
        /// The Audience of the token (url of the front application).
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [disable TLS verify].
        /// </summary>
        public bool DisableTlsVerify { get; set; }
    }
}