// <copyright file="IPermissionProvider.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for converting permission strings to numeric IDs.
    /// </summary>
    public interface IPermissionProvider
    {
        /// <summary>
        /// Gets a dictionary of all permissions, where the key is the permission name and the value is the corresponding numeric ID.
        /// </summary>
        /// <returns>Dictionary of all permissions.</returns>
        Dictionary<int, string> GetAll();
    }
}
