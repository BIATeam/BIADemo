// BIADemo only
// <copyright file="PlaneTypeAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.NotificationModule.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate;

    /// <summary>
    /// The application service used for notification type.
    /// </summary>
    public class NotificationTypeAppService : AppServiceBase<NotificationType>, INotificationTypeAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTypeAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public NotificationTypeAppService(ITGenericRepository<NotificationType> repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Return options.
        /// </summary>
        /// <returns>List of OptionDto.</returns>
        public Task<IEnumerable<OptionDto>> GetAllOptionsAsync()
        {
            return this.Repository.GetAllResultAsync(selectResult: new NotificationTypeOptionMapper().EntityToDto());
        }
    }
}