// <copyright file="IRoleApiRepository.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for the Role Api Repository.
    /// </summary>
    /// <typeparam name="T">Type of object returned by the role API.</typeparam>
    public interface IRoleApiRepository<T>
        where T : class
    {
        /// <summary>
        /// Get the roles of a user for a specified app in a specified context.
        /// </summary>
        /// <param name="appName">The short name of the application.</param>
        /// <param name="userLogin">The user login to get the roles for.</param>
        /// <returns>The list of roles for the user.</returns>
        Task<T> GetRolesFromApi(string appName, string userLogin);
    }
}