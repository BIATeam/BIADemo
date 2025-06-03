// <copyright file="UserSelectBuilder.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// The select builder of the user entity.
    /// </summary>
    /// <typeparam name="TUser">The type of user.</typeparam>
    public static class UserSelectBuilder<TUser>
        where TUser : BaseUser
    {
        /// <summary>
        /// Gets the expression used to select user.
        /// </summary>
        /// <returns>The expression.</returns>
        public static Expression<Func<TUser, BaseUserDto>> EntityToDto()
        {
            return user => new BaseUserDto
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
        /// <returns>The expression.</returns>
        public static Expression<Func<TUser, UserInfoDto>> SelectUserInfo()
        {
            return user => new UserInfoDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                Login = user.Login,
                LastName = user.LastName,
                IsActive = user.IsActive,
            };
        }
    }
}