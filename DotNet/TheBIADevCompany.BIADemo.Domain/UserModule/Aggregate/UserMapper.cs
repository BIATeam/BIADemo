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
        /// Gets or sets the collection used for expressions to access fields.
        /// </summary>
        public override ExpressionCollection<User> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<User>
                   {
                       { "Id", user => user.Id },
                       { "LastName", user => user.LastName },
                       { "FirstName", user => user.FirstName },
                       { "Login", user => user.Login },
                       { "Guid", user => user.Guid },
                   };
            }
        }

        /// <summary>
        /// Create a user DTO from an entity.
        /// </summary>
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

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.IncludesForUpdate"/>
        public override Expression<Func<User, object>>[] IncludesForUpdate()
        {
            return new Expression<Func<User, object>>[] { x => x.Roles };
        }
    }
}