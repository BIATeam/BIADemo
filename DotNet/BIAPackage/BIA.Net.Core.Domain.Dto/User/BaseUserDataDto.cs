// <copyright file="BaseUserDataDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>
    /// UserData Dto.
    /// </summary>
    public class BaseUserDataDto
    {
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the current team.
        /// </summary>
        [JsonProperty("currentTeams")]
        public ICollection<CurrentTeamDto> CurrentTeams { get; set; } = new List<CurrentTeamDto>();

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
