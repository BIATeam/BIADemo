// <copyright file="NotificationDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Dto.Notification
{
    using System;
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;

    /// <summary>
    /// The DTO used for notifications.
    /// </summary>
    public class NotificationDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the Hangfire job identifier.
        /// </summary>
        public string JobId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the notification type identifier.
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// Gets or sets the notification type.
        /// </summary>
        public NotificationTypeDto Type { get; set; }

        /// <summary>
        /// Gets or sets whether the notification has been read.
        /// </summary>
        public bool Read { get; set; }

        /// <summary>
        /// Gets or sets the date the notification was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who triggered the notification.
        /// </summary>
        public int CreatedById { get; set; }

        /// <summary>
        /// Gets or sets the user who triggered the notification.
        /// </summary>
        public virtual UserDto CreatedBy { get; set; }

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

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public virtual SiteDto Site { get; set; }

        /// <summary>
        /// Gets or sets the list of users to be notified.
        /// </summary>
        public IList<NotificationUserDto> NotificationUsers { get; set; }

        /// <summary>
        /// Gets ot sets the route to load on notification click.
        /// </summary>
        public string TargetRoute { get; set; }

        /// <summary>
        /// Gets or sets the object identifier related to this notification.
        /// </summary>
        public int TargetId { get; set; }
    }
}