// BIADemo only
// <copyright file="PlaneMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;

    /// <summary>
    /// The mapper used for plane.
    /// </summary>
    public class PlaneMapper : BaseMapper<PlaneDto, Plane, int>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<Plane> ExpressionCollection
        {
            // It is not necessary to implement this function if you to not use the mapper for filtered list. In BIADemo it is use only for Calc SpreadSheet.
            get
            {
                return new ExpressionCollection<Plane>
                {
                    { HeaderName.Id, plane => plane.Id },
                    { HeaderName.Msn, plane => plane.Msn },
                    { HeaderName.IsActive, plane => plane.IsActive },
                    { HeaderName.LastFlightDate, plane => plane.LastFlightDate },
                    { HeaderName.DeliveryDate, plane => plane.DeliveryDate },
                    { HeaderName.SyncTime, plane => plane.SyncTime },
                    { HeaderName.Capacity, plane => plane.Capacity },
                    { HeaderName.Probability, plane => plane.Probability },
                    { HeaderName.FuelLevel, plane => plane.FuelLevel },
                    { HeaderName.EstimatedPrice, plane => plane.EstimatedPrice },
                    { HeaderName.PlaneType, plane => plane.PlaneType != null ? plane.PlaneType.Title : null },
                    { HeaderName.ConnectingAirports, plane => plane.ConnectingAirports.Select(x => x.Name).OrderBy(x => x) },
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
            entity.LastFlightDate = dto.LastFlightDate;
            entity.DeliveryDate = dto.DeliveryDate;
            entity.SyncTime = string.IsNullOrEmpty(dto.SyncTime) ? null : TimeSpan.Parse(dto.SyncTime, new CultureInfo("en-US"));
            entity.Capacity = dto.Capacity;
            entity.Probability = dto.Probability;
            entity.FuelLevel = dto.FuelLevel;
            entity.EstimatedPrice = dto.EstimatedPrice;

            // Mapping relationship 1-* : Site
            if (dto.SiteId != 0)
            {
                entity.SiteId = dto.SiteId;
            }

            // Mapping relationship 0..1-* : PlaneType
            entity.PlaneTypeId = dto.PlaneType?.Id;

            // Mapping relationship *-* : ICollection<OptionDto> ConnectingAirports
            if (dto.ConnectingAirports != null && dto.ConnectingAirports?.Any() == true)
            {
                foreach (var airportDto in dto.ConnectingAirports.Where(x => x.DtoState == DtoState.Deleted))
                {
                    var connectingAirport = entity.ConnectingAirports.FirstOrDefault(x => x.Id == airportDto.Id);
                    if (connectingAirport != null)
                    {
                        entity.ConnectingAirports.Remove(connectingAirport);
                    }
                }

                entity.ConnectingPlaneAirports = entity.ConnectingPlaneAirports ?? new List<PlaneAirport>();
                foreach (var airportDto in dto.ConnectingAirports.Where(w => w.DtoState == DtoState.Added))
                {
                    entity.ConnectingPlaneAirports.Add(new PlaneAirport
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
                LastFlightDate = entity.LastFlightDate,
                DeliveryDate = entity.DeliveryDate,
                SyncTime = entity.SyncTime.Value.ToString(@"hh\:mm\:ss"),
                Capacity = entity.Capacity,
                Probability = entity.Probability,
                FuelLevel = entity.FuelLevel,
                EstimatedPrice = entity.EstimatedPrice,

                // Mapping relationship 1-* : Site
                SiteId = entity.SiteId,

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
                    Id = ca.Id,
                    Display = ca.Name,
                }).OrderBy(x => x.Display).ToList(),
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Func<PlaneDto, object[]> DtoToRecord(List<string> headerNames = null)
        {
            return x =>
            {
                List<object> records = new List<object>();

                if (headerNames?.Any() == true)
                {
                    foreach (string headerName in headerNames)
                    {
                        if (string.Equals(headerName, HeaderName.Id, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.Id));
                        }

                        if (string.Equals(headerName, HeaderName.Msn, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.Msn));
                        }

                        if (string.Equals(headerName, HeaderName.IsActive, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVBool(x.IsActive));
                        }

                        if (string.Equals(headerName, HeaderName.LastFlightDate, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVDateTime(x.LastFlightDate));
                        }

                        if (string.Equals(headerName, HeaderName.DeliveryDate, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVDate(x.DeliveryDate));
                        }

                        if (string.Equals(headerName, HeaderName.SyncTime, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVTime(x.SyncTime));
                        }

                        if (string.Equals(headerName, HeaderName.Capacity, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.Capacity));
                        }

                        if (string.Equals(headerName, HeaderName.PlaneType, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.PlaneType?.Display));
                        }

                        if (string.Equals(headerName, HeaderName.ConnectingAirports, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVList(x.ConnectingAirports));
                        }

                        if (string.Equals(headerName, HeaderName.Probability, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.Probability));
                        }

                        if (string.Equals(headerName, HeaderName.FuelLevel, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.FuelLevel));
                        }

                        if (string.Equals(headerName, HeaderName.EstimatedPrice, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.EstimatedPrice));
                        }
                    }
                }

                return records.ToArray();
            };
        }

        /// <inheritdoc/>
        public override void MapEntityKeysInDto(Plane entity, PlaneDto dto)
        {
            dto.Id = entity.Id;
            dto.SiteId = entity.SiteId;
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.IncludesForUpdate"/>
        public override Expression<Func<Plane, object>>[] IncludesForUpdate()
        {
            return new Expression<Func<Plane, object>>[] { x => x.ConnectingAirports };
        }

        /// <summary>
        /// Header Name.
        /// </summary>
        public struct HeaderName
        {
            /// <summary>
            /// Header Name Id.
            /// </summary>
            public const string Id = "id";

            /// <summary>
            /// Header Name Msn.
            /// </summary>
            public const string Msn = "msn";

            /// <summary>
            /// Header Name IsActive.
            /// </summary>
            public const string IsActive = "isActive";

            /// <summary>
            /// Header Name LastFlightDate.
            /// </summary>
            public const string LastFlightDate = "lastFlightDate";

            /// <summary>
            /// Header Name DeliveryDate.
            /// </summary>
            public const string DeliveryDate = "deliveryDate";

            /// <summary>
            /// Header Name SyncTime.
            /// </summary>
            public const string SyncTime = "syncTime";

            /// <summary>
            /// Header Name Capacity.
            /// </summary>
            public const string Capacity = "capacity";

            /// <summary>
            /// Header Name Propability.
            /// </summary>
            public const string Probability = "probability";

            /// <summary>
            /// Header Name Fuel Level.
            /// </summary>
            public const string FuelLevel = "fuelLevel";

            /// <summary>
            /// Header Name Estimated Price.
            /// </summary>
            public const string EstimatedPrice = "estimatedPrice";


            /// <summary>
            /// Header Name PlaneType.
            /// </summary>
            public const string PlaneType = "planeType";

            /// <summary>
            /// Header Name ConnectingAirports.
            /// </summary>
            public const string ConnectingAirports = "connectingAirports";
        }
    }
}