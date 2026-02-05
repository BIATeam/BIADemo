// <copyright file="PermissionIdConverter.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Generic converter for transforming permission strings to/from numeric IDs using any enum.
    /// Usage: Instantiate with your project's PermissionId enum type.
    /// Example: new PermissionIdConverter&lt;MyProject.PermissionId&gt;(logger)
    /// </summary>
    /// <typeparam name="TPermissionEnum">The enum type containing permission identifiers.</typeparam>
    public class PermissionIdConverter<TPermissionEnum> : IPermissionIdConverter
        where TPermissionEnum : struct, System.Enum
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionIdConverter{TPermissionEnum}"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public PermissionIdConverter(ILogger<PermissionIdConverter<TPermissionEnum>> logger)
        {
            this.logger = logger;
        }

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
                // Silently skip permissions not in this enum - warnings handled at authentication level
            }

            return permissionIds;
        }

        /// <inheritdoc/>
        public List<string> ConvertToNames(IEnumerable<int> permissionIds)
        {
            var permissions = new List<string>();

            foreach (var id in permissionIds)
            {
                if (System.Enum.IsDefined(typeof(TPermissionEnum), id))
                {
                    var permissionName = System.Enum.GetName(typeof(TPermissionEnum), id);
                    if (!string.IsNullOrEmpty(permissionName))
                    {
                        permissions.Add(permissionName);
                    }
                }
                // Silently skip IDs not in this enum - warnings handled at authentication level
            }

            return permissions;
        }
    }
}
