namespace TheBIADevCompany.BIADemo.Application.Notification
{
    using BIA.Net.Core.Application.Notification;
    using TheBIADevCompany.BIADemo.Domain.Dto.Notification;
    using TheBIADevCompany.BIADemo.Domain.Notification.Entities;

    public interface INotificationAppService : IBaseNotificationAppService<NotificationDto, NotificationListItemDto, Notification>
    {
    }
}
