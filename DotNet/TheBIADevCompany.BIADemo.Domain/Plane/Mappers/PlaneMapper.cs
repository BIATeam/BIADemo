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
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;

    /// <summary>
    /// The mapper used for Plane.
    /// </summary>
    public class PlaneMapper : BaseMapper<PlaneDto, Plane, int>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<Plane> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Plane>
                {
                    { HeaderName.Id, plane => plane.Id },
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
            entity ??= new Plane();

            // Map parent relationship 1-* : SiteId
            if (dto.SiteId != 0)
            {
                entity.SiteId = dto.SiteId;
            }

            entity.Id = dto.Id;

            // Begin BIADemo
            entity.IsFixed = dto.IsFixed;

            // End BIADemo
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
            return entity => new PlaneDto
            {
                SiteId = entity.SiteId,
                Id = entity.Id,

                // Begin BIADemo
                IsFixed = entity.IsFixed,

                // End BIADemo
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
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Func<PlaneDto, object[]> DtoToRecord(List<string> headerNames = null)
        {
            return x =>
            {
                List<object> records = [];

                if (headerNames != null && headerNames.Count > 0)
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

                        if (string.Equals(headerName, HeaderName.Manufacturer, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.Manufacturer));
                        }

                        if (string.Equals(headerName, HeaderName.IsActive, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVBool(x.IsActive));
                        }

                        if (string.Equals(headerName, HeaderName.IsMaintenance, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVBool(x.IsMaintenance));
                        }

                        if (string.Equals(headerName, HeaderName.FirstFlightDate, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVDateTime(x.FirstFlightDate));
                        }

                        if (string.Equals(headerName, HeaderName.LastFlightDate, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVDateTime(x.LastFlightDate));
                        }

                        if (string.Equals(headerName, HeaderName.DeliveryDate, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVDate(x.DeliveryDate));
                        }

                        if (string.Equals(headerName, HeaderName.NextMaintenanceDate, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVDate(x.NextMaintenanceDate));
                        }

                        if (string.Equals(headerName, HeaderName.SyncTime, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVTime(x.SyncTime));
                        }

                        if (string.Equals(headerName, HeaderName.SyncFlightDataTime, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVTime(x.SyncFlightDataTime));
                        }

                        if (string.Equals(headerName, HeaderName.Capacity, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.Capacity));
                        }

                        if (string.Equals(headerName, HeaderName.MotorsCount, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.MotorsCount));
                        }

                        if (string.Equals(headerName, HeaderName.TotalFlightHours, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.TotalFlightHours));
                        }

                        if (string.Equals(headerName, HeaderName.Probability, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.Probability));
                        }

                        if (string.Equals(headerName, HeaderName.FuelCapacity, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.FuelCapacity));
                        }

                        if (string.Equals(headerName, HeaderName.FuelLevel, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.FuelLevel));
                        }

                        if (string.Equals(headerName, HeaderName.OriginalPrice, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.OriginalPrice));
                        }

                        if (string.Equals(headerName, HeaderName.EstimatedPrice, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.EstimatedPrice));
                        }

                        if (string.Equals(headerName, HeaderName.PlaneType, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.PlaneType?.Display));
                        }

                        if (string.Equals(headerName, HeaderName.SimilarTypes, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVList(x.SimilarTypes));
                        }

                        if (string.Equals(headerName, HeaderName.CurrentAirport, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.CurrentAirport?.Display));
                        }

                        if (string.Equals(headerName, HeaderName.ConnectingAirports, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVList(x.ConnectingAirports));
                        }
                    }
                }

                return [.. records];
            };
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
            /// Header name for id.
            /// </summary>
            public const string Id = "id";

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