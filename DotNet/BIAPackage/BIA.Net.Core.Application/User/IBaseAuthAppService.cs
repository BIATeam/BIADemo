// <copyright file="IBaseAuthAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.User
{
    using System.Collections.Immutable;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// Interface AuthService.
    /// </summary>
    public interface IBaseAuthAppService
    {
        /// <summary>
        /// Logins.
        /// </summary>
        /// <returns>The JWT.</returns>
        Task<string> LoginAsync();
    }
}