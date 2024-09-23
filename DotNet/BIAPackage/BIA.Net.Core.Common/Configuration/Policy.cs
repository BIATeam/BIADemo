// <copyright file="Policy.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
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
        /// Gets or sets the require claim.
        /// </summary>
        public RequireClaim RequireClaim { get; set; }
    }
}
