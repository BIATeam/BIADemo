// <copyright file="TokenAndTeamsDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using BIA.Net.Core.Domain.Dto.Base;
    using System.Collections.Generic;

    /// <summary>
    /// The DTO used to manage site.
    /// </summary>
    public class TokenAndTeamsDto
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        public object AuthInfo { get; set; }

        /// <summary>
        /// Gets or sets the teams.
        /// </summary>
        public ICollection<TeamDto> AllTeams { get; set; }
    }
}