// <copyright file="IBaseFrontAuthAppService.cs" company="TheBIADevCompany">
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
    public interface IBaseFrontAuthAppService : IBaseAuthAppService
    {
        /// <summary>
        /// Logins the on teams asynchronous.
        /// </summary>
        /// <param name="loginParam">The login parameter.</param>
        /// <param name="teamsConfig">The teams configuration.</param>
        /// <returns>AuthInfo.</returns>
        Task<AuthInfoDto<AdditionalInfoDto>> LoginOnTeamsAsync(LoginParamDto loginParam, ImmutableList<BiaTeamConfig<Team>> teamsConfig);
    }
}