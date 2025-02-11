// <copyright file="IAuthAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;

    /// <summary>
    /// Interface AuthService.
    /// </summary>
    public interface IAuthAppService
    {
#if BIA_BACK_TO_BACK_AUTH
        /// <summary>
        /// Logins.
        /// </summary>
        /// <returns>The JWT.</returns>
        Task<string> LoginAsync();
#endif
#if BIA_FRONT_FEATURE

        /// <summary>
        /// Logins the on teams asynchronous.
        /// </summary>
        /// <param name="loginParam">The login parameter.</param>
        /// <returns>AuthInfo.</returns>
        Task<AuthInfoDto<AdditionalInfoDto>> LoginOnTeamsAsync(LoginParamDto loginParam);
#endif
    }
}