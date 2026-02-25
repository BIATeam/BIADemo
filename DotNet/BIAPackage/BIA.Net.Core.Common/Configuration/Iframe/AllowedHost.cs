// <copyright file="AllowedHost.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Configuration.Iframe
{
    /// <summary>
    /// Represents an authorized host that is allowed to communicate with this iframe.
    /// </summary>
    public class AllowedHost
    {
        /// <summary>
        /// Gets or sets the Label.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the code culture.
        /// </summary>
        public string Url { get; set; }
    }
}