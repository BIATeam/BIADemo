// <copyright file="INotificationLightAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Notification
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application;
    using BIA.Net.Core.Domain.Dto.Base;
    using TheBIADevCompany.BIADemo.Domain.Dto.Notification;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate;

    /// <summary>
    /// The interface defining the notification light application service.
    /// </summary>
    public interface INotificationLightAppService : ICrudAppServiceBase<NotificationDto, Notification, LazyLoadDto>
    {
        /// <summary>
        /// Gets the number of unread notifications.
        /// </summary>
        /// <returns>The number of unread notifications.</returns>
        Task<int> GetUnreadCount();
    }
}