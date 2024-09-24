// BIADemo only
// <copyright file="MaintenanceTeamMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompanyModule.Aggregate
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Principal;
    using BIA.Net.Core.Common.Helpers;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The mapper used for AircraftMaintenanceCompany.
    /// </summary>
    public class MaintenanceTeamMapper : TTeamMapper<MaintenanceTeamDto, MaintenanceTeam>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaintenanceTeamMapper"/> class.
        /// </summary>
        /// <param name="principal">The principal.</param>
        public MaintenanceTeamMapper(IPrincipal principal)
            : base(principal)
        {
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<MaintenanceTeam> ExpressionCollection
        {
            // It is not necessary to implement this function if you to not use the mapper for filtered list. In BIADemo it is use only for Calc SpreadSheet.
            get
            {
                return new ExpressionCollection<MaintenanceTeam>
                {
                    { "Id", aircraftMaintenanceCompany => aircraftMaintenanceCompany.Id },
                    { "Title", aircraftMaintenanceCompany => aircraftMaintenanceCompany.Title },
                };
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(MaintenanceTeamDto dto, MaintenanceTeam entity)
        {
            if (entity == null)
            {
                entity = new MaintenanceTeam();
            }

            entity.Id = dto.Id;
            entity.Title = dto.Title;
            entity.TeamTypeId = (int)TeamTypeId.MaintenanceTeam;
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
            entity.TotalOperationCost = dto.TotalOperationCost;
            entity.AverageOperationCost = dto.AverageOperationCost;
            entity.TotalTravelDuration = dto.TotalTravelDuration;
            entity.AverageTravelDuration = dto.AverageTravelDuration;

            // Mapping relationship 1-* : AircraftMaintenanceCompany
            if (dto.AircraftMaintenanceCompanyId != 0)
            {
                entity.AircraftMaintenanceCompanyId = dto.AircraftMaintenanceCompanyId;
            }

            // Mapping relationship 0..1-* : Country
            entity.CurrentCountryId = dto.CurrentCountry?.Id;

            // Mapping relationship 0..1-* : Airport
            entity.CurrentAirportId = dto.CurrentAirport.Id;

            // Mapping relationship *-* : ICollection<OptionDto> OperationAirports
            if (dto.OperationAirports != null && dto.OperationAirports?.Any() == true)
            {
                foreach (var airportDto in dto.OperationAirports.Where(x => x.DtoState == DtoState.Deleted))
                {
                    var connectingAirport = entity.OperationAirports.FirstOrDefault(x => x.Id == airportDto.Id);
                    if (connectingAirport != null)
                    {
                        entity.OperationAirports.Remove(connectingAirport);
                    }
                }

                entity.OperationMaintenanceTeamAirports = entity.OperationMaintenanceTeamAirports ?? new List<MaintenanceTeamAirport>();
                foreach (var airportDto in dto.OperationAirports.Where(w => w.DtoState == DtoState.Added))
                {
                    entity.OperationMaintenanceTeamAirports.Add(new MaintenanceTeamAirport
                    { AirportId = airportDto.Id, MaintenanceTeamId = dto.Id });
                }
            }

            if (dto.OperationCountries != null && dto.OperationCountries?.Any() == true)
            {
                foreach (var planeTypeDto in dto.OperationCountries.Where(x => x.DtoState == DtoState.Deleted))
                {
                    var similarType = entity.OperationCountries.FirstOrDefault(x => x.Id == planeTypeDto.Id);
                    if (similarType != null)
                    {
                        entity.OperationCountries.Remove(similarType);
                    }
                }

                entity.OperationMaintenanceTeamCountries = entity.OperationMaintenanceTeamCountries ?? new List<MaintenanceTeamCountry>();
                foreach (var countryDto in dto.OperationCountries.Where(w => w.DtoState == DtoState.Added))
                {
                    entity.OperationMaintenanceTeamCountries.Add(new MaintenanceTeamCountry
                    { CountryId = countryDto.Id, MaintenanceTeamId = dto.Id });
                }
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<MaintenanceTeam, MaintenanceTeamDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(x => new MaintenanceTeamDto
            {
                Code = x.Code,
                IsActive = x.IsActive,
                IsApproved = x.IsApproved,
                FirstOperation = x.FirstOperation,
                LastOperation = x.LastOperation,
                ApprovedDate = x.ApprovedDate,
                NextOperation = x.NextOperation,
                MaxTravelDuration = x.MaxTravelDuration.Value.ToString(@"hh\:mm\:ss"),
                MaxOperationDuration = x.MaxOperationDuration.ToString(@"hh\:mm\:ss"),
                OperationCount = x.OperationCount,
                IncidentCount = x.IncidentCount,
                TotalOperationDuration = x.TotalOperationDuration,
                AverageOperationDuration = x.AverageOperationDuration,
                TotalTravelDuration = x.TotalTravelDuration,
                AverageTravelDuration = x.AverageTravelDuration,
                TotalOperationCost = x.TotalOperationCost,
                AverageOperationCost = x.AverageOperationCost,

                // Mapping relationship 1-* : AircraftMaintenanceCompany
                AircraftMaintenanceCompanyId = x.AircraftMaintenanceCompanyId,

                // Should correspond to MaintenanceTeam_Update permission (but without use the roles *_Member that is not determined at list display)
                CanUpdate =
                    this.UserRoleIds.Contains((int)RoleId.MaintenanceTeamAdmin) ||
                    this.UserRoleIds.Contains((int)RoleId.Admin),

                // Should correspond to MaintenanceTeam_Member_List_Access (but without use the roles *_Member that is not determined at list display)
                CanMemberListAccess =
                    this.UserRoleIds.Contains((int)RoleId.Admin) ||
                    x.AircraftMaintenanceCompany.Members.Any(m => m.UserId == this.UserId) ||
                    x.Members.Any(m => m.UserId == this.UserId),

                // Mapping relationship 0..1-* : Country
                CurrentCountry = x.CurrentCountry != null ? new OptionDto
                {
                    Id = x.CurrentCountry.Id,
                    Display = x.CurrentCountry.Name,
                }
                : null,

                CurrentAirport = new OptionDto
                {
                    Id = x.CurrentAirport.Id,
                    Display = x.CurrentAirport.Name,
                },

                // Mapping relationship *-* : ICollection<Airports>
                OperationAirports = x.OperationAirports.Select(ca => new OptionDto
                {
                    Id = ca.Id,
                    Display = ca.Name,
                }).OrderBy(x => x.Display).ToList(),

                OperationCountries = x.OperationCountries.Select(ca => new OptionDto
                {
                    Id = ca.Id,
                    Display = ca.Name,
                }).OrderBy(x => x.Display).ToList(),
            });
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Func<MaintenanceTeamDto, object[]> DtoToRecord(List<string> headerNames = null)
        {
            return x => (new object[]
            {
                CSVString(x.Title),
                CSVString(x.Code),
                CSVBool(x.IsActive),
                CSVBool(x.IsApproved.GetValueOrDefault()),
                CSVDateTime(x.FirstOperation),
                CSVDateTime(x.LastOperation),
                CSVDate(x.ApprovedDate),
                CSVDate(x.NextOperation),
                CSVTime(x.MaxTravelDuration),
                CSVTime(x.MaxOperationDuration),
                CSVNumber(x.OperationCount),
                CSVNumber(x.IncidentCount),
                CSVNumber(x.TotalOperationDuration),
                CSVNumber(x.AverageOperationDuration),
                CSVNumber(x.TotalTravelDuration),
                CSVNumber(x.AverageTravelDuration),
                CSVNumber(x.TotalOperationCost),
                CSVNumber(x.AverageOperationCost),
                CSVString(x.CurrentAirport.Display),
                CSVString(x.CurrentCountry?.Display),
            });
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Expression<Func<MaintenanceTeam, object>>[] IncludesForUpdate()
        {
            return new Expression<Func<MaintenanceTeam, object>>[] { x => x.OperationAirports, x => x.OperationCountries };
        }
    }
}