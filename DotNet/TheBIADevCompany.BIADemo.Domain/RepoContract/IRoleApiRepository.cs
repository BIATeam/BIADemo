// <copyright file="IRoleApiRepository.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.RepoContract
{
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Domain.Api.RolesForApp;

    /// <summary>
    /// Interface for the Role Api Repository.
    /// </summary>
    public interface IRoleApiRepository
    {
        /// <summary>
        /// Get the roles of a user for a specified app in a specified context.
        /// </summary>
        /// <param name="appName">The short name of the application.</param>
        /// <param name="userLogin">The user login to get the roles for.</param>
        /// <returns>The list of roles for the user.</returns>
        Task<ApiRolesForApp> GetRolesFromApi(string appName, string userLogin);
    }
}