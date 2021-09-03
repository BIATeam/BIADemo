// <copyright file="NotificationAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Notification
{
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application;
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.Dto.Notification;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;
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