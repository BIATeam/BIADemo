// BIADemo only
// <copyright file="EngineMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Mapper;
    using TheBIADevCompany.BIADemo.Domain.Dto.Fleet;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

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
                return new ExpressionCollection<Engine>(base.ExpressionCollection)
                {
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
                    { HeaderName.PrincipalPart, engine => engine.PrincipalPart != null ? engine.PrincipalPart.SN : null },
                    { HeaderName.InstalledParts, engine => engine.InstalledParts.Select(x => x.SN).OrderBy(x => x) },
                };
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(EngineDto dto, ref Engine entity)
        {
            base.DtoToEntity(dto, ref entity);

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

            // Mapping relationship 1-* : Plane
            if (dto.PlaneId != 0)
            {
                entity.PlaneId = dto.PlaneId;
            }

            entity.PrincipalPartId = dto.PrincipalPart?.Id;
            if (dto.InstalledParts != null && dto.InstalledParts?.Any() == true)
            {
                foreach (var partDto in dto.InstalledParts.Where(x => x.DtoState == DtoState.Deleted))
                {
                    var installedPart = entity.InstalledParts.FirstOrDefault(x => x.Id == partDto.Id);
                    if (installedPart != null)
                    {
                        entity.InstalledParts.Remove(installedPart);
                    }
                }

                entity.InstalledEngineParts = entity.InstalledEngineParts ?? new List<EnginePart>();
                foreach (var partDto in dto.InstalledParts.Where(w => w.DtoState == DtoState.Added))
                {
                    entity.InstalledEngineParts.Add(new EnginePart
                    { PartId = partDto.Id, EngineId = dto.Id });
                }
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Engine, EngineDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new EngineDto
            {
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

                PrincipalPart = entity.PrincipalPart != null ? new OptionDto
                {
                    Id = entity.PrincipalPart.Id,
                    Display = entity.PrincipalPart.SN,
                }
                : null,

                InstalledParts = entity.InstalledParts.Select(ca => new OptionDto
                {
                    Id = ca.Id,
                    Display = ca.SN,
                }).OrderBy(x => x.Display).ToList(),
            });
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToCellMapping"/>
        public override Dictionary<string, Func<string>> DtoToCellMapping(EngineDto dto)
        {
            return new Dictionary<string, Func<string>>(base.DtoToCellMapping(dto))
            {
                { HeaderName.Reference, () => CSVString(dto.Reference) },
                { HeaderName.Manufacturer, () => CSVString(dto.Manufacturer) },
                { HeaderName.NextMaintenanceDate, () => CSVDateTime(dto.NextMaintenanceDate) },
                { HeaderName.LastMaintenanceDate, () => CSVDateTime(dto.LastMaintenanceDate) },
                { HeaderName.DeliveryDate, () => CSVDate(dto.DeliveryDate) },
                { HeaderName.ExchangeDate, () => CSVDate(dto.ExchangeDate) },
                { HeaderName.IgnitionTime, () => CSVTime(dto.IgnitionTime) },
                { HeaderName.SyncTime, () => CSVTime(dto.SyncTime) },
                { HeaderName.Power, () => CSVNumber(dto.Power) },
                { HeaderName.NoiseLevel, () => CSVNumber(dto.NoiseLevel) },
                { HeaderName.AverageFlightHours, () => CSVNumber(dto.AverageFlightHours) },
                { HeaderName.FlightHours, () => CSVNumber(dto.FlightHours) },
                { HeaderName.AverageFuelConsumption, () => CSVNumber(dto.AverageFuelConsumption) },
                { HeaderName.FuelConsumption, () => CSVNumber(dto.FuelConsumption) },
                { HeaderName.EstimatedPrice, () => CSVNumber(dto.EstimatedPrice) },
                { HeaderName.OriginalPrice, () => CSVNumber(dto.OriginalPrice) },
                { HeaderName.IsToBeMaintained, () => CSVBool(dto.IsToBeMaintained) },
                { HeaderName.IsHybrid, () => CSVBool(dto.IsHybrid) },
                { HeaderName.PrincipalPart, () => CSVString(dto.PrincipalPart?.Display) },
                { HeaderName.InstalledParts, () => CSVList(dto.InstalledParts) },
            };
        }

        /// <inheritdoc/>
        public override void MapEntityKeysInDto(Engine entity, EngineDto dto)
        {
            base.MapEntityKeysInDto(entity, dto);
            dto.PlaneId = entity.PlaneId;
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Expression<Func<Engine, object>>[] IncludesForUpdate()
        {
            return
            [
                x => x.InstalledParts,
            ];
        }

        /// <summary>
        /// Header Name.
        /// </summary>
        public struct HeaderName
        {
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

            /// <summary>
            /// Header Name principalPart.
            /// </summary>
            public const string PrincipalPart = "principalPart";

            /// <summary>
            /// Header Name installedParts.
            /// </summary>
            public const string InstalledParts = "installedParts";
        }
    }
}