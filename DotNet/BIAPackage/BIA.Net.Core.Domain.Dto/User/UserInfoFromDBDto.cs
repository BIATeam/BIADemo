// <copyright file="UserInfoFromDBDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    /// <summary>
    /// The DTO used for user.
    /// </summary>
    public class UserInfoFromDBDto
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public UserInfoDto UserInfo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where the user is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}