// <copyright file="EnvironmentConfiguration.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class EnvironmentConfiguration
    {
        /// <summary>
        /// Duration of the cache for ldap Group Member List in ldap.
        /// </summary>
        public string Type { get; set; }

        public string UrlMatomo { get; set; }

        public string SiteIdMatomo { get; set; }

        public string[] UrlsAdditionalJS { get; set; }

        public string[] UrlsAdditionalCSS { get; set; }
    }
}
