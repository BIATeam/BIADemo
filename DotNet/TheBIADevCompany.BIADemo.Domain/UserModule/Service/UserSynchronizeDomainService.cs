// <copyright file="UserSynchronizeDomainService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Service
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;
    using static TheBIADevCompany.BIADemo.Crosscutting.Common.Rights;

    /// <summary>
    /// The service used for synchronization between AD and DB.
    /// </summary>
    public class UserSynchronizeDomainService : IUserSynchronizeDomainService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly ITGenericRepository<User, int> repository;

        /// <summary>
        /// The AD helper.
        /// </summary>
        private readonly IUserDirectoryRepository<UserFromDirectory> userDirectoryHelper;

        /// <summary>
        /// The user IdentityKey Domain Service.
        /// </summary>
        private readonly IUserIdentityKeyDomainService userIdentityKeyDomainService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSynchronizeDomainService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="configuration">The configuration of the BiaNet section.</param>
        /// <param name="adHelper">The AD helper.</param>
        /// <param name="userIdentityKeyDomainService">The user IdentityKey Domain Service.</param>
        public UserSynchronizeDomainService(ITGenericRepository<User, int> repository, IUserDirectoryRepository<UserFromDirectory> adHelper, IUserIdentityKeyDomainService userIdentityKeyDomainService)
        {
            this.repository = repository;
            this.userDirectoryHelper = adHelper;
            this.userIdentityKeyDomainService = userIdentityKeyDomainService;
        }

        /// <inheritdoc cref="IUserSynchronizeDomainService.SynchronizeFromADGroupAsync"/>
        public async Task SynchronizeFromADGroupAsync(bool fullSynchro = false)
        {
            List<User> users = (await this.repository.GetAllEntityAsync(includes: [x => x.Roles])).ToList();
            List<string> usersSidInDirectory = (await this.userDirectoryHelper.GetAllUsersSidInRoleToSync("User", fullSynchro))?.ToList();

            if (usersSidInDirectory == null)
            {
                // If user in DB just synchronize the field of the active user.
                foreach (User user in users)
                {
                    if (user.IsActive)
                    {
                        var userFromDirectory = await this.userDirectoryHelper.ResolveUserByIdentityKey(this.userIdentityKeyDomainService.GetDatabaseIdentityKey(user), fullSynchro);
                        if (userFromDirectory != null)
                        {
                            this.ResynchronizeUser(user, userFromDirectory);
                        }
                    }
                }
            }
            else if (usersSidInDirectory.Count > 0)
            {
                ConcurrentBag<UserFromDirectory> usersFromDirectory = new ConcurrentBag<UserFromDirectory>();

                Parallel.ForEach(usersSidInDirectory, sid =>
                {
                    var userFromDirectory = this.userDirectoryHelper.ResolveUserBySid(sid, fullSynchro).Result;
                    if (userFromDirectory != null)
                    {
                        usersFromDirectory.Add(userFromDirectory);
                    }
                });

                foreach (User user in users)
                {
                    var userFromDirectory = usersFromDirectory.FirstOrDefault(this.userIdentityKeyDomainService.CheckDirectoryIdentityKey(this.userIdentityKeyDomainService.GetDatabaseIdentityKey(user)).Compile());
                    if (userFromDirectory == null)
                    {
                        if (user.IsActive)
                        {
                            this.DeactivateUser(user);
                        }
                    }
                    else
                    {
                        this.ResynchronizeUser(user, userFromDirectory);
                    }
                }

                foreach (UserFromDirectory userFromDirectory in usersFromDirectory)
                {
#pragma warning disable S6602 // "Find" method should be used instead of the "FirstOrDefault" extension
                    var foundUser = users.FirstOrDefault(this.userIdentityKeyDomainService.CheckDatabaseIdentityKey(this.userIdentityKeyDomainService.GetDirectoryIdentityKey(userFromDirectory)).Compile());
#pragma warning restore S6602 // "Find" method should be used instead of the "FirstOrDefault" extension

                    this.AddOrActiveUserFromDirectory(userFromDirectory, foundUser);
                }
            }

            await this.repository.UnitOfWork.CommitAsync();
        }

        /// <summary>
        /// Deactivaye a user.
        /// </summary>
        /// <param name="user">The user to deactivate.</param>
        public void DeactivateUser(User user)
        {
            user.Roles.Clear();
            user.IsActive = false;
        }

        /// <summary>
        /// Add or active User from AD.
        /// </summary>
        /// <param name="userFormDirectory">the user in Directory.</param>
        /// <param name="foundUser">the User if exist in repository.</param>
        /// <returns>The async task.</returns>
        public User AddOrActiveUserFromDirectory(UserFromDirectory userFormDirectory, User foundUser)
        {
            if (foundUser == null)
            {
                if (this.userIdentityKeyDomainService.GetDirectoryIdentityKey(userFormDirectory) != this.userIdentityKeyDomainService.GetDirectoryIdentityKey(new UserFromDirectory()))
                {
                    // Create the missing user
                    User user = new User();
                    UserFromDirectory.UpdateUserFieldFromDirectory(user, userFormDirectory);
                    this.repository.Add(user);
                    return user;
                }
            }
            else if (!foundUser.IsActive)
            {
                foundUser.IsActive = true;
            }

            return foundUser;
        }

        private void ResynchronizeUser(User user, UserFromDirectory userFromDirectory)
        {
            if (userFromDirectory != null)
            {
                UserFromDirectory.UpdateUserFieldFromDirectory(user, userFromDirectory);
            }
        }
    }
}