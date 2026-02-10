// <copyright file="PermissionService.cs" company="BIA">
//     BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Permission
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BIA.Net.Core.Domain.Dto.App;

    /// <summary>
    /// Service to manage permissions.
    /// </summary>
    public sealed class PermissionService : IPermissionService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionService"/> class using the provided permission providers and
        /// populates the Permissions collection.
        /// </summary>
        /// <param name="permissionProviders">A collection of IPermissionProvider instances used to retrieve permissions.</param>
        public PermissionService(IEnumerable<IPermissionProvider> permissionProviders)
        {
            this.Permissions = [.. permissionProviders.SelectMany(p => p.Permissions).Select(p => new PermissionDto { PermissionId = p.Key, Name = p.Value })];
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<PermissionDto> Permissions { get; }

        /// <inheritdoc/>
        public IEnumerable<int> ConvertToIds(IEnumerable<string> permissionNames)
        {
            return this.Permissions.Where(p => permissionNames.Contains(p.Name)).Select(p => p.PermissionId);
        }

        /// <inheritdoc/>
        public IEnumerable<string> ConvertToNames(IEnumerable<int> permissionIds)
        {
            return this.Permissions.Where(p => permissionIds.Contains(p.PermissionId)).Select(p => p.Name);
        }

        /// <inheritdoc/>
        public int GetPermissionId(string permissionName)
        {
            var permissionIds = this.Permissions.Where(p => p.Name == permissionName).Select(p => p.PermissionId);
            if (!permissionIds.Any())
            {
                throw new InvalidOperationException($"Unable to find permission ID for permission name '{permissionName}'.");
            }

            if (permissionIds.Count() > 1)
            {
                throw new InvalidOperationException($"Multiple permission ID for permission name '{permissionName}'.");
            }

            return permissionIds.First();
        }

        /// <inheritdoc/>
        public string GetPermissionName(int permissionId)
        {
            var permissionNames = this.Permissions.Where(p => p.PermissionId == permissionId).Select(p => p.Name);
            if (!permissionNames.Any())
            {
                throw new InvalidOperationException($"Unable to find permission name for permission ID '{permissionId}'.");
            }

            if (permissionNames.Count() > 1)
            {
                throw new InvalidOperationException($"Multiple permission name for permission ID '{permissionId}'.");
            }

            return permissionNames.First();
        }
    }
}
