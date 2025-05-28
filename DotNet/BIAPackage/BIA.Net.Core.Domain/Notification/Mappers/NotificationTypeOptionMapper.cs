// <copyright file="NotificationTypeOptionMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Notification.Mappers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.Notification.Entities;
    using BIA.Net.Core.Domain.Service;

    /// <summary>
    /// The mapper used for notification type.
    /// </summary>
    public class NotificationTypeOptionMapper : BaseMapper<OptionDto, NotificationType, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTypeOptionMapper"/> class.
        /// </summary>
        /// <param name="userContext">the user context.</param>
        public NotificationTypeOptionMapper(UserContext userContext)
        {
            UserContext = userContext;
        }

        /// <summary>
        /// The user context language and culture.
        /// </summary>
        private UserContext UserContext { get; set; }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<NotificationType, OptionDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new OptionDto
            {
                Display = entity.NotificationTypeTranslations.Where(rt => rt.Language.Code == UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? entity.Label,
            });
        }
    }
}