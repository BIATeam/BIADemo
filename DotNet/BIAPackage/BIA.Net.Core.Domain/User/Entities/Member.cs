// <copyright file="Member.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Entities
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Entity;

    /// <summary>
    /// The member entity.
    /// </summary>
    public class Member : BaseEntityVersioned<int>
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the team.
        /// </summary>
        public virtual Team Team { get; set; }

        /// <summary>
        /// Gets or sets the team id.
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// Gets or sets the member roles.
        /// </summary>
        public virtual ICollection<MemberRole> MemberRoles { get; set; }
    }
}