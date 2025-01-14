// <copyright file="IUserSynchronizeDomainService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;
    using TheBIADevCompany.BIADemo.Domain.User.Models;

    /// <summary>
    /// The interface defining the user synchronize domain service.
    /// </summary>
    public interface IUserSynchronizeDomainService
    {
        /// <summary>
        /// Synchronize the users in DB from the Idp.
        /// </summary>
        /// <returns>The result of the task.</returns>
        Task SynchronizeFromIdpAsync();

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

        /// <summary>
        /// Deactivaye a user.
        /// </summary>
        /// <param name="user">The user to deactivate.</param>
        void DeactivateUser(User user);

        /// <summary>
        /// Updates the user field from directory.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="userDirectory">The user directory.</param>
        void UpdateUserFieldFromDirectory(User user, UserFromDirectory userDirectory);
    }
}