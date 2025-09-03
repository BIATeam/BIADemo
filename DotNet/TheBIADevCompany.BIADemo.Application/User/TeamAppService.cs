// <copyright file="TeamAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System.Collections.Immutable;
    using System.Security.Claims;
    using System.Security.Principal;
    using BIA.Net.Core.Application.User;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.User.Entities;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.User;
    using TheBIADevCompany.BIADemo.Domain.User.Mappers;

    /// <summary>
    /// The application service used for team.
    /// </summary>
    /// <typeparam name="TEnumTeamTypeId">The type for enum Team Type Id.</typeparam>
    /// <typeparam name="TTeamMapper">The type of team Mapper.</typeparam>
    public class TeamAppService : BaseTeamAppService<TeamTypeId, TeamMapper>, ITeamAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public TeamAppService(ITGenericRepository<BaseEntityTeam, int> repository, IPrincipal principal)
            : base(repository, principal)
        {
        }

        /// <summary>
        /// Check autorize based on teamTypeId.
        /// </summary>
        /// <param name="principal"><see cref="ClaimsPrincipal"/>.</param>
        /// <param name="teamTypeId">the type team Id.</param>
        /// <param name="teamId">the team Id.</param>
        /// <param name="roleSuffix">the last part of the permission.</param>
        /// <returns>true if authorized.</returns>
        public bool IsAuthorizeForTeamType(ClaimsPrincipal principal, TeamTypeId teamTypeId, int teamId, string roleSuffix)
        {
            return this.IsAuthorizeForTeamType(principal, teamTypeId, teamId, roleSuffix, TeamConfig.Config);
        }

        /// <inheritdoc/>
        public ImmutableList<TeamConfigDto> GetTeamsConfig()
        {
            return this.GetTeamsConfig(TeamConfig.Config);
        }
    }
}