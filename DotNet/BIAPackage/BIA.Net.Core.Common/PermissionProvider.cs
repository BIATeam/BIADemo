// <copyright file="PermissionProvider.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Generic permission provider based on enum.
    /// </summary>
    /// <typeparam name="TPermissionEnum">The enum type containing permission identifiers.</typeparam>
    public sealed class PermissionProvider<TPermissionEnum> : IPermissionProvider
        where TPermissionEnum : struct, System.Enum
    {
        /// <inheritdoc/>
        public Dictionary<int, string> GetAll()
        {
            var permissions = new Dictionary<int, string>();
            foreach (var permission in System.Enum.GetValues<TPermissionEnum>())
            {
                permissions.Add(Convert.ToInt32(permission), permission.ToString());
            }

            return permissions;
        }
    }
}
