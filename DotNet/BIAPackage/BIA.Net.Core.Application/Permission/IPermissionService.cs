// <copyright file="IPermissionService.cs" company="BIA">
//     BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Permission
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.App;

    /// <summary>
    /// Interface for permission service.
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        /// Gets all permissions from the enums.
        /// </summary>
        /// <returns>List of all permissions.</returns>
        IEnumerable<PermissionDto> GetAllPermissions();
    }
}
