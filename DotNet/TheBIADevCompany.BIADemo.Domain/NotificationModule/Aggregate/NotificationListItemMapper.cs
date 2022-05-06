// <copyright file="NotificationMapper.cs" company="TheBIADevCompany">
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
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<Notification> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Notification>
                {
                    { "TitleTranslated", notification => notification.NotificationTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Title).FirstOrDefault() ?? notification.Title },
                    { "DescriptionTranslated", notification => notification.NotificationTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Description).FirstOrDefault() ?? notification.Description },
                    { "CreatedDate", notification => notification.CreatedDate },
                    { "Type", notification => notification.Type.NotificationTypeTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? notification.Type.Label },
                    { "Read", notification => notification.Read },
                    { "CreatedBy", notification => notification.CreatedBy.FirstName + notification.CreatedBy.LastName + " (" + notification.CreatedBy.Login + ")" },
                    { "NotifiedTeams", notification => notification.NotifiedTeams.Select(x => x.Team.Title).OrderBy(x => x) },
                    { "NotifiedUsers", notification => notification.NotifiedUsers.Select(x => x.User.FirstName + " " + x.User.LastName + " (" + x.User.Login + ")").OrderBy(x => x) },
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
        public override Func<NotificationListItemDto, object[]> DtoToRecord()
        {
            return x => (new object[]
            {
                CSVString(x.TitleTranslated),
                CSVString(x.DescriptionTranslated),
                CSVString(x.Type?.Display),
                x.Read ? "X" : string.Empty,
                x.CreatedDate.ToString("yyyy-MM-dd"),
                CSVString(x.CreatedBy?.Display),
                CSVString(string.Join(" - ", x.NotifiedUsers?.Select(ca => ca.Display).ToList())),
                CSVString(string.Join(" - ", x.NotifiedTeams?.Select(nt => nt.Display).ToList())),
                CSVString(x.JData),
            });
        }
    }
}