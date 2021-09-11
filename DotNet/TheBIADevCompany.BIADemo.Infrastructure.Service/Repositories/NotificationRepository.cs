// <copyright file="NotificationRepository.cs" company="BIA.Net">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Service.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Notification;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Domain.Dto.Notification;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Aggregate;

    /// <summary>
    /// The class representing a NotificationRepository.
    /// </summary>
    /// <seealso cref="BIA.Net.Core.Domain.INotificationRepository" />
    public class NotificationRepository : MailRepository, INotification
    {
        private readonly ITGenericRepository<Notification> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationRepository"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="repository">The notification entity repository.</param>
        public NotificationRepository(IOptions<BiaNetSection> configuration, ITGenericRepository<Notification> repository)
            : base(configuration)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Create a notification displayed in UI.
        /// </summary>
        /// <param name="notification">The noticiation.</param>
        /// <returns>the task.</returns>
        public async Task CreateNotification(NotificationDto notification)
        {
            await this.AddAsync(notification);
        }

        /// <summary>
        /// Return the list of unreadIds.
        /// </summary>
        /// <param name="userId">the user Id.</param>
        /// <returns>The list of int.</returns>
        public async Task<List<int>> GetUnreadIds(int userId)
        {
            var results = await this.repository.GetAllResultAsync<int>(selectResult: x => x.Id, filter: x => !x.Read);

            return results.ToList();
        }

        private async Task<NotificationDto> AddAsync(NotificationDto dto)
        {
            if (dto != null)
            {
                var entity = new Notification();
                new NotificationMapper().DtoToEntity(dto, entity);
                this.repository.Add(entity);
                await this.repository.UnitOfWork.CommitAsync();
                dto.Id = entity.Id;
            }

            return dto;
        }

        private async Task<NotificationDto> UpdateAsync(NotificationDto dto)
        {
            if (dto != null)
            {
                var mapper = new NotificationMapper();

                var entity = await this.repository.GetEntityAsync(id: dto.Id);
                if (entity == null)
                {
                    throw new ElementNotFoundException();
                }

                mapper.DtoToEntity(dto, entity);
                this.repository.Update(entity);
                await this.repository.UnitOfWork.CommitAsync();
                dto.DtoState = DtoState.Unchanged;
            }

            return dto;
        }
    }
}
