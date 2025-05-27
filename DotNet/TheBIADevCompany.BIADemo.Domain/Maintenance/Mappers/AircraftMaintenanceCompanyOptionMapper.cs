// BIADemo only
// <copyright file="AircraftMaintenanceCompanyOptionMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Maintenance.Mappers
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Domain.Bia.Base.Mappers;
    using TheBIADevCompany.BIADemo.Domain.Maintenance.Entities;

    /// <summary>
    /// The mapper used for aircraftMaintenanceCompany option.
    /// </summary>
    public class AircraftMaintenanceCompanyOptionMapper : BaseMapper<OptionDto, AircraftMaintenanceCompany, int>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<AircraftMaintenanceCompany, OptionDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new OptionDto
            {
                Display = entity.Title,
            });
        }
    }
}
