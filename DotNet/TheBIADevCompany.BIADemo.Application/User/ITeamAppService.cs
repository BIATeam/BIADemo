// <copyright file="ITeamAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System.Collections.Immutable;
    using System.Security.Claims;
    using BIA.Net.Core.Application.User;
    using BIA.Net.Core.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;

    /// <summary>
    /// The interface defining the application service for team.
    /// </summary>
    /// <typeparam name="TEnumTeamTypeId">The type of enum for TeamTypeId.</typeparam>
    public interface ITeamAppService : IBaseTeamAppService<TeamTypeId>
    {
        /// <summary>
        /// Check autorize based on teamTypeId.
        /// </summary>
        /// <param name="principal"><see cref="ClaimsPrincipal"/>.</param>
        /// <param name="teamTypeId">the type team Id.</param>
        /// <param name="teamId">the team Id.</param>
        /// <param name="roleSuffix">the last part of the permission.</param>
        /// <returns>true if authorized.</returns>
        bool IsAuthorizeForTeamType(ClaimsPrincipal principal, TeamTypeId teamTypeId, int teamId, string roleSuffix);

        /// <summary>
        /// Get the list of <see cref="TeamConfigDto"/>.
        /// </summary>
        /// <returns><see cref="ImmutableList{T}"/> of <see cref="TeamConfigDto"/>.</returns>
        ImmutableList<TeamConfigDto> GetTeamsConfig();
    }
}