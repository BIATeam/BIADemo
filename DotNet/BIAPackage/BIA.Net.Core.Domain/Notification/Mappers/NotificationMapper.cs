// <copyright file="NotificationMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Notification.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Notification;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.Notification.Entities;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Translation.Entities;
    using BIA.Net.Core.Domain.User;

    /// <summary>
    /// The mapper used for user.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="NotificationMapper"/> class.
    /// </remarks>
    /// <param name="userContext">the user context.</param>
    public class NotificationMapper(UserContext userContext) : BaseMapper<NotificationDto, Notification, int>
    {
        /// <inheritdoc />
        public override ExpressionCollection<Notification> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Notification>(base.ExpressionCollection)
                {
                    { HeaderName.Title, notification => notification.Title },
                    { HeaderName.Description, notification => notification.Description },
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
        public override void DtoToEntity(NotificationDto dto, ref Notification entity)
        {
            base.DtoToEntity(dto, ref entity);

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

        /// <inheritdoc />
        public override Expression<Func<Notification, NotificationDto>> EntityToDto(string mapperMode)
        {
            return this.EntityToDto().CombineMapping(entity => new NotificationDto
            {
                Title = entity.Title,
                Description = entity.Description,
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

                NotifiedTeams = entity.NotifiedTeams.Select(nt => new NotificationTeamDto
                {
                    Id = nt.TeamId,
                    Team = new OptionDto
                    {
                        Display = nt.Team != null ? nt.Team.Title : string.Empty,
                        Id = nt.Team != null ? nt.Team.Id : 0,
                    },
                    TeamTypeId = nt.Team != null ? nt.Team.TeamTypeId : (int)BiaTeamTypeId.Root,

                    Roles = nt.Roles != null ? nt.Roles.Select(ntr => new OptionDto
                    {
                        Id = ntr.RoleId,
                        Display = ntr.Role.RoleTranslations.Where(rt => rt.Language.Code == this.UserContext.Language).Select(rt => rt.Label).FirstOrDefault() ?? ntr.Role.Label,
                    }).ToList() : null,
                }).ToList(),

                NotifiedUsers = entity.NotifiedUsers.Select(nu => new OptionDto
                {
                    Id = nu.User.Id,
                    Display = nu.User.Display(),
                }).ToList(),

                NotificationTranslations = entity.NotificationTranslations.Select(nt => new NotificationTranslationDto
                {
                    DtoState = DtoState.Unchanged,
                    Id = nt.Id,
                    LanguageId = nt.LanguageId,
                    Title = nt.Title,
                    Description = nt.Description,
                }).ToList(),
            });
        }

        /// <inheritdoc/>
        public override Expression<Func<Notification, object>>[] IncludesBeforeDelete()
        {
            return
            [
                x => x.NotifiedTeams,
                x => x.NotifiedUsers,
            ];
        }

        /// <summary>
        /// Header names.
        /// </summary>
        public struct HeaderName
        {
            /// <summary>
            /// Header name for title.
            /// </summary>
            public const string Title = "title";

            /// <summary>
            /// Header name for description.
            /// </summary>
            public const string Description = "description";

            /// <summary>
            /// Header name for title translated.
            /// </summary>
            public const string TitleTranslated = "titleTranslated";

            /// <summary>
            /// Header name for description translated.
            /// </summary>
            public const string DescriptionTranslated = "descriptionTranslated";

            /// <summary>
            /// Header name for created date.
            /// </summary>
            public const string CreatedDate = "createdDate";

            /// <summary>
            /// Header name for type.
            /// </summary>
            public const string Type = "type";

            /// <summary>
            /// Header name for read.
            /// </summary>
            public const string Read = "read";

            /// <summary>
            /// Header name for created by.
            /// </summary>
            public const string CreatedBy = "createdBy";

            /// <summary>
            /// Header name for notified teams.
            /// </summary>
            public const string NotifiedTeams = "notifiedTeams";

            /// <summary>
            /// Header name for notified users.
            /// </summary>
            public const string NotifiedUsers = "NntifiedUsers";
        }
    }
}