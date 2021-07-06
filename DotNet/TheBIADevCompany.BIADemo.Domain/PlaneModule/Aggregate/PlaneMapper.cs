// BIADemo only
// <copyright file="PlaneMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;

    /// <summary>
    /// The mapper used for plane.
    /// </summary>
    public class PlaneMapper : BaseMapper<PlaneDto, Plane>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<Plane> ExpressionCollection
        {
            // It is not necessary to implement this function if you to not use the mapper for filtered list. In BIADemo it is use only for Calc SpreadSheet.
            get
            {
                return new ExpressionCollection<Plane>
                {
                    { "Id", plane => plane.Id },
                    { "Msn", plane => plane.Msn },
                    { "IsActive", plane => plane.IsActive },
                    { "FirstFlightDate", plane => plane.FirstFlightDate },
                    { "FirstFlightTime", plane => plane.FirstFlightDate },
                    { "LastFlightDate", plane => plane.LastFlightDate },
                    { "Capacity", plane => plane.Capacity },
                    { "Site", plane => plane.Site.Title },
                    { "PlaneType", plane => plane.PlaneType != null ? plane.PlaneType.Title : null },
                    { "ConnectingAirports", plane => plane.ConnectingAirports.Select(x => x.Airport.Name).OrderBy(x => x) },
                };
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(PlaneDto dto, Plane entity)
        {
            if (entity == null)
            {
                entity = new Plane();
            }

            entity.Id = dto.Id;
            entity.Msn = dto.Msn;
            entity.IsActive = dto.IsActive;
            entity.FirstFlightDate = new DateTime(
                dto.FirstFlightDate.Year,
                dto.FirstFlightDate.Month,
                dto.FirstFlightDate.Day,
                dto.FirstFlightTime.Hour,
                dto.FirstFlightTime.Minute,
                dto.FirstFlightTime.Second);
            entity.LastFlightDate = dto.LastFlightDate;
            entity.Capacity = dto.Capacity;

            // Mapping relationship 1-* : Site
            if (dto.Site != null)
            {
                entity.SiteId = dto.Site.Id;
            }

            // Mapping relationship 0..1-* : PlaneType
            entity.PlaneTypeId = dto.PlaneType?.Id;

            // Mapping relationship *-* : ICollection<Airports>
            if (dto.ConnectingAirports?.Any() == true)
            {
                foreach (var airportDto in dto.ConnectingAirports.Where(x => x.DtoState == DtoState.Deleted))
                {
                    var connectingAirport = entity.ConnectingAirports.FirstOrDefault(x => x.AirportId == airportDto.Id && x.PlaneId == dto.Id);
                    if (connectingAirport != null)
                    {
                        entity.ConnectingAirports.Remove(connectingAirport);
                    }
                }

                entity.ConnectingAirports = entity.ConnectingAirports ?? new List<PlaneAirport>();
                foreach (var airportDto in dto.ConnectingAirports.Where(w => w.DtoState == DtoState.Added))
                {
                    entity.ConnectingAirports.Add(new PlaneAirport
                    { AirportId = airportDto.Id, PlaneId = dto.Id });
                }
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Plane, PlaneDto>> EntityToDto()
        {
            return entity => new PlaneDto
            {
                Id = entity.Id,
                Msn = entity.Msn,
                IsActive = entity.IsActive,
                FirstFlightDate = entity.FirstFlightDate.Date,
                FirstFlightTime = new DateTime(
                    entity.FirstFlightDate.Year,
                    entity.FirstFlightDate.Month,
                    entity.FirstFlightDate.Day,
                    entity.FirstFlightDate.Hour,
                    entity.FirstFlightDate.Minute,
                    entity.FirstFlightDate.Second),
                LastFlightDate = entity.LastFlightDate,
                Capacity = entity.Capacity,

                // Mapping relationship 1-* : Site
                Site = new SiteDto
                {
                    Id = entity.Site.Id,
                    Title = entity.Site.Title,
                },

                // Mapping relationship 0..1-* : PlaneType
                PlaneType = entity.PlaneType != null ? new OptionDto
                {
                    Id = entity.PlaneType.Id,
                    Display = entity.PlaneType.Title,
                }
                : null,

                // Mapping relationship *-* : ICollection<Airports>
                ConnectingAirports = entity.ConnectingAirports.Select(ca => new OptionDto
                {
                    Id = ca.Airport.Id,
                    Display = ca.Airport.Name,
                }).ToList(),
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.IncludesForUpdate"/>
        public override Expression<Func<Plane, object>>[] IncludesForUpdate()
        {
            return new Expression<Func<Plane, object>>[] { x => x.ConnectingAirports };
        }
    }
}