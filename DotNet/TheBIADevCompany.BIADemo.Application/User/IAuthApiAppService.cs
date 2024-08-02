// <copyright file="IAuthApiAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface AuthApiAppService.
    /// </summary>
    public interface IAuthApiAppService
    {
        /// <summary>
        /// Logins the asynchronous.
        /// </summary>
        /// <returns>The token.</returns>
        Task<string> LoginAsync();
    }
}