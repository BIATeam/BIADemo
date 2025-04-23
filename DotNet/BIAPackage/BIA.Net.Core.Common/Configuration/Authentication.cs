// <copyright file="Authentication.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// The authentication configuration.
    /// </summary>
    public class Authentication
    {
        /// <summary>
        /// Gets or sets the keycloak configuration.
        /// </summary>
        public Keycloak.Keycloak Keycloak { get; set; }

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