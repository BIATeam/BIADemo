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
        public List<string> TranslateRolesInPermissions(List<string> roles, bool lightToken = false, bool transversal = false)
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

            var userPermissions1 = rights.Where(w => (!lightToken || w.LightToken)
                                                    && (!transversal || w.IsTransversal)
                                                    && w.Name != null
                                                    && w.Roles.Any(a => roles.Contains(a)))
                                         .Select(s => s.Name);
            var userPermissions2 = rights.Where(w => (!lightToken || w.LightToken)
                                                    && (!transversal || w.IsTransversal)
                                                    && w.Names != null
                                                    && w.Roles.Any(a => roles.Contains(a)))
                                         .SelectMany(s => s.Names);

            return userPermissions1.Concat(userPermissions2).Distinct().ToList();
        }

        /// <inheritdoc />
        public List<string> GetRolesForPermission(string permission)
        {
            IEnumerable<Permission> allPermissions = default;

            if (this.configuration.PermissionsByEnv?.Any() == true)
            {
                allPermissions = this.configuration.Permissions.Concat(this.configuration.PermissionsByEnv).ToList();
            }
            else
            {
                allPermissions = this.configuration.Permissions;
            }

            var roles1 = allPermissions.Where(w => w.Name != null && w.Name == permission).SelectMany(p => p.Roles);
            var roles2 = allPermissions.Where(w => w.Names != null && w.Names.Any(name => name == permission)).SelectMany(p => p.Roles);
            return [.. roles1.Concat(roles2).Distinct()];
        }
    }
}