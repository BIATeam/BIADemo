// <copyright file="Notification.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The Notification entity.
    /// </summary>
    public class Notification : VersionedTable, IEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Hangfire job identifier.
        /// </summary>
        public string JobId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the notification type identifier.
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// Gets or sets the notification type.
        /// </summary>
        public virtual NotificationType Type { get; set; }

        /// <summary>
        /// Gets or sets whether the notification has been read.
        /// </summary>
        public bool Read { get; set; }

        /// <summary>
        /// Gets or sets the date the notification was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who triggered the notification.
        /// </summary>
        public int CreatedById { get; set; }

        /// <summary>
        /// Gets or sets the user who triggered the notification.
        /// </summary>
        public virtual User CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the role identifier to be notified, if any.
        /// </summary>
        public int? NotifiedRoleId { get; set; }

        /// <summary>
        /// Gets or sets the role to be notified, if any.
        /// </summary>
        public virtual Role NotifiedRole { get; set; }

        /// <summary>
        /// Gets or sets the site identifier.
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public virtual Site Site { get; set; }

        /// <summary>
        /// Gets or sets the list of users to be notified.
        /// </summary>
        public ICollection<NotificationUser> NotificationUsers { get; set; }

        /// <summary>
        /// Gets ot sets the route to load on notification click.
        /// </summary>
        public string TargetRoute { get; set; }

        /// <summary>
        /// Gets or sets the object identifier related to this notification.
        /// </summary>
        public int TargetId { get; set; }
    }
}