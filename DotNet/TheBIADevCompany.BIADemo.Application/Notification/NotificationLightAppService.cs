// <copyright file="NotificationLightAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Notification
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Application;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.Dto.Notification;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate;

    /// <summary>
    /// The application service used to manage views.
    /// </summary>
    public class NotificationLightAppService : CrudAppServiceBase<NotificationDto, Notification, LazyLoadDto, NotificationLightMapper>, INotificationLightAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationLightAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public NotificationLightAppService(ITGenericRepository<Notification> repository)
            : base(repository)
        {
        }

        /// <inheritdoc/>
        public async Task<int> GetUnreadCount()
        {
            var filters = new LazyLoadDto
            {
                First = 0,
                Rows = 1,
            };

            var unreadNotifications = await this.GetRangeAsync(filters, filter: x => !x.Read);

            return unreadNotifications.Item2;
        }
    }
}