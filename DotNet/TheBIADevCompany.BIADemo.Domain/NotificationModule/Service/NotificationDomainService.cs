// <copyright file="NotificationDomainService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.NotificationModule.Service
{
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// The domain service used to add notifications.
    /// </summary>
    public class NotificationDomainService : FilteredServiceBase<Notification, int>, INotificationDomainService
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
            INotificationQueryCustomizer queryCustomizer)
            : base(repository)
        {
            this.Repository.QueryCustomizer = queryCustomizer;
            this.clientForHubService = clientForHubService;
            this.userId = (principal as BiaClaimsPrincipal).GetUserId();
            bool isTeamAccesAll = (principal as BiaClaimsPrincipal).GetUserPermissions().Any(x => x == Rights.Teams.AccessAll);

            this.FiltersContext.Add(
                 AccessMode.Read,
                 new DirectSpecification<Notification>(n =>
                    (n.NotifiedTeams.Count == 0 || n.NotifiedTeams.Any(nt =>
                        (nt.Roles.Count == 0 && (isTeamAccesAll || nt.Team.Members.Any(member => member.UserId == this.userId)))
                        ||
                        (nt.Roles.Count > 0 && nt.Team.Members.Any(member => member.UserId == this.userId && member.MemberRoles.Any(mr => nt.Roles.Any(ntr => mr.RoleId == ntr.RoleId))))))
                    && (n.NotifiedUsers.Count == 0 || n.NotifiedUsers.Any(u => u.UserId == this.userId))));
        }

        /// <inheritdoc/>
        public async override Task<TOtherObject> AddAsync<TOtherObject, TOtherMapper>(TOtherObject dto, string mapperMode = null)
        {
            return await this.ExecuteWithFrontUserExceptionHandlingAsync(async () =>
            {
                if (dto != null)
                {
                    TOtherMapper mapper = this.InitMapper<TOtherObject, TOtherMapper>();
                    var entity = new Notification();
                    mapper.DtoToEntity(dto, entity, mapperMode, this.Repository.UnitOfWork);
                    this.Repository.Add(entity);
                    await this.Repository.UnitOfWork.CommitAsync();
                    mapper.MapEntityKeysInDto(entity, dto);

                    if (!entity.Read)
                    {
                        _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notification-domain" }, "notification-addUnread", dto);
                    }

                    _ = this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "notifications" }, "refresh-notification", dto);
                }

                return dto;
            });
        }
    }
}