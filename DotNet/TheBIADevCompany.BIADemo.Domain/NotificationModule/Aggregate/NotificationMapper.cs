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
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using TheBIADevCompany.BIADemo.Domain.TranslationModule.Aggregate;

    /// <summary>
    /// The mapper used for user.
    /// </summary>
    public class NotificationMapper : BaseMapper<NotificationDto, Notification>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<Notification> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Notification>
                {
                    { "Title", notification => notification.NotificationTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Title).FirstOrDefault() ?? notification.Title },
                    { "Description", notification => notification.NotificationTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Description).FirstOrDefault() ?? notification.Description },
                    { "CreatedDate", notification => notification.CreatedDate },
                    { "Type", notification => notification.Type.NotificationTypeTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? notification.Type.Label },
                    { "Read", notification => notification.Read },
                    { "CreatedBy", notification => notification.CreatedBy.FirstName + notification.CreatedBy.LastName + " (" + notification.CreatedBy.Login + ")" },
                    {
                        "NotifiedPermissions", notification => notification.NotifiedPermissions.Select(x =>
                        x.Permission.PermissionTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? x.Permission.Label).OrderBy(x => x)
                    },
                    { "NotifiedUsers", notification => notification.NotifiedUsers.Select(x => x.User.FirstName + " " + x.User.LastName + " (" + x.User.Login + ")").OrderBy(x => x) },
                };
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(NotificationDto dto, Notification entity)
        {
            if (entity == null)
            {
                entity = new Notification();
            }

            entity.Id = dto.Id;
            entity.Read = dto.Read;
            entity.Title = dto.Title;
            entity.CreatedDate = dto.CreatedDate;
            entity.Description = dto.Description;
            entity.CreatedById = dto.CreatedBy?.Id;
            entity.TypeId = dto.Type.Id;
            entity.JData = dto.JData;

            if (dto.SiteId != 0)
            {
                entity.SiteId = dto.SiteId;
            }

            // Mapping relationship *-* : ICollection<OptionDto> NotifiedUsers
            if (dto.NotifiedUsers?.Any() == true)
            {
                foreach (var userDto in dto.NotifiedUsers.Where(x => x.DtoState == DtoState.Deleted))
                {
                    var connectingAirport = entity.NotifiedUsers.FirstOrDefault(x => x.UserId == userDto.Id && x.NotificationId == dto.Id);
                    if (connectingAirport != null)
                    {
                        entity.NotifiedUsers.Remove(connectingAirport);
                    }
                }

                entity.NotifiedUsers = entity.NotifiedUsers ?? new List<NotificationUser>();
                foreach (var userDto in dto.NotifiedUsers.Where(w => w.DtoState == DtoState.Added))
                {
                    entity.NotifiedUsers.Add(new NotificationUser
                    { UserId = userDto.Id, NotificationId = dto.Id });
                }
            }

            // Mapping relationship *-* : ICollection<OptionDto> NotifiedPermissions
            if (dto.NotifiedPermissions?.Any() == true)
            {
                foreach (var roleDto in dto.NotifiedPermissions.Where(x => x.DtoState == DtoState.Deleted))
                {
                    var connectingAirport = entity.NotifiedPermissions.FirstOrDefault(x => x.PermissionId == roleDto.Id && x.NotificationId == dto.Id);
                    if (connectingAirport != null)
                    {
                        entity.NotifiedPermissions.Remove(connectingAirport);
                    }
                }

                entity.NotifiedPermissions = entity.NotifiedPermissions ?? new List<NotificationPermission>();
                foreach (var roleDto in dto.NotifiedPermissions.Where(w => w.DtoState == DtoState.Added))
                {
                    entity.NotifiedPermissions.Add(new NotificationPermission
                    { PermissionId = roleDto.Id, NotificationId = dto.Id });
                }
            }

            // Mapping relationship *-1 : ICollection<NotificationTranslationDto> NotificationTranslation
            if (dto.NotificationTranslations?.Any() == true)
            {
                foreach (var notificationTranslationDto in dto.NotificationTranslations.Where(x => x.DtoState == DtoState.Deleted))
                {
                    var notificationTranslation = entity.NotificationTranslations.FirstOrDefault(x => x.LanguageId == notificationTranslationDto.LanguageId && x.NotificationId == dto.Id);
                    if (notificationTranslation != null)
                    {
                        entity.NotificationTranslations.Remove(notificationTranslation);
                    }
                }

                foreach (var notificationTranslationDto in dto.NotificationTranslations.Where(x => x.DtoState == DtoState.Modified))
                {
                    var notificationTranslation = entity.NotificationTranslations.FirstOrDefault(x => x.LanguageId == notificationTranslationDto.LanguageId && x.NotificationId == dto.Id);
                    if (notificationTranslation != null)
                    {
                        notificationTranslation.Title = notificationTranslationDto.Title;
                        notificationTranslation.Description = notificationTranslationDto.Description;
                    }
                }

                entity.NotificationTranslations = entity.NotificationTranslations ?? new List<NotificationTranslation>();
                foreach (var notificationTranslationDto in dto.NotificationTranslations.Where(w => w.DtoState == DtoState.Added))
                {
                    entity.NotificationTranslations.Add(new NotificationTranslation
                    { LanguageId = notificationTranslationDto.LanguageId, NotificationId = dto.Id, Title = notificationTranslationDto.Title, Description = notificationTranslationDto.Description });
                }
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Notification, NotificationDto>> EntityToDto()
        {
            return entity => new NotificationDto
            {
                Id = entity.Id,
                Title = entity.NotificationTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Title).FirstOrDefault() ?? entity.Title,
                Description = entity.NotificationTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Description).FirstOrDefault() ?? entity.Description,
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

                SiteId = entity.SiteId,

                NotifiedPermissions = entity.NotifiedPermissions.Select(np => new OptionDto
                {
                    Id = np.Permission.Id,
                    Display = np.Permission.PermissionTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? np.Permission.Label,
                }).ToList(),

                NotifiedUsers = entity.NotifiedUsers.Select(nu => new OptionDto
                {
                    Id = nu.User.Id,
                    Display = nu.User.FirstName + " " + nu.User.LastName + " (" + nu.User.Login + ")",
                }).ToList(),
            };
        }

        /// <inheritdoc/>
        public override void MapEntityKeysInDto(Notification entity, NotificationDto dto)
        {
            dto.Id = entity.Id;
            dto.SiteId = entity.SiteId;
            dto.NotifiedPermissions = entity.NotifiedPermissions?.Select(nr => new OptionDto
            {
                Id = nr.PermissionId,
                Display = nr.Permission?.Code,
            }).ToList();

            dto.NotifiedUsers = entity.NotifiedUsers?.Select(nu => new OptionDto
            {
                Id = nu.UserId,
            }).ToList();
        }

        /// <inheritdoc/>
        public override Expression<Func<Notification, object>>[] IncludesBeforeDelete()
        {
            return new Expression<Func<Notification, object>>[] { x => x.NotifiedPermissions, x => x.NotifiedUsers };
        }

        // IncludesForUpdate done with the Query customizer because ...Select(..) not managed in .Net Core => it could be rechalenge with EF 6:
        // x => x.NotifiedPermissions, x => x.NotifiedPermissions.Select(y => y.Permission), x => x.NotifiedUsers
    }
}