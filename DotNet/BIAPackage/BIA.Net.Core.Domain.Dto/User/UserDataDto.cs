// <copyright file="UserDataDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using BIA.Net.Core.Domain.Dto.Option;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// UserData Dto.
    /// </summary>
    public class UserDataDto
    {
        public UserDataDto()
        {
            CurrentTeams = new List<CurrentTeamDto>();
        }
        /// <summary>
        /// Gets or sets the current team.
        /// </summary>
        public ICollection<CurrentTeamDto> CurrentTeams { get; set; }

        public int GetCurrentTeamId(int teamTypeId)
        {
            var CurrentSite = CurrentTeams?.Where(t => t.TeamTypeId == teamTypeId).FirstOrDefault();
            if (CurrentSite != null)
            {
                return CurrentSite.TeamId;
            }
            return 0;
        }

        public CurrentTeamDto GetCurrentTeam(int teamTypeId)
        {
            var CurrentTeam = CurrentTeams?.Where(t => t.TeamTypeId == teamTypeId).FirstOrDefault();
            if (CurrentTeam != null)
            {
                return CurrentTeam;
            }
            return null;
        }

    }
}
