// <copyright file="MemberInfoDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.User
{
    /// <summary>
    /// The DTO used to manage site member.
    /// </summary>
    public class MemberInfoDto
    {
        /// <summary>
        /// Gets or sets the user first name.
        /// </summary>
        public string UserFirstName { get; set; }

        /// <summary>
        /// Gets or sets the user last name.
        /// </summary>
        public string UserLastName { get; set; }

        /// <summary>
        /// Gets or sets the user login.
        /// </summary>
        public string UserLogin { get; set; }
    }
}