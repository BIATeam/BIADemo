// BIADemo only
// <copyright file="CountryMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Aggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;

    /// <summary>
    /// The mapper used for country.
    /// </summary>
    public class CountryMapper : BaseMapper<CountryDto, Country, int>
    {
    }
}
