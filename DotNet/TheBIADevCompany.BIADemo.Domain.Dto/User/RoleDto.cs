// <copyright file="RoleDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.User
{
    /// <summary>
    /// The DTO used for role.
    /// </summary>
    public class RoleDto
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the label in english.
        /// </summary>
        public string LabelEn { get; set; }

        /// <summary>
        /// Gets or sets the label in french.
        /// </summary>
        public string LabelFr { get; set; }

        /// <summary>
        /// Gets or sets the label in spanish.
        /// </summary>
        public string LabelEs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role is the default one.
        /// </summary>
        public bool IsDefault { get; set; }
    }
}