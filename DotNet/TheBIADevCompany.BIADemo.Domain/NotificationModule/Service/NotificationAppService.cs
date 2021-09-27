// <copyright file="NotificationAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.NotificationModule.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Notification;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
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
        /// The claims principal.
        /// </summary>
        private readonly List<string> permissions;

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

            // Test if not service
            //if (this.userId != 0)
            {
                this.permissions = (principal as BIAClaimsPrincipal).GetUserRights().ToList();
                this.filtersContext.Add(
                    AccessMode.Read,
                    new DirectSpecification<Notification>(n =>
                        n.SiteId == this.currentSiteId
                        && (n.NotifiedPermissions.Count == 0 || n.NotifiedPermissions.Any(r => this.permissions.Contains(r.Permission.Code)))
                        && (n.NotifiedUsers.Count == 0 || n.NotifiedUsers.Any(u => u.UserId == this.userId))
                        ));
            }
        }

        /// <inheritdoc/>
        public async Task<NotificationDto> SetAsRead(NotificationDto notification)
        {
            notification.Read = true;
            //_ = this.clientForHubService.SendMessage("notification-removeUnread", notification.Id.ToString());
            return await this.UpdateAsync(notification);
        }

        public override async Task<NotificationDto> UpdateAsync(
            NotificationDto dto,
            string accessMode = AccessMode.Update,
            string queryMode = QueryMode.Update,
            string mapperMode = null)
        {
            if (dto != null)
            {
                var mapper = new NotificationMapper();

                var entity = await this.Repository.GetEntityAsync(id: dto.Id, specification: this.GetFilterSpecification(accessMode, this.filtersContext), includes: mapper.IncludesForUpdate(mapperMode), queryMode: queryMode);
                dto.SiteId = entity.SiteId;
                if (entity == null)
                {
                    throw new ElementNotFoundException();
                }

                if (entity.Read && !dto.Read)
                {
                    _ = this.clientForHubService.SendMessage("notification-addUnread", dto);
                }
                else if (!entity.Read && dto.Read)
                {
                    _ = this.clientForHubService.SendMessage("notification-removeUnread", dto.Id);
                }
                /*else
                {*/
                    _ = this.clientForHubService.SendMessage("refresh-notifications", dto);
                /*}*/

                mapper.DtoToEntity(dto, entity, mapperMode);
                this.Repository.Update(entity);
                await this.Repository.UnitOfWork.CommitAsync();
                dto.DtoState = DtoState.Unchanged;
            }

            return dto;
        }

        /// <inheritdoc/>
        public override async Task<NotificationDto> RemoveAsync(
            int id,
            string accessMode = AccessMode.Delete,
            string queryMode = QueryMode.Delete,
            string mapperMode = null)
        {
            var notification = await this.GetAsync(id, accessMode: AccessMode.All);
            await base.RemoveAsync(id, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode);
            _ = this.clientForHubService.SendMessage("notification-removeUnread", notification.Id);
            _ = this.clientForHubService.SendMessage("refresh-notifications", notification);
            return notification;
        }

        /// <inheritdoc/>
        public override async Task<NotificationDto> AddAsync(NotificationDto dto, string mapperMode = null)
        {
            var notification = await base.AddAsync(dto, mapperMode);
            notification = await this.GetAsync(notification.Id, accessMode: AccessMode.All);
            if (!dto.Read)
            {
                _ = this.clientForHubService.SendMessage("notification-addUnread", notification);
            }
            _ = this.clientForHubService.SendMessage("refresh-notifications", notification);
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