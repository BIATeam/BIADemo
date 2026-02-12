// <copyright file="AnnouncementDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Announcement
{
    using System;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.CustomAttribute;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The DTO used to represent an announcement.
    /// </summary>
    public sealed class AnnouncementDto : BaseDtoVersioned<int>
    {
        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        [BiaDtoField(Required = true, Type = "datetime", AsLocalDateTime = true)]
        public DateTimeOffset End { get; set; }

        /// <summary>
        /// Gets or sets the raw content.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string RawContent { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        [BiaDtoField(Required = true, Type = "datetime", AsLocalDateTime = true)]
        public DateTimeOffset Start { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [BiaDtoField(Required = true)]
        public TOptionDto<BiaAnnouncementType> Type { get; set; }
    }
}
