// <copyright file="UserRightDomainService.cs" company="TheBIADevCompany">
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
    public class UserRightDomainService : IUserRightDomainService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly ITGenericRepository<Member> repository;

        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly BiaNetSection configuration;

        /// <summary>
        /// The AD helper.
        /// </summary>
        private readonly IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRightDomainService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="configuration">The configuration of the BiaNet section.</param>
        /// <param name="adHelper">The AD helper.</param>
        public UserRightDomainService(ITGenericRepository<Member> repository, IOptions<BiaNetSection> configuration, IUserDirectoryRepository<UserFromDirectory> adHelper)
        {
            this.repository = repository;
            this.configuration = configuration.Value;
            this.userDirectoryHelper = adHelper;
        }

        /// <inheritdoc cref="IUserRightDomainService.GetRightsForUserAsync"/>
        public async Task<List<string>> GetRightsForUserAsync(List<string> userDirectoryRoles, string sid, int siteId = 0, int roleId = 0)
        {
            var specification = siteId > 0 ? MemberSpecification.SearchForSidAndSite(sid, siteId) : MemberSpecification.SearchForSid(sid);

            var sitesMemberRoles = (await this.repository
                .GetAllResultAsync(
                    s => new UserRoleSelectResult
                    {
                        Roles = s.MemberRoles.Select(mr => new KeyValuePair<int, string>(mr.Role.Id, mr.Role.Code)),
                        UserId = s.UserId,
                    },
                    specification: specification)).Distinct().FirstOrDefault();

            var roles = userDirectoryRoles;
            if (sitesMemberRoles != null)
            {
                if (roleId > 0)
                {
                    var siteMemberRole = sitesMemberRoles.Roles.FirstOrDefault(kvp => kvp.Key == roleId);
                    if (siteMemberRole.Key > 0)
                    {
                        roles.Add(siteMemberRole.Value);
                    }
                }
                else
                {
                    roles.AddRange(sitesMemberRoles.Roles.Select(kvp => kvp.Value));
                }
            }

            return this.TranslateRolesInRights(roles);
        }

        /// <inheritdoc cref="IUserRightDomainService.TranslateRolesInRights"/>
        public List<string> TranslateRolesInRights(List<string> roles)
        {
            var rights = this.configuration.Permissions.ToList();
            var userRights1 = rights.Where(w => w.Name != null && w.Roles.Any(a => roles.Contains(a))).Select(s => s.Name);
            var userRights2 = rights.Where(w => w.Names != null && w.Roles.Any(a => roles.Contains(a))).SelectMany(s => s.Names);
            return userRights1.Concat(userRights2).Distinct().ToList();
        }
    }
}