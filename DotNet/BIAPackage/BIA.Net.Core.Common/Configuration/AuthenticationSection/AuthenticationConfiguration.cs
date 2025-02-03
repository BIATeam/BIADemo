// <copyright file="AuthenticationConfiguration.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common.Configuration.AuthenticationSection
{
    /// <summary>
    /// AuthenticationConfiguration.
    /// </summary>
    public class AuthenticationConfiguration
    {
        /// <summary>
        /// Authentication mode for access to ProfileImageUrl.
        /// </summary>
        public AuthenticationMode Mode { get; set; }

        /// <summary>
        /// Authentication credentials used to access profile image when in AuthentMode Standard = 1.
        /// </summary>
        public CredentialSource CredentialSource { get; set; }

        /// <summary>
        /// Api key used to access profile image when in AuthenMode ApiKey = 3.
        /// </summary>
        public string ApiKey { get; set; }
    }
}
