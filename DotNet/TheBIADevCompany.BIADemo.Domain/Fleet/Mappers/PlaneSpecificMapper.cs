// BIADemo only
// <copyright file="PlaneSpecificMapper.cs" company="TheBIADevCompany">
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
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Mapper;
    using TheBIADevCompany.BIADemo.Domain.Dto.Fleet;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    /// <summary>
    /// The mapper used for plane.
    /// </summary>
    public class PlaneSpecificMapper : BaseMapper<PlaneSpecificDto, Plane, int>
    {
        private readonly PlaneMapper planeMapper;
        private readonly EngineSpecificMapper engineMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneSpecificMapper"/> class.
        /// </summary>
        /// <param name="planeMapper">base plane mapper.</param>
        /// <param name="engineMapper">engine mapper (specific).</param>
        public PlaneSpecificMapper(PlaneMapper planeMapper, EngineSpecificMapper engineMapper)
        {
            this.planeMapper = planeMapper;
            this.engineMapper = engineMapper;
        }

        /// <inheritdoc cref="BaseEntityMapper{Plane}.ExpressionCollection"/>
        public override ExpressionCollection<Plane> ExpressionCollection
        {
            // It is not necessary to implement this function if you to not use the mapper for filtered list. In BIADemo it is use only for Calc SpreadSheet.
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
                    { HeaderName.ConnectingAirports, plane => plane.ConnectingAirports.Select(x => x.Name).OrderBy(x => x) },
                    { HeaderName.CurrentAirport, plane => plane.CurrentAirport != null ? plane.CurrentAirport.Name : null },
                    { HeaderName.SimilarTypes, plane => plane.SimilarTypes.Select(x => x.Title).OrderBy(x => x) },
                };
            }
        }

        /// <inheritdoc cref="BaseEntityMapper{Plane}.ExpressionCollectionFilterIn"/>
        public override ExpressionCollection<Plane> ExpressionCollectionFilterIn
        {
            get
            {
                return new ExpressionCollection<Plane>(
                    base.ExpressionCollectionFilterIn,
                    new ExpressionCollection<Plane>()
                    {
                        { HeaderName.PlaneType, plane => plane.PlaneType.Id },
                        { HeaderName.SimilarTypes, plane => plane.SimilarTypes.Select(x => x.Id) },
                        { HeaderName.CurrentAirport, plane => plane.CurrentAirport.Id },
                        { HeaderName.ConnectingAirports, plane => plane.ConnectingAirports.Select(x => x.Id) },
                    });
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(PlaneSpecificDto dto, ref Plane entity)
        {
            this.planeMapper.DtoToEntity(dto, ref entity);
            entity.Engines ??= new List<Engine>();
            MapEmbeddedItemToEntityCollection(dto.Engines, entity.Engines, this.engineMapper);
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Plane, PlaneSpecificDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new PlaneSpecificDto
            {
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

                // Mapping relationship 1-* : Site
                SiteId = entity.SiteId,

                // Mapping relationship 0..1-* : PlaneType
                PlaneType = entity.PlaneType != null ? new OptionDto
                {
                    Id = entity.PlaneType.Id,
                    Display = entity.PlaneType.Title,
                }
                : null,

                CurrentAirport = entity.CurrentAirport != null ? new OptionDto
                {
                    Id = entity.CurrentAirport.Id,
                    Display = entity.CurrentAirport.Name,
                }
                : null,

                // Mapping relationship *-* : ICollection<Airports>
                ConnectingAirports = entity.ConnectingAirports.Select(ca => new OptionDto
                {
                    Id = ca.Id,
                    Display = ca.Name,
                }).OrderBy(x => x.Display).ToList(),

                SimilarTypes = entity.SimilarTypes.Select(ca => new OptionDto
                {
                    Id = ca.Id,
                    Display = ca.Title,
                }).OrderBy(x => x.Display).ToList(),

                Engines = entity.Engines.AsQueryable().Select(this.engineMapper.EntityToDto()).ToList(),
            });
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToCellMapping"/>
        public override Dictionary<string, Func<string>> DtoToCellMapping(PlaneSpecificDto dto)
        {
            return new Dictionary<string, Func<string>>(base.DtoToCellMapping(dto))
            {
                { HeaderName.Msn, () => CSVString(dto.Msn) },
                { HeaderName.Manufacturer, () => CSVString(dto.Manufacturer) },
                { HeaderName.IsActive, () => CSVBool(dto.IsActive) },
                { HeaderName.IsMaintenance, () => CSVBool(dto.IsMaintenance) },
                { HeaderName.FirstFlightDate, () => CSVDateTime(dto.FirstFlightDate) },
                { HeaderName.LastFlightDate, () => CSVDateTime(dto.LastFlightDate) },
                { HeaderName.DeliveryDate, () => CSVDate(dto.DeliveryDate) },
                { HeaderName.NextMaintenanceDate, () => CSVDate(dto.NextMaintenanceDate) },
                { HeaderName.SyncTime, () => CSVTime(dto.SyncTime) },
                { HeaderName.SyncFlightDataTime, () => CSVTime(dto.SyncFlightDataTime) },
                { HeaderName.Capacity, () => CSVNumber(dto.Capacity) },
                { HeaderName.MotorsCount, () => CSVNumber(dto.MotorsCount) },
                { HeaderName.PlaneType, () => CSVString(dto.PlaneType?.Display) },
                { HeaderName.ConnectingAirports, () => CSVList(dto.ConnectingAirports) },
                { HeaderName.TotalFlightHours, () => CSVNumber(dto.TotalFlightHours) },
                { HeaderName.Probability, () => CSVNumber(dto.Probability) },
                { HeaderName.FuelCapacity, () => CSVNumber(dto.FuelCapacity) },
                { HeaderName.FuelLevel, () => CSVNumber(dto.FuelLevel) },
                { HeaderName.OriginalPrice, () => CSVNumber(dto.OriginalPrice) },
                { HeaderName.EstimatedPrice, () => CSVNumber(dto.EstimatedPrice) },
                { HeaderName.CurrentAirport, () => CSVString(dto.CurrentAirport?.Display) },
                { HeaderName.SimilarTypes, () => CSVList(dto.SimilarTypes) },
            };
        }

        /// <inheritdoc/>
        public override void MapEntityKeysInDto(Plane entity, PlaneSpecificDto dto)
        {
            base.MapEntityKeysInDto(entity, dto);
            dto.SiteId = entity.SiteId;
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.IncludesForUpdate"/>
        public override Expression<Func<Plane, object>>[] IncludesForUpdate()
        {
            return
            [
                x => x.ConnectingAirports,
                x => x.Engines
            ];
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
            /// Header Name Manufacturer.
            /// </summary>
            public const string Manufacturer = "manufacturer";

            /// <summary>
            /// Header Name IsActive.
            /// </summary>
            public const string IsActive = "isActive";

            /// <summary>
            /// Header Name IsMaintenance.
            /// </summary>
            public const string IsMaintenance = "isMaintenance";

            /// <summary>
            /// Header Name FirstFlightDate.
            /// </summary>
            public const string FirstFlightDate = "firstFlightDate";

            /// <summary>
            /// Header Name LastFlightDate.
            /// </summary>
            public const string LastFlightDate = "lastFlightDate";

            /// <summary>
            /// Header Name DeliveryDate.
            /// </summary>
            public const string DeliveryDate = "deliveryDate";

            /// <summary>
            /// Header Name NextMaintenanceDate.
            /// </summary>
            public const string NextMaintenanceDate = "nextMaintenanceDate";

            /// <summary>
            /// Header Name SyncTime.
            /// </summary>
            public const string SyncTime = "syncTime";

            /// <summary>
            /// Header Name SyncFlightDataTime.
            /// </summary>
            public const string SyncFlightDataTime = "syncFlightDataTime";

            /// <summary>
            /// Header Name Capacity.
            /// </summary>
            public const string Capacity = "capacity";

            /// <summary>
            /// Header Name MotorsCount.
            /// </summary>
            public const string MotorsCount = "motorsCount";

            /// <summary>
            /// Header Name TotalFlightHours.
            /// </summary>
            public const string TotalFlightHours = "totalFlightHours";

            /// <summary>
            /// Header Name Propability.
            /// </summary>
            public const string Probability = "probability";

            /// <summary>
            /// Header Name Fuel Capacity.
            /// </summary>
            public const string FuelCapacity = "fuelCapacity";

            /// <summary>
            /// Header Name Fuel Level.
            /// </summary>
            public const string FuelLevel = "fuelLevel";

            /// <summary>
            /// Header Name Original Price.
            /// </summary>
            public const string OriginalPrice = "originalPrice";

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

            /// <summary>
            /// Header Name SimilarTypes.
            /// </summary>
            public const string SimilarTypes = "similarTypes";

            /// <summary>
            /// Header Name CurrentAirport.
            /// </summary>
            public const string CurrentAirport = "currentAirport";
        }
    }
}