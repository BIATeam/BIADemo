// <copyright file="IIdentityProviderRepository.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.User.Models;

    /// <summary>
    /// Interface IdentityProviderRepository.
    /// </summary>
    public interface IIdentityProviderRepository
    {
        /// <summary>
        /// Finds user by identityKey.
        /// </summary>
        /// <param name="identityKey">The identity key.</param>
        /// <param name="paramName">Name of the parameter on the IdP side.</param>
        /// <returns>Get a <see cref="UserFromDirectory"/>.</returns>
        Task<UserFromDirectory> FindUserAsync(string identityKey, string paramName = "username");

        /// <summary>
        /// Returns the list of users matching the search value.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="first">Index start.</param>
        /// <param name="max">Size of the return.</param>
        /// <returns>List of <see cref="UserFromDirectory"/>.</returns>
        Task<List<UserFromDirectory>> SearchUserAsync(string search, int first = 0, int max = 10);
    }
}