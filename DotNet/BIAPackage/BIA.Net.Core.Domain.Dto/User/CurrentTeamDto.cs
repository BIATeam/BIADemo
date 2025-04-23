// <copyright file="CurrentTeamDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// UserData Dto.
    /// </summary>
    public class CurrentTeamDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentTeamDto"/> class.
        /// </summary>
        public CurrentTeamDto()
        {
            this.TeamTypeId = 0;
            this.TeamId = 0;
            this.TeamTitle = string.Empty;
            this.CurrentRoleIds = new List<int>();
        }

        /// <summary>
        /// Gets or sets the current site identifier.
        /// </summary>
        [JsonProperty("teamTypeId")]
        public int TeamTypeId { get; set; }

        /// <summary>
        /// Gets or sets the current site identifier.
        /// </summary>
        [JsonProperty("teamId")]
        public int TeamId { get; set; }

        /// <summary>
        /// Gets or sets the current site identifier.
        /// </summary>
        [JsonProperty("teamTitle")]
        public string TeamTitle { get; set; }

        /// <summary>
        /// Gets or sets the current site identifier.
        /// </summary>
        [JsonProperty("currentRoleIds")]
        public List<int> CurrentRoleIds { get; set; }

        /// <summary>
        /// Gets or sets if the default role should be use else use the current.
        /// </summary>
        [JsonProperty("useDefaultRoles")]
        public bool UseDefaultRoles { get; set; }
    }
}
