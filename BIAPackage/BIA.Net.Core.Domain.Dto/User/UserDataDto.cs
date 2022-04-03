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

        [JsonIgnore]
        public int CurrentSiteId
        {
            get
            {
                var CurrentSite = CurrentTeams?.Where(t => t.TeamTypeId == 1).FirstOrDefault();
                if (CurrentSite != null)
                {
                    return CurrentSite.CurrentTeamId;
                }
                return 0;
            }
        }

        public int GetCurrentTeamId(int teamTypeId)
        {
            var CurrentSite = CurrentTeams?.Where(t => t.TeamTypeId == teamTypeId).FirstOrDefault();
            if (CurrentSite != null)
            {
                return CurrentSite.CurrentTeamId;
            }
            return 0;
        }


    }
}
