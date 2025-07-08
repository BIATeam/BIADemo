// <copyright file="BaseEntityTeamFixable.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Entities
{
    using System;
    using BIA.Net.Core.Domain.Entity.Interface;

    /// <summary>
    /// The team.
    /// </summary>
    public class BaseEntityTeamFixable : BaseEntityTeam, IEntityFixable
    {
        /// <summary>
        /// Gets or sets the is fixed.
        /// </summary>
        public bool IsFixed { get; set; }

        /// <summary>
        /// Gets or sets the fixed date.
        /// </summary>
        public DateTime? FixedDate { get; set; }
    }
}