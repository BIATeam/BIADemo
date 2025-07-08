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
    /// <typeparam name="TAdditionalInfoDto">The type of additional info dto.</typeparam>
    public interface IBaseFrontAuthAppService<TAdditionalInfoDto> : IBaseAuthAppService
        where TAdditionalInfoDto : BaseAdditionalInfoDto, new()
    {
        /// <summary>
        /// Login on teams asynchronous.
        /// </summary>
        /// <param name="loginParam">The login parameter.</param>
        /// <param name="teamsConfig">The teams configuration.</param>
        /// <returns>AuthInfo.</returns>
        /// <typeparam name="TAdditionalInfoDto">The type of AdditionalInfoDto.</typeparam>
        /// <typeparam name="TUserDataDto">The type of UserDataDto.</typeparam>
        Task<AuthInfoDto<TAdditionalInfoDto>> LoginOnTeamsAsync(LoginParamDto loginParam, ImmutableList<BiaTeamConfig<BaseEntityTeam>> teamsConfig);
    }
}