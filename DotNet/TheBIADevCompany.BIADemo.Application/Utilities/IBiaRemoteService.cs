// BIADemo only
// <copyright file="IBiaRemoteService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Utilities
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface BiaRemoteService.
    /// </summary>
    public interface IBiaRemoteService
    {
        /// <summary>
        /// Ping.
        /// </summary>
        /// <returns>Return true if ok.</returns>
        Task<bool> PingAsync();
    }
}