using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Ldap
{
    [Serializable]
    public class SidResolvedGroup : SidResolvedItem
    {
        public List<string> MembersGroupSid;
        public List<string> MembersUserSid;
    }
}
