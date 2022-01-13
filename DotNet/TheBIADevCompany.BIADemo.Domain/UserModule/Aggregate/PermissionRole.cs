// <copyright file="MemberRole.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate
{
    using BIA.Net.Core.Domain;

    /// <summary>
    /// The entity member role.
    /// </summary>
    public class PermissionRole : VersionedTable
    {
        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public virtual Role Role { get; set; }

        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        public virtual Permission Permission { get; set; }

        /// <summary>
        /// Gets or sets the permission id.
        /// </summary>
        public int PermissionId { get; set; }
    }
}