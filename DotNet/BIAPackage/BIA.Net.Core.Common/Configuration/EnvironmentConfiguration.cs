// <copyright file="EnvironmentConfiguration.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common.Configuration
{
    /// <summary>
    /// EnvironmentConfiguration.
    /// </summary>
    public class EnvironmentConfiguration
    {
        /// <summary>
        /// Duration of the cache for ldap Group Member List in ldap.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the URL matomo.
        /// </summary>
        /// <value>
        /// The URL matomo.
        /// </value>
        public string UrlMatomo { get; set; }

        /// <summary>
        /// Gets or sets the site identifier matomo.
        /// </summary>
        /// <value>
        /// The site identifier matomo.
        /// </value>
        public string SiteIdMatomo { get; set; }

        /// <summary>
        /// Gets or sets the urls additional js.
        /// </summary>
        /// <value>
        /// The urls additional js.
        /// </value>
        public string[] UrlsAdditionalJS { get; set; }

        /// <summary>
        /// Gets or sets the urls additional CSS.
        /// </summary>
        /// <value>
        /// The urls additional CSS.
        /// </value>
        public string[] UrlsAdditionalCSS { get; set; }

        /// <summary>
        /// Gets or sets the Android marker environment.
        /// </summary>
        public bool Android { get; set; }
    }
}
