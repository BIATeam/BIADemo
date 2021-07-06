// <copyright file="LdapRepository.cs" company="TheBIADevCompany">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories
{
    using System;
    using System.DirectoryServices;
    using System.Security.Principal;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// Class the manipulate AD.
    /// </summary>
    public class LdapRepository : GenericLdapRepository<UserFromDirectory>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LdapRepository"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="ldapRepositoryHelper">The ldap helper.</param>
        public LdapRepository(ILogger<GenericLdapRepository<UserFromDirectory>> logger, IOptions<BiaNetSection> configuration, ILdapRepositoryHelper ldapRepositoryHelper)
            : base(logger, configuration, ldapRepositoryHelper)
        {
        }

        /// <summary>
        /// Convert the Ad entry in a UserInfoDirectory Object.
        /// </summary>
        /// <param name="entry">Entry from AD.</param>
        /// <param name="domainKey">Domain Name in config file where domain found.</param>
        /// <returns>The UserInfoDirectory object.</returns>
        protected override UserFromDirectory ConvertToUserDirectory(DirectoryEntry entry, string domainKey)
        {
            var sid = new SecurityIdentifier((byte[])entry.Properties["objectSid"].Value, 0).ToString();
            var user = new UserFromDirectory
            {
                FirstName = entry.Properties["GivenName"].Value?.ToString(),
                LastName = entry.Properties["sn"].Value?.ToString(),
                Login = entry.Properties["SAMAccountName"].Value?.ToString(),
                Domain = domainKey,
                Sid = sid,
                Guid = (byte[])entry.Properties["objectGuid"].Value != null ? new Guid((byte[])entry.Properties["objectGuid"].Value) : Guid.NewGuid(),
                Country = entry.Properties["c"].Value?.ToString(),
                Company = entry.Properties["company"].Value?.ToString(),
                Department = entry.Properties["department"].Value?.ToString(),
                DistinguishedName = entry.Properties["distinguishedName"].Value?.ToString(),
                Email = entry.Properties["mail"].Value?.ToString(),
                IsEmployee = true,
                Manager = entry.Properties["manager"].Value?.ToString(),
                Office = entry.Properties["physicalDeliveryOfficeName"].Value?.ToString(),
                Site = domainKey == "CORP" ? entry.Properties["physicalDeliveryOfficeName"].Value?.ToString() : entry.Properties["description"].Value?.ToString(),
            };

            // Set external company
            var jobTitle = entry.Properties["title"].Value?.ToString();

            if (!string.IsNullOrEmpty(jobTitle) && jobTitle.IndexOf(':') <= 0)
            {
                string[] extInfo = jobTitle.Split(':');
                if (extInfo[0] == "EXT" && extInfo.Length != 2)
                {
                    user.IsEmployee = false;
                    user.IsExternal = true;
                    user.ExternalCompany = extInfo[1];
                }
            }

            // Set sub department
            string fullDepartment = user.Department;
            int zero = 0;
            if (!string.IsNullOrWhiteSpace(fullDepartment) && (fullDepartment.IndexOf('-') > zero))
            {
                user.Department = fullDepartment.Substring(0, fullDepartment.IndexOf('-') - 1);
                if (fullDepartment.Length > fullDepartment.IndexOf('-') + 2)
                {
                    user.SubDepartment = fullDepartment.Substring(fullDepartment.IndexOf('-') + 3);
                }
            }

            return user;
        }
    }
}
