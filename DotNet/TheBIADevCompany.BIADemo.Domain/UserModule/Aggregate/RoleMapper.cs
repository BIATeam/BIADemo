// <copyright file="RoleMapper.cs" company="TheBIADevCompany">
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
    /// The mapper used for role.
    /// </summary>
    public class RoleMapper : BaseMapper<RoleDto, Role, int>
    {
        /// <summary>
        /// Create a role DTO from an entity.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <param name="userId">The user Id.</param>
        /// <returns>
        /// The role DTO.
        /// </returns>
        public static Expression<Func<Role, RoleDto>> EntityToDto(int teamId, int userId)
        {
            return entity => new RoleDto
            {
                Id = entity.Id,
                Display = "TODO: Remove this function",
                Code = entity.Code,
                IsDefault = entity.MemberRoles.Any(mr => mr.Member.UserId == userId && mr.Member.TeamId == teamId && mr.IsDefault),
            };
        }
    }
}