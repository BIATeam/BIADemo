// <copyright file="ProjectPermissionService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Permission
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BIA.Net.Core.Application.Permission;
    using BIA.Net.Core.Domain.Dto.App;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;

    /// <summary>
    /// Project-specific permission service that includes PermissionId and OptionPermissionId.
    /// </summary>
    public class ProjectPermissionService : PermissionService
    {
        /// <summary>
        /// Gets all permissions including BiaPermissionId, PermissionId, and OptionPermissionId.
        /// </summary>
        /// <returns>List of all permissions.</returns>
        public override IEnumerable<PermissionDto> GetAllPermissions()
        {
            var permissions = new List<PermissionDto>();

            // Get BIA framework permissions
            permissions.AddRange(base.GetAllPermissions());

            // Extract PermissionId enum
            foreach (PermissionId permission in Enum.GetValues<PermissionId>())
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
