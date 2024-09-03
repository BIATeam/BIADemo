// <copyright file="LdapRepositoryHelper.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Infrastructure.Service.Repositories.Ldap
{
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;

    /// <summary>
    /// Ldap Repository Helper.
    /// </summary>
    public class LdapRepositoryHelper : ILdapRepositoryHelper
    {
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
    }
}
