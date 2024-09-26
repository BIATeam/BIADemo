// <copyright file="RequireClaim.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// RequireClaim.
    /// </summary>
    public class RequireClaim
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the allowed values.
        /// </summary>
        public List<string> AllowedValues { get; set; }
    }
}
