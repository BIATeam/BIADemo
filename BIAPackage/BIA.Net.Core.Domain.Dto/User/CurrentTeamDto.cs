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
            TeamId = 0;
            TeamTitle = "";
            CurrentRoleIds = new List<int>();
        }
        /// <summary>
        /// Gets or sets the current site identifier.
        /// </summary>
        public int TeamTypeId { get; set; }

        /// <summary>
        /// Gets or sets the current site identifier.
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// Gets or sets the current site identifier.
        /// </summary>
        public string TeamTitle { get; set; }

        /// <summary>
        /// Gets or sets the current site identifier.
        /// </summary>
        public List<int> CurrentRoleIds { get; set; }

        /// <summary>
        /// Gets or sets if the default role should be use else use the current.
        /// </summary>
        public bool UseDefaultRoles { get; set; }
    }
}
