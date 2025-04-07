// <copyright file="IBiaWebApiAuthRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration.BiaWebApi;

    /// <summary>
    /// Interface Bia WebApi Repository.
    /// </summary>
    public interface IBiaWebApiAuthRepository
    {
        /// <summary>
        /// Initializes the specified bia web API.
        /// </summary>
        /// <param name="biaWebApi">The bia web API.</param>
        void Init(BiaWebApi biaWebApi);

        /// <summary>
        /// Gets the token without fine graine.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The token.</returns>
        Task<string> GetTokenAsync(string url = "/api/Auth/token");

        /// <summary>
        /// Gets the token with fine graine.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The token.</returns>
        Task<string> LoginAsync(string url = "/api/Auth/login?lightToken=false");
    }
}