// <copyright file="NotificationDataDto.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Dto.Notification
{
    using System.Collections.Generic;

    /// <summary>
    /// The DTO used for notification data.
    /// </summary>
    public class NotificationDataDto
    {
        /// <summary>
        /// Gets or sets the target Angular route parameters.
        /// </summary>
        public string[] Route { get; set; }

        /// <summary>
        /// Gets or sets the i18n translation that will be used to
        /// display text in the action button of the notification.
        /// Defaults to "bia.action".
        /// </summary>
        public string Display { get; set; } = "bia.action";

        /// <summary>
        /// (Auto-switch) Gets or sets the Teams on which the user will be switched to.
        /// Only level 1 teams of different types.
        /// </summary>
        public List<NotificationTeamDto> Teams { get; set; }
    }
}