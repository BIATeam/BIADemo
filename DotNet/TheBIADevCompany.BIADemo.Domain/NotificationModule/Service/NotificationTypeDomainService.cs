// <copyright file="NotificationTypeDomainService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.NotificationModule.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.TranslationModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate;

    /// <summary>
    /// The application service used for notification type.
    /// </summary>
    public class NotificationTypeDomainService : CrudAppServiceBase<OptionDto, NotificationType, int, LazyLoadDto, NotificationTypeOptionMapper>, INotificationTypeDomainService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTypeDomainService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="userContext">The user context.</param>
        public NotificationTypeDomainService(ITGenericRepository<NotificationType, int> repository, UserContext userContext)
            : base(repository)
        {
            this.userContext = userContext;
        }
    }
}