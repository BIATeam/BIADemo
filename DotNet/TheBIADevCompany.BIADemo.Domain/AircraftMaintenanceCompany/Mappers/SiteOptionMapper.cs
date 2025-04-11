// BIADemo only
// <copyright file="SiteOptionMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Mappers
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Domain.Site.Entities;

    /// <summary>
    /// The mapper used for site option.
    /// </summary>
    public class SiteOptionMapper : BaseMapper<OptionDto, Site, int>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Site, OptionDto>> EntityToDto()
        {
            return entity => new OptionDto
            {
                Id = entity.Id,

                Display = entity.Title,

            };
        }
    }
}
