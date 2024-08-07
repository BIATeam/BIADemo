// <copyright file="UserDataDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using System.Collections.Generic;
    using System.Linq;
    using BIA.Net.Core.Domain.Dto.Option;
    using Newtonsoft.Json;

    /// <summary>
    /// UserData Dto.
    /// </summary>
    public class UserDataDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDataDto"/> class.
        /// </summary>
        public UserDataDto()
        {
            this.CurrentTeams = new List<CurrentTeamDto>();
        }

        /// <summary>
        /// Gets or sets the current team.
        /// </summary>
        [JsonProperty("currentTeams")]
        public ICollection<CurrentTeamDto> CurrentTeams { get; set; }

        /// <summary>
        /// Gets the current team identifier.
        /// </summary>
        /// <param name="teamTypeId">The team type identifier.</param>
        /// <returns>the team id and 0 if not found.</returns>
        public int GetCurrentTeamId(int teamTypeId)
        {
            var currentSite = this.CurrentTeams?.FirstOrDefault(t => t.TeamTypeId == teamTypeId);
            if (currentSite != null)
            {
                return currentSite.TeamId;
            }

            return 0;
        }

        /// <summary>
        /// Gets the current team.
        /// </summary>
        /// <param name="teamTypeId">The team type identifier.</param>
        /// <returns>the current Team.</returns>
        public CurrentTeamDto GetCurrentTeam(int teamTypeId)
        {
            var currentTeam = this.CurrentTeams?.FirstOrDefault(t => t.TeamTypeId == teamTypeId);
            if (currentTeam != null)
            {
                return currentTeam;
            }

            return null;
        }
    }
}
