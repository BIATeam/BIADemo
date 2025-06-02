// <copyright file="Notification.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Notification.Entities
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Entity;
    using BIA.Net.Core.Domain.Translation.Entities;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// The Notification entity.
    /// </summary>
    public class Notification : BaseEntityVersioned<int>
    {
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
        public int? CreatedById { get; set; }

        /// <summary>
        /// Gets or sets the user who triggered the notification.
        /// </summary>
        public virtual BaseUser CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the teams to be notified, if any.
        /// </summary>
        public ICollection<NotificationTeam> NotifiedTeams { get; set; }

        /// <summary>
        /// Gets or sets the list of users to be notified.
        /// </summary>
        public ICollection<NotificationUser> NotifiedUsers { get; set; }

        /// <summary>
        /// Gets or sets the route to load on notification click and potentialy other datas. It should be store at camelCase format.
        /// </summary>
        public string JData { get; set; }

        /// <summary>
        /// Gets or sets the notification translations.
        /// </summary>
        public virtual ICollection<NotificationTranslation> NotificationTranslations { get; set; }
    }
}