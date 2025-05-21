// BIADemo only
// <copyright file="PlaneOptionMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Maintenance.Mappers
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Mapper;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    /// <summary>
    /// The mapper used for plane option.
    /// </summary>
    public class PlaneOptionMapper : BaseMapper<OptionDto, Plane, int>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Plane, OptionDto>> EntityToDto()
        {
            return entity => new OptionDto
            {
                Id = entity.Id,
                Display = entity.Msn,
            };
        }
    }
}
