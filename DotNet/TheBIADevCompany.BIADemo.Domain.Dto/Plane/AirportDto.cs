// BIADemo only
// <copyright file="AirportDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Plane
{
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.CustomAttribute;

    /// <summary>
    /// The DTO used to represent a airport.
    /// </summary>
    public class AirportDto : BaseDto<int>
    {
        /// <summary>
        /// Gets or sets the name of the airport.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the City where is the airport.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string City { get; set; }
    }
}
