// BIADemo only
// <copyright file="MaintenanceTeamMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Maintenance.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Security.Principal;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Bia.Base.Mappers;
    using TheBIADevCompany.BIADemo.Domain.Dto.Maintenance;
    using TheBIADevCompany.BIADemo.Domain.Maintenance.Entities;

    /// <summary>
    /// The mapper used for MaintenanceTeam.
    /// </summary>
    public class MaintenanceTeamMapper : BaseTeamMapper<MaintenanceTeamDto, MaintenanceTeam>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaintenanceTeamMapper"/> class.
        /// </summary>
        /// <param name="principal">The principal.</param>
        public MaintenanceTeamMapper(IPrincipal principal)
            : base(principal)
        {
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override int TeamType => (int)TeamTypeId.MaintenanceTeam;

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<MaintenanceTeam> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<MaintenanceTeam>(base.ExpressionCollection)
                {
                    { HeaderName.Code, maintenanceTeam => maintenanceTeam.Code },
                    { HeaderName.IsActive, maintenanceTeam => maintenanceTeam.IsActive },
                    { HeaderName.IsApproved, maintenanceTeam => maintenanceTeam.IsApproved },
                    { HeaderName.FirstOperation, maintenanceTeam => maintenanceTeam.FirstOperation },
                    { HeaderName.LastOperation, maintenanceTeam => maintenanceTeam.LastOperation },
                    { HeaderName.ApprovedDate, maintenanceTeam => maintenanceTeam.ApprovedDate },
                    { HeaderName.NextOperation, maintenanceTeam => maintenanceTeam.NextOperation },
                    { HeaderName.MaxTravelDuration, maintenanceTeam => maintenanceTeam.MaxTravelDuration },
                    { HeaderName.MaxOperationDuration, maintenanceTeam => maintenanceTeam.MaxOperationDuration },
                    { HeaderName.OperationCount, maintenanceTeam => maintenanceTeam.OperationCount },
                    { HeaderName.IncidentCount, maintenanceTeam => maintenanceTeam.IncidentCount },
                    { HeaderName.TotalOperationDuration, maintenanceTeam => maintenanceTeam.TotalOperationDuration },
                    { HeaderName.AverageOperationDuration, maintenanceTeam => maintenanceTeam.AverageOperationDuration },
                    { HeaderName.TotalTravelDuration, maintenanceTeam => maintenanceTeam.TotalTravelDuration },
                    { HeaderName.AverageTravelDuration, maintenanceTeam => maintenanceTeam.AverageTravelDuration },
                    { HeaderName.TotalOperationCost, maintenanceTeam => maintenanceTeam.TotalOperationCost },
                    { HeaderName.AverageOperationCost, maintenanceTeam => maintenanceTeam.AverageOperationCost },
                    { HeaderName.CurrentAirport, maintenanceTeam => maintenanceTeam.CurrentAirport != null ? maintenanceTeam.CurrentAirport.Name : null },
                    { HeaderName.OperationAirports, maintenanceTeam => maintenanceTeam.OperationAirports.Select(x => x.Name).OrderBy(x => x) },
                    { HeaderName.CurrentCountry, maintenanceTeam => maintenanceTeam.CurrentCountry != null ? maintenanceTeam.CurrentCountry.Name : null },
                    { HeaderName.OperationCountries, maintenanceTeam => maintenanceTeam.OperationCountries.Select(x => x.Name).OrderBy(x => x) },
                };
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(MaintenanceTeamDto dto, ref MaintenanceTeam entity)
        {
            var isCreation = entity == null;
            base.DtoToEntity(dto, ref entity);

            // Map parent relationship 1-* : AircraftMaintenanceCompanyId
            if (isCreation)
            {
                if (dto.AircraftMaintenanceCompanyId == 0)
                {
                    throw new BadRequestException("The parent is mandatory.");
                }

                entity.AircraftMaintenanceCompanyId = dto.AircraftMaintenanceCompanyId;
            }
            else if (entity.AircraftMaintenanceCompanyId != dto.AircraftMaintenanceCompanyId && dto.AircraftMaintenanceCompanyId != 0)
            {
                throw new ForbiddenException("It is forbidden to change the parent.");
            }

            entity.Code = dto.Code;
            entity.IsActive = dto.IsActive;
            entity.IsApproved = dto.IsApproved;
            entity.FirstOperation = dto.FirstOperation;
            entity.LastOperation = dto.LastOperation;
            entity.ApprovedDate = dto.ApprovedDate;
            entity.NextOperation = dto.NextOperation;
            entity.MaxTravelDuration = string.IsNullOrEmpty(dto.MaxTravelDuration) ? null : TimeSpan.Parse(dto.MaxTravelDuration, new CultureInfo("en-US"));
            entity.MaxOperationDuration = TimeSpan.Parse(dto.MaxOperationDuration, new CultureInfo("en-US"));
            entity.OperationCount = dto.OperationCount;
            entity.IncidentCount = dto.IncidentCount;
            entity.TotalOperationDuration = dto.TotalOperationDuration;
            entity.AverageOperationDuration = dto.AverageOperationDuration;
            entity.TotalTravelDuration = dto.TotalTravelDuration;
            entity.AverageTravelDuration = dto.AverageTravelDuration;
            entity.TotalOperationCost = dto.TotalOperationCost;
            entity.AverageOperationCost = dto.AverageOperationCost;

            // Map relationship 1-* : CurrentAirport
            entity.CurrentAirportId = dto.CurrentAirport.Id;

            // Map relationship *-* : OperationAirports
            if (dto.OperationAirports != null && dto.OperationAirports.Count != 0)
            {
                foreach (var operationAirportDto in dto.OperationAirports.Where(x => x.DtoState == DtoState.Deleted))
                {
                    var operationAirport = entity.OperationAirports.FirstOrDefault(x => x.Id == operationAirportDto.Id);
                    if (operationAirport != null)
                    {
                        entity.OperationAirports.Remove(operationAirport);
                    }
                }

                entity.OperationMaintenanceTeamAirports = entity.OperationMaintenanceTeamAirports ?? new List<MaintenanceTeamAirport>();
                foreach (var operationAirportDto in dto.OperationAirports.Where(x => x.DtoState == DtoState.Added))
                {
                    entity.OperationMaintenanceTeamAirports.Add(new MaintenanceTeamAirport
                    {
                        AirportId = operationAirportDto.Id,
                        MaintenanceTeamId = dto.Id,
                    });
                }
            }

            // Map relationship 0..1-* : CurrentCountry
            entity.CurrentCountryId = dto.CurrentCountry?.Id;

            // Map relationship *-* : OperationCountries
            if (dto.OperationCountries != null && dto.OperationCountries.Count != 0)
            {
                foreach (var operationCountryDto in dto.OperationCountries.Where(x => x.DtoState == DtoState.Deleted))
                {
                    var operationCountry = entity.OperationCountries.FirstOrDefault(x => x.Id == operationCountryDto.Id);
                    if (operationCountry != null)
                    {
                        entity.OperationCountries.Remove(operationCountry);
                    }
                }

                entity.OperationMaintenanceTeamCountries = entity.OperationMaintenanceTeamCountries ?? new List<MaintenanceTeamCountry>();
                foreach (var operationCountryDto in dto.OperationCountries.Where(x => x.DtoState == DtoState.Added))
                {
                    entity.OperationMaintenanceTeamCountries.Add(new MaintenanceTeamCountry
                    {
                        CountryId = operationCountryDto.Id,
                        MaintenanceTeamId = dto.Id,
                    });
                }
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<MaintenanceTeam, MaintenanceTeamDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new MaintenanceTeamDto
            {
                AircraftMaintenanceCompanyId = entity.AircraftMaintenanceCompanyId,
                Code = entity.Code,
                IsActive = entity.IsActive,
                IsApproved = entity.IsApproved,
                FirstOperation = entity.FirstOperation,
                LastOperation = entity.LastOperation,
                ApprovedDate = entity.ApprovedDate,
                NextOperation = entity.NextOperation,
                MaxTravelDuration = entity.MaxTravelDuration.Value.ToString(@"hh\:mm\:ss"),
                MaxOperationDuration = entity.MaxOperationDuration.ToString(@"hh\:mm\:ss"),
                OperationCount = entity.OperationCount,
                IncidentCount = entity.IncidentCount,
                TotalOperationDuration = entity.TotalOperationDuration,
                AverageOperationDuration = entity.AverageOperationDuration,
                TotalTravelDuration = entity.TotalTravelDuration,
                AverageTravelDuration = entity.AverageTravelDuration,
                TotalOperationCost = entity.TotalOperationCost,
                AverageOperationCost = entity.AverageOperationCost,

                // Map relationship 1-* : CurrentAirport
                CurrentAirport = new OptionDto
                {
                    Id = entity.CurrentAirport.Id,
                    Display = entity.CurrentAirport.Name,
                },

                // Map relationship *-* : OperationAirports
                OperationAirports = entity.OperationAirports.Select(x => new OptionDto
                {
                    Id = x.Id,
                    Display = x.Name,
                }).OrderBy(x => x.Display).ToList(),

                // Map relationship 0..1-* : CurrentCountry
                CurrentCountry = entity.CurrentCountry != null ? new OptionDto
                {
                    Id = entity.CurrentCountry.Id,
                    Display = entity.CurrentCountry.Name,
                }
                : null,

                // Map relationship *-* : OperationCountries
                OperationCountries = entity.OperationCountries.Select(x => new OptionDto
                {
                    Id = x.Id,
                    Display = x.Name,
                }).OrderBy(x => x.Display).ToList(),

                CanUpdate =
                    this.UserRoleIds.Contains((int)RoleId.MaintenanceTeamAdmin) ||
                    this.UserRoleIds.Contains((int)RoleId.Admin),

                CanMemberListAccess =
                    this.UserRoleIds.Contains((int)RoleId.Admin) ||
                    entity.AircraftMaintenanceCompany.Members.Any(m => m.UserId == this.UserId) ||
                    entity.Members.Any(m => m.UserId == this.UserId),
            });
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToCellMapping"/>
        public override Dictionary<string, Func<string>> DtoToCellMapping(MaintenanceTeamDto dto)
        {
            return new Dictionary<string, Func<string>>(base.DtoToCellMapping(dto))
            {
                { HeaderName.Code, () => CSVString(dto.Code) },
                { HeaderName.IsActive, () => CSVBool(dto.IsActive) },
                { HeaderName.IsApproved, () => CSVBool(dto.IsApproved) },
                { HeaderName.FirstOperation, () => CSVDateTime(dto.FirstOperation) },
                { HeaderName.LastOperation, () => CSVDateTime(dto.LastOperation) },
                { HeaderName.ApprovedDate, () => CSVDate(dto.ApprovedDate) },
                { HeaderName.NextOperation, () => CSVDate(dto.NextOperation) },
                { HeaderName.MaxTravelDuration, () => CSVTime(dto.MaxTravelDuration) },
                { HeaderName.MaxOperationDuration, () => CSVTime(dto.MaxOperationDuration) },
                { HeaderName.OperationCount, () => CSVNumber(dto.OperationCount) },
                { HeaderName.IncidentCount, () => CSVNumber(dto.IncidentCount) },
                { HeaderName.TotalOperationDuration, () => CSVNumber(dto.TotalOperationDuration) },
                { HeaderName.AverageOperationDuration, () => CSVNumber(dto.AverageOperationDuration) },
                { HeaderName.TotalTravelDuration, () => CSVNumber(dto.TotalTravelDuration) },
                { HeaderName.AverageTravelDuration, () => CSVNumber(dto.AverageTravelDuration) },
                { HeaderName.TotalOperationCost, () => CSVNumber(dto.TotalOperationCost) },
                { HeaderName.AverageOperationCost, () => CSVNumber(dto.AverageOperationCost) },
                { HeaderName.CurrentAirport, () => CSVString(dto.CurrentAirport?.Display) },
                { HeaderName.OperationAirports, () => CSVList(dto.OperationAirports) },
                { HeaderName.CurrentCountry, () => CSVString(dto.CurrentCountry?.Display) },
                { HeaderName.OperationCountries, () => CSVList(dto.OperationCountries) },
            };
        }

        /// <inheritdoc/>
        public override void MapEntityKeysInDto(MaintenanceTeam entity, MaintenanceTeamDto dto)
        {
            base.MapEntityKeysInDto(entity, dto);
            dto.AircraftMaintenanceCompanyId = entity.AircraftMaintenanceCompanyId;
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.IncludesForUpdate"/>
        public override Expression<Func<MaintenanceTeam, object>>[] IncludesForUpdate()
        {
            return
            [
                x => x.OperationAirports,
                x => x.OperationCountries,
            ];
        }

        /// <summary>
        /// Header names.
        /// </summary>
        public struct HeaderName
        {
            /// <summary>
            /// Header name for aircraft maintenance company id.
            /// </summary>
            public const string AircraftMaintenanceCompanyId = "aircraftMaintenanceCompanyId";

            /// <summary>
            /// Header name for code.
            /// </summary>
            public const string Code = "code";

            /// <summary>
            /// Header name for is active.
            /// </summary>
            public const string IsActive = "isActive";

            /// <summary>
            /// Header name for is approved.
            /// </summary>
            public const string IsApproved = "isApproved";

            /// <summary>
            /// Header name for first operation.
            /// </summary>
            public const string FirstOperation = "firstOperation";

            /// <summary>
            /// Header name for last operation.
            /// </summary>
            public const string LastOperation = "lastOperation";

            /// <summary>
            /// Header name for approved date.
            /// </summary>
            public const string ApprovedDate = "approvedDate";

            /// <summary>
            /// Header name for next operation.
            /// </summary>
            public const string NextOperation = "nextOperation";

            /// <summary>
            /// Header name for max travel duration.
            /// </summary>
            public const string MaxTravelDuration = "maxTravelDuration";

            /// <summary>
            /// Header name for max operation duration.
            /// </summary>
            public const string MaxOperationDuration = "maxOperationDuration";

            /// <summary>
            /// Header name for operation count.
            /// </summary>
            public const string OperationCount = "operationCount";

            /// <summary>
            /// Header name for incident count.
            /// </summary>
            public const string IncidentCount = "incidentCount";

            /// <summary>
            /// Header name for total operation duration.
            /// </summary>
            public const string TotalOperationDuration = "totalOperationDuration";

            /// <summary>
            /// Header name for average operation duration.
            /// </summary>
            public const string AverageOperationDuration = "averageOperationDuration";

            /// <summary>
            /// Header name for total travel duration.
            /// </summary>
            public const string TotalTravelDuration = "totalTravelDuration";

            /// <summary>
            /// Header name for average travel duration.
            /// </summary>
            public const string AverageTravelDuration = "averageTravelDuration";

            /// <summary>
            /// Header name for total operation cost.
            /// </summary>
            public const string TotalOperationCost = "totalOperationCost";

            /// <summary>
            /// Header name for average operation cost.
            /// </summary>
            public const string AverageOperationCost = "averageOperationCost";

            /// <summary>
            /// Header name for current airport.
            /// </summary>
            public const string CurrentAirport = "currentAirport";

            /// <summary>
            /// Header name for operation airports.
            /// </summary>
            public const string OperationAirports = "operationAirports";

            /// <summary>
            /// Header name for current country.
            /// </summary>
            public const string CurrentCountry = "currentCountry";

            /// <summary>
            /// Header name for operation countries.
            /// </summary>
            public const string OperationCountries = "operationCountries";
        }
    }
}