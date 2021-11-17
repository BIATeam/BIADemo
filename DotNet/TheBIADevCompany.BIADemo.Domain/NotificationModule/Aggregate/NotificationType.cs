// <copyright file="NotificationType.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate
{
    using BIA.Net.Core.Domain;
    using System.Collections.Generic;
    using TheBIADevCompany.BIADemo.Domain.TranslationModule.Aggregate;

    /// <summary>
    /// The NotificationType entity.
    /// </summary>
    public class NotificationType : VersionedTable, IEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

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