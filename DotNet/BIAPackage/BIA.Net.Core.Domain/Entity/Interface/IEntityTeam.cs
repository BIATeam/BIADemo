// <copyright file="IEntityTeam.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Entity.Interface
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// The team.
    /// </summary>
    public interface IEntityTeam
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the team type.
        /// </summary>
        public TeamType TeamType { get; set; }

        /// <summary>
        /// Gets or sets the team type id.
        /// </summary>
        public int TeamTypeId { get; set; }

        /// <summary>
        /// Gets or sets the members.
        /// </summary>
        public ICollection<Member> Members { get; set; }
    }
}