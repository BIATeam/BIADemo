// <copyright file="Permission.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
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

        /// <summary>
        /// Gets or sets if the role appear in light token.
        /// </summary>
        public bool LightToken { get; set; }

        /// <summary>
        /// Indicate if the permissions for all teams independant to current team should be included in token.
        /// </summary>
        public bool IsTransversal { get; set; }

        /// <summary>
        /// Gets or sets a collection of source emitter where the permissions are applicable.
        /// </summary>
        public IEnumerable<string> ApplicableSources { get; set; }
    }
}