// <copyright file="RoleMode.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Enum
{
    /// <summary>
    /// Enum for different role mode.
    /// </summary>
    public enum RoleMode
    {
        /// <summary>
        ///  All possible roles are selected
        /// </summary>
        AllRoles = 1,

        /// <summary>
        /// Only one role is selectable
        /// </summary>
        SingleRole = 2,

        /// <summary>
        /// Multi select Role
        /// </summary>
        MultiRoles = 3,
    }
}