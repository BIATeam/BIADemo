// <copyright file="PermissionConverter.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Generic converter for transforming permission strings to/from numeric IDs using any enum.
    /// </summary>
    /// <typeparam name="TPermissionEnum">The enum type containing permission identifiers.</typeparam>
    public sealed class PermissionConverter<TPermissionEnum> : IPermissionConverter
        where TPermissionEnum : struct, System.Enum
    {
        /// <inheritdoc/>
        public List<int> ConvertToIds(IEnumerable<string> permissionNames)
        {
            var permissionIds = new List<int>();

            foreach (var permission in permissionNames)
            {
                if (System.Enum.TryParse<TPermissionEnum>(permission, out var permId))
                {
                    permissionIds.Add(Convert.ToInt32(permId));
                }
            }

            return permissionIds;
        }

        /// <inheritdoc/>
        public List<string> ConvertToNames(IEnumerable<int> permissionIds)
        {
            var permissions = new List<string>();
            foreach (var id in permissionIds.Where(id => System.Enum.IsDefined(typeof(TPermissionEnum), id)))
            {
                var permissionName = System.Enum.GetName(typeof(TPermissionEnum), id);
                if (!string.IsNullOrEmpty(permissionName))
                {
                    permissions.Add(permissionName);
                }
            }

            return permissions;
        }
    }
}
