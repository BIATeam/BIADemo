// <copyright file="NotificationMapper.cs" company="TheBIADevCompany">
//  Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Notification.Mappers
{
    // Begin BIADemo
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;

    // End BIADemo
    using BIA.Net.Core.Domain.Notification.Mappers;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.Dto.Notification;
    using TheBIADevCompany.BIADemo.Domain.Notification.Entities;

    /// <summary>
    /// Notification Mapper.
    /// </summary>
    /// <param name="userContext">The user context.</param>
    public class NotificationMapper(UserContext userContext) :
        BaseNotificationMapper<NotificationDto, Notification>(userContext)
    {
        // Begin BIADemo

        /// <inheritdoc />
        public override Expression<Func<Notification, NotificationDto>> EntityToDto(string mapperMode)
        {
            return base.EntityToDto(mapperMode).CombineMapping(entity => new NotificationDto
            {
                AcknowledgedAt = entity.AcknowledgedAt,
            });
        }

        /// <inheritdoc />
        public override void DtoToEntity(NotificationDto dto, ref Notification entity)
        {
            base.DtoToEntity(dto, ref entity);
            entity.AcknowledgedAt = dto.AcknowledgedAt;
        }

        // End BIADemo
    }
}
