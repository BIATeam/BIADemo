// BIADemo only
// <copyright file="IRemoteBiaApiRwService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Utilities
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface BiaRemoteService.
    /// </summary>
    public interface IRemoteBiaApiRwService
    {
        /// <summary>
        /// Ping.
        /// </summary>
        /// <returns>Return true if ok.</returns>
        Task<bool> PingAsync();
    }
}