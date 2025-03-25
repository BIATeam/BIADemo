// BIADemo only
// <copyright file="AircraftMaintenanceCompanyOptionMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Mappers
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Entities;

    /// <summary>
    /// The mapper used for aircraftMaintenanceCompany option.
    /// </summary>
    public class AircraftMaintenanceCompanyOptionMapper : BaseMapper<OptionDto, AircraftMaintenanceCompany, int>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<AircraftMaintenanceCompany, OptionDto>> EntityToDto()
        {
            return entity => new OptionDto
            {
                Id = entity.Id,

                Display = entity.Title,

            };
        }
    }
}
