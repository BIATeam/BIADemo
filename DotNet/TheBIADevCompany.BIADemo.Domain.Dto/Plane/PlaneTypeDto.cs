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
    /// The DTO used to represent a plane.
    /// </summary>
    public class PlaneTypeDto : BaseDto<int>
    {
        /// <summary>
        /// Gets or sets the Manufacturer's Serial Number.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the first flight date.
        /// </summary>
        [BiaDtoField(Type = "datetime", Required = false)]
        public DateTime? CertificationDate { get; set; }
    }
}