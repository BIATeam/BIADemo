// BIADemo only
// <copyright file="IRemoteAuthService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Utilities
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface BiaRemoteService.
    /// </summary>
    public interface IRemoteAuthService
    {
        /// <summary>
        /// Ping.
        /// </summary>
        /// <returns>Return true if ok.</returns>
        Task<bool> PingAsync();
    }
}