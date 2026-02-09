// <copyright file="PermissionHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Helper class for permissions.
    /// </summary>
    public static class PermissionHelper
    {
        /// <summary>
        /// Retrieves the permission ID associated with the specified permission name using the provided permission
        /// converters.
        /// </summary>
        /// <param name="permissionName">Permission name.</param>
        /// <param name="permissionConverters">Permission converters.</param>
        /// <returns>Permission ID.</returns>
        /// <exception cref="InvalidOperationException">.</exception>
        public static int GetPermissionId(string permissionName, IEnumerable<IPermissionConverter> permissionConverters)
        {
            var permissionIds = permissionConverters.SelectMany(c => c.ConvertToIds([permissionName]));
            if (!permissionConverters.Any())
            {
                throw new InvalidOperationException($"Unable to find permission converters that returned an ID for permission name '{permissionName}'.");
            }

            if (permissionIds.Count() > 1)
            {
                throw new InvalidOperationException($"Multiple permission converters returned an ID for permission name '{permissionName}'.");
            }

            return permissionIds.First();
        }

        /// <summary>
        /// Retrieves the permission name associated with the specified permission ID using the provided permission
        /// converters.
        /// </summary>
        /// <param name="permissionId">The ID of the permission to retrieve the name for.</param>
        /// <param name="permissionConverters">A collection of permission converters used to obtain the permission name.</param>
        /// <returns>The name of the permission corresponding to the given permission ID.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no permission converters are provided or if multiple converters return a name for the same
        /// permission ID.</exception>
        public static string GetPermissionName(int permissionId, IEnumerable<IPermissionConverter> permissionConverters)
        {
            var permissionNames = permissionConverters.SelectMany(c => c.ConvertToNames([permissionId]));
            if (!permissionConverters.Any())
            {
                throw new InvalidOperationException($"Unable to find permission converters that returned a name for permission ID '{permissionId}'.");
            }

            if (permissionNames.Count() > 1)
            {
                throw new InvalidOperationException($"Multiple permission converters returned a name for permission ID '{permissionId}'.");
            }

            return permissionNames.First();
        }
    }
}
