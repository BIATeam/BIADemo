// <copyright file="SidResolvedGroup.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Ldap
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Sid Resolved Group.
    /// </summary>
    [Serializable]
    public class SidResolvedGroup : SidResolvedItem
    {
        /// <summary>
        /// Members Group Sid.
        /// </summary>
        public List<GroupDomainSid> MembersGroupSid { get; set; }

        /// <summary>
        /// Members User Sid.
        /// </summary>
        public List<string> MembersUserSid { get; set; }
    }
}
