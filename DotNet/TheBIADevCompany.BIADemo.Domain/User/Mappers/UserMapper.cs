// <copyright file="UserMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.User.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

    /// <summary>
    /// The mapper used for user.
    /// </summary>
    public class UserMapper : BaseMapper<UserDto, User, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserMapper"/> class.
        /// </summary>
        /// <param name="userContext">the user context.</param>
        public UserMapper(UserContext userContext)
        {
            this.UserContext = userContext;
        }

        /// <summary>
        /// Gets or sets the collection used for expressions to access fields.
        /// </summary>
        public override ExpressionCollection<User> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<User>
                {
                    { HeaderName.Id, user => user.Id },
                    { HeaderName.LastName, user => user.LastName },
                    { HeaderName.FirstName, user => user.FirstName },
                    { HeaderName.Login, user => user.Login },
                    {
                        HeaderName.Roles,
                        user => user.Roles
                        .SelectMany(
                            role => role.RoleTranslations
                            .Where(roleTranslation => roleTranslation.Language.Code == this.UserContext.Language)
                            .Select(roleTranslation => roleTranslation.Label))
                        .Union(
                            user.Roles
                            .Where(role => !role.RoleTranslations.Any(rt => rt.Language.Code == this.UserContext.Language))
                            .Select(role => role.Label))
                        .OrderBy(x => x)
                    },
                    {
                        HeaderName.Teams,
                        user => user.Members
                        .SelectMany(member => member.Team.Title)
                        .OrderBy(x => x)
                    },
                };
            }
        }

        /// <summary>
        /// The user context language and culture.
        /// </summary>
        private UserContext UserContext { get; set; }

        /// <summary>
        /// Create a user DTO from an entity.
        /// </summary>
        /// <param name="mapperMode">the mode for mapping.</param>
        /// <returns>The user DTO.</returns>
        public override Expression<Func<User, UserDto>> EntityToDto(string mapperMode)
        {
            return entity => new UserDto
            {
                Id = entity.Id,
                RowVersion = Convert.ToBase64String(entity.RowVersion),
                LastName = entity.LastName,
                FirstName = entity.FirstName,
                Login = entity.Login,
                Roles = entity.Roles.Select(ca => new OptionDto
                {
                    Id = ca.Id,
                    Display = ca.RoleTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? ca.Label,
                }).ToList(),
                Teams = entity.Members.Select(m => new OptionDto
                {
                    Id = m.Id,
                    Display = m.Team.Title,
                }).ToList(),
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(UserDto dto, User entity, string mapperMode, IUnitOfWork context)
        {
            if ((mapperMode == "RolesInit" || mapperMode == "Roles") && dto.Roles?.Any() == true)
            {
                foreach (var userRoleDto in dto.Roles.Where(x => x.DtoState == DtoState.Deleted || mapperMode == "RolesInit"))
                {
                    var userRole = entity.Roles.FirstOrDefault(x => x.Id == userRoleDto.Id);
                    if (userRole != null)
                    {
                        entity.Roles.Remove(userRole);
                    }
                }

                entity.Roles = entity.Roles ?? new List<Role>();
                foreach (var userRoleDto in dto.Roles.Where(w => w.DtoState == DtoState.Added))
                {
                    Role role = new Role { Id = userRoleDto.Id };
                    context.Attach(role); // required to map on Id (without get element before)
                    entity.Roles.Add(role);
                }
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Func<UserDto, object[]> DtoToRecord(List<string> headerNames = null)
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

                        if (string.Equals(headerName, HeaderName.Guid, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.Guid.ToString()));
                        }

                        if (string.Equals(headerName, HeaderName.Roles, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVList(x.Roles));
                        }

                        if (string.Equals(headerName, HeaderName.Teams, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVList(x.Teams));
                        }
                    }
                }

                return records.ToArray();
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.IncludesForUpdate"/>
        public override Expression<Func<User, object>>[] IncludesForUpdate()
        {
            return new Expression<Func<User, object>>[] { x => x.Roles };
        }

        /// <summary>
        /// Header Name.
        /// </summary>
        public struct HeaderName
        {
            /// <summary>
            /// header name Id.
            /// </summary>
            public const string Id = "id";

            /// <summary>
            /// header name LastName.
            /// </summary>
            public const string LastName = "lastName";

            /// <summary>
            /// header name FirstName.
            /// </summary>
            public const string FirstName = "firstName";

            /// <summary>
            /// header name Login.
            /// </summary>
            public const string Login = "login";

            /// <summary>
            /// header name Guid.
            /// </summary>
            public const string Guid = "guid";

            /// <summary>
            /// header name Roles.
            /// </summary>
            public const string Roles = "roles";

            /// <summary>
            /// header name Teams.
            /// </summary>
            public const string Teams = "teams";
        }
    }
}