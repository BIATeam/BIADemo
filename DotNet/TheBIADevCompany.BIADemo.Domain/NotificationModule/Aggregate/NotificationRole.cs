// <copyright file="NotificationUser.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate
{
    using System;
    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The NotificationUser entity.
    /// </summary>
    public class NotificationRole : VersionedTable
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
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public virtual Role Role { get; set; }
    }
}