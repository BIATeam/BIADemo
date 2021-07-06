// <copyright file="Permission.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// The permission configuration.
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// Gets or sets the name of the permission.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a list of permission names.
        /// </summary>
        public string[] Names { get; set; }

        /// <summary>
        /// Gets or sets the roles associated to the permission.
        /// </summary>
        public IEnumerable<string> Roles { get; set; }
    }
}