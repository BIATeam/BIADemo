// <copyright file="PermissionService.cs" company="BIA">
//     BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Permission
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Dto.App;

    /// <summary>
    /// Service to manage permissions.
    /// </summary>
    public abstract class PermissionService : IPermissionService
    {
        /// <summary>
        /// Gets all permissions from the BiaPermissionId enum.
        /// </summary>
        /// <returns>List of all BIA framework permissions.</returns>
        public virtual IEnumerable<PermissionDto> GetAllPermissions()
        {
            var permissions = new List<PermissionDto>();

            // Extract BiaPermissionId enum
            foreach (BiaPermissionId permission in Enum.GetValues(typeof(BiaPermissionId)))
            {
                permissions.Add(new PermissionDto
                {
                    Name = permission.ToString(),
                    PermissionId = (int)permission,
                    Category = "BiaPermission",
                });
            }

            return permissions.OrderBy(p => p.PermissionId);
        }
    }
}
