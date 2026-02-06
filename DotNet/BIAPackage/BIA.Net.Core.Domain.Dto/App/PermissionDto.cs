// <copyright file="PermissionDto.cs" company="BIA">
//     BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.App
{
    /// <summary>
    /// The DTO used to represent a Permission.
    /// </summary>
    public class PermissionDto
    {
        /// <summary>
        /// Gets or sets the permission name (enum member name).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the permission numeric ID.
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the permission category (BiaPermission, Permission, OptionPermission).
        /// </summary>
        public string Category { get; set; }
    }
}
