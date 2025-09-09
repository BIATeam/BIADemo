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
    using BIA.Net.Core.Domain.Mapper;
    using TheBIADevCompany.BIADemo.Domain.Dto.Fleet;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;
    using static TheBIADevCompany.BIADemo.Crosscutting.Common.Rights;

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
                    { HeaderName.FlightHours, pilot => pilot.FlightHours },
                };
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
            entity.FlightHours = dto.FlightHours;
        }

        /// <inheritdoc />
        public override Expression<Func<Pilot, PilotDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new PilotDto
            {
                IdentificationNumber = entity.IdentificationNumber,
                FlightHours = entity.FlightHours,
            });
        }

        /// <inheritdoc />
        public override Dictionary<string, Func<string>> DtoToCellMapping(PilotDto dto)
        {
            return new Dictionary<string, Func<string>>(base.DtoToCellMapping(dto))
            {
                { HeaderName.IdentificationNumber, () => CSVString(dto.IdentificationNumber) },
                { HeaderName.FlightHours, () => CSVNumber(dto.FlightHours) },
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
            /// Header name for flightHours.
            /// </summary>
            public const string FlightHours = "flightHours";
        }
    }
}