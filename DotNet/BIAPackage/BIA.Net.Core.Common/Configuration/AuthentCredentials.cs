// <copyright file="AuthentCredentials.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    /// <summary>
    /// The authentication configuration.
    /// </summary>
    public class AuthentCredentials
    {
        /// <summary>
        /// Username for the authentication.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password for the authentication.
        /// </summary>
        public string Password { get; set; }
    }
}