// <copyright file="IAuthAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.User;

    /// <summary>
    /// Interface AuthService.
    /// </summary>
    public interface IAuthAppService
    {
        /// <summary>
        /// Logins the on teams asynchronous.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="loginParam">The login parameter.</param>
        /// <returns>AuthInfo.</returns>
        Task<AuthInfoDTO<UserDataDto, AdditionalInfoDto>> LoginOnTeamsAsync(IIdentity identity, LoginParamDto loginParam);

        /// <summary>
        /// Logins the on teams asynchronous.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="loginParam">The login parameter.</param>
        /// <returns>AuthInfo.</returns>
        Task<AuthInfoDTO<UserDataDto, AdditionalInfoDto>> LoginOnTeamsAsync(WindowsIdentity identity, LoginParamDto loginParam);
    }
}