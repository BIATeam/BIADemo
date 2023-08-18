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
        public UserDataDto()
        {
            this.CurrentTeams = new List<CurrentTeamDto>();
        }

        /// <summary>
        /// Gets or sets the current team.
        /// </summary>
        public ICollection<CurrentTeamDto> CurrentTeams { get; set; }

        public int GetCurrentTeamId(int teamTypeId)
        {
            var currentSite = this.CurrentTeams?.FirstOrDefault(t => t.TeamTypeId == teamTypeId);
            if (currentSite != null)
            {
                return currentSite.TeamId;
            }

            return 0;
        }

        public CurrentTeamDto GetCurrentTeam(int teamTypeId)
        {
            return this.CurrentTeams?.FirstOrDefault(t => t.TeamTypeId == teamTypeId);
        }
    }
}
