// BIADemo only
// <copyright file="PlaneSpecificQueryModelMapper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.QueryModels
{
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.Mapper;
    using TheBIADevCompany.BIADemo.Domain.Dto.Fleet;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Mappers;

    /// <summary>
    /// Provides mapping functionality between plane-specific query models, data transfer objects (DTOs), and entity
    /// models for plane-related data queries.
    /// </summary>
    public class PlaneSpecificQueryModelMapper : BiaBaseQueryModelMapper<PlaneSpecificQueryModel, PlaneSpecificDto, PlaneDto, Plane, int, PlaneSpecificMapper>
    {
        private readonly EngineMapper engineMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneSpecificQueryModelMapper"/> class.
        /// </summary>
        /// <param name="planeSpecificMapper">The plane specific mapper.</param>
        /// <param name="engineMapper">The engine mapper.</param>
        public PlaneSpecificQueryModelMapper(PlaneSpecificMapper planeSpecificMapper, EngineMapper engineMapper)
            : base(planeSpecificMapper)
        {
            this.engineMapper = engineMapper;
        }

        /// <inheritdoc/>
        public override Expression<Func<Plane, PlaneSpecificQueryModel>> EntityToDto()
        {
            return entity => new PlaneSpecificQueryModel
            {
                Id = entity.Id,
                Site = entity.Site,
                Msn = entity.Msn,
                IsActive = entity.IsActive,
                LastFlightDate = entity.LastFlightDate,
                DeliveryDate = entity.DeliveryDate,
                SyncFlightDataTime = entity.SyncFlightDataTime,
                Capacity = entity.Capacity,
                PlaneType = entity.PlaneType,
                CurrentAirport = entity.CurrentAirport,
                ConnectingAirports = entity.ConnectingAirports,
                Engines = entity.Engines,
                IsFixed = entity.IsFixed,
                RowVersion = entity.RowVersion,
            };
        }

        /// <inheritdoc/>
        public override PlaneSpecificDto QueryModelToDto(PlaneSpecificQueryModel queryModel)
        {
            return new PlaneSpecificDto
            {
                Id = queryModel.Id,
                SiteId = queryModel.Site.Id,
                Msn = queryModel.Msn,
                IsActive = queryModel.IsActive,
                LastFlightDate = queryModel.LastFlightDate,
                DeliveryDate = queryModel.DeliveryDate,
                SyncFlightDataTime = queryModel.SyncFlightDataTime.ToString(@"hh\:mm\:ss"),
                Capacity = queryModel.Capacity,

                PlaneType = queryModel.PlaneType != null ? new OptionDto
                {
                    Id = queryModel.PlaneType.Id,
                    Display = queryModel.PlaneType.Title,
                }
                : null,

                CurrentAirport = new OptionDto
                {
                    Id = queryModel.CurrentAirport.Id,
                    Display = queryModel.CurrentAirport.Name,
                },

                ConnectingAirports = queryModel.ConnectingAirports.Select(x => new OptionDto
                {
                    Id = x.Id,
                    Display = x.Name,
                }).OrderBy(x => x.Display).ToList(),

                Engines = queryModel.Engines.AsQueryable().Select(this.engineMapper.EntityToDto()).ToList(),

                IsFixed = queryModel.IsFixed,
                RowVersion = (queryModel as IEntityVersioned)?.RowVersionString,
            };
        }

        /// <inheritdoc/>
        protected override PlaneDto QueryModelToDtoListItem(PlaneSpecificQueryModel queryModel)
        {
            return new PlaneDto
            {
                Id = queryModel.Id,
                SiteId = queryModel.Site.Id,
                Msn = queryModel.Msn,
                IsActive = queryModel.IsActive,
                LastFlightDate = queryModel.LastFlightDate,
                DeliveryDate = queryModel.DeliveryDate,
                Capacity = queryModel.Capacity,

                PlaneType = queryModel.PlaneType != null ? new OptionDto
                {
                    Id = queryModel.PlaneType.Id,
                    Display = queryModel.PlaneType.Title,
                }
                : null,

                CurrentAirport = new OptionDto
                {
                    Id = queryModel.CurrentAirport.Id,
                    Display = queryModel.CurrentAirport.Name,
                },

                ConnectingAirports = queryModel.ConnectingAirports.Select(x => new OptionDto
                {
                    Id = x.Id,
                    Display = x.Name,
                }).OrderBy(x => x.Display).ToList(),

                IsFixed = queryModel.IsFixed,
                RowVersion = (queryModel as IEntityVersioned)?.RowVersionString,
            };
        }
    }
}
