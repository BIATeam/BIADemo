// <copyright file="NotificationType.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Notification.Entities
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Entity;
    using BIA.Net.Core.Domain.Translation.Entities;

    /// <summary>
    /// The NotificationType entity.
    /// </summary>
    public class NotificationType : BaseEntityVersioned<int>
    {
        /// <summary>
        /// Gets or sets the notification codes
        /// e.g: Task, Info, Success, Warning, Error.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the notification codes
        /// e.g: Task, Info, Success, Warning, Error.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the notification type translations.
        /// </summary>
        public virtual ICollection<NotificationTypeTranslation> NotificationTypeTranslations { get; set; }
    }
}