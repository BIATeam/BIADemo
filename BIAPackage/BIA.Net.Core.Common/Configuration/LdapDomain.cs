using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Common.Configuration
{
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
        /// Specify if this domain contains groups used by the application
        /// </summary>
        public bool ContainsGroup { get; set; }
        /// <summary>
        /// Specify if this domain contains user used by the application
        /// </summary>
        public bool ContainsUser { get; set; }

    }
}
