// <copyright file="BiaTeamParentConfig.cs" company="BIA">
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
    /// Structure to store config of a parent team.
    /// </summary>
    /// <typeparam name="TTeam">The team type.</typeparam>
    public class BiaTeamParentConfig<TTeam>
    {
        /// <summary>
        /// Team type id.
        /// </summary>
        public int TeamTypeId { get; set; }

        /// <summary>
        /// Get Parent.
        /// </summary>
        public Expression<Func<TTeam, TTeam>> GetParent { get; set; }
    }
}
