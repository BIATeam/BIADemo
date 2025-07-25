// <copyright file="NotificationTranslation.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Translation.Entities
{
    using BIA.Net.Core.Domain.Entity;
    using BIA.Net.Core.Domain.Notification.Entities;

    /// <summary>
    /// The role entity.
    /// </summary>
    public class NotificationTranslation : BaseEntityVersioned<int>
    {
        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        public Language Language { get; set; }

        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        ///  Gets or sets the notification type.
        /// </summary>
        public Notification Notification { get; set; }

        /// <summary>
        /// Gets or sets the notification type id.
        /// </summary>
        public int NotificationId { get; set; }

        /// <summary>
        /// Gets or sets the title translated.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description translated.
        /// </summary>
        public string Description { get; set; }
    }
}