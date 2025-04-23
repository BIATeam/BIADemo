// <copyright file="BiaTeamConfig.cs" company="BIA">
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
    /// Structure to store config of a team.
    /// </summary>
    /// <typeparam name="TTeam">The team type.</typeparam>
    public class BiaTeamConfig<TTeam>
    {
        /// <summary>
        /// Team type Id.
        /// </summary>
        public int TeamTypeId { get; set; }

        /// <summary>
        /// The prefixe to use for dynamicaly manage right.
        /// </summary>
        public string RightPrefix { get; set; }

        /// <summary>
        /// The chilren teams type.
        /// </summary>
        public ImmutableList<BiaTeamChildrenConfig<TTeam>> Children { get; set; }

        /// <summary>
        /// The chilren teams type.
        /// </summary>
        public ImmutableList<BiaTeamParentConfig<TTeam>> Parents { get; set; }

        /// <summary>
        /// The id of roles that can administrate the team.
        /// </summary>
        public int[] AdminRoleIds { get; set; }

        /// <summary>
        /// The id of roles that can administrate the team that come from parent or root level.
        /// </summary>
        public int[] ParentsAdminRoleIds { get; set; }
    }
}
