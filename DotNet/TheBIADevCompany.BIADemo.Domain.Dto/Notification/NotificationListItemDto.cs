// <copyright file="NotificationListItemDto.cs" company="TheBIADevCompany">
//  Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Notification
{
    // Begin BIADemo
    using System;
    using BIA.Net.Core.Domain.Dto.CustomAttribute;

    // End BIADemo
    using BIA.Net.Core.Domain.Dto.Notification;

    /// <summary>
    /// Notification List Item Dto.
    /// </summary>
    public class NotificationListItemDto : BaseNotificationListItemDto
    {
        // Begin BIADemo

        /// <summary>
        /// Gets or sets the date the notification was acknowledged by the user.
        /// </summary>
        [BiaDtoField(AsLocalDateTime = true)]
        public DateTimeOffset? AcknowledgedAt { get; set; }

        // End BIADemo
    }
}
