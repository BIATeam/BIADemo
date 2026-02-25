// <copyright file="PermissionProvider.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Permission
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Generic permission provider based on enum.
    /// </summary>
    /// <typeparam name="TPermissionEnum">The enum type containing permission identifiers.</typeparam>
    public sealed class PermissionProvider<TPermissionEnum> : IPermissionProvider
        where TPermissionEnum : struct, Enum
    {
        private IReadOnlyDictionary<int, string> permissions;

        /// <inheritdoc/>
        public IReadOnlyDictionary<int, string> Permissions
        {
            get
            {
                this.permissions ??= Enum.GetValues<TPermissionEnum>().ToDictionary(permission => Convert.ToInt32(permission), permission => permission.ToString());
                return this.permissions;
            }
        }
    }
}
