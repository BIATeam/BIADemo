// <copyright file="UserDataDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using BIA.Net.Core.Domain.Dto.Option;
    using System.Collections.Generic;

    /// <summary>
    /// UserData Dto.
    /// </summary>
    public class UserDataDto
    {
        public UserDataDto()
        {
            CurrentSiteId = 0;
            CurrentSiteTitle = "";
            DefaultSiteId = 0;
            Sites = new List<OptionDto>();
            CurrentRoleIds = new List<int>();
            DefaultRoleId = 0;
            Roles = new List<RoleDto>();
        }
        /// <summary>
        /// Gets or sets the current site identifier.
        /// </summary>
        public int CurrentSiteId { get; set; }

        /// <summary>
        /// Gets or sets the current site identifier.
        /// </summary>
        public string CurrentSiteTitle { get; set; }

        /// <summary>
        /// Gets or sets the current site identifier.
        /// </summary>
        public int DefaultSiteId { get; set; }

        /// <summary>
        /// Gets or sets the List of sites 
        /// </summary>
        public List<OptionDto> Sites { get; set; }

        /// <summary>
        /// Gets or sets the current site identifier.
        /// </summary>
        public List<int> CurrentRoleIds { get; set; }

        /// <summary>
        /// Gets or sets the current site identifier.
        /// </summary>
        public int DefaultRoleId { get; set; }

        /// <summary>
        /// Gets or sets the List of sites 
        /// </summary>
        public List<RoleDto> Roles { get; set; }

    }
}
