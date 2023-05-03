// <copyright file="IUserSynchronizeDomainService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Service
{
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The interface defining the user synchronize domain service.
    /// </summary>
    public interface IUserSynchronizeDomainService
    {
        /// <summary>
        /// Synchronize the users in DB from the AD User group.
        /// </summary>
        /// <param name="fullSynchro">If true resynchronize existing user.</param>
        /// <returns>The result of the task.</returns>
        Task SynchronizeFromADGroupAsync(bool fullSynchro = false);

        /// <summary>
        /// Add or active User from AD.
        /// </summary>
        /// <param name="userFormDirectory">the user in Directory.</param>
        /// <param name="foundUser">the User if exist in repository.</param>
        /// <returns>The async task.</returns>
        User AddOrActiveUserFromDirectory(UserFromDirectory userFormDirectory, User foundUser);
    }
}