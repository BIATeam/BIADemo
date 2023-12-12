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
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;

    /// <summary>
    /// The mapper used for member.
    /// </summary>
    public class MemberMapper : BaseMapper<MemberDto, Member, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMapper"/> class.
        /// </summary>
        /// <param name="userContext">the user context</param>
        public MemberMapper(UserContext userContext)
        {
            this.UserContext = userContext;
        }

        /// <inheritdoc/>
        public override ExpressionCollection<Member> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Member>
                {
                    { "Id", member => member.Id },
                    {
                        "Roles", member => member.MemberRoles.Select(x =>
                        x.Role.RoleTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? x.Role.Label).OrderBy(x => x)
                    },
                    { "User", member => member.User.FirstName + " " + member.User.LastName + " (" + member.User.Login + ")" },
                };
            }
        }

        /// <summary>
        /// The user context langage and culture.
        /// </summary>
        private UserContext UserContext { get; set; }

        /// <inheritdoc/>
        public override Expression<Func<Member, MemberDto>> EntityToDto()
        {
            return entity => new MemberDto
            {
                Id = entity.Id,
                TeamId = entity.TeamId,
                User = new OptionDto
                {
                    Id = entity.User.Id,
                    Display = entity.User.SelectDisplay() + (entity.User.IsActive ? string.Empty : " **Disabled**"),
                },
                Roles = entity.MemberRoles.Select(x => new OptionDto { Id = x.RoleId, Display = x.Role.RoleTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? x.Role.Label }),
            };
        }

        /// <inheritdoc/>
        public override void MapEntityKeysInDto(Member entity, MemberDto dto)
        {
            dto.Id = entity.Id;
            dto.TeamId = entity.TeamId;
        }

        /// <inheritdoc/>
        public override void DtoToEntity(MemberDto dto, Member entity)
        {
            if (entity == null)
            {
                entity = new Member();
            }

            entity.Id = dto.Id;
            entity.TeamId = dto.TeamId;
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