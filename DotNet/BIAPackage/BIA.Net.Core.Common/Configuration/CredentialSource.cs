// <copyright file="CredentialSource.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    /// <summary>
    /// Credential Source.
    /// </summary>
    public class CredentialSource
    {
        /// <summary>
        /// Gets or sets the vault credentials key.
        /// </summary>
        public string VaultCredentialsKey { get; set; }

        /// <summary>
        /// Gets or sets the env login key.
        /// </summary>
        public string EnvLoginKey { get; set; }

        /// <summary>
        /// Gets or sets the env password key.
        /// </summary>
        public string EnvPasswordKey { get; set; }
    }
}
