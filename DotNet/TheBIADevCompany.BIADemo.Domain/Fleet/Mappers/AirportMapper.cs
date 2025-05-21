// BIADemo only
// <copyright file="AirportMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Mapper;
    using TheBIADevCompany.BIADemo.Domain.Dto.Fleet;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    /// <summary>
    /// The mapper used for plane.
    /// </summary>
    public class AirportMapper : BaseMapper<AirportDto, Airport, int>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<Airport> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Airport>(base.ExpressionCollection)
                {
                    { HeaderName.Name, planeType => planeType.Name },
                    { HeaderName.City, planeType => planeType.City },
                };
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(AirportDto dto, ref Airport entity)
        {
            base.DtoToEntity(dto, ref entity);

            entity.Name = dto.Name;
            entity.City = dto.City;
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Airport, AirportDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new AirportDto
            {
                Name = entity.Name,
                City = entity.City,
            });
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToCell"/>
        public override string DtoToCell(AirportDto dto, string headerName)
        {
            switch (headerName)
            {
                case HeaderName.Name:
                    return CSVString(dto.Name);
                case HeaderName.City:
                    return CSVString(dto.City);
                default:
                    return base.DtoToCell(dto, headerName);
            }
        }

        /// <summary>
        /// Header Name.
        /// </summary>
        public struct HeaderName
        {
            /// <summary>
            /// header name Name.
            /// </summary>
            public const string Name = "name";

            /// <summary>
            /// header name City.
            /// </summary>
            public const string City = "city";
        }
    }
}