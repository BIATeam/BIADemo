// <copyright file="Role.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.TranslationModule.Aggregate
{
    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate;

    /// <summary>
    /// The role entity.
    /// </summary>
    public class NotificationTypeTranslation : VersionedTable, IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

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
        public NotificationType NotificationType { get; set; }

        /// <summary>
        /// Gets or sets the notification type id.
        /// </summary>
        public int NotificationTypeId { get; set; }

        /// <summary>
        /// Gets or sets the label translated.
        /// </summary>
        public string Label { get; set; }
    }
}