// <copyright file="UserPermissionDomainService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

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

        /// <inheritdoc cref="IUserPermissionDomainService.TranslateRolesInPermissions"/>
        public List<string> TranslateRolesInPermissions(List<string> roles, bool lightToken)
        {
            var rights = this.configuration.Permissions.ToList();
            var userPermissions1 = rights.Where(w => (!lightToken || w.LightToken) && w.Name != null && w.Roles.Any(a => roles.Contains(a))).Select(s => s.Name);
            var userPermissions2 = rights.Where(w => (!lightToken || w.LightToken) && w.Names != null && w.Roles.Any(a => roles.Contains(a))).SelectMany(s => s.Names);
            return userPermissions1.Concat(userPermissions2).Distinct().ToList();
        }
    }
}