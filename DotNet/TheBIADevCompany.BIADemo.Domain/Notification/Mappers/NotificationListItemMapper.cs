namespace TheBIADevCompany.BIADemo.Domain.Notification.Mappers
{
    using BIA.Net.Core.Domain.Notification.Mappers;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.Dto.Notification;
    using TheBIADevCompany.BIADemo.Domain.Notification.Entities;

    public class NotificationListItemMapper(UserContext userContext) :
        BaseNotificationListItemMapper<NotificationListItemDto, Notification>(userContext)
    {
    }
}
