// <copyright file="NotificationTypeDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Notification
{
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The DTO used to represent the notification type.
    /// </summary>
    public class NotificationTypeDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the notification code;s
        /// e.g: Task, Info, Success, Warning, Error.
        /// </summary>
        public string Code { get; set; }
    }
}