// <copyright file="IUserRepository.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// Defines a contract for a repository that manages user entities and provides user-specific data retrieval
    /// operations.
    /// </summary>
    /// <typeparam name="TUserEntity">The type of user entity managed by the repository. Must inherit from <see cref="BaseEntityUser"/>.</typeparam>
    public interface IUserRepository<TUserEntity> : ITGenericRepository<TUserEntity, int>
        where TUserEntity : BaseEntityUser
    {
        /// <summary>
        /// Retrieves a dictionary mapping user login names to their corresponding full names.
        /// </summary>
        /// <param name="logins">A collection of user login names for which to retrieve full names. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a dictionary where each key is a
        /// login name from the input collection, and each value is the user's full name in the format "LastName
        /// FirstName". If no users are found for the specified logins, the dictionary will be empty.</returns>
        Task<Dictionary<string, string>> GetUserFullNamesPerLogins(IEnumerable<string> logins);
    }
}