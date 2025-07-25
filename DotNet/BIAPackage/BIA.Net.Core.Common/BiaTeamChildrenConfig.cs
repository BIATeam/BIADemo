﻿// <copyright file="BiaTeamChildrenConfig.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Configuration;

    /// <summary>
    /// Structure to store config of children team.
    /// </summary>
    /// <typeparam name="TTeam">The team type.</typeparam>
    public class BiaTeamChildrenConfig<TTeam>
    {
        /// <summary>
        /// Team type id.
        /// </summary>
        public int TeamTypeId { get; set; }

        /// <summary>
        /// Get childs.
        /// </summary>
        public Expression<Func<TTeam, IEnumerable<TTeam>>> GetChilds { get; set; }
    }
}
