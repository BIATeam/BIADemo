// <copyright file="UserSelectBuilder.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Bia.User
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// The select builder of the user entity.
    /// </summary>
    public static class UserSelectBuilder<TUser>
        where TUser : User
    {
        /// <summary>
        /// Gets the expression used to select user.
        /// </summary>
        /// <returns>The expression.</returns>
        public static Expression<Func<TUser, UserDto>> EntityToDto()
        {
            return user => new UserDto
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