// BIADemo only
// <copyright file="AircraftMaintenanceCompanyDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;

    /// <summary>
    /// The DTO used to represent a AircraftMaintenanceCompany.
    /// </summary>
    public class AircraftMaintenanceCompanyDto : BaseDto<int>
    {
        /// <summary>
        /// Gets or sets the Manufacturer's Serial Number.
        /// </summary>
        public string Name { get; set; }
    }
}