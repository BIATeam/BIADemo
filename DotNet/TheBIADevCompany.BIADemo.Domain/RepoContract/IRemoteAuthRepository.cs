// BIADemo only
// <copyright file="IRemoteAuthRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.RepoContract
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface Bia Remote Repository.
    /// </summary>
    public interface IRemoteAuthRepository
    {
        /// <summary>
        /// Ping the Api.
        /// </summary>
        /// <returns>Return true if api is ok.</returns>
        Task<bool> PingAsync();
    }
}