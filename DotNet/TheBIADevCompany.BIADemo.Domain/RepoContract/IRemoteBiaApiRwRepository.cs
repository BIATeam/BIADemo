// BIADemo only
// <copyright file="IRemoteBiaApiRwRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.RepoContract
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface Bia Remote Repository.
    /// </summary>
    public interface IRemoteBiaApiRwRepository
    {
        /// <summary>
        /// Ping the Api.
        /// </summary>
        /// <returns>Return true if api is ok.</returns>
        Task<bool> PingAsync();
    }
}