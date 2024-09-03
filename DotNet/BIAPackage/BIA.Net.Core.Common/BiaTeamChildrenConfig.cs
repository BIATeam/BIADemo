// <copyright file="BiaTeamChildrenConfig.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

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
