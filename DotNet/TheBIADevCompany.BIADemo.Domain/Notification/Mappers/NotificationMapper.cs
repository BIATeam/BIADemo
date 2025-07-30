// <copyright file="NotificationMapper.cs" company="TheBIADevCompany">
//  Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Notification.Mappers
{
    using BIA.Net.Core.Domain.Notification.Mappers;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.Dto.Notification;
    using TheBIADevCompany.BIADemo.Domain.Notification.Entities;

    /// <summary>
    /// Notification Mapper.
    /// </summary>
    public class NotificationMapper(UserContext userContext) :
        BaseNotificationMapper<NotificationDto, Notification>(userContext)
    {
    }
}
