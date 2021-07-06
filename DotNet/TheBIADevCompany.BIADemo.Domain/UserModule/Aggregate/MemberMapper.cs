// <copyright file="MemberMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;

    /// <summary>
    /// The mapper used for member.
    /// </summary>
    public class MemberMapper : BaseMapper<MemberDto, Member>
    {
        /// <summary>
        /// Gets or sets the collection used for expressions to access fields.
        /// </summary>
        public override ExpressionCollection<Member> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Member>
                   {
                       { "Id", member => member.Id },
                       { "SiteId", member => member.SiteId },
                       { "UserId", member => member.UserId },
                   };
            }
        }

        /// <summary>
        /// Create a member DTO from an entity.
        /// </summary>
        /// <returns>The member DTO.</returns>
        public override Expression<Func<Member, MemberDto>> EntityToDto()
        {
            return entity => new MemberDto
            {
                Id = entity.Id,
                SiteId = entity.SiteId,
                UserId = entity.UserId,
                UserLastName = entity.User.LastName,
                UserFirstName = entity.User.FirstName,
                UserLogin = entity.User.Login,
                Roles = entity.MemberRoles.Select(s => new MemberRoleDto { RoleId = s.RoleId, MemberId = entity.Id }),
            };
        }

        /// <summary>
        /// Create a member entity from a DTO.
        /// </summary>
        /// <param name="dto">The member DTO.</param>
        /// <param name="entity">The entity to update.</param>
        public override void DtoToEntity(MemberDto dto, Member entity)
        {
            if (entity == null)
            {
                entity = new Member();
            }

            entity.Id = dto.Id;
            entity.SiteId = dto.SiteId;
            entity.UserId = dto.UserId;
            foreach (var roleDto in dto.Roles.Where(w => w.DtoState == DtoState.Deleted))
            {
                var memberRole =
                    entity.MemberRoles.FirstOrDefault(f =>
                        f.RoleId == roleDto.RoleId && f.MemberId == roleDto.MemberId);
                if (memberRole == null)
                {
                    continue;
                }

                entity.MemberRoles.Remove(memberRole);
            }

            entity.MemberRoles = entity.MemberRoles ?? new List<MemberRole>();
            foreach (var roleDto in dto.Roles.Where(w => w.DtoState == DtoState.Added))
            {
                entity.MemberRoles.Add(new MemberRole { RoleId = roleDto.RoleId, MemberId = roleDto.MemberId });
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.IncludesForUpdate"/>
        public override Expression<Func<Member, object>>[] IncludesForUpdate()
        {
            return new Expression<Func<Member, object>>[] { member => member.MemberRoles };
        }
    }
}