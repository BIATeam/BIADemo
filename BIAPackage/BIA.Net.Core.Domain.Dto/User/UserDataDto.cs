// <copyright file="UserDataDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

using BIA.Net.Core.Domain.Dto.Option;
using System.Collections.Generic;

namespace BIA.Net.Core.Domain.Dto.User
{
    /// <summary>
    /// UserData Dto.
    /// </summary>
    public class UserDataDto
    {
        public UserDataDto()
        {
            CurrentSiteId = 0;
            DefaultSiteId = 0;
            Sites = new List<OptionDto>();
            CurrentRoleId = 0;
            DefaultRoleId = 0;
            Roles = new List<OptionDto>();
        }
        /// <summary>
        /// Gets or sets the current site identifier.
        /// </summary>
        public int CurrentSiteId { get; set; }

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
        public int CurrentRoleId { get; set; }

        /// <summary>
        /// Gets or sets the current site identifier.
        /// </summary>
        public int DefaultRoleId { get; set; }

        /// <summary>
        /// Gets or sets the List of sites 
        /// </summary>
        public List<OptionDto> Roles { get; set; }

    }
}
