// <copyright file="RoleModeDto.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The DTO used for notifications.
    /// </summary>
    public class RoleModeDto
    {
        /// <summary>
        /// Gets or sets is the defaul value.
        /// </summary>
        public int TeamTypeId { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        public RoleMode roleMode { get; set; }
    }
}