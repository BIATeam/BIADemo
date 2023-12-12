// <copyright file="NotificationMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Principal;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Notification;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Service;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.TranslationModule.Aggregate;

    /// <summary>
    /// The mapper used for user.
    /// </summary>
    public class NotificationMapper : BaseMapper<NotificationDto, Notification, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationMapper"/> class.
        /// </summary>
        /// <param name="userContext">the user context</param>
        public NotificationMapper(UserContext userContext)
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
                    { "Title", notification => notification.Title },
                    { "Description", notification => notification.Description },
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

        /// <summary>
        /// The user context langage and culture.
        /// </summary>
        private UserContext UserContext { get; set; }

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

            // Mapping relationship *-* : ICollection<OptionDto> NotifiedUsers
            if (dto.NotifiedUsers != null && dto.NotifiedUsers?.Any() == true)
            {
                foreach (var userDto in dto.NotifiedUsers.Where(x => x.DtoState == DtoState.Deleted))
                {
                    var user = entity.NotifiedUsers.FirstOrDefault(x => x.UserId == userDto.Id);
                    if (user != null)
                    {
                        entity.NotifiedUsers.Remove(user);
                    }
                }

                entity.NotifiedUsers = entity.NotifiedUsers ?? new List<NotificationUser>();
                foreach (var userDto in dto.NotifiedUsers.Where(w => w.DtoState == DtoState.Added))
                {
                    entity.NotifiedUsers.Add(new NotificationUser
                    { UserId = userDto.Id, NotificationId = dto.Id });
                }
            }

            // Mapping relationship *-* : ICollection<OptionDto> NotifiedTeams
            if (dto.NotifiedTeams != null && dto.NotifiedTeams?.Any() == true)
            {
                foreach (var teamDto in dto.NotifiedTeams.Where(x => x.DtoState == DtoState.Deleted))
                {
                    var notifiedTeams = entity.NotifiedTeams.FirstOrDefault(x => x.TeamId == teamDto.Team.Id);
                    if (notifiedTeams != null)
                    {
                        entity.NotifiedTeams.Remove(notifiedTeams);
                    }
                }

                entity.NotifiedTeams = entity.NotifiedTeams ?? new List<NotificationTeam>();
                foreach (var teamDto in dto.NotifiedTeams.Where(w => w.DtoState == DtoState.Added))
                {
                    entity.NotifiedTeams.Add(new NotificationTeam
                    {
                        TeamId = teamDto.Team.Id,
                        NotificationId = dto.Id,
                        Roles = teamDto.Roles != null ? teamDto.Roles.Select(role => new NotificationTeamRole
                        {
                            NotificationTeamId = teamDto.Id,
                            RoleId = role.Id,
                        }).ToList() : null,
                    });
                }

                foreach (var teamDto in dto.NotifiedTeams.Where(x => x.DtoState == DtoState.Modified))
                {
                    var notifiedTeam = entity.NotifiedTeams.FirstOrDefault(x => x.TeamId == teamDto.Team.Id);
                    if (notifiedTeam != null)
                    {
                        foreach (var roleDto in teamDto.Roles.Where(x => x.DtoState == DtoState.Deleted))
                        {
                            var role = notifiedTeam.Roles.FirstOrDefault(x => x.RoleId == roleDto.Id);
                            if (role != null)
                            {
                                notifiedTeam.Roles.Remove(role);
                            }
                        }

                        notifiedTeam.Roles = notifiedTeam.Roles ?? new List<NotificationTeamRole>();
                        foreach (var roleDto in teamDto.Roles.Where(w => w.DtoState == DtoState.Added))
                        {
                            notifiedTeam.Roles.Add(new NotificationTeamRole
                            {
                                RoleId = roleDto.Id,
                                NotificationTeamId = teamDto.Id,
                            });
                        }
                    }
                }
            }

            // Mapping relationship *-1 : ICollection<NotificationTranslationDto> NotificationTranslation
            if (dto.NotificationTranslations != null && dto.NotificationTranslations?.Any() == true)
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
        public override Expression<Func<Notification, NotificationDto>> EntityToDto(string mapperMode)
        {
            return entity => new NotificationDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
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

                NotifiedTeams = entity.NotifiedTeams.Select(nt => new NotificationTeamDto
                {
                    Id = nt.TeamId,
                    Team = new OptionDto
                    {
                        Display = (nt.Team != null) ? nt.Team.Title : string.Empty,
                        Id = (nt.Team != null) ? nt.Team.Id : 0,
                    },
                    TeamTypeId = nt.Team != null ? nt.Team.TeamTypeId : (int)TeamTypeId.Root,

                    Roles = nt.Roles != null ? nt.Roles.Select(ntr => new OptionDto
                    {
                        Id = ntr.RoleId,
                        Display = ntr.Role.RoleTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? ntr.Role.Label,
                    }).ToList() : null,
                }).ToList(),

                NotifiedUsers = entity.NotifiedUsers.Select(nu => new OptionDto
                {
                    Id = nu.User.Id,
                    Display = nu.User.FirstName + " " + nu.User.LastName + " (" + nu.User.Login + ")",
                }).ToList(),

                NotificationTranslations = entity.NotificationTranslations.Select(nt => new NotificationTranslationDto
                {
                    DtoState = DtoState.Unchanged,
                    Id = nt.Id,
                    LanguageId = nt.LanguageId,
                    Title = nt.Title,
                    Description = nt.Description,
                }).ToList(),
            };
        }

        /// <inheritdoc/>
        public override void MapEntityKeysInDto(Notification entity, NotificationDto dto)
        {
            dto.Id = entity.Id;
        }

        /// <inheritdoc/>
        public override Expression<Func<Notification, object>>[] IncludesBeforeDelete()
        {
            return new Expression<Func<Notification, object>>[] { x => x.NotifiedTeams, x => x.NotifiedUsers };
        }
    }
}