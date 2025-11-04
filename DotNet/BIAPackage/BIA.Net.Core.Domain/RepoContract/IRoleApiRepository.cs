// <copyright file="IRoleApiRepository.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for the Role Api Repository.
    /// </summary>
    public interface IRoleApiRepository
    {
        /// <summary>
        /// Get the roles of a user for a specified app in a specified context.
        /// </summary>
        /// <param name="appName">The short name of the application.</param>
        /// <param name="context">The context in which to get the roles.</param>
        /// <param name="userLogin">The user login to get the roles for.</param>
        /// <returns>The list of roles for the user.</returns>
        Task<IEnumerable<string>> GetRolesFromApi(string appName, string context, string userLogin);
    }
}