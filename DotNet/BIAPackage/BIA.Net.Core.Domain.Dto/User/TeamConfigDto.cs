// <copyright file="TeamConfigDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using BIA.Net.Core.Common.Enum;

    /// <summary>
    /// The DTO used for notifications.
    /// </summary>
    public class TeamConfigDto
    {
        /// <summary>
        /// Gets or sets is the default value.
        /// </summary>
        public int TeamTypeId { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        public RoleMode RoleMode { get; set; }

        /// <summary>
        /// Gets or sets if appear in UI header (normaly should not be use in back).
        /// </summary>
        public bool InHeader { get; set; }
    }
}