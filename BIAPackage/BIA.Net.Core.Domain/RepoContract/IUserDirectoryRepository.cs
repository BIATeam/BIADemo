// <copyright file="IUserDirectoryRepository.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
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
        Task<string> AddUsersInGroup(IEnumerable<IUserFromDirectory> usersFromDirectory, string roleLabel);

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
        /// Return the roles from Ad and fake
        /// </summary>
        /// <param name="login">le login of the user</param>
        /// <returns>list of roles</returns>
        Task<List<string>> GetUserRolesBySid(string sid);

        Task<TUserFromDirectory> ResolveUserBySid(string sid);

        Task<string> ResolveUserSidByLogin(string domain, string login);
        Task<string> ResolveUserDomainByLogin(string login);
    }
}