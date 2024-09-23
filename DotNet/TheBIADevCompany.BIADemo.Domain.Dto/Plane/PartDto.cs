// BIADemo only
// <copyright file="PartDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Plane
{
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The DTO used to represent a Part.
    /// </summary>
    public class PartDto : BaseDto<int>
    {
        /// <summary>
        /// Gets or sets the serial number.
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// Gets or sets the family.
        /// </summary>
        public string Family { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        public decimal Price { get; set; }
    }
}
