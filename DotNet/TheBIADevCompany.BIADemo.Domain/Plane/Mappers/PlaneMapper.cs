// BIADemo only
// <copyright file="PlaneMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Plane.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;

    /// <summary>
    /// The mapper used for Plane.
    /// </summary>
    public class PlaneMapper : ReflectionMapper<PlaneDto, Plane, int>
    {
        /// <inheritdoc cref="ReflectionMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<Plane> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Plane>(base.ExpressionCollection)
                {
                    { HeaderName.Msn, plane => plane.Msn },
                    { HeaderName.Manufacturer, plane => plane.Manufacturer },
                    { HeaderName.IsActive, plane => plane.IsActive },
                    { HeaderName.IsMaintenance, plane => plane.IsMaintenance },
                    { HeaderName.FirstFlightDate, plane => plane.FirstFlightDate },
                    { HeaderName.LastFlightDate, plane => plane.LastFlightDate },
                    { HeaderName.DeliveryDate, plane => plane.DeliveryDate },
                    { HeaderName.NextMaintenanceDate, plane => plane.NextMaintenanceDate },
                    { HeaderName.SyncTime, plane => plane.SyncTime },
                    { HeaderName.SyncFlightDataTime, plane => plane.SyncFlightDataTime },
                    { HeaderName.Capacity, plane => plane.Capacity },
                    { HeaderName.MotorsCount, plane => plane.MotorsCount },
                    { HeaderName.TotalFlightHours, plane => plane.TotalFlightHours },
                    { HeaderName.Probability, plane => plane.Probability },
                    { HeaderName.FuelCapacity, plane => plane.FuelCapacity },
                    { HeaderName.FuelLevel, plane => plane.FuelLevel },
                    { HeaderName.OriginalPrice, plane => plane.OriginalPrice },
                    { HeaderName.EstimatedPrice, plane => plane.EstimatedPrice },
                    { HeaderName.PlaneType, plane => plane.PlaneType != null ? plane.PlaneType.Title : null },
                    { HeaderName.SimilarTypes, plane => plane.SimilarTypes.Select(x => x.Title).OrderBy(x => x) },
                    { HeaderName.CurrentAirport, plane => plane.CurrentAirport != null ? plane.CurrentAirport.Name : null },
                    { HeaderName.ConnectingAirports, plane => plane.ConnectingAirports.Select(x => x.Name).OrderBy(x => x) },
                };
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(PlaneDto dto, Plane entity)
        {
            base.DtoToEntity(dto, entity);

            // Map parent relationship 1-* : SiteId
            if (dto.SiteId != 0)
            {
                entity.SiteId = dto.SiteId;
            }

            entity.Msn = dto.Msn;
            entity.Manufacturer = dto.Manufacturer;
            entity.IsActive = dto.IsActive;
            entity.IsMaintenance = dto.IsMaintenance;
            entity.FirstFlightDate = dto.FirstFlightDate;
            entity.LastFlightDate = dto.LastFlightDate;
            entity.DeliveryDate = dto.DeliveryDate;
            entity.NextMaintenanceDate = dto.NextMaintenanceDate;
            entity.SyncTime = string.IsNullOrEmpty(dto.SyncTime) ? null : TimeSpan.Parse(dto.SyncTime, new CultureInfo("en-US"));
            entity.SyncFlightDataTime = TimeSpan.Parse(dto.SyncFlightDataTime, new CultureInfo("en-US"));
            entity.Capacity = dto.Capacity;
            entity.MotorsCount = dto.MotorsCount;
            entity.TotalFlightHours = dto.TotalFlightHours;
            entity.Probability = dto.Probability;
            entity.FuelCapacity = dto.FuelCapacity;
            entity.FuelLevel = dto.FuelLevel;
            entity.OriginalPrice = dto.OriginalPrice;
            entity.EstimatedPrice = dto.EstimatedPrice;

            // Map relationship 0..1-* : PlaneType
            entity.PlaneTypeId = dto.PlaneType?.Id;

            // Map relationship *-* : SimilarTypes
            if (dto.SimilarTypes != null && dto.SimilarTypes.Count != 0)
            {
                foreach (var similarTypeDto in dto.SimilarTypes.Where(x => x.DtoState == DtoState.Deleted))
                {
                    var similarType = entity.SimilarTypes.FirstOrDefault(x => x.Id == similarTypeDto.Id);
                    if (similarType != null)
                    {
                        entity.SimilarTypes.Remove(similarType);
                    }
                }

                entity.SimilarPlaneType = entity.SimilarPlaneType ?? new List<PlanePlaneType>();
                foreach (var similarTypeDto in dto.SimilarTypes.Where(x => x.DtoState == DtoState.Added))
                {
                    entity.SimilarPlaneType.Add(new PlanePlaneType
                    {
                        PlaneTypeId = similarTypeDto.Id,
                        PlaneId = dto.Id,
                    });
                }
            }

            // Map relationship 1-* : CurrentAirport
            entity.CurrentAirportId = dto.CurrentAirport.Id;

            // Map relationship *-* : ConnectingAirports
            if (dto.ConnectingAirports != null && dto.ConnectingAirports.Count != 0)
            {
                foreach (var connectingAirportDto in dto.ConnectingAirports.Where(x => x.DtoState == DtoState.Deleted))
                {
                    var connectingAirport = entity.ConnectingAirports.FirstOrDefault(x => x.Id == connectingAirportDto.Id);
                    if (connectingAirport != null)
                    {
                        entity.ConnectingAirports.Remove(connectingAirport);
                    }
                }

                entity.ConnectingPlaneAirports = entity.ConnectingPlaneAirports ?? new List<PlaneAirport>();
                foreach (var connectingAirportDto in dto.ConnectingAirports.Where(x => x.DtoState == DtoState.Added))
                {
                    entity.ConnectingPlaneAirports.Add(new PlaneAirport
                    {
                        AirportId = connectingAirportDto.Id,
                        PlaneId = dto.Id,
                    });
                }
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Plane, PlaneDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new PlaneDto
            {
                SiteId = entity.SiteId,
                Msn = entity.Msn,
                Manufacturer = entity.Manufacturer,
                IsActive = entity.IsActive,
                IsMaintenance = entity.IsMaintenance,
                FirstFlightDate = entity.FirstFlightDate,
                LastFlightDate = entity.LastFlightDate,
                DeliveryDate = entity.DeliveryDate,
                NextMaintenanceDate = entity.NextMaintenanceDate,
                SyncTime = entity.SyncTime.Value.ToString(@"hh\:mm\:ss"),
                SyncFlightDataTime = entity.SyncFlightDataTime.ToString(@"hh\:mm\:ss"),
                Capacity = entity.Capacity,
                MotorsCount = entity.MotorsCount,
                TotalFlightHours = entity.TotalFlightHours,
                Probability = entity.Probability,
                FuelCapacity = entity.FuelCapacity,
                FuelLevel = entity.FuelLevel,
                OriginalPrice = entity.OriginalPrice,
                EstimatedPrice = entity.EstimatedPrice,

                // Map relationship 0..1-* : PlaneType
                PlaneType = entity.PlaneType != null ? new OptionDto
                {
                    Id = entity.PlaneType.Id,
                    Display = entity.PlaneType.Title,
                }
                : null,

                // Map relationship *-* : SimilarTypes
                SimilarTypes = entity.SimilarTypes.Select(x => new OptionDto
                {
                    Id = x.Id,
                    Display = x.Title,
                }).OrderBy(x => x.Display).ToList(),

                // Map relationship 1-* : CurrentAirport
                CurrentAirport = new OptionDto
                {
                    Id = entity.CurrentAirport.Id,
                    Display = entity.CurrentAirport.Name,
                },

                // Map relationship *-* : ConnectingAirports
                ConnectingAirports = entity.ConnectingAirports.Select(x => new OptionDto
                {
                    Id = x.Id,
                    Display = x.Name,
                }).OrderBy(x => x.Display).ToList(),
                RowVersion = Convert.ToBase64String(entity.RowVersion),
            });
        }

        /// <inheritdoc cref="ReflectionMapper{TDto,TEntity}.DtoToCell"/>
        protected override string DtoToCell(PlaneDto dto, string headerName)
        {
            if (string.Equals(headerName, HeaderName.Msn, StringComparison.OrdinalIgnoreCase))
            {
                return CSVString(dto.Msn);
            }

            if (string.Equals(headerName, HeaderName.Manufacturer, StringComparison.OrdinalIgnoreCase))
            {
                return CSVString(dto.Manufacturer);
            }

            if (string.Equals(headerName, HeaderName.IsActive, StringComparison.OrdinalIgnoreCase))
            {
                return CSVBool(dto.IsActive);
            }

            if (string.Equals(headerName, HeaderName.IsMaintenance, StringComparison.OrdinalIgnoreCase))
            {
                return CSVBool(dto.IsMaintenance);
            }

            if (string.Equals(headerName, HeaderName.FirstFlightDate, StringComparison.OrdinalIgnoreCase))
            {
                return CSVDateTime(dto.FirstFlightDate);
            }

            if (string.Equals(headerName, HeaderName.LastFlightDate, StringComparison.OrdinalIgnoreCase))
            {
                return CSVDateTime(dto.LastFlightDate);
            }

            if (string.Equals(headerName, HeaderName.DeliveryDate, StringComparison.OrdinalIgnoreCase))
            {
                return CSVDate(dto.DeliveryDate);
            }

            if (string.Equals(headerName, HeaderName.NextMaintenanceDate, StringComparison.OrdinalIgnoreCase))
            {
                return CSVDate(dto.NextMaintenanceDate);
            }

            if (string.Equals(headerName, HeaderName.SyncTime, StringComparison.OrdinalIgnoreCase))
            {
                return CSVTime(dto.SyncTime);
            }

            if (string.Equals(headerName, HeaderName.SyncFlightDataTime, StringComparison.OrdinalIgnoreCase))
            {
                return CSVTime(dto.SyncFlightDataTime);
            }

            if (string.Equals(headerName, HeaderName.Capacity, StringComparison.OrdinalIgnoreCase))
            {
                return CSVNumber(dto.Capacity);
            }

            if (string.Equals(headerName, HeaderName.MotorsCount, StringComparison.OrdinalIgnoreCase))
            {
                return CSVNumber(dto.MotorsCount);
            }

            if (string.Equals(headerName, HeaderName.TotalFlightHours, StringComparison.OrdinalIgnoreCase))
            {
                return CSVNumber(dto.TotalFlightHours);
            }

            if (string.Equals(headerName, HeaderName.Probability, StringComparison.OrdinalIgnoreCase))
            {
                return CSVNumber(dto.Probability);
            }

            if (string.Equals(headerName, HeaderName.FuelCapacity, StringComparison.OrdinalIgnoreCase))
            {
                return CSVNumber(dto.FuelCapacity);
            }

            if (string.Equals(headerName, HeaderName.FuelLevel, StringComparison.OrdinalIgnoreCase))
            {
                return CSVNumber(dto.FuelLevel);
            }

            if (string.Equals(headerName, HeaderName.OriginalPrice, StringComparison.OrdinalIgnoreCase))
            {
                return CSVNumber(dto.OriginalPrice);
            }

            if (string.Equals(headerName, HeaderName.EstimatedPrice, StringComparison.OrdinalIgnoreCase))
            {
                return CSVNumber(dto.EstimatedPrice);
            }

            if (string.Equals(headerName, HeaderName.PlaneType, StringComparison.OrdinalIgnoreCase))
            {
                return CSVString(dto.PlaneType?.Display);
            }

            if (string.Equals(headerName, HeaderName.SimilarTypes, StringComparison.OrdinalIgnoreCase))
            {
                return CSVList(dto.SimilarTypes);
            }

            if (string.Equals(headerName, HeaderName.CurrentAirport, StringComparison.OrdinalIgnoreCase))
            {
                return CSVString(dto.CurrentAirport?.Display);
            }

            if (string.Equals(headerName, HeaderName.ConnectingAirports, StringComparison.OrdinalIgnoreCase))
            {
                return CSVList(dto.ConnectingAirports);
            }

            return base.DtoToCell(dto, headerName);
        }

        /// <inheritdoc/>
        public override void MapEntityKeysInDto(Plane entity, PlaneDto dto)
        {
            base.MapEntityKeysInDto(entity, dto);
            dto.SiteId = entity.SiteId;
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.IncludesForUpdate"/>
        public override Expression<Func<Plane, object>>[] IncludesForUpdate()
        {
            return
            [
                x => x.SimilarTypes,
                x => x.ConnectingAirports,
            ];
        }

        /// <summary>
        /// Header names.
        /// </summary>
        public struct HeaderName
        {
            /// <summary>
            /// Header name for site id.
            /// </summary>
            public const string SiteId = "siteId";

            /// <summary>
            /// Header name for msn.
            /// </summary>
            public const string Msn = "msn";

            /// <summary>
            /// Header name for manufacturer.
            /// </summary>
            public const string Manufacturer = "manufacturer";

            /// <summary>
            /// Header name for is active.
            /// </summary>
            public const string IsActive = "isActive";

            /// <summary>
            /// Header name for is maintenance.
            /// </summary>
            public const string IsMaintenance = "isMaintenance";

            /// <summary>
            /// Header name for first flight date.
            /// </summary>
            public const string FirstFlightDate = "firstFlightDate";

            /// <summary>
            /// Header name for last flight date.
            /// </summary>
            public const string LastFlightDate = "lastFlightDate";

            /// <summary>
            /// Header name for delivery date.
            /// </summary>
            public const string DeliveryDate = "deliveryDate";

            /// <summary>
            /// Header name for next maintenance date.
            /// </summary>
            public const string NextMaintenanceDate = "nextMaintenanceDate";

            /// <summary>
            /// Header name for sync time.
            /// </summary>
            public const string SyncTime = "syncTime";

            /// <summary>
            /// Header name for sync flight data time.
            /// </summary>
            public const string SyncFlightDataTime = "syncFlightDataTime";

            /// <summary>
            /// Header name for capacity.
            /// </summary>
            public const string Capacity = "capacity";

            /// <summary>
            /// Header name for motors count.
            /// </summary>
            public const string MotorsCount = "motorsCount";

            /// <summary>
            /// Header name for total flight hours.
            /// </summary>
            public const string TotalFlightHours = "totalFlightHours";

            /// <summary>
            /// Header name for probability.
            /// </summary>
            public const string Probability = "probability";

            /// <summary>
            /// Header name for fuel capacity.
            /// </summary>
            public const string FuelCapacity = "fuelCapacity";

            /// <summary>
            /// Header name for fuel level.
            /// </summary>
            public const string FuelLevel = "fuelLevel";

            /// <summary>
            /// Header name for original price.
            /// </summary>
            public const string OriginalPrice = "originalPrice";

            /// <summary>
            /// Header name for estimated price.
            /// </summary>
            public const string EstimatedPrice = "estimatedPrice";

            /// <summary>
            /// Header name for plane type.
            /// </summary>
            public const string PlaneType = "planeType";

            /// <summary>
            /// Header name for similar types.
            /// </summary>
            public const string SimilarTypes = "similarTypes";

            /// <summary>
            /// Header name for current airport.
            /// </summary>
            public const string CurrentAirport = "currentAirport";

            /// <summary>
            /// Header name for connecting airports.
            /// </summary>
            public const string ConnectingAirports = "connectingAirports";
        }
    }
}