// <copyright file="INotificationAppService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Notification
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Notification;
    using BIA.Net.Core.Domain.Notification.Entities;
    using BIA.Net.Core.Domain.Service;

    /// <summary>
    /// The interface defining the notification application service.
    /// </summary>
    public interface INotificationAppService : ICrudAppServiceListAndItemBase<NotificationDto, NotificationListItemDto, Notification, int, LazyLoadDto>
    {
        /// <summary>
        /// Set the notification as read.
        /// </summary>
        /// <param name="dto">The notification dto.</param>
        /// <returns>A task.</returns>
        Task SetAsRead(NotificationDto dto);

        /// <summary>
        /// Set the notification as unread.
        /// </summary>
        /// <param name="dto">The notification dto.</param>
        /// <returns>A task.</returns>
        Task SetUnread(NotificationDto dto);

        /// <summary>
        /// Return the list of unreadIds.
        /// </summary>
        /// <param name="userId">the user Id.</param>
        /// <returns>The list of int.</returns>
        Task<List<int>> GetUnreadIds(int userId);

        /// <summary>
        /// Retrieve notification with all access.
        /// </summary>
        /// <param name="pagingFilterFormatDto">The paging filter.</param>
        /// <returns><see cref="IEnumerable{NotificationListItemDto}"/> results and total as int.</returns>
        Task<(IEnumerable<NotificationListItemDto> Results, int Total)> GetRangeWithAllAccessAsync(PagingFilterFormatDto pagingFilterFormatDto);
    }
}