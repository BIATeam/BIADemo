// <copyright file="BaseEntityTeam.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Entities
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Entity;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.Notification.Entities;
    using BIA.Net.Core.Domain.View.Entities;

    /// <summary>
    /// The team.
    /// </summary>
    public class BaseEntityTeam : BaseEntityVersioned<int>, IEntityTeam
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the team type.
        /// </summary>
        public virtual TeamType TeamType { get; set; }

        /// <summary>
        /// Gets or sets the team type id.
        /// </summary>
        public int TeamTypeId { get; set; }

        /// <summary>
        /// Gets or sets the members.
        /// </summary>
        public virtual ICollection<Member> Members { get; set; }

        /// <summary>
        /// Gets or sets the collection of view site.
        /// </summary>
        public ICollection<ViewTeam> ViewTeams { get; set; }

        /// <summary>
        /// Gets or sets the collection of notification teams.
        /// </summary>
        public ICollection<NotificationTeam> NotificationTeams { get; set; }

        /// <summary>
        /// Gets or sets the collection of users that have the team as default.
        /// </summary>
        public ICollection<UserDefaultTeam> UserDefaultTeams { get; set; }
    }
}