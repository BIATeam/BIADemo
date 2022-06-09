// <copyright file="UserMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;

    /// <summary>
    /// The mapper used for user.
    /// </summary>
    public class UserMapper : BaseMapper<UserDto, User, int>
    {
        /// <summary>
        /// Header Name.
        /// </summary>
        public enum HeaderName
        {
            /// <summary>
            /// header name Id.
            /// </summary>
            Id,

            /// <summary>
            /// header name LastName.
            /// </summary>
            LastName,

            /// <summary>
            /// header name FirstName.
            /// </summary>
            FirstName,

            /// <summary>
            /// header name Login.
            /// </summary>
            Login,

            /// <summary>
            /// header name Guid.
            /// </summary>
            Guid,

            /// <summary>
            /// header name Roles.
            /// </summary>
            Roles,
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
                    { HeaderName.Id.ToString(), user => user.Id },
                    { HeaderName.LastName.ToString(), user => user.LastName },
                    { HeaderName.FirstName.ToString(), user => user.FirstName },
                    { HeaderName.Login.ToString(), user => user.Login },
                    { HeaderName.Guid.ToString(), user => user.Guid },
                };
            }
        }

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
                LastName = entity.LastName,
                FirstName = entity.FirstName,
                Login = entity.Login,
                Guid = entity.Guid,
                Roles = entity.Roles.Select(ca => new OptionDto
                {
                    Id = ca.Id,
                    Display = ca.RoleTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? ca.Label,
                }).ToList(),
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(UserDto dto, User entity, string mapperMode, IUnitOfWork context)
        {
            if (mapperMode == "Roles" && dto.Roles?.Any() == true)
            {
                foreach (var userRoleDto in dto.Roles.Where(x => x.DtoState == DtoState.Deleted))
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
                    context.Attach(role); // requiered to map on Id (without get element before)
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
                        if (string.Equals(headerName, HeaderName.LastName.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.LastName));
                        }

                        if (string.Equals(headerName, HeaderName.FirstName.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.FirstName));
                        }

                        if (string.Equals(headerName, HeaderName.Login.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.Login));
                        }

                        if (string.Equals(headerName, HeaderName.Guid.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.Guid.ToString()));
                        }

                        if (string.Equals(headerName, HeaderName.Roles.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVList(x.Roles));
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
    }
}