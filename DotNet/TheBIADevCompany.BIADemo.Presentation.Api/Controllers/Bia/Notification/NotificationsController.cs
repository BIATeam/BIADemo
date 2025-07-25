// <copyright file="NotificationsController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
// #define UseHubForClientInNotification

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia.Notification
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
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The API controller used to manage Notifications.
    /// </summary>
    public partial class NotificationsController : BiaControllerBase
    {
        /// <summary>
        /// The notification application service.
        /// </summary>
        private readonly INotificationAppService notificationService;
        private readonly IBiaClaimsPrincipalService biaClaimsPrincipalService;

#if UseHubForClientInNotification
        private readonly IClientForHubService clientForHubService;
#endif

#if UseHubForClientInNotification
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationsController"/> class.
        /// </summary>
        /// <param name="notificationService">The notification application service.</param>
        /// <param name="clientForHubService">The hub for client.</param>
        /// <param name="biaClaimsPrincipalService">The BIA claims principal service.</param>
        public NotificationsController(INotificationAppService notificationService, IClientForHubService clientForHubService, IBiaClaimsPrincipalService biaClaimsPrincipalService)
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationsController"/> class.
        /// </summary>
        /// <param name="notificationService">The notification application service.</param>
        /// <param name="biaClaimsPrincipalService">The BIA claims principal service.</param>
        public NotificationsController(INotificationAppService notificationService, IBiaClaimsPrincipalService biaClaimsPrincipalService)
#endif
        {
#if UseHubForClientInNotification
            this.clientForHubService = clientForHubService;
#endif
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
        [Authorize(Roles = BiaRights.Notifications.ListAccess)]
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
        [Authorize(Roles = BiaRights.Notifications.ListAccess)]
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
        [Authorize(Roles = BiaRights.Notifications.Create)]
        public async Task<IActionResult> Add([FromBody] NotificationDto dto)
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
        [Authorize(Roles = BiaRights.Notifications.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] NotificationDto dto)
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
        [Authorize(Roles = BiaRights.Notifications.Read)]
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
        [Authorize(Roles = BiaRights.Notifications.Delete)]
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
        [Authorize(Roles = BiaRights.Notifications.Delete)]
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
        [Authorize(Roles = BiaRights.Notifications.Read)]
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
        [Authorize(Roles = BiaRights.Notifications.Read)]
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
        [Authorize(Roles = BiaRights.Notifications.ListAccess)]
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
            return this.File(buffer, BiaConstants.Csv.ContentType + ";charset=utf-8", $"Notifications{BiaConstants.Csv.Extension}");
        }
    }
}