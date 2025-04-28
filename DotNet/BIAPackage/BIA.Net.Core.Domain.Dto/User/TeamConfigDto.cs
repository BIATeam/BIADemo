// <copyright file="TeamConfigDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
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
    public class TeamConfigDto
    {
        /// <summary>
        /// Gets or sets is the default value.
        /// </summary>
        public int TeamTypeId { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        public RoleMode RoleMode { get; set; }

        /// <summary>
        /// Gets or sets if appear in UI header (normaly should not be use in back).
        /// </summary>
        public bool InHeader { get; set; }

        /// <summary>
        /// Indicates weither the team selector can be celared or not.
        /// </summary>
        public bool CanBeCleared { get; set; }
    }
}