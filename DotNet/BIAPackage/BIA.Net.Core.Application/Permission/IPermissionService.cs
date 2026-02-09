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
        /// Gets the list of permissions.
        /// </summary>
        IReadOnlyCollection<PermissionDto> Permissions { get; }

        /// <summary>
        /// Converts permission string names to their numeric IDs.
        /// </summary>
        /// <param name="permissionNames">The permission names to convert.</param>
        /// <returns>List of numeric IDs. Returns empty list if no valid conversions.</returns>
        List<int> ConvertToIds(IEnumerable<string> permissionNames);

        /// <summary>
        /// Converts numeric permission IDs to their string names.
        /// </summary>
        /// <param name="permissionIds">The permission IDs to convert.</param>
        /// <returns>List of permission names. Returns empty list if no valid conversions.</returns>
        List<string> ConvertToNames(IEnumerable<int> permissionIds);

        /// <summary>
        /// Retrieves the permission ID associated with the specified permission name using the provided permission
        /// converters.
        /// </summary>
        /// <param name="permissionName">Permission name.</param>
        /// <returns>Permission ID.</returns>
        /// <exception cref="InvalidOperationException">.</exception>
        int GetPermissionId(string permissionName);

        /// <summary>
        /// Retrieves the permission name associated with the specified permission ID using the provided permission
        /// converters.
        /// </summary>
        /// <param name="permissionId">Permission ID.</param>
        /// <returns>Permission name.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no permission converters are provided or if multiple converters return a name for the same
        /// permission ID.</exception>
        string GetPermissionName(int permissionId);
    }
}
