// <copyright file="IUserIdentityKeyDomainService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
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
                where TUser : BaseEntityUser;

        /// <summary>
        /// Checks the database identity key.
        /// </summary>
        /// <param name="identityKeys">The identity keys.</param>
        /// <typeparam name="TUser">The type of user.</typeparam>
        /// <returns>The checks the database identity key.</returns>
        Expression<Func<TUser, bool>> CheckDatabaseIdentityKey<TUser>(List<string> identityKeys)
                where TUser : BaseEntityUser;

        /// <summary>
        /// Gets the database identity key.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <typeparam name="TUser">The type of user.</typeparam>
        /// <returns>The database identity key.</returns>
        string GetDatabaseIdentityKey<TUser>(TUser user)
                where TUser : BaseEntityUser;

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
        /// <typeparam name="TUserFromDirectory">The type of user from directory.</typeparam>
        /// <returns>The checks the directory identity key.</returns>
        Expression<Func<TUserFromDirectory, bool>> CheckDirectoryIdentityKey<TUserFromDirectory>(string identityKey)
            where TUserFromDirectory : IUserFromDirectory;

        /// <summary>
        /// Gets the directory identity key.
        /// </summary>
        /// <param name="userFromDirectory">The user from directory.</param>
        /// <returns>The directory identity key.</returns>
        string GetDirectoryIdentityKey(IUserFromDirectory userFromDirectory);

        /// <summary>
        /// Gets the Identity Type to search object with the identity key from Directory.
        /// It is use by the function UserPrincipal.FindByIdentity.
        /// </summary>
        /// <returns>Return the Identity Key.</returns>
        public int GetIdentityKeyType();
    }
}