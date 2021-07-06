// BIADemo only
// <copyright file="PlaneListMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;

    /// <summary>
    /// The mapper used for plane.
    /// </summary>
    public class PlaneListMapper : BaseMapper<PlaneListItemDto, Plane>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<Plane> ExpressionCollection
        {
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

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Plane, PlaneListItemDto>> EntityToDto()
        {
            return entity => new PlaneListItemDto
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
                Site = entity.Site.Title,

                // Mapping relationship 0..1-* : PlaneType
                PlaneType = entity.PlaneType != null ? entity.PlaneType.Title : null,

                // Mapping relationship *-* : ICollection<Airports>
                ConnectingAirports = string.Join(", ", entity.ConnectingAirports.Select(x => x.Airport.Name).OrderBy(x => x)),
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Func<PlaneListItemDto, object[]> DtoToRecord()
        {
            return x => new object[]
            {
                x.Msn,
                x.IsActive ? "X" : string.Empty,
                x.FirstFlightDate.ToString("yyyy-MM-dd"),
                x.FirstFlightTime.ToString("hh:mm"),
                x.LastFlightDate?.ToString("yyyy-MM-dd hh:mm"),
                x.Capacity.ToString(),
                x.Site,
                x.PlaneType,
                x.ConnectingAirports,
            };
        }
    }
}