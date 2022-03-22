// <copyright file="NotificationTeam.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate
{
    using System;
    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The NotificationTeam entity.
    /// </summary>
    public class NotificationTeam : VersionedTable
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
    }
}