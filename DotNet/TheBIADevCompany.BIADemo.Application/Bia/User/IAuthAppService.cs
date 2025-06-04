// <copyright file="IAuthAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Bia.User
{
    using System.Collections.Immutable;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// Interface AuthService.
    /// </summary>
    /// <typeparam name="TUserDto">The type of user dto.</typeparam>
    /// <typeparam name="TUser">The type of user.</typeparam>
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
        /// <param name="teamsConfig">The teams configuration.</param>
        /// <returns>AuthInfo.</returns>
        Task<AuthInfoDto<AdditionalInfoDto>> LoginOnTeamsAsync(LoginParamDto loginParam, ImmutableList<BiaTeamConfig<Team>> teamsConfig);
#endif
    }
}