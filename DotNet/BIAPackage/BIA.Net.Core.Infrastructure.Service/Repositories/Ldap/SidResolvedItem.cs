﻿// <copyright file="SidResolvedItem.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories.Ldap
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// enum for item type.
    /// </summary>
    public enum SidResolvedItemType
    {
        /// <summary>
        /// type user.
        /// </summary>
        User = 1,

        /// <summary>
        /// type group.
        /// </summary>
        Group = 2,
    }

    /// <summary>
    /// SidResolvedItem.
    /// </summary>
    [Serializable]
    public class SidResolvedItem
    {
        /// <summary>
        /// DomainKey.
        /// </summary>
        public string DomainKey { get; set; }

        /// <summary>
        /// Type.
        /// </summary>
        public SidResolvedItemType Type { get; set; }
    }
}
