// <copyright file="IUserPermissionDomainService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// The interface defining the user right domain service.
    /// </summary>
    public interface IUserPermissionDomainService
    {
        /// <summary>
        /// Translate the roles in rights.
        /// </summary>
        /// <param name="roles">The list of roles.</param>
        /// <param name="lightToken">if true select only lightToken permission.</param>
        /// <param name="transversal">transversal permissions independant to current team.</param>
        /// <returns>The list of rights.</returns>
        List<string> TranslateRolesInPermissions(List<string> roles, bool lightToken = false, bool transversal = false);

        /// <summary>
        /// Get the roles that have the specified permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns>The list of roles.</returns>
        List<string> GetRolesForPermission(string permission);
    }
}