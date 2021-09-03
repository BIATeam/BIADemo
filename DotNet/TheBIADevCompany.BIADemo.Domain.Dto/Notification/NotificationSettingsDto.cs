// <copyright file="NotificationSettingsDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Notification
{
    using BIA.Net.Core.Domain.Dto.Base;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;

    /// <summary>
    /// The DTO used for setting up a notification.
    /// </summary>
    public class NotificationSettingsDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the identifier of the user who triggered the notification.
        /// </summary>
        public int CreatedById { get; set; }

        /// <summary>
        /// Gets or sets the role identifier to be notified, if any.
        /// </summary>
        public int? NotifiedRoleId { get; set; }

        /// <summary>
        /// Gets or sets the role to be notified, if any.
        /// </summary>
        public virtual RoleDto NotifiedRole { get; set; }

        /// <summary>
        /// Gets or sets the site identifier.
        /// </summary>
        public int SiteId { get; set; }
    }
}