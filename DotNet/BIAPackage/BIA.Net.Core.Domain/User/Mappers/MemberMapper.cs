// <copyright file="MemberMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.User;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// The mapper used for member.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="MemberMapper"/> class.
    /// </remarks>
    /// <param name="userContext">the user context.</param>
    public class MemberMapper(UserContext userContext) : BaseMapper<MemberDto, Member, int>
    {
        /// <inheritdoc/>
        public override ExpressionCollection<Member> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Member>(base.ExpressionCollection)
                {
                    {
                        HeaderName.Roles,
                        member => member.MemberRoles
                        .SelectMany(
                            memberRole => memberRole.Role.RoleTranslations
                            .Where(roleTranslation => roleTranslation.Language.Code == this.UserContext.Language)
                            .Select(roleTranslation => roleTranslation.Label))
                        .Union(
                            member.MemberRoles
                            .Where(memberRole => !memberRole.Role.RoleTranslations.Any(rt => rt.Language.Code == this.UserContext.Language))
                            .Select(memberRole => memberRole.Role.Label))
                        .OrderBy(x => x)
                    },
                    { HeaderName.User, member => member.User.LastName + " " + member.User.FirstName + " (" + member.User.Login + ")" },
                    { HeaderName.FirstName, member => member.User.FirstName },
                    { HeaderName.LastName, member => member.User.LastName },
                    { HeaderName.Login, member => member.User.Login },
                    { HeaderName.IsActive, member => member.User.IsActive },
                };
            }
        }

        /// <inheritdoc cref="BaseEntityMapper{Member}.ExpressionCollectionFilter"/>
        public override ExpressionCollection<Member> ExpressionCollectionFilterIn
        {
            get
            {
                return new ExpressionCollection<Member>(
                    base.ExpressionCollectionFilterIn,
                    new ExpressionCollection<Member>()
                    {
                        { HeaderName.Roles, member => member.MemberRoles.Select(memberRole => memberRole.Role.Id) },
                        { HeaderName.User, member => member.User.Id },
                    });
            }
        }

        /// <summary>
        /// The user context language and culture.
        /// </summary>
        private UserContext UserContext { get; set; } = userContext;

        /// <inheritdoc/>
        public override void DtoToEntity(MemberDto dto, ref Member entity)
        {
            base.DtoToEntity(dto, ref entity);

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

        /// <inheritdoc/>
        public override Expression<Func<Member, MemberDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new MemberDto
            {
                TeamId = entity.TeamId,
                User = new OptionDto
                {
                    Id = entity.User.Id,
                    Display = entity.User.Display() + (entity.User.IsActive ? string.Empty : " **Disabled**"),
                },
                FirstName = entity.User.FirstName,
                LastName = entity.User.LastName,
                Login = entity.User.Login,
                IsActive = entity.User.IsActive,
                Roles = entity.MemberRoles.Select(x => new OptionDto { Id = x.RoleId, Display = x.Role.RoleTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? x.Role.Label }),
            });
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToCellMapping"/>
        public override Dictionary<string, Func<string>> DtoToCellMapping(MemberDto dto)
        {
            return new Dictionary<string, Func<string>>(base.DtoToCellMapping(dto))
            {
                { HeaderName.User, () => CSVString(dto.User?.Display) },
                { HeaderName.LastName, () => CSVString(dto.LastName) },
                { HeaderName.FirstName, () => CSVString(dto.FirstName) },
                { HeaderName.Login, () => CSVString(dto.Login) },
                { HeaderName.IsActive, () => CSVBool(dto.IsActive) },
                { HeaderName.Roles, () => CSVList(dto.Roles) },
            };
        }

        /// <inheritdoc/>
        public override void MapEntityKeysInDto(Member entity, MemberDto dto)
        {
            base.MapEntityKeysInDto(entity, dto);
            dto.TeamId = entity.TeamId;
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.IncludesForUpdate"/>
        public override Expression<Func<Member, object>>[] IncludesForUpdate()
        {
            return
            [
                x => x.MemberRoles
            ];
        }

        /// <summary>
        /// Header Name.
        /// </summary>
        public struct HeaderName
        {
            /// <summary>
            /// Header Name User.
            /// </summary>
            public const string User = "user";

            /// <summary>
            /// Header Name LastName.
            /// </summary>
            public const string LastName = "lastName";

            /// <summary>
            /// Header Name FirstName.
            /// </summary>
            public const string FirstName = "firstName";

            /// <summary>
            /// Header Name Login.
            /// </summary>
            public const string Login = "login";

            /// <summary>
            /// Header Name IsActive.
            /// </summary>
            public const string IsActive = "isActive";

            /// <summary>
            /// Header Name Roles.
            /// </summary>
            public const string Roles = "roles";
        }
    }
}