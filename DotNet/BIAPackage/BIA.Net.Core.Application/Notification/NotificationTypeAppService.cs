// <copyright file="NotificationTypeAppService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Notification
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Notification.Entities;
    using BIA.Net.Core.Domain.Notification.Mappers;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;

    /// <summary>
    /// The application service used for notification type.
    /// </summary>
    public class NotificationTypeAppService : DomainServiceBase<NotificationType, int>, INotificationTypeAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTypeAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="userContext">The user context.</param>
        public NotificationTypeAppService(ITGenericRepository<NotificationType, int> repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Return options.
        /// </summary>
        /// <returns>List of OptionDto.</returns>
        public Task<IEnumerable<OptionDto>> GetAllOptionsAsync()
        {
            NotificationTypeOptionMapper mapper = this.InitMapper<OptionDto, NotificationTypeOptionMapper>();
            return this.Repository.GetAllResultAsync(selectResult: mapper.EntityToDto());
        }
    }
}