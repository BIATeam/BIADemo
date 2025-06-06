// <copyright file="BiaRoleId.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Common.Enum
{
    /// <summary>
    /// The enumeration of all roles.
    /// </summary>
    public enum BiaRoleId
    {
        /// <summary>
        /// The admin role identifier.
        /// </summary>
        Admin = 10001,

        /// <summary>
        /// The admin of the worker service role identifier.
        /// </summary>
        BackAdmin = 10002,

        /// <summary>
        /// The read only on worker service role identifier.
        /// </summary>
        BackReadOnly = 10003,
    }
}