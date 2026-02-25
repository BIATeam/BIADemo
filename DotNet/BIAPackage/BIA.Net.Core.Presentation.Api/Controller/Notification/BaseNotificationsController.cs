// <copyright file="BaseNotificationsController.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Presentation.Api.Controller.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Notification;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Notification;
#pragma warning disable BIA001 // Forbidden reference to Domain layer in Presentation layer
    using BIA.Net.Core.Domain.Notification.Entities;
#pragma warning restore BIA001 // Forbidden reference to Domain layer in Presentation layer
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The API controller used to manage Notifications.
    /// </summary>
    /// <typeparam name="TBaseNotificationDto">The type of the notification DTO.</typeparam>
    /// <typeparam name="TBaseNotificationListItemDto">The type of the notification list item DTO.</typeparam>
    /// <typeparam name="TBaseNotification">The type of the notification entity.</typeparam>
    public abstract class BaseNotificationsController<TBaseNotificationDto, TBaseNotificationListItemDto, TBaseNotification> : BiaControllerBase
        where TBaseNotificationDto : BaseNotificationDto, new()
        where TBaseNotificationListItemDto : BaseNotificationListItemDto, new()
        where TBaseNotification : BaseNotification, new()
    {
        /// <summary>
        /// The notification application service.
        /// </summary>
        private readonly IBaseNotificationAppService<TBaseNotificationDto, TBaseNotificationListItemDto, TBaseNotification> notificationService;
        private readonly IBiaClaimsPrincipalService biaClaimsPrincipalService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseNotificationsController{TBaseNotificationDto, TBaseNotificationListItemDto, TBaseNotification}"/> class.
        /// </summary>
        /// <param name="notificationService">The notification service.</param>
        /// <param name="biaClaimsPrincipalService">The bia claims principal service.</param>
        protected BaseNotificationsController(IBaseNotificationAppService<TBaseNotificationDto, TBaseNotificationListItemDto, TBaseNotification> notificationService, IBiaClaimsPrincipalService biaClaimsPrincipalService)
        {
            this.notificationService = notificationService;
            this.biaClaimsPrincipalService = biaClaimsPrincipalService;
        }

        /// <summary>
        /// Get all notifications with filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of notifications.</returns>
        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(BiaPermissionId.Notification_List_Access))]
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            var (results, total) = await this.notificationService.GetRangeAsync(filters);
            this.HttpContext.Response.Headers.Append(BiaConstants.HttpHeaders.TotalCount, total.ToString());
            return this.Ok(results);
        }

        /// <summary>
        /// Get all notifications with filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of notifications.</returns>
        [HttpPost("allCrossSite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(BiaPermissionId.Notification_List_Access))]
        public async Task<IActionResult> GetAllCrossSite([FromBody] PagingFilterFormatDto filters)
        {
            var (results, total) = await this.notificationService.GetRangeWithAllAccessAsync(filters);
            this.HttpContext.Response.Headers.Append(BiaConstants.HttpHeaders.TotalCount, total.ToString());
            return this.Ok(results);
        }

        /// <summary>
        /// Add a notification.
        /// </summary>
        /// <param name="dto">The notification DTO.</param>
        /// <returns>The result of the creation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(BiaPermissionId.Notification_Create))]
        public async Task<IActionResult> Add([FromBody] TBaseNotificationDto dto)
        {
            try
            {
                var createdDto = await this.notificationService.AddAsync(dto);
                return this.CreatedAtAction("Get", new { id = createdDto.Id }, createdDto);
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
        }

        /// <summary>
        /// Update a notification.
        /// </summary>
        /// <param name="id">The notification identifier.</param>
        /// <param name="dto">The notification DTO.</param>
        /// <returns>The result of the update.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(BiaPermissionId.Notification_Update))]
        public async Task<IActionResult> Update(int id, [FromBody] TBaseNotificationDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                var updatedDto = await this.notificationService.UpdateAsync(dto);
                return this.Ok(updatedDto);
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Sets a notification as read.
        /// </summary>
        /// <param name="id">The notification identifier.</param>
        /// <returns>Ok() if success, error if failed.</returns>
        [HttpPut("setAsRead/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(BiaPermissionId.Notification_Read))]
        public async Task<IActionResult> SetAsRead(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var dto = await this.notificationService.GetAsync(id);

                // If it's the first time this notification is read
                if (!dto.Read)
                {
                    await this.notificationService.SetAsRead(dto);
                }

                return this.Ok();
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Remove a notification.
        /// </summary>
        /// <param name="id">The notification identifier.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(BiaPermissionId.Notification_Delete))]
        public async Task<IActionResult> Remove(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                await this.notificationService.RemoveAsync(id);
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Removes the specified notificationsType ids.
        /// </summary>
        /// <param name="ids">The notificationsType ids.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(BiaPermissionId.Notification_Delete))]
        public async Task<IActionResult> Remove([FromQuery] List<int> ids)
        {
            if (ids?.Any() != true)
            {
                return this.BadRequest();
            }

            try
            {
                await this.notificationService.RemoveAsync(ids);

                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Get a notification by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The notification.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(BiaPermissionId.Notification_Read))]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var dto = await this.notificationService.GetAsync(id);

                // If it's the first time this notification is read
                if (!dto.Read)
                {
                    await this.notificationService.SetAsRead(dto);
                    dto.Read = true;
                }

                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Set a notification to unread and return notification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The notification.</returns>
        [HttpGet("setUnRead/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(BiaPermissionId.Notification_Read))]
        public async Task<IActionResult> SetUnread(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var dto = await this.notificationService.GetAsync(id);

                // If it's the first time this notification is read
                if (dto.Read)
                {
                    await this.notificationService.SetUnread(dto);
                }

                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Get unread notifications Ids.
        /// </summary>
        /// <returns>The number of unread notifications.</returns>
        [HttpGet("unreadIds")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(BiaPermissionId.Notification_List_Access))]
        public async Task<IActionResult> GetUnreadIds()
        {
            int userId = this.biaClaimsPrincipalService.GetUserId();
            try
            {
                var dto = await this.notificationService.GetUnreadIds(userId);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Generates a csv file according to the filters.
        /// </summary>
        /// <param name="filters">filters ( <see cref="PagingFilterFormatDto"/>).</param>
        /// <returns>a csv file.</returns>
        [HttpPost("csv")]
        public virtual async Task<IActionResult> GetFile([FromBody] PagingFilterFormatDto filters)
        {
            byte[] buffer = await this.notificationService.GetCsvAsync(filters);
            return this.File(buffer, BiaConstants.Csv.ContentType + $";charset={BiaConstants.Csv.CharsetEncoding}", $"Notifications{BiaConstants.Csv.Extension}");
        }
    }
}
