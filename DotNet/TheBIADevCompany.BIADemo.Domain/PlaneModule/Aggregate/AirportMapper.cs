// BIADemo only
// <copyright file="AirportMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;

    /// <summary>
    /// The mapper used for plane.
    /// </summary>
    public class AirportMapper : BaseMapper<AirportDto, Airport, int>
    {
        /// <summary>
        /// Header Name.
        /// </summary>
        public enum HeaderName
        {
            /// <summary>
            /// header name Id.
            /// </summary>
            Id,

            /// <summary>
            /// header name Name.
            /// </summary>
            Name,

            /// <summary>
            /// header name City.
            /// </summary>
            City,
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<Airport> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Airport>
                {
                    { HeaderName.Id.ToString(), planeType => planeType.Id },
                    { HeaderName.Name.ToString(), planeType => planeType.Name },
                    { HeaderName.City.ToString(), planeType => planeType.City },
                };
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(AirportDto dto, Airport entity)
        {
            if (entity == null)
            {
                entity = new Airport();
            }

            entity.Id = dto.Id;
            entity.Name = dto.Name;
            entity.City = dto.City;
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Airport, AirportDto>> EntityToDto()
        {
            return entity => new AirportDto
            {
                Id = entity.Id,
                Name = entity.Name,
                City = entity.City,
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Func<AirportDto, object[]> DtoToRecord(List<string> headerNames = null)
        {
            return x =>
            {
                List<object> records = new List<object>();

                if (headerNames?.Any() == true)
                {
                    foreach (string headerName in headerNames)
                    {
                        if (string.Equals(headerName, HeaderName.Name.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.Name));
                        }

                        if (string.Equals(headerName, HeaderName.City.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.City));
                        }
                    }
                }

                return records.ToArray();
            };
        }
    }
}