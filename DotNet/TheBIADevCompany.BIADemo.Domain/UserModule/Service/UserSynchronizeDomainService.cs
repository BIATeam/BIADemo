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
        private readonly ITGenericRepository<User> repository;

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
        public UserSynchronizeDomainService(ITGenericRepository<User> repository, IUserDirectoryRepository<UserFromDirectory> adHelper)
        {
            this.repository = repository;
            this.userDirectoryHelper = adHelper;
        }

        /// <inheritdoc cref="IUserSynchronizeDomainService.SynchronizeFromADGroupAsync"/>
        public async Task SynchronizeFromADGroupAsync(bool fullSynchro = false)
        {
            List<User> users = (await this.repository.GetAllEntityAsync()).ToList();
            List<string> usersSidInDirectory = (await this.userDirectoryHelper.GetAllUsersSidInRoleToSync("User")).ToList();

            if (usersSidInDirectory != null)
            {
                var resynchronizeTasks = new List<Task>();
                foreach (User user in users)
                {
                    if (user.Domain == "--")
                    {
                        // remap the Domain with the login (use only for migration at V3.2.0)
                        string domain = await this.userDirectoryHelper.ResolveUserDomainByLogin(user.Login);
                        if (domain != null)
                        {
                            user.Domain = domain;
                            this.repository.Update(user);
                        }
                    }

                    if (user.Sid == "--")
                    {
                        // remap the Sid with the login (use only for migration at V3.2.0)
                        string sid = await this.userDirectoryHelper.ResolveUserSidByLogin(user.Domain, user.Login);
                        if (sid != null)
                        {
                            user.Sid = sid;
                            this.repository.Update(user);
                        }
                    }

                    if (!usersSidInDirectory.Contains(user.Sid) && user.IsActive)
                    {
                        user.IsActive = false;
                        this.repository.Update(user);
                    }

                    if (fullSynchro && usersSidInDirectory.Contains(user.Sid))
                    {
                        resynchronizeTasks.Add(this.ResynchronizeUser(user));
                    }
                }

                await Task.WhenAll(resynchronizeTasks);

                var usersToAdd = new List<User>();

                foreach (string sid in usersSidInDirectory)
                {
                    var foundUser = users.FirstOrDefault(a => a.Sid == sid);

                    if (foundUser == null)
                    {
                        var userFormDirectory = await this.userDirectoryHelper.ResolveUserBySid(sid);

                        // Create the missing user
                        User user = new User();
                        UserFromDirectory.UpdateUserFieldFromDirectory(user, userFormDirectory);
                        usersToAdd.Add(user);
                    }
                    else if (!foundUser.IsActive)
                    {
                        foundUser.IsActive = true;
                        this.repository.Update(foundUser);
                    }
                }

                this.repository.AddRange(usersToAdd);

                await this.repository.UnitOfWork.CommitAsync();
            }
        }

        private async Task ResynchronizeUser(User user)
        {
            if (user.Sid != "--")
            {
                var userFormDirectory = await this.userDirectoryHelper.ResolveUserBySid(user.Sid);
                if (userFormDirectory != null)
                {
                    UserFromDirectory.UpdateUserFieldFromDirectory(user, userFormDirectory);
                    this.repository.Update(user);
                }
            }
        }
    }
}