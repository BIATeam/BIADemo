// <copyright file="IUserProfileRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.RepoContract
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.User;

    /// <summary>
    /// Interface UserProfileRepository.
    /// </summary>
    public interface IUserProfileRepository
    {
        /// <summary>
        /// Gets the user profile.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns>The user profile.</returns>
        Task<UserProfileDto> GetAsync(string login);
    }
}