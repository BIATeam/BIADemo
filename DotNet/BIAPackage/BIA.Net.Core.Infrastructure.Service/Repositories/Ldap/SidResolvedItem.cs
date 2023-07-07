// <copyright file="SidResolvedItem.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Ldap
{
    using System;
    using System.Collections.Generic;
    using System.Text;

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
