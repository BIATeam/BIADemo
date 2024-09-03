// <copyright file="NotificationTypeOptionMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.User;

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
            this.UserContext = userContext;
        }

        /// <summary>
        /// The user context langage and culture.
        /// </summary>
        private UserContext UserContext { get; set; }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<NotificationType, OptionDto>> EntityToDto()
        {
            return entity => new OptionDto
            {
                Id = entity.Id,
                Display = entity.NotificationTypeTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? entity.Label,
            };
        }
    }
}