// BIADemo only
// <copyright file="CountryOptionMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Maintenance.Mappers
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Mapper;
    using TheBIADevCompany.BIADemo.Domain.Maintenance.Entities;

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
                Display = entity.Name,
            };
        }
    }
}
