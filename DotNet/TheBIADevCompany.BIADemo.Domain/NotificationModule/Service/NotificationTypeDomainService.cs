// <copyright file="NotificationTypeDomainService.cs" company="TheBIADevCompany">
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
    public class NotificationTypeDomainService : AppServiceBase<NotificationType, int>, INotificationTypeDomainService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTypeDomainService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="userContext">The user context.</param>
        public NotificationTypeDomainService(ITGenericRepository<NotificationType, int> repository)
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