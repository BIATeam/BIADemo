// <copyright file="SidResolvedGroup.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Ldap
{
    using System;
    using System.Collections.Generic;

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
