// <copyright file="NotificationDto.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Notification
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The DTO used for notifications.
    /// </summary>
    public class BaseNotificationDto : BaseDto<int>
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the title translated.
        /// </summary>
        public string TitleTranslated { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the description translated.
        /// </summary>
        public string DescriptionTranslated { get; set; }

        /// <summary>
        /// Gets or sets the notification type identifier.
        /// </summary>
        public OptionDto Type { get; set; }

        /// <summary>
        /// Gets or sets whether the notification has been read.
        /// </summary>
        public bool Read { get; set; }

        /// <summary>
        /// Gets or sets the date the notification was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the user who triggered the notification.
        /// </summary>
        public OptionDto CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the teams to be notified, if any.
        /// The users amongst one of these teams will be notified if
        /// the have, for these teams, one of the given roles (NotifiedTeams.Roles).
        /// </summary>
        public ICollection<NotificationTeamDto> NotifiedTeams { get; set; }

        /// <summary>
        /// Gets or sets the list of users id to be notified.
        /// </summary>
        public ICollection<OptionDto> NotifiedUsers { get; set; }

        /// <summary>
        /// Gets ot sets the target info to load on notification click or custom action.
        /// </summary>
        public string JData { get; set; }

        /// <summary>
        /// Gets ot sets the notification translations.
        /// </summary>
        public ICollection<NotificationTranslationDto> NotificationTranslations { get; set; }
    }
}