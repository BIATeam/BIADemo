// <copyright file="IBaseFrontAuthAppService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
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
        /// <typeparam name="TAdditionalInfoDto">The type of AdditionalInfoDto.</typeparam>
        /// <typeparam name="TUserDataDto">The type of UserDataDto.</typeparam>
        Task<AuthInfoDto<TAdditionalInfoDto>> LoginOnTeamsAsync<TAdditionalInfoDto, TUserDataDto>(LoginParamDto loginParam, ImmutableList<BiaTeamConfig<Team>> teamsConfig)
             where TAdditionalInfoDto : BaseAdditionalInfoDto, new()
             where TUserDataDto : BaseUserDataDto, new();
    }
}