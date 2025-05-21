// <copyright file="NotificationTeam.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Notification.Entities
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Entity;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

    /// <summary>
    /// The NotificationTeam entity.
    /// </summary>
    public class NotificationTeam : BaseEntityVersioned<int>
    {
        /// <summary>
        /// Gets or sets the notification identifier.
        /// </summary>
        public int NotificationId { get; set; }

        /// <summary>
        /// Gets or sets the notification.
        /// </summary>
        public virtual Notification Notification { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the team.
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// Gets or sets the team.
        /// </summary>
        public virtual Team Team { get; set; }

        /// <summary>
        /// Gets or sets the specific roles to target.
        /// </summary>
        public ICollection<NotificationTeamRole> Roles { get; set; }
    }
}