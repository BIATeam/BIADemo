// <copyright file="TeamDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;

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
        /// Gets or sets the parent.
        /// </summary>
        public int ParentTeamId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string ParentTeamTitle { get; set; }
    }
}