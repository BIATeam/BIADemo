// <copyright file="NotificationAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Notification
{
    using System.Security.Principal;
    using BIA.Net.Core.Application.Notification;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.Dto.Notification;
    using TheBIADevCompany.BIADemo.Domain.Notification.Entities;
    using TheBIADevCompany.BIADemo.Domain.Notification.Mappers;
    using TheBIADevCompany.BIADemo.Domain.RepoContract.QueryCustomizer;

    /// <summary>
    /// Notification Service.
    /// </summary>
    public class NotificationAppService : BaseNotificationAppService<
        NotificationDto,
        NotificationListItemDto,
        Notification,
        NotificationMapper,
        NotificationListItemMapper>, INotificationAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="clientForHubService">The client for hub service.</param>
        /// <param name="queryCustomizer">The query customizer.</param>
        public NotificationAppService(ITGenericRepository<Notification, int> repository, IPrincipal principal, IClientForHubService clientForHubService, INotificationQueryCustomizer queryCustomizer)
            : base(repository, principal, clientForHubService, queryCustomizer)
        {
        }
    }
}
