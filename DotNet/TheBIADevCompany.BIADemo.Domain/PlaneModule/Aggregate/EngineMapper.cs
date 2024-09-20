// BIADemo only
// <copyright file="EngineMapper.cs" company="TheBIADevCompany">
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
    /// The mapper used for engine.
    /// </summary>
    public class EngineMapper : BaseMapper<EngineDto, Engine, int>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<Engine> ExpressionCollection
        {
            // It is not necessary to implement this function if you to not use the mapper for filtered list. In BIADemo it is use only for Calc SpreadSheet.
            get
            {
                return new ExpressionCollection<Engine>
                {
                    { HeaderName.Id, engine => engine.Id },
                    { HeaderName.Reference, engine => engine.Reference },
                    { HeaderName.Manufacturer, engine => engine.Manufacturer },
                    { HeaderName.NextMaintenanceDate, engine => engine.NextMaintenanceDate },
                    { HeaderName.LastMaintenanceDate, engine => engine.LastMaintenanceDate },
                    { HeaderName.DeliveryDate, engine => engine.DeliveryDate },
                    { HeaderName.ExchangeDate, engine => engine.ExchangeDate },
                    { HeaderName.SyncTime, engine => engine.SyncTime },
                    { HeaderName.IgnitionTime, engine => engine.IgnitionTime },
                    { HeaderName.Power, engine => engine.Power },
                    { HeaderName.NoiseLevel, engine => engine.NoiseLevel },
                    { HeaderName.FlightHours, engine => engine.FlightHours },
                    { HeaderName.AverageFlightHours, engine => engine.AverageFlightHours },
                    { HeaderName.FuelConsumption, engine => engine.FuelConsumption },
                    { HeaderName.AverageFuelConsumption, engine => engine.AverageFuelConsumption },
                    { HeaderName.OriginalPrice, engine => engine.OriginalPrice },
                    { HeaderName.EstimatedPrice, engine => engine.EstimatedPrice },
                    { HeaderName.IsToBeMaintained, engine => engine.IsToBeMaintained },
                    { HeaderName.IsHybrid, engine => engine.IsHybrid },
                };
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(EngineDto dto, Engine entity)
        {
            if (entity == null)
            {
                entity = new Engine();
            }

            entity.Id = dto.Id;
            entity.Reference = dto.Reference;
            entity.Manufacturer = dto.Manufacturer;
            entity.NextMaintenanceDate = dto.NextMaintenanceDate;
            entity.LastMaintenanceDate = dto.LastMaintenanceDate;
            entity.DeliveryDate = dto.DeliveryDate;
            entity.ExchangeDate = dto.ExchangeDate;
            entity.SyncTime = TimeSpan.Parse(dto.SyncTime, new CultureInfo("en-US"));
            entity.IgnitionTime = dto.IgnitionTime != null ? TimeSpan.Parse(dto.IgnitionTime, new CultureInfo("en-US")) : null;
            entity.Power = dto.Power;
            entity.NoiseLevel = dto.NoiseLevel;
            entity.FlightHours = dto.FlightHours;
            entity.AverageFlightHours = dto.AverageFlightHours;
            entity.FuelConsumption = dto.FuelConsumption;
            entity.AverageFuelConsumption = dto.AverageFuelConsumption;
            entity.OriginalPrice = dto.OriginalPrice;
            entity.EstimatedPrice = dto.EstimatedPrice;
            entity.IsToBeMaintained = dto.IsToBeMaintained;
            entity.IsHybrid = dto.IsHybrid;

            // Mapping relationship 1-* : Site
            if (dto.PlaneId != 0)
            {
                entity.PlaneId = dto.PlaneId;
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Engine, EngineDto>> EntityToDto()
        {
            return entity => new EngineDto
            {
                Id = entity.Id,
                Reference = entity.Reference,
                Manufacturer = entity.Manufacturer,
                NextMaintenanceDate = entity.NextMaintenanceDate,
                LastMaintenanceDate = entity.LastMaintenanceDate,
                DeliveryDate = entity.DeliveryDate,
                ExchangeDate = entity.ExchangeDate,
                SyncTime = entity.SyncTime.ToString(@"hh\:mm\:ss"),
                IgnitionTime = entity.IgnitionTime.GetValueOrDefault().ToString(@"hh\:mm\:ss"),
                Power = entity.Power,
                NoiseLevel = entity.NoiseLevel,
                FlightHours = entity.FlightHours,
                AverageFlightHours = entity.AverageFlightHours,
                FuelConsumption = entity.FuelConsumption,
                AverageFuelConsumption = entity.AverageFuelConsumption,
                OriginalPrice = entity.OriginalPrice,
                EstimatedPrice = entity.EstimatedPrice,
                IsToBeMaintained = entity.IsToBeMaintained,
                IsHybrid = entity.IsHybrid,

                // Mapping relationship 1-* : Plane
                PlaneId = entity.PlaneId,
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Func<EngineDto, object[]> DtoToRecord(List<string> headerNames = null)
        {
            return x =>
            {
                List<object> records = new List<object>();

                if (headerNames?.Any() == true)
                {
                    foreach (string headerName in headerNames)
                    {
                        if (string.Equals(headerName, HeaderName.Reference, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.Reference));
                        }

                        if (string.Equals(headerName, HeaderName.Manufacturer, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.Manufacturer));
                        }

                        if (string.Equals(headerName, HeaderName.NextMaintenanceDate, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVDateTime(x.NextMaintenanceDate));
                        }

                        if (string.Equals(headerName, HeaderName.LastMaintenanceDate, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVDateTime(x.LastMaintenanceDate));
                        }

                        if (string.Equals(headerName, HeaderName.DeliveryDate, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVDate(x.DeliveryDate));
                        }

                        if (string.Equals(headerName, HeaderName.ExchangeDate, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVDate(x.ExchangeDate));
                        }

                        if (string.Equals(headerName, HeaderName.IgnitionTime, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVTime(x.IgnitionTime));
                        }

                        if (string.Equals(headerName, HeaderName.SyncTime, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVTime(x.SyncTime));
                        }

                        if (string.Equals(headerName, HeaderName.Power, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.Power.Value));
                        }

                        if (string.Equals(headerName, HeaderName.NoiseLevel, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.NoiseLevel));
                        }

                        if (string.Equals(headerName, HeaderName.AverageFlightHours, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.AverageFlightHours.Value));
                        }

                        if (string.Equals(headerName, HeaderName.FlightHours, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.FlightHours));
                        }

                        if (string.Equals(headerName, HeaderName.AverageFuelConsumption, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.AverageFuelConsumption.Value));
                        }

                        if (string.Equals(headerName, HeaderName.FuelConsumption, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.FuelConsumption));
                        }

                        if (string.Equals(headerName, HeaderName.EstimatedPrice, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.EstimatedPrice.Value));
                        }

                        if (string.Equals(headerName, HeaderName.OriginalPrice, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.OriginalPrice));
                        }

                        if (string.Equals(headerName, HeaderName.IsToBeMaintained, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVBool(x.IsToBeMaintained));
                        }

                        if (string.Equals(headerName, HeaderName.IsHybrid, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVBool(x.IsHybrid.GetValueOrDefault()));
                        }
                    }
                }

                return records.ToArray();
            };
        }

        /// <inheritdoc/>
        public override void MapEntityKeysInDto(Engine entity, EngineDto dto)
        {
            dto.Id = entity.Id;
            dto.PlaneId = entity.PlaneId;
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
            /// Header Name Reference.
            /// </summary>
            public const string Reference = "reference";

            /// <summary>
            /// Header Name Manufacturer.
            /// </summary>
            public const string Manufacturer = "manufacturer";

            /// <summary>
            /// Header Name NextMaintenanceDate.
            /// </summary>
            public const string NextMaintenanceDate = "nextMaintenanceDate";

            /// <summary>
            /// Header Name LastMaintenanceDate.
            /// </summary>
            public const string LastMaintenanceDate = "lastMaintenanceDate";

            /// <summary>
            /// Header Name DeliveryDate.
            /// </summary>
            public const string DeliveryDate = "deliveryDate";

            /// <summary>
            /// Header Name ExchangeDate.
            /// </summary>
            public const string ExchangeDate = "exchangeDate";

            /// <summary>
            /// Header Name SyncTime.
            /// </summary>
            public const string SyncTime = "syncTime";

            /// <summary>
            /// Header Name IgnitionTime.
            /// </summary>
            public const string IgnitionTime = "ignitionTime";

            /// <summary>
            /// Header Name Power.
            /// </summary>
            public const string Power = "power";

            /// <summary>
            /// Header Name NoiseLevel.
            /// </summary>
            public const string NoiseLevel = "noiseLevel";

            /// <summary>
            /// Header Name FlightHours.
            /// </summary>
            public const string FlightHours = "flightHours";

            /// <summary>
            /// Header Name AverageFlightHours.
            /// </summary>
            public const string AverageFlightHours = "averageFlightHours";

            /// <summary>
            /// Header Name FuelConsumption.
            /// </summary>
            public const string FuelConsumption = "fuelConsumption";

            /// <summary>
            /// Header Name AverageFuelConsumption.
            /// </summary>
            public const string AverageFuelConsumption = "averageFuelConsumption";

            /// <summary>
            /// Header Name OriginalPrice.
            /// </summary>
            public const string OriginalPrice = "originalPrice";

            /// <summary>
            /// Header Name EstimatedPrice.
            /// </summary>
            public const string EstimatedPrice = "estimatedPrice";

            /// <summary>
            /// Header Name IsToBeMaintained.
            /// </summary>
            public const string IsToBeMaintained = "isToBeMaintained";

            /// <summary>
            /// Header Name IsHybrid.
            /// </summary>
            public const string IsHybrid = "isHybrid";
        }
    }
}