// <copyright file="MembersDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The DTO used for members.
    /// </summary>
    public class MembersDto
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public IEnumerable<OptionDto> Users { get; set; }

        /// <summary>
        /// Gets or sets the site id.
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public IEnumerable<OptionDto> Roles { get; set; }
    }
}