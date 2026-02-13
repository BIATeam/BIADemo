// BIADemo only
// <copyright file="PilotListMapper.cs" company="TheBIADevCompany">
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

    /// <summary>
    /// The mapper used for Pilot list.
    /// </summary>
    public class PilotListMapper : BaseMapper<PilotListDto, Pilot, Guid>
    {
        /// <inheritdoc />
        public override ExpressionCollection<Pilot> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Pilot>(base.ExpressionCollection)
                {
                    { HeaderName.IdentificationNumber, pilot => pilot.IdentificationNumber },
                    { HeaderName.Name, pilot => pilot.FirstName + " " + pilot.LastName },
                    { HeaderName.FlightHours, pilot => pilot.FlightHours },
                };
            }
        }

        /// <inheritdoc />
        public override void DtoToEntity(PilotListDto dto, ref Pilot entity)
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
        public override Expression<Func<Pilot, PilotListDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new PilotListDto
            {
                IdentificationNumber = entity.IdentificationNumber,
                Name = entity.FirstName + " " + entity.LastName,
                FlightHours = entity.FlightHours,
            });
        }

        /// <inheritdoc />
        public override Dictionary<string, Func<string>> DtoToCellMapping(PilotListDto dto)
        {
            return new Dictionary<string, Func<string>>(base.DtoToCellMapping(dto))
            {
                { HeaderName.IdentificationNumber, () => CSVString(dto.IdentificationNumber) },
                { HeaderName.Name, () => CSVString(dto.Name) },
                { HeaderName.FlightHours, () => CSVNumber(dto.FlightHours) },
            };
        }

        /// <inheritdoc/>
        public override void MapEntityKeysInDto(Pilot entity, PilotListDto dto)
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
            /// Header name for name.
            /// </summary>
            public const string Name = "name";

            /// <summary>
            /// Header name for flightHours.
            /// </summary>
            public const string FlightHours = "flightHours";
        }
    }
}