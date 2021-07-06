// <copyright file="Authentication.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace BIA.Net.Core.Common.Configuration
{
    /// <summary>
    /// The authentication configuration.
    /// </summary>
    public class Authentication
    {
        /// <summary>
        /// The Ldap domain.
        /// </summary>
        public IEnumerable<LdapDomain> LdapDomains { get; set; }

        /// <summary>
        /// Duration of the cache for ldap Group Member List in ldap.
        /// </summary>
        public int LdapCacheGroupDuration { get; set; }

        /// <summary>
        /// Duration of the cache for user property in ldap.
        /// </summary>
        public int LdapCacheUserDuration { get; set; }
    }
}