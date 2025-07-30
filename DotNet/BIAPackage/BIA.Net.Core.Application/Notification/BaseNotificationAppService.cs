// <copyright file="BaseNotificationAppService.cs" company="BIA">
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
    /// Base Notification App Service.
    /// </summary>
    /// <typeparam name="TBaseNotificationDto">The type of the base notification dto.</typeparam>
    /// <typeparam name="TBaseNotificationListItemDto">The type of the base notification list item dto.</typeparam>
    /// <typeparam name="TBaseNotification">The type of the base notification.</typeparam>
    /// <typeparam name="TBaseNotificationMapper">The type of the base notification mapper.</typeparam>
    /// <typeparam name="TBaseNotificationListItemMapper">The type of the base notification list item mapper.</typeparam>
    public abstract class BaseNotificationAppService<
        TBaseNotificationDto,
        TBaseNotificationListItemDto,
        TBaseNotification,
        TBaseNotificationMapper,
        TBaseNotificationListItemMapper> :
        CrudAppServiceListAndItemBase<
            TBaseNotificationDto,
            TBaseNotificationListItemDto,
            TBaseNotification,
            int,
            LazyLoadDto,
            TBaseNotificationMapper,
            TBaseNotificationListItemMapper>
        where TBaseNotificationDto : BaseNotificationDto, new()
        where TBaseNotificationListItemDto : BaseNotificationListItemDto, new()
        where TBaseNotification : BaseNotification, new()
        where TBaseNotificationMapper : BaseNotificationMapper<TBaseNotificationDto, TBaseNotification>
        where TBaseNotificationListItemMapper : BaseNotificationListItemMapper<TBaseNotificationListItemDto, TBaseNotification>
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
        /// Initializes a new instance of the <see cref="BaseNotificationAppService{TBaseNotificationDto, TBaseNotificationListItemDto, TBaseNotification, TBaseNotificationMapper, TBaseNotificationListItemMapper}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="clientForHubService">The client for hub service.</param>
        /// <param name="queryCustomizer">The query customizer.</param>
        protected BaseNotificationAppService(
            ITGenericRepository<TBaseNotification, int> repository,
            IPrincipal principal,
            IClientForHubService clientForHubService,
            IBaseNotificationQueryCustomizer<TBaseNotification> queryCustomizer)
            : base(repository)
        {
            this.Repository.QueryCustomizer = queryCustomizer;
            this.clientForHubService = clientForHubService;
            this.userId = (principal as BiaClaimsPrincipal).GetUserId();
            bool isTeamAccesAll = (principal as BiaClaimsPrincipal).GetUserPermissions().Any(x => x == BiaRights.Teams.AccessAll);

            this.FiltersContext.Add(
                 AccessMode.Read,
                 new DirectSpecification<TBaseNotification>(n =>
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

        /// <inheritdoc cref="IBaseNotificationAppService{TBaseNotificationDto, TBaseNotificationListItemDto, TBaseNotification}.SetAsRead"/>
        public async Task SetAsRead(TBaseNotificationDto dto)
        {
            var notification = await this.Repository.GetEntityAsync(dto.Id);

            notification.Read = true;
            await this.Repository.UnitOfWork.CommitAsync();
            dto.Read = true;

            _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notification-domain" }, "notification-removeUnread", notification.Id);
            _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notifications" }, "refresh-notification", dto);
        }

        /// <inheritdoc cref="IBaseNotificationAppService{TBaseNotificationDto, TBaseNotificationListItemDto, TBaseNotification}.SetUnread"/>
        public async Task SetUnread(TBaseNotificationDto dto)
        {
            var notification = await this.Repository.GetEntityAsync(dto.Id);

            notification.Read = false;
            await this.Repository.UnitOfWork.CommitAsync();
            dto.Read = false;

            _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notification-domain" }, "notification-addUnread", dto);
            _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notifications" }, "refresh-notification", dto);
        }

        /// <inheritdoc/>
        public override async Task<TBaseNotificationDto> UpdateAsync(
            TBaseNotificationDto dto,
            string accessMode = AccessMode.Update,
            string queryMode = QueryMode.Update,
            string mapperMode = null)
        {
            if (dto != null)
            {
                TBaseNotificationMapper mapper = this.InitMapper<TBaseNotificationDto, TBaseNotificationMapper>();

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
        public override async Task<TBaseNotificationDto> RemoveAsync(
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
        public override async Task<List<TBaseNotificationDto>> RemoveAsync(List<int> ids, string accessMode = "Delete", string queryMode = "Delete", string mapperMode = null, bool bypassFixed = false)
        {
            var deletedDtos = await base.RemoveAsync(ids, accessMode, queryMode, mapperMode, bypassFixed: bypassFixed);

            _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notification-domain" }, "notification-removeSeveralUnread", deletedDtos.Select(s => s.Id).ToList());
            _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notifications" }, "refresh-notifications-several", deletedDtos);

            return deletedDtos;
        }

        /// <inheritdoc/>
        public override async Task<TBaseNotificationDto> AddAsync(TBaseNotificationDto dto, string mapperMode = null)
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
                specification: this.FiltersContext[AccessMode.Read] & new DirectSpecification<TBaseNotification>(x => !x.Read));

            return results.ToList();
        }

        /// <inheritdoc cref="IBaseNotificationAppService{TBaseNotificationDto, TBaseNotificationListItemDto, TBaseNotification}.GetRangeWithAllAccessAsync"/>
        public async Task<(IEnumerable<TBaseNotificationListItemDto> Results, int Total)> GetRangeWithAllAccessAsync(PagingFilterFormatDto pagingFilterFormatDto)
        {
            return await this.GetRangeAsync(pagingFilterFormatDto, accessMode: AccessMode.All);
        }
    }
}