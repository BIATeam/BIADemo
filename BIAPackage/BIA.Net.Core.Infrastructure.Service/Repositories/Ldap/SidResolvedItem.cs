using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Ldap
{
    public enum SidResolvedItemType
    {
        User = 1,
        Group = 2,
    }

    [Serializable]
    public class SidResolvedItem
    {
        public string domainKey;
        public SidResolvedItemType type;
    }
}
