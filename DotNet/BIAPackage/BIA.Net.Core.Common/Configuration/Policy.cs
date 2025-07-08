// <copyright file="Policy.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// Policy.
    /// </summary>
    public class Policy
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the require claims.
        /// </summary>
        public IEnumerable<RequireClaim> RequireClaims { get; set; }
    }
}
