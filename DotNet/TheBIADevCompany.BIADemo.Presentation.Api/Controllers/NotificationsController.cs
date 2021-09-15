// BIADemo only
// <copyright file="NotificationsController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
#define UseHubForClientInNotification

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Notification;
#if UseHubForClientInNotification
    using BIA.Net.Core.Domain.RepoContract;
#endif
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
#if UseHubForClientInNotification
    using Microsoft.AspNetCore.SignalR;
#endif
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Service;

    /// <summary>
    /// The API controller used to manage Notifications.
    /// </summary>
    public class NotificationsController : BiaControllerBase
    {
        /// <summary>
        /// The notification application service.
        /// </summary>
        private readonly INotificationAppService notificationService;
        private readonly IPrincipal principal;

#if UseHubForClientInNotification
        private readonly IClientForHubRepository clientForHubService;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationsController"/> class.
        /// </summary>
        /// <param name="notificationService">The notification application service.</param>
        /// <param name="principal">The current user.</param>
        /// <param name="clientForHubService">The hub for client.</param>
#if UseHubForClientInNotification
        public NotificationsController(INotificationAppService notificationService, IPrincipal principal,  IClientForHubRepository clientForHubService)
#else
        public NotificationsController(INotificationAppService notificationService)
#endif
        {
#if UseHubForClientInNotification
            this.clientForHubService = clientForHubService;
#endif
            this.notificationService = notificationService;
            this.principal = principal;

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
        [Authorize(Roles = Rights.Notifications.ListAccess)]
        public async Task<IActionResult> GetAll([FromBody] LazyLoadDto filters)
        {
            try
            {
                var (results, total) = await this.notificationService.GetRangeAsync(filters);
                this.HttpContext.Response.Headers.Add(BIAConstants.HttpHeaders.TotalCount, total.ToString());
                return this.Ok(results);
            }
            catch (Exception e)
            {
                return this.StatusCode(500, "Internal server error " + e.Message);
            }
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
        [Authorize(Roles = Rights.Notifications.Create)]
        public async Task<IActionResult> Add([FromBody] NotificationDto dto)
        {
            try
            {
                var createdDto = await this.notificationService.AddAsync(dto);
#if UseHubForClientInNotification
                await this.clientForHubService.SendMessage("refresh-notifications", string.Empty);
#endif
                return this.CreatedAtAction("Get", new { id = createdDto.Id }, createdDto);
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
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
        [Authorize(Roles = Rights.Notifications.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] NotificationDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                var updatedDto = await this.notificationService.UpdateAsync(dto);
#if UseHubForClientInNotification
                await this.clientForHubService.SendMessage("refresh-notifications", string.Empty);
#endif
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
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
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
        [Authorize(Roles = Rights.Notifications.Delete)]
        public async Task<IActionResult> Remove(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                await this.notificationService.RemoveAsync(id);
#if UseHubForClientInNotification
                await this.clientForHubService.SendMessage("refresh-notifications", string.Empty);
#endif
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
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
        [Authorize(Roles = Rights.Notifications.Read)]
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
                    dto = await this.notificationService.SetAsRead(dto);
#if UseHubForClientInNotification
                    await this.clientForHubService.SendMessage("refresh-notifications", string.Empty);
#endif
                }

                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Get unread notifications count.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The number of unread notifications.</returns>
        [HttpGet("unreadIds")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Notifications.ListAccess)]
        public async Task<IActionResult> GetUnreadIds()
        {
            int userId = (this.principal as BIAClaimsPrincipal).GetUserId();
            try
            {
                var dto = await this.notificationService.GetUnreadIds(userId);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
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
        [Authorize(Roles = Rights.Notifications.Delete)]
        public async Task<IActionResult> Remove([FromQuery] List<int> ids)
        {
            if (ids?.Any() != true)
            {
                return this.BadRequest();
            }

            try
            {
                foreach (int id in ids)
                {
                    await this.notificationService.RemoveAsync(id);
                }

#if UseHubForClientInNotification
                await this.clientForHubService.SendMessage("refresh-notifications", string.Empty);
#endif
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Generates a csv file according to the filters.
        /// </summary>
        /// <param name="filters">filters ( <see cref="FileFiltersDto"/>).</param>
        /// <returns>a csv file.</returns>
        [HttpPost("csv")]
        public virtual async Task<IActionResult> GetFile([FromBody] FileFiltersDto filters)
        {
            byte[] buffer = await this.notificationService.GetCsvAsync(filters);
            return this.File(buffer, BIAConstants.Csv.ContentType + ";charset=utf-8", $"Notifications{BIAConstants.Csv.Extension}");
        }
    }
}