using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Common.Configuration
{
    public class EnvironmentConfiguration
    {
        /// <summary>
        /// Duration of the cache for ldap Group Member List in ldap.
        /// </summary>
        public string Type { get; set; }

        public string UrlMatomo { get; set; }
    }
}
