namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories.QueryCustomizer
{
    using BIA.Net.Core.Infrastructure.Data.Repositories.QueryCustomizer;
    using TheBIADevCompany.BIADemo.Domain.Notification.Entities;
    using TheBIADevCompany.BIADemo.Domain.RepoContract.QueryCustomizer;

    public class NotificationQueryCustomizer : BaseNotificationQueryCustomizer<Notification>, INotificationQueryCustomizer
    {
    }
}
