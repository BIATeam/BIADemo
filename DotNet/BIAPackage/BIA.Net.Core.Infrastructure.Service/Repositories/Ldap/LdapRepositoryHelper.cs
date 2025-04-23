// <copyright file="LdapRepositoryHelper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Infrastructure.Service.Repositories.Ldap
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;

    /// <summary>
    /// Ldap Repository Helper.
    /// </summary>
    public class LdapRepositoryHelper : ILdapRepositoryHelper
    {
        private readonly Dictionary<string, string> netBiosName = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LdapRepositoryHelper"/> class.
        /// </summary>
        /// <param name="localCache">The local cache.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        public LdapRepositoryHelper(IBiaLocalCache localCache, IBiaDistributedCache distributedCache)
        {
            // Here we keep usage of new (and not injection) because this is a cache only for LdapRepositoryHelper and not share we other classes
            this.DistributedCache = distributedCache;
            this.LocalCache = localCache;
        }

        /// <summary>
        /// The cache for groups.
        /// </summary>
        public IBiaDistributedCache DistributedCache { get; set; }

        /// <summary>
        /// The cache for user.
        /// </summary>
        public IBiaLocalCache LocalCache { get; set; }

        /// <summary>
        /// Test if Is Local server is connected on a domain.
        /// </summary>
        /// <param name="localLdapName">The connected domain.</param>
        /// <returns>True if server is on a domain.</returns>
        public bool IsLocalServerOnADomain(out string localLdapName)
        {
            try
            {
                localLdapName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            }
            catch (Exception)
            {
                localLdapName = null;
                return false;
            }

            return !string.IsNullOrEmpty(localLdapName);
        }

        /// <summary>
        /// Test if Is Local Machine domain.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="isConfig">Specify if domain is from config.</param>
        /// <returns>True if it is a local domain.</returns>
        public bool IsLocalMachineName(string domain, bool isConfig)
        {
            return (isConfig && domain == ".") || domain.ToLower() == Environment.MachineName.ToLower();
        }

        /// <summary>
        /// Test if Is server domain.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="isConfig">Specify if domain is from config.</param>
        /// <returns>True if it is the server domain.</returns>
        public bool IsServerDomain(string domain, bool isConfig)
        {
            if (isConfig && domain == ".")
            {
                return true;
            }

            var domainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            string netBiosDomainName = this.ConvertToNetBiosName(domainName);

            return domain.ToLower() == netBiosDomainName.ToLower();
        }

        /// <summary>
        /// Converts domain name to net bios name (=shortname).
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        /// <returns>the netbios name.</returns>
        public string ConvertToNetBiosName(string domainName)
        {
            string netBiosDomainName;
            if (this.netBiosName.ContainsKey(domainName))
            {
                netBiosDomainName = this.netBiosName[domainName];
            }
            else
            {
                netBiosDomainName = NativeMethods.GetNetbiosNameForDomain(domainName);
                this.netBiosName.Add(domainName, netBiosDomainName);
            }

            return netBiosDomainName;
        }
    }
}
