// <copyright file="TeamMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.User.Mappers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;

    // Begin BIADemo
    // BIAToolKit - Begin Partial TeamMapperUsing MaintenanceTeam
    // BIAToolKit - Begin Nested AncestorTeam AircraftMaintenanceCompany
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Entities;

    // BIAToolKit - End Nested AncestorTeam AircraftMaintenanceCompany
    // BIAToolKit - End Partial TeamMapperUsing MaintenanceTeam
    // End BIADemo
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

    // BIAToolKit - Begin TeamMapperUsing
    // BIAToolKit - End TeamMapperUsing

    /// <summary>
    /// The mapper used for site.
    /// </summary>
    public class TeamMapper : BaseMapper<TeamDto, Team, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamMapper"/> class.
        /// </summary>
        /// <param name="userContext">the user context.</param>
        public TeamMapper(UserContext userContext)
        {
            this.UserContext = userContext;
        }

        /// <summary>
        /// The user context language and culture.
        /// </summary>
        private UserContext UserContext { get; set; }

        /// <summary>
        /// Create a site DTO from a entity.
        /// </summary>
        /// <returns>The site DTO.</returns>
        public override Expression<Func<Team, TeamDto>> EntityToDto()
        {
            return entity => new TeamDto { Id = entity.Id, Title = entity.Title, TeamTypeId = entity.TeamTypeId };
        }

        /// <summary>
        /// Create a site DTO from a entity.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// The site DTO.
        /// </returns>
        public Expression<Func<Team, TeamDto>> EntityToDto(int userId)
        {
            return entity => new TeamDto
            {
                Id = entity.Id,
                Title = entity.Title,
                TeamTypeId = entity.TeamTypeId,
                IsDefault = entity.Members.Any(member => member.UserId == userId && member.IsDefault),
                Roles = entity.Members.FirstOrDefault(member => member.UserId == userId).MemberRoles.Select(mem => new RoleDto
                {
                    Id = mem.RoleId,
                    IsDefault = mem.IsDefault,
                    Code = mem.Role.Code,
                    Display = mem.Role.RoleTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? mem.Role.Label,
                }).ToList(),

                // Map the parent properties if usefull in project:
                ParentTeamId = GetParentTeamId(entity),
                ParentTeamTitle = GetParentTeamTitle(entity),
            };
        }

        /// <summary>
        /// Retrieve the parent team id.
        /// </summary>
        /// <param name="team">Child <see cref="Team"/>.</param>
        /// <returns>Parent Team id as <see cref="int"/>.</returns>
        private static int GetParentTeamId(Team team)
        {
            return team switch
            {
                // Begin BIADemo
                // BIAToolKit - Begin Partial TeamMapperParentTeamId MaintenanceTeam
                // BIAToolKit - Begin Nested Parent AircraftMaintenanceCompanyId
                MaintenanceTeam maintenanceTeam => team.TeamTypeId == (int)TeamTypeId.MaintenanceTeam ? maintenanceTeam.AircraftMaintenanceCompanyId : 0,

                // BIAToolKit - End Nested Parent AircraftMaintenanceCompanyId
                // BIAToolKit - End Partial TeamMapperParentTeamId MaintenanceTeam
                // End BIADemo
                // BIAToolKit - Begin TeamMapperParentTeamId
                // BIAToolKit - End TeamMapperParentTeamId
                _ => 0
            };
        }

        /// <summary>
        /// Retrieve the parent team title.
        /// </summary>
        /// <param name="team">Child <see cref="Team"/>.</param>
        /// <returns>Parent Team title as <see cref="string"/>.</returns>
        private static string GetParentTeamTitle(Team team)
        {
            return team switch
            {
                // Begin BIADemo
                // BIAToolKit - Begin Partial TeamMapperParentTeamTitle MaintenanceTeam
                // BIAToolKit - Begin Nested AncestorTeam AircraftMaintenanceCompany
                MaintenanceTeam maintenanceTeam => team.TeamTypeId == (int)TeamTypeId.MaintenanceTeam ? maintenanceTeam.AircraftMaintenanceCompany.Title : string.Empty,

                // BIAToolKit - End Nested AncestorTeam AircraftMaintenanceCompany
                // BIAToolKit - End Partial TeamMapperParentTeamTitle MaintenanceTeam
                // End BIADemo
                // BIAToolKit - Begin TeamMapperParentTeamTitle
                // BIAToolKit - End TeamMapperParentTeamTitle
                _ => string.Empty
            };
        }
    }
}