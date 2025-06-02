// <copyright file="UserSynchronizeDomainService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Bia.User
{
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.User.Models;
    using BIA.Net.Core.Domain.User.Services;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

    /// <summary>
    /// The service used for synchronization between AD and DB.
    /// </summary>
    public class UserSynchronizeDomainService : BaseUserSynchronizeDomainService<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSynchronizeDomainService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="adHelper">The AD helper.</param>
        /// <param name="userIdentityKeyDomainService">The user IdentityKey Domain Service.</param>
        /// <param name="identityProviderRepository">The identity provider repository.</param>
        public UserSynchronizeDomainService(
            ITGenericRepository<User, int> repository,
            IUserDirectoryRepository<UserFromDirectory> adHelper,
            IUserIdentityKeyDomainService<User> userIdentityKeyDomainService,
            IIdentityProviderRepository identityProviderRepository)
            : base(repository, adHelper, userIdentityKeyDomainService, identityProviderRepository)
        {
        }

        /// <summary>
        /// UpdateUserField with the userFromDirectory object.
        /// </summary>
        /// <param name="user">the user object to update.</param>
        /// <param name="userDirectory">the user from directory object containing values.</param>
        public override void UpdateUserFieldFromDirectory(User user, UserFromDirectory userDirectory)
        {
            base.UpdateUserFieldFromDirectory(user, userDirectory);
            user.Country = userDirectory.Country?.Length > 10 ? userDirectory.Country?.Substring(0, 10) : userDirectory.Country ?? string.Empty;
            user.Department = userDirectory.Department?.Length > 50 ? userDirectory.Department?.Substring(0, 50) : userDirectory.Department ?? string.Empty;
            user.DistinguishedName = userDirectory.DistinguishedName?.Length > 250 ? userDirectory.DistinguishedName?.Substring(0, 250) : userDirectory.DistinguishedName ?? string.Empty;
            user.Manager = userDirectory.Manager?.Length > 250 ? userDirectory.Manager?.Substring(0, 250) : userDirectory.Manager;
            user.ExternalCompany = userDirectory.ExternalCompany?.Length > 50 ? userDirectory.ExternalCompany?.Substring(0, 50) : userDirectory.ExternalCompany;
            user.IsEmployee = userDirectory.IsEmployee;
            user.IsExternal = userDirectory.IsExternal;
            user.Company = userDirectory.Company?.Length > 50 ? userDirectory.Company?.Substring(0, 50) : userDirectory.Company ?? string.Empty;
            user.Office = userDirectory.Office?.Length > 20 ? userDirectory.Office?.Substring(0, 20) : userDirectory.Office ?? string.Empty;
            user.Site = userDirectory.Site?.Length > 50 ? userDirectory.Site?.Substring(0, 50) : userDirectory.Site ?? string.Empty;
            user.SubDepartment = userDirectory.SubDepartment?.Length > 50 ? userDirectory.SubDepartment?.Substring(0, 50) : userDirectory.SubDepartment;
        }
    }
}