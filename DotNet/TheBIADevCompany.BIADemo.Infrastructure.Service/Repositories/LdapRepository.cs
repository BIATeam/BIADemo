// <copyright file="LdapRepository.cs" company="TheBIADevCompany">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories
{
    using System;
    using System.DirectoryServices;
    using System.DirectoryServices.AccountManagement;
    using System.Security.Principal;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Service;

    /// <summary>
    /// Class the manipulate AD.
    /// </summary>
    public class LdapRepository : GenericLdapRepository<UserFromDirectory>
    {
        private readonly IUserIdentityKeyDomainService userIdentityKeyDomainService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LdapRepository"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="ldapRepositoryHelper">The ldap helper.</param>
        /// <param name="userIdentityKeyDomainService">The user Identity Key Domain Service.</param>
        public LdapRepository(
            ILogger<GenericLdapRepository<UserFromDirectory>> logger,
            IOptions<BiaNetSection> configuration,
            ILdapRepositoryHelper ldapRepositoryHelper,
            IUserIdentityKeyDomainService userIdentityKeyDomainService)
            : base(logger, configuration, ldapRepositoryHelper)
        {
            this.userIdentityKeyDomainService = userIdentityKeyDomainService;
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

        /// <summary>
        /// Gets the Identity Type to search object with the identity key from Directory.
        /// It is use by the function UserPrincipal.FindByIdentity.
        /// If you change it parse all other #IdentityKey to be sure thare is a match (Database, Ldap, Idp, WindowsIdentity).
        /// </summary>
        /// <returns>Return the Identity Key.</returns>
        protected override IdentityType GetIdentityKeyType()
        {
            return IdentityType.SamAccountName;
        }

        /// <summary>
        /// Gets the Identity Key to compare with User in database.
        /// It is use to specify the unique identifier that is compare during the authentication process.
        /// If you change it parse all other #IdentityKey to be sure thare is a match (Database, Ldap, Idp, WindowsIdentity).
        /// </summary>
        /// <param name="userFromDirectory">the userFromDirectory.</param>
        /// <returns>Return the Identity Key.</returns>
        protected override string GetIdentityKey(UserFromDirectoryDto userFromDirectory)
        {
            return this.userIdentityKeyDomainService.GetDirectoryIdentityKey(userFromDirectory);
        }
    }
}
