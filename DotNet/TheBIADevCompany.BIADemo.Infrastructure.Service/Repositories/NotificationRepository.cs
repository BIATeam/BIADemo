// <copyright file="NotificationRepository.cs" company="BIA.Net">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Notification;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Domain.Dto.Notification;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Service;

    /// <summary>
    /// The class representing a NotificationRepository.
    /// </summary>
    /// <seealso cref="BIA.Net.Core.Domain.INotificationRepository" />
    public class NotificationRepository : MailRepository, INotification
    {
        private readonly INotificationAppService notificationAppService;
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="notificationAppService">The notification service.</param>
        public NotificationRepository(IOptions<BiaNetSection> configuration, INotificationAppService notificationAppService)
            : base(configuration)
        {
            this.notificationAppService = notificationAppService;
        }

        /// <summary>
        /// Create a notification displayed in UI.
        /// </summary>
        /// <param name="notification">The noticiation.</param>
        /// <returns>the task.</returns>
        public async Task CreateNotification(NotificationDto notification)
        {
            await this.notificationAppService.AddAsync(notification);
        }
    }
}
