// <copyright file="UserFromDirectoryDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using System;

    /// <summary>
    /// The DTO used for user coming from AD.
    /// </summary>
    public class UserFromDirectoryDto
    {
        /// <summary>
        /// Gets or sets the Display Name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the Identity Key.
        /// </summary>
        public string IdentityKey { get; set; }

        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        public string Domain { get; set; }
    }
}