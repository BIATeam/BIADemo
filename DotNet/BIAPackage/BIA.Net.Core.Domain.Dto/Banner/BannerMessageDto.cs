// <copyright file="BannerMessageDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Banner
{
    using System;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.CustomAttribute;

    /// <summary>
    /// The DTO used to represent a banner message.
    /// </summary>
    public sealed class BannerMessageDto : BaseDtoVersioned<int>
    {
        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        [BiaDtoField(Required = true, Type = "datetime")]
        public DateTime End { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the raw content.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string RawContent { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        [BiaDtoField(Required = true, Type = "datetime")]
        public DateTime Start { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [BiaDtoField(Required = true)]
        public BiaBannerType Type { get; set; }
    }
}
