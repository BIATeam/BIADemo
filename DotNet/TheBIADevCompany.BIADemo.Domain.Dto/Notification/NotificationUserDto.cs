// <copyright file="NotificationUserDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Notification
{
    using BIA.Net.Core.Domain.Dto.Base;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;

    /// <summary>
    /// The DTO used to represent the notification user.
    /// </summary>
    public class NotificationUserDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the notification identifier.
        /// </summary>
        public int NotificationId { get; set; }

        /// <summary>
        /// Gets or sets the notification.
        /// </summary>
        public NotificationDto Notification { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public UserDto User { get; set; }

        /// <summary>
        /// Gets or sets whether the user has read the notification.
        /// </summary>
        public bool Read { get; set; }
    }
}