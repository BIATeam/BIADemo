// <copyright file="NotificationListItemMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Notification;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Service;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using TheBIADevCompany.BIADemo.Domain.TranslationModule.Aggregate;

    /// <summary>
    /// The mapper used for user.
    /// </summary>
    public class NotificationListItemMapper : BaseMapper<NotificationListItemDto, Notification, int>
    {
        /// <summary>
        /// Header Name.
        /// </summary>
        public enum HeaderName
        {
            /// <summary>
            /// header name TitleTranslated.
            /// </summary>
            TitleTranslated,

            /// <summary>
            /// header name DescriptionTranslated.
            /// </summary>
            DescriptionTranslated,

            /// <summary>
            /// header name CreatedDate.
            /// </summary>
            CreatedDate,

            /// <summary>
            /// header name Type.
            /// </summary>
            Type,

            /// <summary>
            /// header name Read.
            /// </summary>
            Read,

            /// <summary>
            /// header name CreatedBy.
            /// </summary>
            CreatedBy,

            /// <summary>
            /// header name NotifiedTeams.
            /// </summary>
            NotifiedTeams,

            /// <summary>
            /// header name NotifiedUsers.
            /// </summary>
            NotifiedUsers,
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<Notification> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Notification>
                {
                    { HeaderName.TitleTranslated.ToString(), notification => notification.NotificationTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Title).FirstOrDefault() ?? notification.Title },
                    { HeaderName.DescriptionTranslated.ToString(), notification => notification.NotificationTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Description).FirstOrDefault() ?? notification.Description },
                    { HeaderName.CreatedDate.ToString(), notification => notification.CreatedDate },
                    { HeaderName.Type.ToString(), notification => notification.Type.NotificationTypeTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? notification.Type.Label },
                    { HeaderName.Read.ToString(), notification => notification.Read },
                    { HeaderName.CreatedBy.ToString(), notification => notification.CreatedBy.FirstName + notification.CreatedBy.LastName + " (" + notification.CreatedBy.Login + ")" },
                    { HeaderName.NotifiedTeams.ToString(), notification => notification.NotifiedTeams.Select(x => x.Team.Title).OrderBy(x => x) },
                    { HeaderName.NotifiedUsers.ToString(), notification => notification.NotifiedUsers.Select(x => x.User.FirstName + " " + x.User.LastName + " (" + x.User.Login + ")").OrderBy(x => x) },
                };
            }
        }

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
                    Display = entity.CreatedBy.FirstName + " " + entity.CreatedBy.LastName + " (" + entity.CreatedBy.Login + ")",
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
                    Display = (nt.Team != null ? nt.Team.Title : string.Empty) + ((nt.Roles == null || nt.Roles.Count == 0) ? string.Empty : " (" + string.Join(", ", nt.Roles.Select(ntr => ntr.Role.RoleTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? ntr.Role.Label)) + ")"),
                }).ToList(),

                NotifiedUsers = entity.NotifiedUsers.Select(nu => new OptionDto
                {
                    Id = nu.User.Id,
                    Display = nu.User.FirstName + " " + nu.User.LastName + " (" + nu.User.Login + ")",
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
                        if (string.Equals(headerName, HeaderName.TitleTranslated.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.TitleTranslated));
                        }

                        if (string.Equals(headerName, HeaderName.DescriptionTranslated.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.DescriptionTranslated));
                        }

                        if (string.Equals(headerName, HeaderName.Type.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.Type?.Display));
                        }

                        if (string.Equals(headerName, HeaderName.Read.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(x.Read ? "X" : string.Empty);
                        }

                        if (string.Equals(headerName, HeaderName.CreatedDate.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(x.CreatedDate.ToString("yyyy-MM-dd"));
                        }

                        if (string.Equals(headerName, HeaderName.CreatedBy.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(x.CreatedBy?.Display));
                        }

                        if (string.Equals(headerName, HeaderName.NotifiedUsers.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(string.Join(" - ", x.NotifiedUsers?.Select(ca => ca.Display).ToList())));
                        }

                        if (string.Equals(headerName, HeaderName.NotifiedTeams.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            records.Add(CSVString(string.Join(" - ", x.NotifiedTeams?.Select(nt => nt.Display).ToList())));
                        }
                    }
                }

                records.Add(CSVString(x.JData));

                return records.ToArray();
            };
        }
    }
}