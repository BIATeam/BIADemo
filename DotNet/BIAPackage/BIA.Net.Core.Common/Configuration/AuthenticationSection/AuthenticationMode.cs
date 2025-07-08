// <copyright file="AuthenticationMode.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration.AuthenticationSection
{
    /// <summary>
    /// The authentication configuration.
    /// </summary>
    public enum AuthenticationMode
    {
        /// <summary>
        /// Default authentication (windows).
        /// </summary>
        Default,

        /// <summary>
        /// Standard authentication (username and password).
        /// </summary>
        Standard,

        /// <summary>
        /// Token authentication using a bearer token.
        /// </summary>
        Token,

        /// <summary>
        /// ApiKey authentication
        /// </summary>
        ApiKey,

        /// <summary>
        /// Anonymous authentication
        /// </summary>
        Anonymous,
    }
}