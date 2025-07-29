namespace TheBIADevCompany.BIADemo.Application.Notification
{
    using System.Security.Principal;
    using BIA.Net.Core.Application.Notification;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.Dto.Notification;
    using TheBIADevCompany.BIADemo.Domain.Notification.Entities;
    using TheBIADevCompany.BIADemo.Domain.Notification.Mappers;
    using TheBIADevCompany.BIADemo.Domain.RepoContract.QueryCustomizer;

    public class NotificationAppService : BaseNotificationAppService<
        NotificationDto,
        NotificationListItemDto,
        Notification,
        NotificationMapper,
        NotificationListItemMapper>, INotificationAppService
    {
        public NotificationAppService(ITGenericRepository<Notification, int> repository, IPrincipal principal, IClientForHubService clientForHubService, INotificationQueryCustomizer queryCustomizer) : base(repository, principal, clientForHubService, queryCustomizer)
        {
        }
    }
}
