// <copyright file="MemberDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.User
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The DTO used for members.
    /// </summary>
    public class MemberDto : BaseDto<int>
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public OptionDto User { get; set; }

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