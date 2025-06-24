// <copyright file="UserPermissionDomainService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using BIA.Net.Core.Common.Configuration;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The domain service used for user right.
    /// </summary>
    public class UserPermissionDomainService : IUserPermissionDomainService
    {
        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPermissionDomainService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="configuration">The configuration of the BiaNet section.</param>
        public UserPermissionDomainService(IOptions<BiaNetSection> configuration)
        {
            this.configuration = configuration.Value;
        }

        /// <inheritdoc />
        public List<string> TranslateRolesInPermissions(List<string> roles, bool lightToken = false)
        {
            IEnumerable<Permission> rights = default;

            if (this.configuration.PermissionsByEnv?.Any() == true)
            {
                rights = this.configuration.Permissions.Concat(this.configuration.PermissionsByEnv).ToList();
            }
            else
            {
                rights = this.configuration.Permissions;
            }

            var userPermissions1 = rights.Where(w => (!lightToken || w.LightToken) && w.Name != null && w.Roles.Any(a => roles.Contains(a))).Select(s => s.Name);
            var userPermissions2 = rights.Where(w => (!lightToken || w.LightToken) && w.Names != null && w.Roles.Any(a => roles.Contains(a))).SelectMany(s => s.Names);
            return userPermissions1.Concat(userPermissions2).Distinct().ToList();
        }
    }
}