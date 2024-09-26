// <copyright file="AdditionalInfoDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using System.Collections.Generic;

    /// <summary>
    /// Adtitionnal Information send to the front. Can be customized.
    /// </summary>
    public class AdditionalInfoDto
    {
        /// <summary>
        /// Gets or sets the user info.
        /// </summary>
        public UserInfoDto UserInfo { get; set; }

        /// <summary>
        /// Gets or sets the teams.
        /// </summary>
        public ICollection<TeamDto> Teams { get; set; }
    }
}
