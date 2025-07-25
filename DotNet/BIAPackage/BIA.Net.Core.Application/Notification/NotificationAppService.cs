// <copyright file="NotificationAppService.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Notification
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Notification;
    using BIA.Net.Core.Domain.Notification.Entities;
    using BIA.Net.Core.Domain.Notification.Mappers;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;

    /// <summary>
    /// The application service used to manage views.
    /// </summary>
    public class NotificationAppService : CrudAppServiceListAndItemBase<NotificationDto, NotificationListItemDto, Notification, int, LazyLoadDto, NotificationMapper, NotificationListItemMapper>, INotificationAppService
    {
        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly int userId;

        /// <summary>
        /// The signalR Service.
        /// </summary>
        private readonly IClientForHubService clientForHubService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        /// <param name="clientForHubService">Client for hub.</param>
        /// <param name="queryCustomizer">Query customizer to include permission at update.</param>
        /// <param name="userContext">The user context.</param>
        public NotificationAppService(
            ITGenericRepository<Notification, int> repository,
            IPrincipal principal,
            IClientForHubService clientForHubService,
            INotificationQueryCustomizer queryCustomizer)
            : base(repository)
        {
            this.Repository.QueryCustomizer = queryCustomizer;
            this.clientForHubService = clientForHubService;
            this.userId = (principal as BiaClaimsPrincipal).GetUserId();
            bool isTeamAccesAll = (principal as BiaClaimsPrincipal).GetUserPermissions().Any(x => x == BiaRights.Teams.AccessAll);

            this.FiltersContext.Add(
                 AccessMode.Read,
                 new DirectSpecification<Notification>(n =>
                    (
                        n.NotifiedUsers.Count == 0
                        &&
                        n.NotifiedTeams.Count == 0
                    )
                    ||
                    (

                        // V5: see my team notification even if I am not the user.
                        n.NotifiedTeams.Count != 0
                        &&
                        n.NotifiedTeams.Any(nt =>
                            (nt.Roles.Count == 0 && (isTeamAccesAll || nt.Team.Members.Any(member => member.UserId == this.userId)))
                            ||
                            (nt.Roles.Count > 0 && nt.Team.Members.Any(member => member.UserId == this.userId && member.MemberRoles.Any(mr => nt.Roles.Any(ntr => mr.RoleId == ntr.RoleId)))))
                    )
                    ||
                    (

                        // V5: see nominative notification even if not in the team or role
                        n.NotifiedUsers.Count != 0
                        &&
                        n.NotifiedUsers.Any(u => u.UserId == this.userId)
                    )));
        }

        /// <inheritdoc/>
        public async Task SetAsRead(NotificationDto dto)
        {
            var notification = await this.Repository.GetEntityAsync(dto.Id);

            notification.Read = true;
            await this.Repository.UnitOfWork.CommitAsync();
            dto.Read = true;

            _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notification-domain" }, "notification-removeUnread", notification.Id);
            _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notifications" }, "refresh-notification", dto);
        }

        /// <inheritdoc/>
        public async Task SetUnread(NotificationDto dto)
        {
            var notification = await this.Repository.GetEntityAsync(dto.Id);

            notification.Read = false;
            await this.Repository.UnitOfWork.CommitAsync();
            dto.Read = false;

            _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notification-domain" }, "notification-addUnread", dto);
            _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notifications" }, "refresh-notification", dto);
        }

        /// <inheritdoc/>
        public override async Task<NotificationDto> UpdateAsync(
            NotificationDto dto,
            string accessMode = AccessMode.Update,
            string queryMode = QueryMode.Update,
            string mapperMode = null)
        {
            if (dto != null)
            {
                NotificationMapper mapper = this.InitMapper<NotificationDto, NotificationMapper>();

                var entity = await this.Repository.GetEntityAsync(id: dto.Id, specification: this.GetFilterSpecification(accessMode, this.FiltersContext), includes: mapper.IncludesForUpdate(mapperMode), queryMode: queryMode);
                if (entity == null)
                {
                    throw new ElementNotFoundException();
                }

                if (entity.Read && !dto.Read)
                {
                    _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notification-domain" }, "notification-addUnread", dto);
                }
                else if (!entity.Read && dto.Read)
                {
                    _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notification-domain" }, "notification-removeUnread", dto.Id);
                }

                _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notifications" }, "refresh-notification", dto);

                mapper.DtoToEntity(dto, ref entity, mapperMode, this.Repository.UnitOfWork);

                await this.Repository.UnitOfWork.CommitAsync();
                dto.DtoState = DtoState.Unchanged;
                mapper.MapEntityKeysInDto(entity, dto);
            }

            return dto;
        }

        /// <inheritdoc/>
        public override async Task<NotificationDto> RemoveAsync(
            int id,
            string accessMode = AccessMode.Delete,
            string queryMode = QueryMode.Delete,
            string mapperMode = null,
            bool bypassFixed = false)
        {
            var notification = await base.RemoveAsync(id, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode, bypassFixed: bypassFixed);
            _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notification-domain" }, "notification-removeUnread", notification.Id);
            _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notifications" }, "refresh-notification", notification);
            return notification;
        }

        /// <inheritdoc/>
        public override async Task<List<NotificationDto>> RemoveAsync(List<int> ids, string accessMode = "Delete", string queryMode = "Delete", string mapperMode = null, bool bypassFixed = false)
        {
            var deletedDtos = await base.RemoveAsync(ids, accessMode, queryMode, mapperMode, bypassFixed: bypassFixed);

            _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notification-domain" }, "notification-removeSeveralUnread", deletedDtos.Select(s => s.Id).ToList());
            _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notifications" }, "refresh-notifications-several", deletedDtos);

            return deletedDtos;
        }

        /// <inheritdoc/>
        public override async Task<NotificationDto> AddAsync(NotificationDto dto, string mapperMode = null)
        {
            var notification = await base.AddAsync(dto, mapperMode);

            if (!dto.Read)
            {
                _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notification-domain" }, "notification-addUnread", notification);
            }

            _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notifications" }, "refresh-notification", notification);
            return notification;
        }

        /// <summary>
        /// Return the list of unreadIds.
        /// </summary>
        /// <param name="userId">the user Id.</param>
        /// <returns>The list of int.</returns>
        public async Task<List<int>> GetUnreadIds(int userId)
        {
            var results = await this.Repository.GetAllResultAsync(
                selectResult: x => x.Id,
                specification: this.FiltersContext[AccessMode.Read] & new DirectSpecification<Notification>(x => !x.Read));

            return results.ToList();
        }

        /// <inheritdoc/>
        public async Task<(IEnumerable<NotificationListItemDto> Results, int Total)> GetRangeWithAllAccessAsync(PagingFilterFormatDto pagingFilterFormatDto)
        {
            return await this.GetRangeAsync(pagingFilterFormatDto, accessMode: AccessMode.All);
        }
    }
}