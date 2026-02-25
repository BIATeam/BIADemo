// <copyright file="IPermissionProvider.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Permission
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for permission providers.
    /// </summary>
    public interface IPermissionProvider
    {
        /// <summary>
        /// Dictionary of all permissions, where the key is the permission name and the value is the corresponding numeric ID.
        /// </summary>
        IReadOnlyDictionary<int, string> Permissions { get; }
    }
}
