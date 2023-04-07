// <copyright file="IUserDirectoryRepository.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using BIA.Net.Core.Common.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// The interface defining the User directory repository.
    /// </summary>
    public interface IUserDirectoryRepository<TUserFromDirectory> where TUserFromDirectory : IUserFromDirectory, new()
    {
        /// <summary>
        /// Search all user whose match the Query.
        /// </summary>
        /// <param name="search">String to find in the User Directory.</param>
        /// <param name="ldapName">Name of LDAP group to search in.</param>
        /// <returns>The list of users.</returns>
        List<TUserFromDirectory> SearchUsers(string search, string ldapName = null);

        /// <summary>
        /// Add a user in a group of the Ldap.
        /// </summary>
        /// <param name="usersFromDirectory">The users list.</param>
        /// <param name="roleLabel">Label of the role.</param>
        Task<List<string>> AddUsersInGroup(IEnumerable<IUserFromDirectory> usersFromDirectory, string roleLabel);

        /// <summary>
        /// Remove a user in a group of the Ldap.
        /// </summary>
        /// <param name="usersGuid">The user GUID list.</param>
        /// <param name="roleLabel">Label of the role.</param>
        Task<List<IUserFromDirectory>> RemoveUsersInGroup(List<IUserFromDirectory> usersFromRepositoryToRemove, string roleLabel);

        /// <summary>
        /// Return all users recursively in a role. To use only for synchronisation
        /// </summary>
        /// <param name="role">The role label.</param>
        /// <returns>The list of users.</returns>
        Task<IEnumerable<string>> GetAllUsersSidInRoleToSync(string roleLabel);

        /// <summary>
        /// Return the list of Ldap Groups corresponding to the role.
        /// </summary>
        /// <param name="roleLabel">the role.</param>
        /// <returns>The list of Ldap Groups.</returns>
        List<LdapGroup> GetLdapGroupsForRole(string roleLabel);

        /// <summary>
        /// Return the roles from Ad and fake
        /// </summary>
        /// <param name="isUserInDB">true is use exist in db</param>
        /// <param name="sid">the sid of the user</param>
        /// <returns>list of roles</returns>
        Task<List<string>> GetUserRolesBySid(bool isUserInDB, string sid);

        Task<TUserFromDirectory> ResolveUserBySid(string sid, bool forceRefresh = false);

        /// <summary>
        /// Resolves the user by identity key.
        /// </summary>
        /// <param name="login">The login key.</param>
        /// <param name="forceRefresh">To force to refresh cache.</param>
        /// <returns>The user.</returns>
        Task<TUserFromDirectory> ResolveUserByLogin(string login, bool forceRefresh = false);

        /// <summary>
        /// Resolves the user by sid.
        /// </summary>
        /// <param name="domain">The user domain.</param>
        /// <param name="login">The user login.</param>
        /// <returns>The user.</returns>
        Task<TUserFromDirectory> ResolveUser(string domain, string login);
    }
}