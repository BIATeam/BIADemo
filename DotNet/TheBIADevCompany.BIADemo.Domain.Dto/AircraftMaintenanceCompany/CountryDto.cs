// BIADemo only
// <copyright file="CountryDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.CustomAttribute;

    /// <summary>
    /// The DTO used to represent a country.
    /// </summary>
    public class CountryDto : BaseDto<int>
    {
        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        [BiaDtoField(Required = true)]
        public string Name { get; set; }
    }
}
