// <copyright file="UserDefaultTeam.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.User.Entities
{
    using BIA.Net.Core.Domain;

    /// <summary>
    /// The user team default entity.
    /// </summary>
    public class UserDefaultTeam : VersionedTable, IEntity<int>
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        public int Id { get; set; }

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
    }
}
