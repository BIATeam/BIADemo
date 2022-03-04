// <copyright file="TeamDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

using BIA.Net.Core.Domain.Dto.Base;
using System.Collections.Generic;

namespace BIA.Net.Core.Domain.Dto.User
{ 
    /// <summary>
    /// The DTO used to manage site.
    /// </summary>
    public class TeamDto : BaseDto<int>
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public int TeamTypeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the site is the default one.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public ICollection<RoleDto> Roles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is administrator of this team.
        /// </summary>
        public bool IsUserTeamAdmin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is administrator of the notification.
        /// </summary>
        public bool IsUserNotificationAdmin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is administrator of the view.
        /// </summary>
        public bool IsUserViewAdmin { get; set; }
    }
}