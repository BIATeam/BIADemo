// <copyright file="IBaseTeamAppService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.User
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// The interface defining the application service for team.
    /// </summary>
    /// <typeparam name="TEnumTeamTypeId">The type of enum for TeamTypeId.</typeparam>
    public interface IBaseTeamAppService<TEnumTeamTypeId> : ICrudAppServiceBase<BaseDtoVersionedTeam, Team, int, PagingFilterFormatDto>
        where TEnumTeamTypeId : struct, Enum
    {
        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// <returns>The list of production sites.</returns>
        Task<IEnumerable<OptionDto>> GetAllOptionsAsync();

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="teamsConfig">The teams configuration.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userPermissions">The user rights.</param>
        /// <returns>all teams.</returns>
        Task<IEnumerable<BaseDtoVersionedTeam>> GetAllAsync(ImmutableList<BiaTeamConfig<Team>> teamsConfig, int userId = 0, IEnumerable<string> userPermissions = null);

        /// <summary>
        /// Check autorize based on teamTypeId.
        /// </summary>
        /// <param name="principal"><see cref="ClaimsPrincipal"/>.</param>
        /// <param name="teamTypeId">the type team Id.</param>
        /// <param name="teamId">the team Id.</param>
        /// <param name="roleSuffix">the last part of the permission.</param>
        /// <param name="teamsConfig">The teams configuration.</param>
        /// <returns>true if authorized.</returns>
        bool IsAuthorizeForTeamType(ClaimsPrincipal principal, TEnumTeamTypeId teamTypeId, int teamId, string roleSuffix, ImmutableList<BiaTeamConfig<Team>> teamsConfig);
    }
}