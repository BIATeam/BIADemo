// <copyright file="BaseUserMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// The mapper used for user.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UserMapper"/> class.
    /// </remarks>
    /// <param name="userContext">the user context.</param>
    /// <typeparam name="TUserDto">The type of user dto.</typeparam>
    /// <typeparam name="TUser">The type of user.</typeparam>
    public abstract class BaseUserMapper<TUserDto, TUser>(UserContext userContext) : BaseMapper<TUserDto, TUser, int>()
        where TUser : BaseEntityUser, IEntity<int>, new()
        where TUserDto : BaseUserDto, new()
    {
        /// <summary>
        /// Gets or sets the collection used for expressions to access fields.
        /// </summary>
        public override ExpressionCollection<TUser> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<TUser>(base.ExpressionCollection)
                {
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
        protected UserContext UserContext { get; set; } = userContext;

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(TUserDto dto, ref TUser entity, string mapperMode, IUnitOfWork context)
        {
            base.DtoToEntity(dto, ref entity, mapperMode, context);

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

        /// <summary>
        /// Create a user DTO from an entity.
        /// </summary>
        /// <param name="mapperMode">the mode for mapping.</param>
        /// <returns>The user DTO.</returns>
        public override Expression<Func<TUser, TUserDto>> EntityToDto(string mapperMode)
        {
            return base.EntityToDto(mapperMode).CombineMapping(entity => new TUserDto
            {
                LastName = entity.LastName,
                FirstName = entity.FirstName,
                Login = entity.Login,
                Roles = entity.Roles.Select(ca => new OptionDto
                {
                    Id = ca.Id,
                    Display = ca.RoleTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? ca.Label,
                }).ToList(),
                Teams = entity.Members
                .OrderBy(m => m.Team.TeamTypeId)
                .ThenBy(m => m.Team.Title)
                .Select(m => new UserTeamDto
                {
                    Title = m.Team.Title,
                    TeamTypeId = m.Team.TeamTypeId,
                })
                .ToList(),
            });
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToCellMapping"/>
        public override Dictionary<string, Func<string>> DtoToCellMapping(TUserDto dto)
        {
            return new Dictionary<string, Func<string>>(base.DtoToCellMapping(dto))
            {
                { HeaderName.LastName, () => CSVString(dto.LastName) },
                { HeaderName.FirstName, () => CSVString(dto.FirstName) },
                { HeaderName.Login, () => CSVString(dto.Login) },
                { HeaderName.Guid, () => CSVString(dto.Guid.ToString()) },
                { HeaderName.Roles, () => CSVList(dto.Roles) },
                { HeaderName.Teams, () => CSVList(dto.Teams.Select(t => new OptionDto { Display = t.Title })) },
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.IncludesForUpdate"/>
        public override Expression<Func<TUser, object>>[] IncludesForUpdate()
        {
            return
            [
                x => x.Roles,
            ];
        }

        /// <summary>
        /// Header Name.
        /// </summary>
        public struct HeaderName
        {
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