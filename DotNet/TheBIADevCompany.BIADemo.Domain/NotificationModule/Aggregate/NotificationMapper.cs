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
                    { "Title", notification => notification.Title },
                    { "Description", notification => notification.Description },
                    { "CreatedDate", notification => notification.CreatedDate },
                    { "Type", notification => notification.Type.Code },
                    { "Read", notification => notification.Read },
                    { "CreatedBy", notification => notification.CreatedBy.FirstName + notification.CreatedBy.LastName + " (" + notification.CreatedBy.Login + ")" },
                    { "NotifiedPermissions", notification => notification.NotifiedPermissions.Select(x => x.Permission.Code).OrderBy(x => x) },
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

            // Mapping relationship *-* : ICollection<Airports>
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

            // Mapping relationship *-* : ICollection<Airports>
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

        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Notification, NotificationDto>> EntityToDto()
        {
            var JsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

            return entity => new NotificationDto
            {
                Id = entity.Id,
                CreatedDate = entity.CreatedDate,

                CreatedBy = entity.CreatedBy != null ? new OptionDto
                {
                    Id = entity.CreatedBy.Id,
                    Display = entity.CreatedBy.FirstName + " " + entity.CreatedBy.LastName + " (" + entity.CreatedBy.Login + ")",
                }
                : null,

                Description = entity.Description,
                Read = entity.Read,
                Title = entity.Title,
                Type = new OptionDto
                {
                    Id = entity.TypeId,
                    Display = entity.Type.Code,
                },

                JData = entity.JData,

                SiteId = entity.SiteId,

                NotifiedPermissions = entity.NotifiedPermissions.Select(nr => new OptionDto
                {
                    Id = nr.Permission.Id,
                    Display = nr.Permission.Code,
                }).ToList(),

                NotifiedUsers = entity.NotifiedUsers.Select(nu => new OptionDto
                {
                    Id = nu.User.Id,
                    Display = nu.User.FirstName + " " + nu.User.LastName + " (" + nu.User.Login + ")",
                }).ToList(),
            };
        }
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.IncludesForUpdate"/>
        public override Expression<Func<Notification, object>>[] IncludesForUpdate()
        {
            return new Expression<Func<Notification, object>>[] { x => x.NotifiedPermissions, x => x.NotifiedUsers };
        }
    }
}