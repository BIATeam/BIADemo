// <copyright file="MemberMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.User.Mappers
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
    using TheBIADevCompany.BIADemo.Domain.User;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

    /// <summary>
    /// The mapper used for member.
    /// </summary>
    public class MemberMapper : BaseMapper<MemberDto, Member, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberMapper"/> class.
        /// </summary>
        /// <param name="userContext">the user context.</param>
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
                        "Roles",
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
                    { "User", member => member.User.LastName + " " + member.User.FirstName + " (" + member.User.Login + ")" },
                    { "FirstName", member => member.User.FirstName },
                    { "LastName", member => member.User.LastName },
                    { "Login", member => member.User.Login },
                    { "IsActive", member => member.User.IsActive },
                };
            }
        }

        /// <summary>
        /// The user context language and culture.
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
                    Display = entity.User.Display() + (entity.User.IsActive ? string.Empty : " **Disabled**"),
                },
                FirstName = entity.User.FirstName,
                LastName = entity.User.LastName,
                Login = entity.User.Login,
                IsActive = entity.User.IsActive,
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
        public override void DtoToEntity(MemberDto dto, ref Member entity)
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

        /// <inheritdoc/>
        public override Func<MemberDto, object[]> DtoToRecord(List<string> headerNames = null)
        {
            return x =>
            {
                List<object> records = new List<object>();

                if (headerNames?.Any() == true)
                {
                    foreach (string headerName in headerNames)
                    {
                        if (string.Equals(headerName, HeaderName.Id, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVNumber(x.Id));
                        }

                        if (string.Equals(headerName, HeaderName.User, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.User?.Display));
                        }

                        if (string.Equals(headerName, HeaderName.LastName, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.LastName));
                        }

                        if (string.Equals(headerName, HeaderName.FirstName, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.FirstName));
                        }

                        if (string.Equals(headerName, HeaderName.Login, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.Login));
                        }

                        if (string.Equals(headerName, HeaderName.IsActive, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVBool(x.IsActive));
                        }

                        if (string.Equals(headerName, HeaderName.Roles, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVList(x.Roles));
                        }
                    }
                }

                return records.ToArray();
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.IncludesForUpdate"/>
        public override Expression<Func<Member, object>>[] IncludesForUpdate()
        {
            return new Expression<Func<Member, object>>[] { member => member.MemberRoles };
        }

        /// <summary>
        /// Header Name.
        /// </summary>
        public struct HeaderName
        {
            /// <summary>
            /// Header Name Id.
            /// </summary>
            public const string Id = "id";

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