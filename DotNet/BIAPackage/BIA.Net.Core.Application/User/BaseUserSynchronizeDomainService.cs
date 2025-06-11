// <copyright file="BaseUserSynchronizeDomainService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.User
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.User.Entities;
    using BIA.Net.Core.Domain.User.Services;

    /// <summary>
    /// The service used for synchronization between AD and DB.
    /// </summary>
    /// <typeparam name="TUser">The type of user.</typeparam>
    /// <typeparam name="TUserFromDirectoryDto">The type of user from directory dto.</typeparam>
    /// <typeparam name="TUserFromDirectory">The type of user from directory.</typeparam>
    public class BaseUserSynchronizeDomainService<TUser, TUserFromDirectoryDto, TUserFromDirectory> : IBaseUserSynchronizeDomainService<TUser, TUserFromDirectory>
        where TUser : BaseUser, new()
        where TUserFromDirectoryDto : BaseUserFromDirectoryDto, new()
        where TUserFromDirectory : IUserFromDirectory, new()
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly ITGenericRepository<TUser, int> repository;

        /// <summary>
        /// The AD helper.
        /// </summary>
        private readonly IUserDirectoryRepository<TUserFromDirectoryDto, TUserFromDirectory> userDirectoryHelper;

        /// <summary>
        /// The user IdentityKey Domain Service.
        /// </summary>
        private readonly IUserIdentityKeyDomainService userIdentityKeyDomainService;

        private readonly IIdentityProviderRepository<TUserFromDirectory> identityProviderRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseUserSynchronizeDomainService{TUser, TUserFromDirectoryDto, TUserFromDirectory}" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="adHelper">The AD helper.</param>
        /// <param name="userIdentityKeyDomainService">The user IdentityKey Domain Service.</param>
        /// <param name="identityProviderRepository">The identity provider repository.</param>
        public BaseUserSynchronizeDomainService(
            ITGenericRepository<TUser, int> repository,
            IUserDirectoryRepository<TUserFromDirectoryDto, TUserFromDirectory> adHelper,
            IUserIdentityKeyDomainService userIdentityKeyDomainService,
            IIdentityProviderRepository<TUserFromDirectory> identityProviderRepository)
        {
            this.repository = repository;
            this.userDirectoryHelper = adHelper;
            this.userIdentityKeyDomainService = userIdentityKeyDomainService;
            this.identityProviderRepository = identityProviderRepository;
        }

        /// <inheritdoc cref="IBaseUserSynchronizeDomainService.SynchronizeFromIdpAsync"/>
        public async Task SynchronizeFromIdpAsync()
        {
            IEnumerable<TUser> users = await this.repository.GetAllEntityAsync(filter: user => !string.IsNullOrWhiteSpace(user.Login) && user.IsActive);

            if (users?.Any() == true)
            {
                foreach (TUser user in users)
                {
                    List<TUserFromDirectory> userFromDirectories = await this.identityProviderRepository.SearchUserAsync(user.Login, 0, 1);
                    if (userFromDirectories.Count == 1 && string.Equals(userFromDirectories[0].Login, user.Login, StringComparison.OrdinalIgnoreCase))
                    {
                        TUserFromDirectory userFromDirectory = userFromDirectories[0];
                        this.ResynchronizeUser(user, userFromDirectory);
                    }
                }

                await this.repository.UnitOfWork.CommitAsync();
            }
        }

        /// <inheritdoc cref="IBaseUserSynchronizeDomainService.SynchronizeFromADGroupAsync"/>
        public async Task SynchronizeFromADGroupAsync(bool fullSynchro = false)
        {
            List<TUser> users = (await this.repository.GetAllEntityAsync(includes: new Expression<Func<TUser, object>>[] { x => x.Roles })).ToList();
            List<string> usersSidInDirectory = (await this.userDirectoryHelper.GetAllUsersSidInRoleToSync("User", fullSynchro))?.ToList();

            if (usersSidInDirectory == null)
            {
                // If user in DB just synchronize the field of the active user.
                foreach (TUser user in users)
                {
                    if (user.IsActive)
                    {
                        try
                        {
                            var userFromDirectory = await this.userDirectoryHelper.ResolveUserByIdentityKey(this.userIdentityKeyDomainService.GetDatabaseIdentityKey(user), fullSynchro);
                            if (userFromDirectory != null)
                            {
                                this.ResynchronizeUser(user, userFromDirectory);
                            }
                        }
                        catch (System.Runtime.InteropServices.COMException exception)
                        {
                            if (exception.ErrorCode == -2147023570)
                            {
                                this.DeactivateUser(user);
                            }
                        }
                    }
                }
            }
            else if (usersSidInDirectory.Count > 0)
            {
                ConcurrentBag<TUserFromDirectory> usersFromDirectory = new ConcurrentBag<TUserFromDirectory>();

                Parallel.ForEach(usersSidInDirectory, sid =>
                {
                    var userFromDirectory = this.userDirectoryHelper.ResolveUserBySid(sid, fullSynchro).Result;
                    if (userFromDirectory != null)
                    {
                        usersFromDirectory.Add(userFromDirectory);
                    }
                });

                foreach (TUser user in users)
                {
                    var userFromDirectory = usersFromDirectory.FirstOrDefault(this.userIdentityKeyDomainService.CheckDirectoryIdentityKey<TUserFromDirectory>(this.userIdentityKeyDomainService.GetDatabaseIdentityKey(user)).Compile());
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

                foreach (TUserFromDirectory userFromDirectory in usersFromDirectory)
                {
#pragma warning disable S6602 // "Find" method should be used instead of the "FirstOrDefault" extension
                    TUser foundUser = users.FirstOrDefault(this.userIdentityKeyDomainService.CheckDatabaseIdentityKey<TUser>(this.userIdentityKeyDomainService.GetDirectoryIdentityKey(userFromDirectory)).Compile());
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
        public void DeactivateUser(TUser user)
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
        public TUser AddOrActiveUserFromDirectory(TUserFromDirectory userFormDirectory, TUser foundUser)
        {
            if (foundUser == null)
            {
                if (this.userIdentityKeyDomainService.GetDirectoryIdentityKey(userFormDirectory) != this.userIdentityKeyDomainService.GetDirectoryIdentityKey(new TUserFromDirectory()))
                {
                    // Create the missing user
                    TUser user = new TUser();
                    this.UpdateUserFieldFromDirectory(user, userFormDirectory);
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

        /// <summary>
        /// UpdateUserField with the userFromDirectory object.
        /// </summary>
        /// <param name="user">the user object to update.</param>
        /// <param name="userDirectory">the user from directory object containing values.</param>
        public virtual void UpdateUserFieldFromDirectory(TUser user, TUserFromDirectory userDirectory)
        {
            user.Login = userDirectory.Login?.ToUpper();
            user.FirstName = userDirectory.FirstName?.Length > 50 ? userDirectory.FirstName?.Substring(0, 50) : userDirectory.FirstName ?? string.Empty;
            user.LastName = userDirectory.LastName?.Length > 50 ? userDirectory.LastName?.Substring(0, 50) : userDirectory.LastName ?? string.Empty;
            user.IsActive = true;
            user.LastSyncDate = DateTime.Now;
        }

        private void ResynchronizeUser(TUser user, TUserFromDirectory userFromDirectory)
        {
            if (userFromDirectory != null)
            {
                this.UpdateUserFieldFromDirectory(user, userFromDirectory);
            }
        }
    }
}