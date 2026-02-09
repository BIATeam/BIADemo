// <copyright file="BiaPermissionService.cs" company="BIA">
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
    /// Service to manage BIA permissions.
    /// </summary>
    public abstract class BiaPermissionService : IPermissionService
    {
        /// <inheritdoc/>
        public virtual IEnumerable<PermissionDto> GetAllPermissions()
        {
            var permissions = new List<PermissionDto>();
            foreach (BiaPermissionId permission in Enum.GetValues<BiaPermissionId>())
            {
                permissions.Add(new PermissionDto
                {
                    Name = permission.ToString(),
                    PermissionId = (int)permission,
                });
            }

            return permissions.OrderBy(p => p.PermissionId);
        }
    }
}
