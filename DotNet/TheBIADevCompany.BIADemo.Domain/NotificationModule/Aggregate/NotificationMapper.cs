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
    using TheBIADevCompany.BIADemo.Domain.Dto.Notification;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;

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
            entity.JobId = dto.JobId;
            entity.Read = dto.Read;
            entity.Title = dto.Title;
            entity.CreatedDate = dto.CreatedDate;
            entity.Description = dto.Description;
            entity.CreatedById = dto.CreatedById;

            if (dto.Site != null)
            {
                entity.SiteId = dto.Site.Id;
            }

            // Mapping relationship 0..1-* : PlaneType
            entity.TypeId = dto.Type.Id;

            // Mapping relationship *-* : ICollection<Airports>
            if (dto.NotificationUsers?.Any() == true)
            {
                foreach (var userDto in dto.NotificationUsers.Where(x => x.DtoState == DtoState.Deleted))
                {
                    var notifiedUser = entity.NotificationUsers.FirstOrDefault(x => x.UserId == userDto.Id && x.NotificationId == dto.Id);
                    if (notifiedUser != null)
                    {
                        entity.NotificationUsers.Remove(notifiedUser);
                    }
                }

                entity.NotificationUsers = entity.NotificationUsers ?? new List<NotificationUser>();

                foreach (var userDto in dto.NotificationUsers.Where(w => w.DtoState == DtoState.Added))
                {
                    entity.NotificationUsers.Add(new NotificationUser
                    { UserId = userDto.Id, NotificationId = dto.Id });
                }
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Notification, NotificationDto>> EntityToDto()
        {
            return entity => new NotificationDto
            {
                Id = entity.Id,
                JobId = entity.JobId,
                CreatedBy = new UserDto()
                {
                    Id = entity.CreatedBy.Id,
                    FirstName = entity.CreatedBy.FirstName,
                    LastName = entity.CreatedBy.LastName,
                    Login = entity.CreatedBy.Login,
                },
                CreatedById = entity.CreatedById,
                CreatedDate = entity.CreatedDate,
                Description = entity.Description,
                NotifiedRole = new RoleDto
                {
                    Id = entity.NotifiedRole.Id,
                    LabelEn = entity.NotifiedRole.LabelEn,
                    LabelFr = entity.NotifiedRole.LabelFr,
                    LabelEs = entity.NotifiedRole.LabelEs,
                },
                NotifiedRoleId = entity.NotifiedRoleId,
                Read = entity.Read,
                SiteId = entity.SiteId,
                Title = entity.Title,
                TypeId = entity.TypeId,
                Site = new SiteDto
                {
                    Id = entity.Site.Id,
                    Title = entity.Site.Title,
                },
                Type = entity.Type != null ? new NotificationTypeDto
                {
                    Id = entity.Type.Id,
                    Code = entity.Type.Code,
                }
                : null,
                NotificationUsers = entity.NotificationUsers.Select(nu => new NotificationUserDto
                {
                    UserId = nu.UserId,
                    NotificationId = nu.NotificationId,
                    Read = nu.Read,
                }).ToList(),
            };
        }
    }
}