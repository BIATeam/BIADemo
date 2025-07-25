// <copyright file="NotificationTranslationDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Notification
{
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The DTO used for notifications.
    /// </summary>
    public class NotificationTranslationDto : BaseDto<int>
    {
        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
    }
}