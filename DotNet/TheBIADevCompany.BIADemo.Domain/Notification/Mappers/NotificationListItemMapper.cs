// <copyright file="NotificationListItemMapper.cs" company="TheBIADevCompany">
//  Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Notification.Mappers
{
    // Begin BIADemo
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain;

    // End BIADemo
    using BIA.Net.Core.Domain.Notification.Mappers;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.Dto.Notification;
    using TheBIADevCompany.BIADemo.Domain.Notification.Entities;

    /// <summary>
    /// Notification List Item Mapper.
    /// </summary>
    /// <param name="userContext">The user context.</param>
    public class NotificationListItemMapper(UserContext userContext) :
        BaseNotificationListItemMapper<NotificationListItemDto, Notification>(userContext)
    {
        // Begin BIADemo

        /// <inheritdoc />
        public override ExpressionCollection<Notification> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Notification>(base.ExpressionCollection)
                {
                    { HeaderName.AcknowledgedAt, notification => notification.AcknowledgedAt },
                };
            }
        }

        /// <inheritdoc />
        public override Expression<Func<Notification, NotificationListItemDto>> EntityToDto(string mapperMode)
        {
            return base.EntityToDto(mapperMode).CombineMapping(entity => new NotificationListItemDto
            {
                AcknowledgedAt = entity.AcknowledgedAt,
            });
        }

        /// <inheritdoc />
        public override System.Collections.Generic.Dictionary<string, Func<string>> DtoToCellMapping(NotificationListItemDto dto)
        {
            return new System.Collections.Generic.Dictionary<string, Func<string>>(base.DtoToCellMapping(dto))
            {
                { HeaderName.AcknowledgedAt, () => CSVDate(dto.AcknowledgedAt?.UtcDateTime) },
            };
        }

        /// <summary>
        /// Header names specific to this mapper.
        /// </summary>
        public new struct HeaderName
        {
            /// <summary>
            /// Header name AcknowledgedAt.
            /// </summary>
            public const string AcknowledgedAt = "acknowledgedAt";
        }

        // End BIADemo
    }
}
