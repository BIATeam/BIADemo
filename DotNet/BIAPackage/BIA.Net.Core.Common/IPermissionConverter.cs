// <copyright file="IPermissionConverter.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for converting permission strings to numeric IDs.
    /// </summary>
    public interface IPermissionConverter
    {
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
    }
}
