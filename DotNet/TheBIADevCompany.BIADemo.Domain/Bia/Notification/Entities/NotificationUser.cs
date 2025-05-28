// <copyright file="NotificationUser.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Bia.Notification.Entities
{
    using System;
    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Domain.Bia.User.Entities;
    using TheBIADevCompany.BIADemo.Domain.Notification.Entities;

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