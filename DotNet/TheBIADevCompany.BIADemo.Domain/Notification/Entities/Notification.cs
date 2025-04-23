// <copyright file="Notification.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Notification.Entities
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Domain.Translation.Entities;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

    /// <summary>
    /// The Notification entity.
    /// </summary>
    public class Notification : VersionedTable, IEntity<int>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

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
        public virtual User CreatedBy { get; set; }

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