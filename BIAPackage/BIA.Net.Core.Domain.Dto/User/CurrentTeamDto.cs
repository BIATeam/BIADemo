// <copyright file="CurrentTeamDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using BIA.Net.Core.Domain.Dto.Option;
    using System.Collections.Generic;

    /// <summary>
    /// UserData Dto.
    /// </summary>
    public class CurrentTeamDto
    {
        public CurrentTeamDto()
        {
            TeamTypeId = 0;
            CurrentTeamId = 0;
            CurrentTeamTitle = "";
            CurrentRoleIds = new List<int>();
        }
        /// <summary>
        /// Gets or sets the current site identifier.
        /// </summary>
        public int TeamTypeId { get; set; }

        /// <summary>
        /// Gets or sets the current site identifier.
        /// </summary>
        public int CurrentTeamId { get; set; }

        /// <summary>
        /// Gets or sets the current site identifier.
        /// </summary>
        public string CurrentTeamTitle { get; set; }

        /// <summary>
        /// Gets or sets the current site identifier.
        /// </summary>
        public List<int> CurrentRoleIds { get; set; }
    }
}
