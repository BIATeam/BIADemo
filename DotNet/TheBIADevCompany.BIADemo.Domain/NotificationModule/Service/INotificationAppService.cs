// <copyright file="INotificationAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.NotificationModule.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Notification;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate;

    /// <summary>
    /// The interface defining the notification application service.
    /// </summary>
    public interface INotificationAppService : ICrudAppServiceBase<NotificationDto, Notification, LazyLoadDto>
    {
        /// <summary>
        /// Set the notification as read.
        /// </summary>
        /// <param name="notification">The notification data transfer object.</param>
        /// <returns>A task returning the updated data transfer object.</returns>
        Task<NotificationDto> SetAsRead(NotificationDto notification);

        /// <summary>
        /// Return the list of unreadIds.
        /// </summary>
        /// <param name="userId">the user Id.</param>
        /// <returns>The list of int.</returns>
        Task<List<int>> GetUnreadIds(int userId);
    }
}