// <copyright file="LdapRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories
{
    using System;
    using System.DirectoryServices;
    using System.DirectoryServices.AccountManagement;
    using System.DirectoryServices.ActiveDirectory;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using System.Security.Principal;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.User.Services;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.User.Models;

    /// <summary>
    /// Class the manipulate AD.
    /// </summary>
    public class LdapRepository : GenericLdapRepository<UserFromDirectoryDto, UserFromDirectory>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LdapRepository"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="ldapRepositoryHelper">The ldap helper.</param>
        /// <param name="userIdentityKeyDomainService">The user Identity Key Domain Service.</param>
        public LdapRepository(
            ILogger<LdapRepository> logger,
            IOptions<BiaNetSection> configuration,
            ILdapRepositoryHelper ldapRepositoryHelper,
            IUserIdentityKeyDomainService userIdentityKeyDomainService)
            : base(logger, configuration, ldapRepositoryHelper, userIdentityKeyDomainService)
        {
        }

        /// <summary>
        /// Convert the Ad entry in a UserInfoDirectory Object.
        /// </summary>
        /// <param name="entry">Entry from AD.</param>
        /// <param name="domainKey">Domain Name in config file where domain found.</param>
        /// <returns>The UserInfoDirectory object.</returns>
        [SupportedOSPlatform("windows")]
        protected override UserFromDirectory ConvertToUserDirectory(DirectoryEntry entry, string domainKey)
        {
            var sid = new SecurityIdentifier((byte[])entry.Properties["objectSid"].Value, 0).ToString();
            UserFromDirectory user;
            string localLdapName;
            if (!this.ldapRepositoryHelper.IsLocalServerOnADomain(out localLdapName))
            {
                user = new UserFromDirectory
                {
                    FirstName = entry.Name,
                    Login = entry.Name,
                    Domain = domainKey,
                    Sid = sid,
                };
            }
            else
            {
                if (domainKey == ".")
                {
                    domainKey = this.ldapRepositoryHelper.ConvertToNetBiosName(localLdapName);
                }

                user = new UserFromDirectory
                {
                    FirstName = entry.Properties["GivenName"].Value?.ToString(),
                    LastName = entry.Properties["sn"].Value?.ToString(),
                    Login = entry.Properties["SAMAccountName"].Value?.ToString(),
                    Domain = domainKey,
                    Sid = sid,
                    Guid = (byte[])entry.Properties["objectGuid"].Value != null ? new Guid((byte[])entry.Properties["objectGuid"].Value) : Guid.NewGuid(),
                    Email = entry.Properties["mail"].Value?.ToString(),
#if BIA_USER_CUSTOM_FIELDS_BACK
                    Country = entry.Properties["c"].Value?.ToString(),
                    Company = entry.Properties["company"].Value?.ToString(),
                    Department = entry.Properties["department"].Value?.ToString(),
                    DistinguishedName = entry.Properties["distinguishedName"].Value?.ToString(),
                    IsEmployee = true,
                    Manager = entry.Properties["manager"].Value?.ToString(),
                    Office = entry.Properties["physicalDeliveryOfficeName"].Value?.ToString(),
                    Site = entry.Properties["description"].Value?.ToString(),
#endif
                };

#if BIA_USER_CUSTOM_FIELDS_BACK

                // Set external company
                var name = entry.Properties["cn"].Value?.ToString();

                if (!string.IsNullOrEmpty(name)
                    && name.IndexOf('[') > -1
                    && name.IndexOf(']') > -1
                    && name.IndexOf('[') < name.IndexOf(']')
                    && name.Split('[')[1].Split(']')[0] == "EXT")
                {
                        user.IsEmployee = false;
                        user.IsExternal = true;
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
#endif
            }

            return user;
        }
    }
}
