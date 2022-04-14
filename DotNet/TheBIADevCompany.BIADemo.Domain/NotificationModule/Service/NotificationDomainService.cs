// <copyright file="NotificationDomainService.cs" company="TheBIADevCompany">
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
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// The application service used to manage views.
    /// </summary>
    public class NotificationDomainService : CrudAppServiceBase<NotificationDto, Notification, int, LazyLoadDto, NotificationMapper>, INotificationDomainService
    {
        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly int userId;

        /// <summary>
        /// The signalR Service.
        /// </summary>
        private readonly IClientForHubRepository clientForHubService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationDomainService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        /// <param name="clientForHubService">Client for hub.</param>
        /// <param name="queryCustomizer">Query customizer to include permission at update.</param>
        /// <param name="userContext">The user context.</param>
        public NotificationDomainService(
            ITGenericRepository<Notification, int> repository,
            IPrincipal principal,
            IClientForHubRepository clientForHubService,
            INotificationQueryCustomizer queryCustomizer,
            UserContext userContext)
            : base(repository)
        {
            this.userContext = userContext;
            this.Repository.QueryCustomizer = queryCustomizer;
            this.clientForHubService = clientForHubService;
            this.userId = (principal as BIAClaimsPrincipal).GetUserId();
            bool isTeamAccesAll = (principal as BIAClaimsPrincipal).GetUserPermissions().Any(x => x == Rights.Teams.AccessAll);

            this.filtersContext.Add(
                 AccessMode.Read,
                 new DirectSpecification<Notification>(n =>
                    (n.NotifiedTeams.Count == 0 || n.NotifiedTeams.Any(nt =>
                        (nt.Roles.Count == 0 && (isTeamAccesAll ||  nt.Team.Members.Any(member => member.UserId == this.userId)))
                        ||
                        (nt.Roles.Count > 0 && nt.Team.Members.Any(member => member.UserId == this.userId && member.MemberRoles.Any(mr => nt.Roles.Any(ntr => mr.RoleId == ntr.RoleId))))))
                    && (n.NotifiedUsers.Count == 0 || n.NotifiedUsers.Any(u => u.UserId == this.userId))));
        }

        /// <inheritdoc/>
        public async Task SetAsRead(int id)
        {
            var notification = await this.Repository.GetEntityAsync(id);

            notification.Read = true;
            await this.Repository.UnitOfWork.CommitAsync();

            _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notification-domain" }, "notification-removeUnread", notification.Id);
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

                var entity = await this.Repository.GetEntityAsync(id: dto.Id, specification: this.GetFilterSpecification(accessMode, this.filtersContext), includes: mapper.IncludesForUpdate(mapperMode), queryMode: queryMode);
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

                _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notifications" }, "refresh-notifications", dto);

                mapper.DtoToEntity(dto, entity, mapperMode);

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
            string mapperMode = null)
        {
            var notification = await base.RemoveAsync(id, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode);
            _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notification-domain" }, "notification-removeUnread", notification.Id);
            _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notifications" }, "refresh-notifications", notification);
            return notification;
        }

        /// <inheritdoc/>
        public override async Task<List<NotificationDto>> RemoveAsync(List<int> ids, string accessMode = "Delete", string queryMode = "Delete", string mapperMode = null)
        {
            var deletedDtos = await base.RemoveAsync(ids, accessMode, queryMode, mapperMode);

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

            _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notifications" }, "refresh-notifications", notification);
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