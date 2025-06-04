// <copyright file="TeamMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.User.Mappers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.User.Entities;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;

    // BIAToolKit - Begin TeamMapperUsing
    // Begin BIAToolKit Generation Ignore
    // BIAToolKit - Begin Partial TeamMapperUsing MaintenanceTeam
    using TheBIADevCompany.BIADemo.Domain.Maintenance.Entities;

    // BIAToolKit - End Partial TeamMapperUsing MaintenanceTeam
    // End BIAToolKit Generation Ignore
    // BIAToolKit - End TeamMapperUsing

    /// <summary>
    /// The mapper used for site.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="TeamMapper"/> class.
    /// </remarks>
    /// <param name="userContext">the user context.</param>
    public class TeamMapper(UserContext userContext) : BaseMapper<BaseDtoVersionedTeam, Team, int>, ITeamMapper
    {
        /// <summary>
        /// The user context language and culture.
        /// </summary>
        private UserContext UserContext { get; set; } = userContext;

        /// <summary>
        /// Create a site DTO from a entity.
        /// </summary>
        /// <returns>The site DTO.</returns>
        public override Expression<Func<Team, BaseDtoVersionedTeam>> EntityToDto()
        {
            return entity => new BaseDtoVersionedTeam { Id = entity.Id, Title = entity.Title, TeamTypeId = entity.TeamTypeId };
        }

        /// <summary>
        /// Create a site DTO from a entity.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// The site DTO.
        /// </returns>
        public Expression<Func<Team, BaseDtoVersionedTeam>> EntityToDto(int userId)
        {
            return entity => new BaseDtoVersionedTeam
            {
                Id = entity.Id,
                Title = entity.Title,
                TeamTypeId = entity.TeamTypeId,
                IsDefault = entity.UserDefaultTeams.Any(x => x.UserId == userId && x.TeamId == entity.Id),
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
                // BIAToolKit - Begin TeamMapperParentTeamId
                // Begin BIAToolKit Generation Ignore
                // BIAToolKit - Begin Partial TeamMapperParentTeamId MaintenanceTeam
                MaintenanceTeam maintenanceTeam => team.TeamTypeId == (int)TeamTypeId.MaintenanceTeam ? maintenanceTeam.AircraftMaintenanceCompanyId : 0,

                // BIAToolKit - End Partial TeamMapperParentTeamId MaintenanceTeam
                // End BIAToolKit Generation Ignore
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
                // BIAToolKit - Begin TeamMapperParentTeamTitle
                // Begin BIAToolKit Generation Ignore
                // BIAToolKit - Begin Partial TeamMapperParentTeamTitle MaintenanceTeam
                MaintenanceTeam maintenanceTeam => team.TeamTypeId == (int)TeamTypeId.MaintenanceTeam ? maintenanceTeam.AircraftMaintenanceCompany.Title : string.Empty,

                // BIAToolKit - End Partial TeamMapperParentTeamTitle MaintenanceTeam
                // End BIAToolKit Generation Ignore
                // BIAToolKit - End TeamMapperParentTeamTitle
                _ => string.Empty
            };
        }
    }
}