// <copyright file="IUserIdentityKeyDomainService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.User.Entities;
    using BIA.Net.Core.Domain.User.Models;

    /// <summary>
    /// Interface UserIdentityKey Domain Service.
    /// </summary>
    public interface IUserIdentityKeyDomainService
    {
        /// <summary>
        /// Checks the database identity key.
        /// </summary>
        /// <param name="identityKey">The identity key.</param>
        /// <typeparam name="TUser">The type of user.</typeparam>
        /// <returns>The checks the database identity key.</returns>
        Expression<Func<TUser, bool>> CheckDatabaseIdentityKey<TUser>(string identityKey)
                where TUser : BaseUser;

        /// <summary>
        /// Checks the database identity key.
        /// </summary>
        /// <param name="identityKeys">The identity keys.</param>
        /// <typeparam name="TUser">The type of user.</typeparam>
        /// <returns>The checks the database identity key.</returns>
        Expression<Func<TUser, bool>> CheckDatabaseIdentityKey<TUser>(List<string> identityKeys)
                where TUser : BaseUser;

        /// <summary>
        /// Gets the database identity key.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <typeparam name="TUser">The type of user.</typeparam>
        /// <returns>The database identity key.</returns>
        string GetDatabaseIdentityKey<TUser>(TUser user)
                where TUser : BaseUser;

        /// <summary>
        /// Gets the userDto identity key.
        /// </summary>
        /// <param name="user">The user dto.</param>
        /// <returns>The userDto identity key.</returns>
        string GetDtoIdentityKey(BaseUserDto user);

        /// <summary>
        /// Checks the directory identity key.
        /// </summary>
        /// <param name="identityKey">The identity key.</param>
        /// <returns>The checks the directory identity key.</returns>
        Expression<Func<UserFromDirectory, bool>> CheckDirectoryIdentityKey(string identityKey);

        /// <summary>
        /// Gets the directory identity key.
        /// </summary>
        /// <param name="userFromDirectory">The user from directory.</param>
        /// <returns>The directory identity key.</returns>
        string GetDirectoryIdentityKey(UserFromDirectory userFromDirectory);

        /// <summary>
        /// Gets the directory identity key.
        /// </summary>
        /// <param name="userFromDirectory">The user from directory.</param>
        /// <returns>The directory identity key.</returns>
        string GetDirectoryIdentityKey(UserFromDirectoryDto userFromDirectory);
    }
}