// <copyright file="NotificationTranslationDto.cs" company="TheBIADevCompany">
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
    public class RoleTranslationDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        public string Label { get; set; }
    }
}