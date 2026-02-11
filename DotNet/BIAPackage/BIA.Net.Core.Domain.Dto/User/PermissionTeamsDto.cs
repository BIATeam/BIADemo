// <copyright file="PermissionTeamsDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// List of teams where user has a permission.
    /// </summary>
    public class PermissionTeamsDto
    {
        /// <summary>
        /// Permission.
        /// </summary>
        [JsonProperty("permissionId")]
        public int PermissionId { get; set; }

        /// <summary>
        /// List of team ids.
        /// </summary>
        [JsonProperty("teamIds")]
        public List<int> TeamIds { get; set; }

        /// <summary>
        /// As permission on all teams.
        /// </summary>
        [JsonProperty("isGlobal")]
        public bool IsGlobal { get; set; }
    }
}
