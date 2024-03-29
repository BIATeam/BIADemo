// <copyright file="UserInfoDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    /// <summary>
    /// The DTO used for user.
    /// </summary>
    public class UserInfoDto
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public int Id { get; set; }

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
        /// Gets or sets the country from AD.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the default language.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where the user is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}