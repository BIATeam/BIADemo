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
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;

    /// <summary>
    /// The mapper used for member.
    /// </summary>
    public class MemberMapper : BaseMapper<MemberDto, Member>
    {
        /// <inheritdoc/>
        public override ExpressionCollection<Member> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Member>
                {
                    { "Id", member => member.Id },
                    { "Roles", member => member.MemberRoles.Select(x => x.Role.Code).OrderBy(x => x) },
                    { "User", member => member.User.FirstName + " " + member.User.LastName + " (" + member.User.Login + ")" },
                };
            }
        }

        /// <inheritdoc/>
        public override Expression<Func<Member, MemberDto>> EntityToDto()
        {
            return entity => new MemberDto
            {
                Id = entity.Id,
                SiteId = entity.SiteId,
                User = new OptionDto
                {
                    Id = entity.User.Id,
                    Display = entity.User.FirstName + " " + entity.User.LastName + " (" + entity.User.Login + ")" + (entity.User.IsActive ? string.Empty : " **Disabled**"),
                },
                Roles = entity.MemberRoles.Select(s => new OptionDto { Id = s.RoleId, Display = s.Role.Code }),
            };
        }

        /// <inheritdoc/>
        public override void MapEntityKeysInDto(Member entity, MemberDto dto)
        {
            dto.Id = entity.Id;
            dto.SiteId = entity.SiteId;
        }

        /// <inheritdoc/>
        public override void DtoToEntity(MemberDto dto, Member entity)
        {
            if (entity == null)
            {
                entity = new Member();
            }

            entity.Id = dto.Id;
            entity.SiteId = dto.SiteId;
            entity.UserId = dto.User.Id;
            foreach (var roleDto in dto.Roles.Where(w => w.DtoState == DtoState.Deleted))
            {
                var memberRole = entity.MemberRoles.FirstOrDefault(f => f.RoleId == roleDto.Id && f.MemberId == dto.Id);
                if (memberRole == null)
                {
                    continue;
                }

                entity.MemberRoles.Remove(memberRole);
            }

            entity.MemberRoles = entity.MemberRoles ?? new List<MemberRole>();
            foreach (var roleDto in dto.Roles.Where(w => w.DtoState == DtoState.Added))
            {
                entity.MemberRoles.Add(new MemberRole { RoleId = roleDto.Id, MemberId = dto.Id });
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.IncludesForUpdate"/>
        public override Expression<Func<Member, object>>[] IncludesForUpdate()
        {
            return new Expression<Func<Member, object>>[] { member => member.MemberRoles };
        }
    }
}