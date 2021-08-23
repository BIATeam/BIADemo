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
        /// The current site identifier.
        /// </summary>
        private readonly int currentSiteId;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public NotificationAppService(ITGenericRepository<Notification> repository, IPrincipal principal)
            : base(repository)
        {
            this.currentSiteId = (principal as BIAClaimsPrincipal).GetUserData<UserDataDto>().CurrentSiteId;
        }

        /// <inheritdoc/>
        public override Task<NotificationDto> AddAsync(NotificationDto dto, string mapperMode = null)
        {
            dto.Site = new SiteDto { Id = this.currentSiteId };
            return base.AddAsync(dto, mapperMode);
        }
    }
}