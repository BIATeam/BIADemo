// <copyright file="NotificationsController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Notification;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;

    /// <summary>
    /// The API controller used to manage notifications.
    /// </summary>
    public class NotificationsController : BiaControllerBase
    {
        /// <summary>
        /// The service notification.
        /// </summary>
        private readonly INotificationAppService notificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationsController"/> class.
        /// </summary>
        /// <param name="notificationService">The notification service.</param>
        public NotificationsController(INotificationAppService notificationService)
        {
            this.notificationService = notificationService;
        }

        /// <summary>
        /// Gets all notifications that I can see.
        /// </summary>
        /// <returns>The list of notifications.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Rights.Notifications.ListAccess)]
        public async Task<IActionResult> GetAll()
        {
            var results = await this.notificationService.GetAllAsync();

            return this.Ok(results);
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
            var (results, total) = await this.notificationService.GetRangeAsync(filters);
            this.HttpContext.Response.Headers.Add(BIAConstants.HttpHeaders.TotalCount, total.ToString());
            return this.Ok(results);
        }

        /// <summary>
        /// Removes the specified notification id.
        /// </summary>
        /// <param name="id">The identifier of the notification.</param>
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
        /// Removes the specified user ids.
        /// </summary>
        /// <param name="ids">The identifiers of the user.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Users.Delete)]
        public async Task<IActionResult> Remove([FromQuery] List<int> ids)
        {
            try
            {
                if (ids == null || !ids.Any())
                {
                    return this.BadRequest();
                }

                foreach (int id in ids)
                {
                    await this.notificationService.RemoveAsync(id);
                }

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
    }
}