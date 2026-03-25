// <copyright file="Announcement.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Announcement.Entities
{
    using System;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Entity;
    using global::Audit.EntityFramework;

    /// <summary>
    /// Represents an announcement to display.
    /// </summary>
    [AuditInclude]
    public sealed class Announcement : BaseEntityVersioned<int>
    {
        /// <summary>
        /// Gets or sets the type relation.
        /// </summary>
        public AnnouncementType Type { get; set; }

        /// <summary>
        /// Gets or sets the type identifier.
        /// </summary>
        public BiaAnnouncementType TypeId { get; set; }

        /// <summary>
        /// Gets or sets the raw content HTML.
        /// </summary>
        public string RawContent { get; set; }

        /// <summary>
        /// Gets or sets the start display date.
        /// </summary>
        public DateTimeOffset Start { get; set; }

        /// <summary>
        /// Gets or sets the end display date.
        /// </summary>
        public DateTimeOffset End { get; set; }
    }
}
