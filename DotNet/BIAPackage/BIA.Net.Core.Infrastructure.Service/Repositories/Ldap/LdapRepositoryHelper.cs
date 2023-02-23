using BIA.Net.Core.Common.Configuration;
using BIA.Net.Core.Domain.RepoContract;
using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Ldap
{
    public class LdapRepositoryHelper : ILdapRepositoryHelper
    {
        /// <summary>
        /// The cache for groups
        /// </summary>
        public IBIADistributedCache distributedCache { get; set; }

        /// <summary>
        /// The cache for user
        /// </summary>
        public IBIALocalCache localCache { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericLdapRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public LdapRepositoryHelper(IBIALocalCache localCache, IBIADistributedCache distributedCache)
        {
            // Here we keep usage of new (and not injection) because this is a cache only for LdapRepositoryHelper and not share we other classes
            this.distributedCache = distributedCache;
            this.localCache = localCache;
        }
    }
}
