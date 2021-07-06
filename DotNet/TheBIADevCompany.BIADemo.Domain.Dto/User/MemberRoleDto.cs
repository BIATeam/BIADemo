// <copyright file="MemberRoleDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.User
{
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The DTO used for member roles.
    /// </summary>
    public class MemberRoleDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the member id.
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        public int RoleId { get; set; }
    }
}