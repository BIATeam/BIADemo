// <copyright file="NotificationAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.NotificationModule.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Notification;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate;

    /// <summary>
    /// The application service used to manage views.
    /// </summary>
    public class NotificationAppService : CrudAppServiceBase<NotificationDto, Notification, LazyLoadDto, NotificationMapper>, INotificationAppService
    {
        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly int currentSiteId;

        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly int userId;

        /// <summary>
        /// The signalR Service.
        /// </summary>
        private readonly IClientForHubRepository clientForHubService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        /// <param name="clientForHubService">Client for hub.</param>
        public NotificationAppService(
            ITGenericRepository<Notification> repository,
            IPrincipal principal, 
            IClientForHubRepository clientForHubService)
            : base(repository)
        {
            var userDataDto = (principal as BIAClaimsPrincipal).GetUserData<UserDataDto>();
            this.clientForHubService = clientForHubService;
            this.currentSiteId = userDataDto == null ? 0 : userDataDto.CurrentSiteId;
            this.userId = (principal as BIAClaimsPrincipal).GetUserId();
            this.filtersContext.Add(
                AccessMode.Read,
                new DirectSpecification<Notification>(n =>
                    n.SiteId == this.currentSiteId
                    && (n.NotifiedRoles.Count == 0 || n.NotifiedRoles.Any(r => r.Role.MemberRoles.Any(mr => mr.Member.UserId == this.userId)))
                    && (n.NotifiedUsers.Count == 0 || n.NotifiedUsers.Any(u => u.UserId == this.userId))
                    ));
        }

        /// <inheritdoc/>
        public async Task<NotificationDto> SetAsRead(NotificationDto notification)
        {
            notification.Read = true;
            _ = this.clientForHubService.SendMessage("notification-read", notification.Id.ToString());
            return await this.UpdateAsync(notification);
        }

        /// <inheritdoc/>
        public override Task<NotificationDto> AddAsync(NotificationDto dto, string mapperMode = null)
        {
            var notification = base.AddAsync(dto, mapperMode);
            _ = this.clientForHubService.SendMessage("notification-sent", notification.Result);
            _ = this.clientForHubService.SendMessage("refresh-notifications", notification.Result);
            return notification;
        }

        /// <summary>
        /// Return the list of unreadIds.
        /// </summary>
        /// <param name="userId">the user Id.</param>
        /// <returns>The list of int.</returns>
        public async Task<List<int>> GetUnreadIds(int userId)
        {
            var results = await this.Repository.GetAllResultAsync<int>(
                selectResult: x => x.Id,
                specification: this.filtersContext[AccessMode.Read] & new DirectSpecification<Notification>(x => !x.Read));

            return results.ToList();
        }
    }
}