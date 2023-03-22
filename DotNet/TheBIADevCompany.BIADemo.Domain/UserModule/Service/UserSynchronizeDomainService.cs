// <copyright file="UserSynchronizeDomainService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

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
        /// Initializes a new instance of the <see cref="UserSynchronizeDomainService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="configuration">The configuration of the BiaNet section.</param>
        /// <param name="adHelper">The AD helper.</param>
        public UserSynchronizeDomainService(ITGenericRepository<User, int> repository, IUserDirectoryRepository<UserFromDirectory> adHelper)
        {
            this.repository = repository;
            this.userDirectoryHelper = adHelper;
        }

        /// <inheritdoc cref="IUserSynchronizeDomainService.SynchronizeFromADGroupAsync"/>
        public async Task SynchronizeFromADGroupAsync(bool fullSynchro = false)
        {
            List<User> users = (await this.repository.GetAllEntityAsync()).ToList();
            List<string> usersSidInDirectory = (await this.userDirectoryHelper.GetAllUsersSidInRoleToSync("User"))?.ToList();

            if (usersSidInDirectory?.Count > 0)
            {
                List<UserFromDirectory> usersFromDirectory = new List<UserFromDirectory>();
                foreach (string sid in usersSidInDirectory)
                {
                    // 4s for 40 users
                    var userFromDirectory = await this.userDirectoryHelper.ResolveUserBySid(sid);
                    if (userFromDirectory != null)
                    {
                        usersFromDirectory.Add(userFromDirectory);
                    }
                }

                var resynchronizeTasks = new List<Task>();
                foreach (User user in users)
                {
                    //TODO : use the key in setting.
                    if (!usersFromDirectory.Any(u => u.Login == user.Login))
                    {
                        if (user.IsActive)
                        {
                            this.DeactivateUser(user);
                        }
                    }
                    else
                    {
                        if (fullSynchro)
                        {
                            resynchronizeTasks.Add(this.ResynchronizeUser(user));
                        }
                    }
                }

                await Task.WhenAll(resynchronizeTasks);

                foreach (UserFromDirectory user in usersFromDirectory)
                {
                    //TODO : use the key in setting.
                    var foundUser = users.FirstOrDefault(a => a.Login == user.Login);

                    this.AddOrActiveUserFromDirectory(user, foundUser);
                }

                await this.repository.UnitOfWork.CommitAsync();
            }
        }

        /// <summary>
        /// Deactivaye a user.
        /// </summary>
        /// <param name="user">The user to deactivate.</param>
        public void DeactivateUser(User user)
        {
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
                if (userFormDirectory != null)
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

        private async Task ResynchronizeUser(User user)
        {
            if (user.Sid != "--")
            {
                var userFormDirectory = await this.userDirectoryHelper.ResolveUserBySid(user.Sid);
                if (userFormDirectory != null)
                {
                    UserFromDirectory.UpdateUserFieldFromDirectory(user, userFormDirectory);
                }
            }
        }
    }
}