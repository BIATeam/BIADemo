// <copyright file="NotificationAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.NotificationModule.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Notification;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate;

    /// <summary>
    /// The application service used to manage views.
    /// </summary>
    public class NotificationAppService : CrudAppServiceBase<NotificationDto, Notification, LazyLoadDto, NotificationMapper>, INotificationAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public NotificationAppService(ITGenericRepository<Notification> repository)
            : base(repository)
        {
        }

        /// <inheritdoc/>
        public async Task<NotificationDto> SetAsRead(NotificationDto notification)
        {
            notification.Read = true;
            return await this.UpdateAsync(notification);
        }

        /// <summary>
        /// Return the list of unreadIds.
        /// </summary>
        /// <param name="userId">the user Id.</param>
        /// <returns>The list of int.</returns>
        public async Task<List<int>> GetUnreadIds(int userId)
        {
            var results = await this.Repository.GetAllResultAsync<int>(selectResult: x => x.Id, filter: x => !x.Read);

            return results.ToList();
        }
    }
}