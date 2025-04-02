// <copyright file="IBiaApiAuthRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface BiaApiAuthRepository.
    /// </summary>
    public interface IBiaApiAuthRepository
    {
        /// <summary>
        /// Gets the token with the fine permissions for an API of a BIA application.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="url">The URL login.</param>
        /// <returns>The token with the fine permissions.</returns>
        Task<string> LoginAsync(string baseAddress, string url = "/api/Auth/login?lightToken=false");

        /// <summary>
        /// Gets the token without the fine permissions for an API of a BIA application.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="url">The URL login.</param>
        /// <returns>The token without the fine permissions.</returns>
        Task<string> GetTokenAsync(string baseAddress, string url = "/api/Auth/token");
    }
}