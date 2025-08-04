// <copyright file="UserSelectBuilder.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.User.Entities;
    using BIA.Net.Core.Domain.User.Services;

    /// <summary>
    /// The select builder of the user entity.
    /// </summary>
    /// <typeparam name="TUser">The type of user.</typeparam>
    public static class UserSelectBuilder<TUser>
        where TUser : BaseEntityUser
    {
        /// <summary>
        /// Gets the expression used to select user.
        /// </summary>
        /// <typeparam name="TBaseUserDto">The type of the base user dto.</typeparam>
        /// <returns>
        /// The expression.
        /// </returns>
        public static Expression<Func<TUser, TBaseUserDto>> EntityToDto<TBaseUserDto>()
            where TBaseUserDto : BaseUserDto, new()
        {
            return user => new TBaseUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                Login = user.Login,
                LastName = user.LastName,
            };
        }

        /// <summary>
        /// Gets the expression used to select user.
        /// </summary>
        /// <param name="userIdentityKeyDomainService">The userIdentityKeyDomain Service.</param>
        /// <returns>The expression.</returns>
        public static Expression<Func<TUser, UserInfoFromDBDto>> SelectUserInfo(IUserIdentityKeyDomainService userIdentityKeyDomainService)
        {
            return user => new UserInfoFromDBDto
            {
                Id = user.Id,
                IdentityKey = userIdentityKeyDomainService.GetDatabaseIdentityKey(user),
                IsActive = user.IsActive,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        }
    }
}