// <copyright file="UserDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.User
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;

    /// <summary>
    /// The DTO used for user.
    /// </summary>
    public class UserDto : BaseUserDto
    {
        // Place here the custom user fields to retrieve in front.
#if BIA_USER_CUSTOM_FIELDS
        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string Country { get; set; }
#endif
    }
}