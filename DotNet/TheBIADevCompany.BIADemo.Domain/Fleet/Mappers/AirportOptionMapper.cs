// BIADemo only
// <copyright file="AirportOptionMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Mappers
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Domain.Bia.Base.Mappers;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    /// <summary>
    /// The mapper used for plane.
    /// </summary>
    public class AirportOptionMapper : BaseMapper<OptionDto, Airport, int>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Airport, OptionDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new OptionDto
            {
                Display = entity.Name,
            });
        }
    }
}