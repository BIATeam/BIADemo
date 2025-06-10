// <copyright file="UserIdentityKeyDomainService.cs" company="BIA">
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
    /// This class MAnage the identity key during authentication and relation beetween Database, Directory and identity Provider.
    /// </summary>
    public class UserIdentityKeyDomainService : IUserIdentityKeyDomainService
    {
        // -------------------------------- DataBase EntityKey --------------------------------------

        /// <summary>
        /// Check the Identity Key from the User in database.
        /// If you change it parse all other #IdentityKey to be sure thare is a match (Database => Directory, Idp & WindowsIdentity).
        /// </summary>
        /// <param name="identityKey">the identity Key.</param>
        /// <typeparam name="TUser">The type of user.</typeparam>
        /// <returns>Expression to compare.</returns>
        public Expression<Func<TUser, bool>> CheckDatabaseIdentityKey<TUser>(string identityKey)
            where TUser : BaseUser
        {
            return user => user.Login == identityKey;
        }

        /// <summary>
        /// Check the Identity Key from the User in database.
        /// If you change it parse all other #IdentityKey to be sure thare is a match (Database => Directory, Idp & WindowsIdentity).
        /// </summary>
        /// <param name="identityKeys">the list of identity Keys.</param>
        /// <typeparam name="TUser">The type of user.</typeparam>
        /// <returns>Expression to compare.</returns>
        public Expression<Func<TUser, bool>> CheckDatabaseIdentityKey<TUser>(List<string> identityKeys)
             where TUser : BaseUser
        {
            return user => identityKeys.Contains(user.Login);
        }

        /// <summary>
        /// Gets the Identity Key to compare with User in database.
        /// It is use to specify the unique identifier that is compare during the authentication process.
        /// If you change it parse all other #IdentityKey to be sure thare is a match (Database, Ldap, Idp, WindowsIdentity).
        /// </summary>
        /// <param name="user">the user.</param>
        /// <typeparam name="TUser">The type of user.</typeparam>
        /// <returns>Return the Identity Key.</returns>
        public string GetDatabaseIdentityKey<TUser>(TUser user)
            where TUser : BaseUser
        {
            return user.Login;
        }

        /// <summary>
        /// Gets the Identity Key to compare with UserDto.
        /// It is use to specify the unique identifier that is compare during the authentication process.
        /// If you change it parse all other #IdentityKey to be sure thare is a match (Database, Ldap, Idp, WindowsIdentity).
        /// </summary>
        /// <param name="user">the user.</param>
        /// <returns>Return the Identity Key.</returns>
        public string GetDtoIdentityKey(BaseUserDto user)
        {
            return user.Login;
        }

        // -------------------------------- Directory EntityKey --------------------------------------

        /// <summary>
        /// Check the Identity Key from the User in database.
        /// It is use to specify the unique identifier that is compare during the authentication process.
        /// If you change it parse all other #IdentityKey to be sure thare is a match (Database => Directory, Idp & WindowsIdentity).
        /// </summary>
        /// <param name="identityKey">the identityKey.</param>
        /// <typeparam name="TUserFromDirectory">The type of user from directory.</typeparam>
        /// <returns>Expression to compare.</returns>
        public Expression<Func<TUserFromDirectory, bool>> CheckDirectoryIdentityKey<TUserFromDirectory>(string identityKey)
            where TUserFromDirectory : IUserFromDirectory
        {
            return userFromDirectory => userFromDirectory.Login == identityKey;
        }

        /// <summary>
        /// Gets the Identity Key to compare with User in database.
        /// It is use to specify the unique identifier that is compare during the authentication process.
        /// If you change it parse all other #IdentityKey to be sure thare is a match (Database, Ldap, Idp, WindowsIdentity).
        /// </summary>
        /// <param name="userFromDirectory">the userFromDirectory.</param>
        /// <returns>Return the Identity Key.</returns>
        public string GetDirectoryIdentityKey(IUserFromDirectory userFromDirectory)
        {
            return userFromDirectory.Login;
        }

        /// <summary>
        /// Gets the Identity Key to compare with User in database.
        /// It is use to specify the unique identifier that is compare during the authentication process.
        /// If you change it parse all other #IdentityKey to be sure thare is a match (Database, Ldap, Idp, WindowsIdentity).
        /// </summary>
        /// <param name="userFromDirectory">the userFromDirectory.</param>
        /// <returns>Return the Identity Key.</returns>
        public string GetDirectoryIdentityKey(UserFromDirectoryDto userFromDirectory)
        {
            return userFromDirectory.IdentityKey;
        }

        // -------------------------------- Identity Provider --------------------------------------
    }
}
