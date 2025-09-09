// BIADemo only
// <copyright file="FlightMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Mapper;
    using TheBIADevCompany.BIADemo.Domain.Dto.Fleet;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    /// <summary>
    /// The mapper used for Flight.
    /// </summary>
    public class FlightMapper : BaseMapper<FlightDto, Flight, string>
    {
        /// <inheritdoc />
        public override ExpressionCollection<Flight> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Flight>(base.ExpressionCollection)
                {
                    { HeaderName.DepartureAirport, flight => flight.DepartureAirport.Name },
                    { HeaderName.ArrivalAirport, flight => flight.ArrivalAirport.Name },
                };
            }
        }

        /// <inheritdoc />
        public override void DtoToEntity(FlightDto dto, ref Flight entity)
        {
            var isCreation = entity == null;
            base.DtoToEntity(dto, ref entity);

            // Map parent relationship 1-* : SiteId
            if (isCreation)
            {
                if (dto.SiteId == 0)
                {
                    throw new BadRequestException("The parent is mandatory.");
                }

                entity.SiteId = dto.SiteId;
            }
            else if (entity.SiteId != dto.SiteId && dto.SiteId != 0)
            {
                throw new ForbiddenException("It is forbidden to change the parent.");
            }

            entity.DepartureAirportId = dto.DepartureAirport.Id;
            entity.ArrivalAirportId = dto.ArrivalAirport.Id;
        }

        /// <inheritdoc />
        public override Expression<Func<Flight, FlightDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new FlightDto
            {
                SiteId = entity.SiteId,
                DepartureAirport = new OptionDto
                {
                    Id = entity.DepartureAirport.Id,
                    Display = entity.DepartureAirport.Name,
                },
                ArrivalAirport = new OptionDto
                {
                    Id = entity.ArrivalAirport.Id,
                    Display = entity.ArrivalAirport.Name,
                },
            });
        }

        /// <inheritdoc />
        public override Dictionary<string, Func<string>> DtoToCellMapping(FlightDto dto)
        {
            return new Dictionary<string, Func<string>>(base.DtoToCellMapping(dto))
            {
                { HeaderName.DepartureAirport, () => CSVString(dto.DepartureAirport?.Display) },
                { HeaderName.ArrivalAirport, () => CSVString(dto.ArrivalAirport?.Display) },
            };
        }

        /// <inheritdoc/>
        public override void MapEntityKeysInDto(Flight entity, FlightDto dto)
        {
            base.MapEntityKeysInDto(entity, dto);
            dto.SiteId = entity.SiteId;
        }

        /// <inheritdoc />
        public override Expression<Func<Flight, object>>[] IncludesForUpdate()
        {
            return [];
        }

        /// <summary>
        /// Header names.
        /// </summary>
        public struct HeaderName
        {
            /// <summary>
            /// Header name for departure airport.
            /// </summary>
            public const string DepartureAirport = "departureAirport";

            /// <summary>
            /// Header name for arrival airport.
            /// </summary>
            public const string ArrivalAirport = "arrivalAirport";
        }
    }
}