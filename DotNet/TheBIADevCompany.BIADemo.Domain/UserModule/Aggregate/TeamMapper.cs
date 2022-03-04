// <copyright file="TeamMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.User;

    /// <summary>
    /// The mapper used for site.
    /// </summary>
    public class TeamMapper : BaseMapper<TeamDto, Team, int>
    {
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
            };
        }
    }
}