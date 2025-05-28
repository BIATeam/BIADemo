// <copyright file="NotificationUser.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Notification.Entities
{
    using System;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// The NotificationUser entity.
    /// </summary>
    public class NotificationUser : VersionedTable
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
        /// Gets or sets the identifier of the user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public virtual User User { get; set; }
    }
}