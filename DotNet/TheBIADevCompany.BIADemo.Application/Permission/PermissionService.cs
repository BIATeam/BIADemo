// <copyright file="PermissionService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Permission
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BIA.Net.Core.Application.Permission;
    using BIA.Net.Core.Domain.Dto.App;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;

    /// <summary>
    /// Project-specific permission service.
    /// </summary>
    public class PermissionService : BiaPermissionService
    {
        /// <inheritdoc/>
        public override IEnumerable<PermissionDto> GetAllPermissions()
        {
            var permissions = new List<PermissionDto>(base.GetAllPermissions());
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
