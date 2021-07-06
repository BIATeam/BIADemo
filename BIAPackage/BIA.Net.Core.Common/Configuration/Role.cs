// <copyright file="Roles.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace BIA.Net.Core.Common.Configuration
{
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
        /// Gets or sets the type of the role (Fake or Ldap).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the value of the role.
        /// </summary>
        public IEnumerable<LdapGroup> LdapGroups { get; set; }
    }
}