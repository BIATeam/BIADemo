// <copyright file="NotificationLightMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using TheBIADevCompany.BIADemo.Domain.Dto.Notification;

    /// <summary>
    /// The mapper used for user.
    /// </summary>
    public class NotificationLightMapper : BaseMapper<NotificationDto, Notification>
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

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Notification, NotificationDto>> EntityToDto()
        {
            return entity => new NotificationDto
            {
                Id = entity.Id,
                //CreatedBy = entity.CreatedBy != null ? new UserDto()
                //{
                //    Id = entity.CreatedBy.Id,
                //    FirstName = entity.CreatedBy.FirstName,
                //    LastName = entity.CreatedBy.LastName,
                //    Login = entity.CreatedBy.Login,
                //} : null,
                CreatedDate = entity.CreatedDate,
                Description = entity.Description,
                Read = entity.Read,
                Title = entity.Title,
                TypeId = entity.TypeId,
                TargetId = entity.TargetId,
                TargetRoute = entity.TargetRoute,
            };
        }
    }
}