// <copyright file="ILdapRepositoryHelper.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    /// <summary>
    /// ILdapRepositoryHelper.
    /// </summary>
    public interface ILdapRepositoryHelper
    {
        /// <summary>
        /// Test if Is Local server is connected on a domain.
        /// </summary>
        /// <param name="domain">The connected domain.</param>
        /// <returns>True if server is on a domain.</returns>
        public bool IsLocalServerOnADomain(out string domain);

        /// <summary>
        /// Test if Is Local Machine domain.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="isConfig">Specify if domain is from config.</param>
        /// <returns>True if it is a local domain.</returns>
        public bool IsLocalMachineName(string domain, bool isConfig);

        /// <summary>
        /// Test if Is server domain.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="isConfig">Specify if domain is from config.</param>
        /// <returns>True if it is the server domain.</returns>
        public bool IsServerDomain(string domain, bool isConfig);

        /// <summary>
        /// Converts domain name to net bios name (=shortname).
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        /// <returns>the netbios name.</returns>
        public string ConvertToNetBiosName(string domainName);
    }
}
