// BIADemo only
// <copyright file="PilotMapper.cs" company="TheBIADevCompany">
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
    /// The mapper used for Pilot.
    /// </summary>
    public class PilotMapper : BaseMapper<PilotDto, Pilot, Guid>
    {
        /// <inheritdoc />
        public override ExpressionCollection<Pilot> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Pilot>(base.ExpressionCollection)
                {
                    { HeaderName.IdentificationNumber, pilot => pilot.IdentificationNumber },
                    { HeaderName.FirstName, pilot => pilot.FirstName },
                    { HeaderName.LastName, pilot => pilot.LastName },
                    { HeaderName.Birthdate, pilot => pilot.Birthdate },
                    { HeaderName.CPLDate, pilot => pilot.CPLDate },
                    { HeaderName.BaseAirport, pilot => pilot.BaseAirport != null ? pilot.BaseAirport.Name : null },
                    { HeaderName.FlightHours, pilot => pilot.FlightHours },
                    { HeaderName.FirstFlightDate, pilot => pilot.FirstFlightDate },
                    { HeaderName.LastFlightDate, pilot => pilot.LastFlightDate },
                };
            }
        }

        /// <inheritdoc />
        public override ExpressionCollection<Pilot> ExpressionCollectionFilterIn
        {
            get
            {
                return new ExpressionCollection<Pilot>(
                    base.ExpressionCollectionFilterIn,
                    new ExpressionCollection<Pilot>()
                    {
                        { HeaderName.BaseAirport, plane => plane.BaseAirport.Id },
                    });
            }
        }

        /// <inheritdoc />
        public override void DtoToEntity(PilotDto dto, ref Pilot entity)
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

            entity.IdentificationNumber = dto.IdentificationNumber;
            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.Birthdate = dto.Birthdate;
            entity.CPLDate = dto.CPLDate;
            entity.FlightHours = dto.FlightHours;
            entity.FirstFlightDate = dto.FirstFlightDate;
            entity.LastFlightDate = dto.LastFlightDate;

            // Map relationship 1-* : BaseAirport
            entity.BaseAirportId = dto.BaseAirport?.Id;
        }

        /// <inheritdoc />
        public override Expression<Func<Pilot, PilotDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new PilotDto
            {
                IdentificationNumber = entity.IdentificationNumber,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Birthdate = entity.Birthdate,
                CPLDate = entity.CPLDate,
                FlightHours = entity.FlightHours,
                FirstFlightDate = entity.FirstFlightDate,
                LastFlightDate = entity.LastFlightDate,

                // Map relationship 1-* : BaseAirport
                BaseAirport = entity.BaseAirport != null ? new OptionDto
                {
                    Id = entity.BaseAirport.Id,
                    Display = entity.BaseAirport.Name,
                }
                : null,
            });
        }

        /// <inheritdoc />
        public override Dictionary<string, Func<string>> DtoToCellMapping(PilotDto dto)
        {
            return new Dictionary<string, Func<string>>(base.DtoToCellMapping(dto))
            {
                { HeaderName.IdentificationNumber, () => CSVString(dto.IdentificationNumber) },
                { HeaderName.BaseAirport, () => CSVString(dto.FirstName) },
                { HeaderName.BaseAirport, () => CSVString(dto.LastName) },
                { HeaderName.BaseAirport, () => CSVDate(dto.Birthdate) },
                { HeaderName.BaseAirport, () => CSVDate(dto.CPLDate) },
                { HeaderName.BaseAirport, () => CSVString(dto.BaseAirport?.Display) },
                { HeaderName.FlightHours, () => CSVNumber(dto.FlightHours) },
                { HeaderName.FirstFlightDate, () => CSVDateTime(dto.FirstFlightDate.UtcDateTime) },
                { HeaderName.LastFlightDate, () => CSVDateTime(dto.LastFlightDate?.UtcDateTime) },
            };
        }

        /// <inheritdoc/>
        public override void MapEntityKeysInDto(Pilot entity, PilotDto dto)
        {
            base.MapEntityKeysInDto(entity, dto);
            dto.SiteId = entity.SiteId;
        }

        /// <inheritdoc />
        public override Expression<Func<Pilot, object>>[] IncludesForUpdate()
        {
            return [];
        }

        /// <summary>
        /// Header names.
        /// </summary>
        public struct HeaderName
        {
            /// <summary>
            /// Header name for identificationNumber.
            /// </summary>
            public const string IdentificationNumber = "identificationNumber";

            /// <summary>
            /// Header name for firs name.
            /// </summary>
            public const string FirstName = "firstName";

            /// <summary>
            /// Header name for last name.
            /// </summary>
            public const string LastName = "lastName";

            /// <summary>
            /// Header name for birthdate.
            /// </summary>
            public const string Birthdate = "birthdate";

            /// <summary>
            /// Header name for cpl date.
            /// </summary>
            public const string CPLDate = "cplDate";

            /// <summary>
            /// Header name for base airport.
            /// </summary>
            public const string BaseAirport = "baseAirport";

            /// <summary>
            /// Header name for flightHours.
            /// </summary>
            public const string FlightHours = "flightHours";

            /// <summary>
            /// Header name for first flight date.
            /// </summary>
            public const string FirstFlightDate = "firstFlightDate";

            /// <summary>
            /// Header name for last flight date.
            /// </summary>
            public const string LastFlightDate = "lastFlightDate";
        }
    }
}