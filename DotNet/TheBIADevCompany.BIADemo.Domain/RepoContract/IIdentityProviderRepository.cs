// <copyright file="IIdentityProviderRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.RepoContract
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// Interface IdentityProviderRepository.
    /// </summary>
    public interface IIdentityProviderRepository
    {
        /// <summary>
        /// Returns the list of users matching the search value.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="returnSize">Size of the return.</param>
        /// <returns>Get a <see cref="UserFromDirectory"/>.</returns>
        Task<List<UserFromDirectory>> SearchAsync(string search, int returnSize);
    }
}