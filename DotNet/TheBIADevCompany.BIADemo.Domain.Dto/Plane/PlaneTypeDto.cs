// BIADemo only
// <copyright file="PlaneTypeDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Plane
{
    using System;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.CustomAttribute;

    /// <summary>
    /// The DTO used to represent a plane type.
    /// </summary>
    public class PlaneTypeDto : BaseDto<int>
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the certification date.
        /// </summary>
        [BiaDtoField(Required = false, Type = "datetime")]
        public DateTime? CertificationDate { get; set; }
    }
}