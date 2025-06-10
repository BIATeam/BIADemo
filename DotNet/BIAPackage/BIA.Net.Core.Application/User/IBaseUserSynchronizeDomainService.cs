// <copyright file="IBaseUserSynchronizeDomainService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.User
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// The interface defining the user synchronize domain service.
    /// </summary>
    /// <typeparam name="TUser">The type of user.</typeparam>
    /// <typeparam name="TUserFromDirectory">The type of user from directory.</typeparam>
    public interface IBaseUserSynchronizeDomainService<TUser, TUserFromDirectory>
        where TUser : BaseUser, new()
        where TUserFromDirectory : IUserFromDirectory, new()
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
        TUser AddOrActiveUserFromDirectory(TUserFromDirectory userFormDirectory, TUser foundUser);

        /// <summary>
        /// Deactivaye a user.
        /// </summary>
        /// <param name="user">The user to deactivate.</param>
        void DeactivateUser(TUser user);

        /// <summary>
        /// Updates the user field from directory.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="userDirectory">The user directory.</param>
        void UpdateUserFieldFromDirectory(TUser user, TUserFromDirectory userDirectory);
    }
}