// <copyright file="NotificationDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.User
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The DTO used for notifications.
    /// </summary>
    public class RoleDto : BaseDto<int>
    {
        /// <summary>
        /// Gets or sets is the defaul value.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the label in english.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the role translations.
        /// </summary>
        public virtual ICollection<RoleTranslationDto> RoleTranslations { get; set; }
    }
}