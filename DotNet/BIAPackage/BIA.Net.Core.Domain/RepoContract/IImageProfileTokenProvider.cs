// <copyright file="IImageProfileTokenProvider.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface defining how to get token for external source.
    /// </summary>
    public interface IImageProfileTokenProvider
    {
        /// <summary>
        /// Get the token from external source.
        /// </summary>
        /// <returns>Return the token.</returns>
        Task<string> GetTokenAsync();
    }
}
