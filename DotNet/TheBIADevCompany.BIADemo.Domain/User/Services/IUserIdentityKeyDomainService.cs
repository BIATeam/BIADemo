// <copyright file="IUserIdentityKeyDomainService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.User.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain.Dto.User;
#if BIA_FRONT_FEATURE
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;
    using TheBIADevCompany.BIADemo.Domain.User.Models;
#endif

    /// <summary>
    /// Interface UserIdentityKey Domain Service.
    /// </summary>
    public interface IUserIdentityKeyDomainService
    {
#if BIA_FRONT_FEATURE
        /// <summary>
        /// Checks the database identity key.
        /// </summary>
        /// <param name="identityKey">The identity key.</param>
        /// <returns>The checks the database identity key.</returns>
        Expression<Func<User, bool>> CheckDatabaseIdentityKey(string identityKey);

        /// <summary>
        /// Checks the database identity key.
        /// </summary>
        /// <param name="identityKeys">The identity keys.</param>
        /// <returns>The checks the database identity key.</returns>
        Expression<Func<User, bool>> CheckDatabaseIdentityKey(List<string> identityKeys);

        /// <summary>
        /// Gets the database identity key.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The database identity key.</returns>
        string GetDatabaseIdentityKey(User user);

        /// <summary>
        /// Gets the userDto identity key.
        /// </summary>
        /// <param name="user">The user dto.</param>
        /// <returns>The userDto identity key.</returns>
        string GetDtoIdentityKey(UserDto user);

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
#endif

        /// <summary>
        /// Gets the directory identity key.
        /// </summary>
        /// <param name="userFromDirectory">The user from directory.</param>
        /// <returns>The directory identity key.</returns>
        string GetDirectoryIdentityKey(UserFromDirectoryDto userFromDirectory);
    }
}