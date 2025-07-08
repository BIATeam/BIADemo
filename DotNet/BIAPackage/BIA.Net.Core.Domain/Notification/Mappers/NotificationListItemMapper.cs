// <copyright file="NotificationListItemMapper.cs" company="BIA">
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
    /// The mapper used for user.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="NotificationListItemMapper"/> class.
    /// </remarks>
    /// <param name="userContext">the user context.</param>
    public class NotificationListItemMapper(UserContext userContext) : BaseMapper<NotificationListItemDto, Notification, int>
    {
        /// <inheritdoc />
        public override ExpressionCollection<Notification> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Notification>(base.ExpressionCollection)
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
        public override ExpressionCollection<Notification> ExpressionCollectionFilterIn
        {
            get
            {
                return new ExpressionCollection<Notification>(
                    base.ExpressionCollectionFilterIn,
                    new ExpressionCollection<Notification>()
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
        public override Expression<Func<Notification, NotificationListItemDto>> EntityToDto(string mapperMode)
        {
            return this.EntityToDto().CombineMapping(entity => new NotificationListItemDto
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
        public override Dictionary<string, Func<string>> DtoToCellMapping(NotificationListItemDto dto)
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