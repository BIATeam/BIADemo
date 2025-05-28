// <copyright file="UserDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The DTO used for user.
    /// </summary>
    public class UserDto : BaseDtoVersioned<int>
    {
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        public ICollection<OptionDto> Roles { get; set; }

        /// <summary>
        /// Gets or sets the teams.
        /// </summary>
        public ICollection<UserTeamDto> Teams { get; set; }
    }
}