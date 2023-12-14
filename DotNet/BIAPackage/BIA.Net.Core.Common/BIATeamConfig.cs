// <copyright file="BIATeamConfig.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq.Expressions;

    public class BIATeamChildrenConfig<TTeamTypeIdEnum, TTeam>
    {
        public TTeamTypeIdEnum TypeId { get; set; }

        public Expression<Func<TTeam, IEnumerable<TTeam>>> GetChilds { get; set; }
    }

    public class BIATeamParentConfig<TTeamTypeIdEnum, TTeam>
    {
        public TTeamTypeIdEnum TypeId { get; set; }

        public Expression<Func<TTeam, TTeam>> GetParent { get; set; }
    }

    /// <summary>
    /// Structur to store config of a team.
    /// </summary>
    /// <typeparam name="TTeamTypeIdEnum">The enum for team type ids.</typeparam>
    public class BIATeamConfig<TTeamTypeIdEnum, TTeam>
        where TTeamTypeIdEnum : System.Enum
    {
        /// <summary>
        /// The prefixe to use for dynamicaly manage right.
        /// </summary>
        public string RightPrefix { get; set; }

        /// <summary>
        /// The chilren teams type.
        /// </summary>
        public ImmutableList<BIATeamChildrenConfig<TTeamTypeIdEnum, TTeam>> Children { get; set; }

        /// <summary>
        /// The chilren teams type.
        /// </summary>
        public ImmutableList<BIATeamParentConfig<TTeamTypeIdEnum, TTeam>> Parents { get; set; }
    }
}
