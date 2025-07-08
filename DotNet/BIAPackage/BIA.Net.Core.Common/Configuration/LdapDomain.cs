// <copyright file="LdapDomain.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common.Configuration
{
    /// <summary>
    /// LdapDomain.
    /// </summary>
    public class LdapDomain
    {
        /// <summary>
        /// Short name of the domain.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Full name of the domain.
        /// </summary>
        public string LdapName { get; set; }

        /// <summary>
        /// Filter to accelerate search. (ex: OU=Ingegneria,DC=xxx,DC=xxx).
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// User service account name.
        /// </summary>
        public string LdapServiceAccount { get; set; }

        /// <summary>
        /// User service account pass.
        /// </summary>
        public string LdapServicePass { get; set; }

        /// <summary>
        /// CredentialKeyInWindowsVault.
        /// </summary>
        public string CredentialKeyInWindowsVault { get; set; }

        /// <summary>
        /// Specify if this domain contains groups used by the application.
        /// </summary>
        public bool ContainsGroup { get; set; }

        /// <summary>
        /// Specify if this domain contains user used by the application.
        /// </summary>
        public bool ContainsUser { get; set; }
    }
}
