// BIADemo only
// <copyright file="CountryOptionMapper.cs" company="TheBIADevCompany">
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
    /// The mapper used for country option.
    /// </summary>
    public class CountryOptionMapper : BaseMapper<OptionDto, Country, int>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Country, OptionDto>> EntityToDto()
        {
            return entity => new OptionDto
            {
                Id = entity.Id,

                // BIAToolKit - Begin Display Name
                Display = entity.Name,

                // BIAToolKit - End Display Name
            };
        }
    }
}
