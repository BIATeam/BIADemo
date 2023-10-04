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

        [JsonProperty("currentTeams")]
        /// <summary>
        /// Gets or sets the current team.
        /// </summary>
        public ICollection<CurrentTeamDto> CurrentTeams { get; set; }

        public int GetCurrentTeamId(int teamTypeId)
        {
            var CurrentSite = this.CurrentTeams?.Where(t => t.TeamTypeId == teamTypeId).FirstOrDefault();
            if (CurrentSite != null)
            {
                return CurrentSite.TeamId;
            }

            return 0;
        }

        public CurrentTeamDto GetCurrentTeam(int teamTypeId)
        {
            var CurrentTeam = this.CurrentTeams?.Where(t => t.TeamTypeId == teamTypeId).FirstOrDefault();
            if (CurrentTeam != null)
            {
                return CurrentTeam;
            }

            return null;
        }
    }
}
