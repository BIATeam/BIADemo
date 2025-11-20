// <copyright file="AnnouncementTypeTranslation.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Translation.Entities
{
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Announcement.Entities;
    using BIA.Net.Core.Domain.Entity;

    /// <summary>
    /// Represents an announcement type translation.
    /// </summary>
    public class AnnouncementTypeTranslation : BaseEntityVersioned<int>
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
        ///  Gets or sets the announcement type.
        /// </summary>
        public AnnouncementType AnnouncementType { get; set; }

        /// <summary>
        /// Gets or sets the announcement type id.
        /// </summary>
        public BiaAnnouncementType AnnouncementTypeId { get; set; }

        /// <summary>
        /// Gets or sets the label translated.
        /// </summary>
        public string Label { get; set; }
    }
}
