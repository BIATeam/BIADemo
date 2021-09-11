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
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

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
                    { "TypeId", notification => notification.TypeId },
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
            entity.CreatedById = dto.CreatedById;
            entity.TypeId = dto.TypeId;

            entity.SiteId = dto.SiteId;

            if (dto.NotifiedUserIds?.Any() == true)
            {
                entity.NotifiedUsers = new List<NotificationUser>();

                foreach (var userId in dto.NotifiedUserIds)
                {
                    entity.NotifiedUsers.Add(new NotificationUser
                    { UserId = userId, NotificationId = dto.Id });
                }
            }
            if (dto.NotifiedUserIds?.Any() == true)
            {
                entity.NotifiedRoles = new List<NotificationRole>();

                foreach (var roleId in dto.NotifiedRoleIds)
                {
                    entity.NotifiedRoles.Add(new NotificationRole
                    { RoleId = roleId, NotificationId = dto.Id });
                }
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Notification, NotificationDto>> EntityToDto()
        {
            return entity => new NotificationDto
            {
                Id = entity.Id,
                CreatedById = entity.CreatedById,
                CreatedDate = entity.CreatedDate,
                Description = entity.Description,
                Read = entity.Read,
                Title = entity.Title,
                TypeId = entity.TypeId,
                TargetJson = entity.TargetJson,

                SiteId = entity.SiteId,
                NotifiedRoleIds = entity.NotifiedRoles.Select(nu => nu.RoleId).ToList(),
                NotifiedUserIds = entity.NotifiedUsers.Select(nu => nu.UserId).ToList(),
            };
        }
    }
}