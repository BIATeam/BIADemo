// <copyright file="NotificationListItemMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Notification.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Notification;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.Notification.Entities;
    using TheBIADevCompany.BIADemo.Domain.User;

    /// <summary>
    /// The mapper used for user.
    /// </summary>
    public class NotificationListItemMapper : BaseMapper<NotificationListItemDto, Notification, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationListItemMapper"/> class.
        /// </summary>
        /// <param name="userContext">the user context.</param>
        public NotificationListItemMapper(UserContext userContext)
        {
            this.UserContext = userContext;
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<Notification> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Notification>
                {
                    { HeaderName.TitleTranslated, notification => notification.NotificationTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Title).FirstOrDefault() ?? notification.Title },
                    { HeaderName.DescriptionTranslated, notification => notification.NotificationTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Description).FirstOrDefault() ?? notification.Description },
                    { HeaderName.CreatedDate, notification => notification.CreatedDate },
                    { HeaderName.Type, notification => notification.Type.NotificationTypeTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? notification.Type.Label },
                    { HeaderName.Read, notification => notification.Read },
                    { HeaderName.CreatedBy, notification => notification.CreatedBy.LastName + notification.CreatedBy.FirstName + " (" + notification.CreatedBy.Login + ")" },
                    { HeaderName.NotifiedTeams, notification => notification.NotifiedTeams.Select(x => x.Team.Title).OrderBy(x => x) },
                    { HeaderName.NotifiedUsers, notification => notification.NotifiedUsers.Select(x => x.User.LastName + " " + x.User.FirstName + " (" + x.User.Login + ")").OrderBy(x => x) },
                };
            }
        }

        /// <summary>
        /// The user context language and culture.
        /// </summary>
        private UserContext UserContext { get; set; }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Notification, NotificationListItemDto>> EntityToDto(string mapperMode)
        {
            return entity => new NotificationListItemDto
            {
                Id = entity.Id,
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
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Func<NotificationListItemDto, object[]> DtoToRecord(List<string> headerNames = null)
        {
            return x =>
            {
                List<object> records = new List<object>();

                if (headerNames?.Any() == true)
                {
                    foreach (string headerName in headerNames)
                    {
                        if (string.Equals(headerName, HeaderName.TitleTranslated, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.TitleTranslated));
                        }

                        if (string.Equals(headerName, HeaderName.DescriptionTranslated, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.DescriptionTranslated));
                        }

                        if (string.Equals(headerName, HeaderName.Type, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.Type?.Display));
                        }

                        if (string.Equals(headerName, HeaderName.Read, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(x.Read ? "X" : string.Empty);
                        }

                        if (string.Equals(headerName, HeaderName.CreatedDate, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(x.CreatedDate.ToString("yyyy-MM-dd"));
                        }

                        if (string.Equals(headerName, HeaderName.CreatedBy, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.CreatedBy?.Display));
                        }

                        if (string.Equals(headerName, HeaderName.NotifiedUsers, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(string.Join(" - ", x.NotifiedUsers?.Select(ca => ca.Display).ToList())));
                        }

                        if (string.Equals(headerName, HeaderName.NotifiedTeams, StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(string.Join(" - ", x.NotifiedTeams?.Select(nt => nt.Display).ToList())));
                        }
                    }
                }

                records.Add(CSVString(x.JData));

                return records.ToArray();
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
        }
    }
}