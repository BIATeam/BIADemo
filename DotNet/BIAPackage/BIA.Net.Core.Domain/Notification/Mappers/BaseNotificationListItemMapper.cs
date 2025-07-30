// <copyright file="BaseNotificationListItemMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Notification.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Notification;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.Notification.Entities;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.User;

    /// <summary>
    /// Base Notification List Item Mapper.
    /// </summary>
    /// <typeparam name="TBaseNotificationListItemDto">The type of the base notification list item dto.</typeparam>
    /// <typeparam name="TBaseNotification">The type of the base notification.</typeparam>
    /// <seealso cref="BIA.Net.Core.Domain.Mapper.BaseMapper&lt;TBaseNotificationListItemDto, TBaseNotification, int&gt;" />
    public abstract class BaseNotificationListItemMapper<TBaseNotificationListItemDto, TBaseNotification>(UserContext userContext) : BaseMapper<TBaseNotificationListItemDto, TBaseNotification, int>
        where TBaseNotificationListItemDto : BaseNotificationListItemDto, new()
        where TBaseNotification : BaseNotification, new()
    {
        /// <inheritdoc />
        public override ExpressionCollection<TBaseNotification> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<TBaseNotification>(base.ExpressionCollection)
                {
                    { HeaderName.TitleTranslated, notification => notification.NotificationTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Title).FirstOrDefault() ?? notification.Title },
                    { HeaderName.DescriptionTranslated, notification => notification.NotificationTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Description).FirstOrDefault() ?? notification.Description },
                    { HeaderName.CreatedDate, notification => notification.CreatedDate },
                    { HeaderName.Type, notification => notification.Type.NotificationTypeTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? notification.Type.Label },
                    { HeaderName.Read, notification => notification.Read },
                    { HeaderName.CreatedBy, notification => notification.CreatedBy.LastName + notification.CreatedBy.FirstName + " (" + notification.CreatedBy.Login + ")" },
                    { HeaderName.NotifiedTeams, notification => notification.NotifiedTeams.Select(x => x.Team.Title).OrderBy(x => x) },
                    { HeaderName.NotifiedUsers, notification => notification.NotifiedUsers.Select(x => x.User.LastName + " " + x.User.FirstName + " (" + x.User.Login + ")").OrderBy(x => x) },
                    { HeaderName.JData, notification => notification.JData },
                };
            }
        }

        /// <inheritdoc />
        public override ExpressionCollection<TBaseNotification> ExpressionCollectionFilterIn
        {
            get
            {
                return new ExpressionCollection<TBaseNotification>(
                    base.ExpressionCollectionFilterIn,
                    new ExpressionCollection<TBaseNotification>()
                    {
                        { HeaderName.Type, notification => notification.Type.Id },
                        { HeaderName.CreatedBy, notification => notification.CreatedBy.Id },
                        { HeaderName.NotifiedUsers, notification => notification.NotifiedUsers.Select(x => x.User.Id) },
                    });
            }
        }

        /// <summary>
        /// The user context language and culture.
        /// </summary>
        private UserContext UserContext { get; set; } = userContext;

        /// <inheritdoc />
        public override Expression<Func<TBaseNotification, TBaseNotificationListItemDto>> EntityToDto(string mapperMode)
        {
            return this.EntityToDto().CombineMapping(entity => new TBaseNotificationListItemDto
            {
                TitleTranslated = entity.NotificationTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Title).FirstOrDefault() ?? entity.Title,
                DescriptionTranslated = entity.NotificationTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Description).FirstOrDefault() ?? entity.Description,

                CreatedDate = entity.CreatedDate,

                CreatedBy = entity.CreatedBy != null ? new OptionDto
                {
                    Id = entity.CreatedBy.Id,
                    Display = entity.CreatedBy.Display(),
                }
                : null,

                Read = entity.Read,
                Type = new OptionDto
                {
                    Id = entity.TypeId,
                    Display = entity.Type.NotificationTypeTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? entity.Type.Label,
                },

                JData = entity.JData,

                NotifiedTeams = entity.NotifiedTeams.Select(nt => new OptionDto
                {
                    Id = nt.Id,
                    Display = (nt.Team != null ? nt.Team.Title : string.Empty) + (nt.Roles == null || nt.Roles.Count == 0 ? string.Empty : " (" + string.Join(", ", nt.Roles.Select(ntr => ntr.Role.RoleTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? ntr.Role.Label)) + ")"),
                }).ToList(),

                NotifiedUsers = entity.NotifiedUsers.Select(nu => new OptionDto
                {
                    Id = nu.User.Id,
                    Display = nu.User.Display(),
                }).ToList(),
            });
        }

        /// <inheritdoc />
        public override Dictionary<string, Func<string>> DtoToCellMapping(TBaseNotificationListItemDto dto)
        {
            return new Dictionary<string, Func<string>>(base.DtoToCellMapping(dto))
            {
                { HeaderName.TitleTranslated, () => CSVString(dto.TitleTranslated) },
                { HeaderName.DescriptionTranslated, () => CSVString(dto.DescriptionTranslated) },
                { HeaderName.Type, () => CSVString(dto.Type?.Display) },
                { HeaderName.Read, () => CSVBool(dto.Read) },
                { HeaderName.CreatedDate, () => CSVDate(dto.CreatedDate) },
                { HeaderName.CreatedBy, () => CSVString(dto.CreatedBy?.Display) },
                { HeaderName.NotifiedUsers, () => CSVList(dto.NotifiedUsers) },
                { HeaderName.NotifiedTeams, () => CSVList(dto.NotifiedTeams) },
                { HeaderName.JData, () => CSVString(dto.JData) },
            };
        }

        /// <summary>
        /// Header Name.
        /// </summary>
        public struct HeaderName
        {
            /// <summary>
            /// header name TitleTranslated.
            /// </summary>
            public const string TitleTranslated = "titleTranslated";

            /// <summary>
            /// header name DescriptionTranslated.
            /// </summary>
            public const string DescriptionTranslated = "descriptionTranslated";

            /// <summary>
            /// header name CreatedDate.
            /// </summary>
            public const string CreatedDate = "createdDate";

            /// <summary>
            /// header name Type.
            /// </summary>
            public const string Type = "type";

            /// <summary>
            /// header name Read.
            /// </summary>
            public const string Read = "read";

            /// <summary>
            /// header name CreatedBy.
            /// </summary>
            public const string CreatedBy = "createdBy";

            /// <summary>
            /// header name NotifiedTeams.
            /// </summary>
            public const string NotifiedTeams = "notifiedTeams";

            /// <summary>
            /// header name NotifiedUsers.
            /// </summary>
            public const string NotifiedUsers = "notifiedUsers";

            /// <summary>
            /// header name JSON Data.
            /// </summary>
            public const string JData = "jData";
        }
    }
}