using System;
using System.Collections.Generic;
using System.Text;

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Ldap
{
    [Serializable]
    public class GroupDomainSid
    {
        public string Domain { get; set; }
        public string Sid { get; set; }
    }
    [Serializable]
    public class SidResolvedGroup : SidResolvedItem
    {
        public List<GroupDomainSid> MembersGroupSid;
        public List<string> MembersUserSid;
    }
}
