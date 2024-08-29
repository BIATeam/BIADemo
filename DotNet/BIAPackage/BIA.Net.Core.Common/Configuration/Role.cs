// <copyright file="Role.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// The roles configuration.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Gets or sets the label of the role.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the type of the role.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the LDAP groups.
        /// </summary>
        public IEnumerable<LdapGroup> LdapGroups { get; set; }

        /// <summary>
        /// Gets or sets the type of the claim.
        /// </summary>
        public string ClaimType { get; set; }

        /// <summary>
        /// Gets or sets the claim values.
        /// </summary>
        public IEnumerable<string> ClaimValues { get; set; }
    }
}