// <copyright file="AnnouncementType.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Announcement.Entities
{
    using System.Collections.Generic;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Entity;
    using BIA.Net.Core.Domain.Translation.Entities;

    /// <summary>
    /// Represents an announcement type with the associated translations.
    /// </summary>
    public class AnnouncementType : BaseEntityVersioned<BiaAnnouncementType>
    {
        /// <summary>
        /// Gets or sets the announcement type translations.
        /// </summary>
        public virtual ICollection<AnnouncementTypeTranslation> AnnouncementTypeTranslations { get; set; }
    }
}
