// <copyright file="IUserPermissionDomainService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Service
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
        /// <param name="roles">The liste of roles.</param>
        /// <returns>The list of rights.</returns>
        List<string> TranslateRolesInPermissions(List<string> roles);
    }
}