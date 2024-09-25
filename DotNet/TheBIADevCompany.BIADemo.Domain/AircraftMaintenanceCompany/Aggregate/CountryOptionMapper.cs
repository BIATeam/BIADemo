// BIADemo only
// <copyright file="CountryOptionMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Aggregate
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;

    /// <summary>
    /// The mapper used for country option.
    /// </summary>
    public class CountryOptionMapper : BaseMapper<CountryOptionDto, Country, int>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Country, CountryOptionDto>> EntityToDto()
        {
            return entity => new CountryOptionDto
            {
                Id = entity.Id,
                Display = entity.Name,
            };
        }
    }
}
