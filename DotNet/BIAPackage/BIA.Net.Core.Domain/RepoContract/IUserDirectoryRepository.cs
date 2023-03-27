// <copyright file="IUserDirectoryRepository.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.Dto.User;

    /// <summary>
    /// The interface defining the User directory repository.
    /// </summary>
    /// <typeparam name="TUserFromDirectory">The type of the user from directory.</typeparam>
    public interface IUserDirectoryRepository<TUserFromDirectory>
        where TUserFromDirectory : IUserFromDirectory, new()
    {
        /// <summary>
        /// Search all user whose match the Query.
        /// </summary>
        /// <param name="search">String to find in the User Directory.</param>
        /// <param name="ldapName">Name of LDAP group to search in.</param>
        /// <param name="max">Max return item.</param>
        /// <returns>The list of users.</returns>
        List<TUserFromDirectory> SearchUsers(string search, string ldapName = null, int max = 10);

        /// <summary>
        /// Add a user in a group of the Ldap.
        /// </summary>
        /// <param name="usersFromDirectory">The users list.</param>
        /// <param name="roleLabel">List of error message.</param>
        /// <returns>List of of error message.</returns>
        Task<List<string>> AddUsersInGroup(IEnumerable<UserFromDirectoryDto> usersFromDirectory, string roleLabel);

        /// <summary>
        /// Remove a user in a group of the Ldap.
        /// </summary>
        /// <param name="usersFromRepositoryToRemove">The users from repository to remove.</param>
        /// <param name="roleLabel">Label of the role.</param>
        /// <returns>List of not removed user.</returns>
        Task<List<UserFromDirectoryDto>> RemoveUsersInGroup(List<UserFromDirectoryDto> usersFromRepositoryToRemove, string roleLabel);

        /// <summary>
        /// Return all users recursively in a role. To use only for synchronisation.
        /// </summary>
        /// <param name="roleLabel">The role label.</param>
        /// <returns>The list of users.</returns>
        Task<IEnumerable<string>> GetAllUsersSidInRoleToSync(string roleLabel);

        /// <summary>
        /// Return the list of Ldap Groups corresponding to the role.
        /// </summary>
        /// <param name="roleLabel">the role.</param>
        /// <returns>The list of Ldap Groups.</returns>
        List<LdapGroup> GetLdapGroupsForRole(string roleLabel);

        /// <summary>
        /// Resolves the user by sid.
        /// </summary>
        /// <param name="sid">The sid.</param>
        /// <param name="forceRefresh">To force to refresh cache.</param>
        /// <returns>The user.</returns>
        Task<TUserFromDirectory> ResolveUserBySid(string sid, bool forceRefresh = false);

        /// <summary>
        /// Determines whether [is sid in groups] [the specified LDAP groups].
        /// </summary>
        /// <param name="ldapGroups">The LDAP groups.</param>
        /// <param name="sid">The sid.</param>
        /// <returns>True if sif in AD group.</returns>
        Task<bool> IsSidInGroups(IEnumerable<LdapGroup> ldapGroups, string sid);
    }
}