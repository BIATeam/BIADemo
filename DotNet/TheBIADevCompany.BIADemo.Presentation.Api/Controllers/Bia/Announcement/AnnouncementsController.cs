// <copyright file="AnnouncementsController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia.Announcement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Announcement;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Announcement;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;

    /// <summary>
    /// The API controller used to manage Announcements.
    /// </summary>
    public class AnnouncementsController : BiaControllerBase
    {
        /// <summary>
        /// The announcement application service.
        /// </summary>
        private readonly IAnnouncementAppService announcementService;

        private readonly IClientForHubService clientForHubService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnouncementsController"/> class.
        /// </summary>
        /// <param name="announcementService">The announcement application service.</param>
        /// <param name="clientForHubService">The hub for client.</param>
        public AnnouncementsController(IAnnouncementAppService announcementService, IClientForHubService clientForHubService)
        {
            this.announcementService = announcementService;
            this.clientForHubService = clientForHubService;
        }

        /// <summary>
        /// Get all announcements with filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of announcements.</returns>
        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Announcements.ListAccess)]
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            var (results, total) = await this.announcementService.GetRangeAsync(filters);
            this.HttpContext.Response.Headers.Append(BiaConstants.HttpHeaders.TotalCount, total.ToString());
            return this.Ok(results);
        }

        /// <summary>
        /// Get an announcement by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The announcement.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Announcements.Read)]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var dto = await this.announcementService.GetAsync(id);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Add an announcement.
        /// </summary>
        /// <param name="dto">The announcement DTO.</param>
        /// <returns>The result of the creation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Announcements.Create)]
        public async Task<IActionResult> Add([FromBody] AnnouncementDto dto)
        {
            try
            {
                var createdDto = await this.announcementService.AddAsync(dto);
                await this.clientForHubService.SendTargetedMessage(string.Empty, "announcements", "change-announcements");
                return this.CreatedAtAction("Get", new { id = createdDto.Id }, createdDto);
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
            catch (ForbiddenException)
            {
                return this.Forbid();
            }
        }

        /// <summary>
        /// Update an announcement.
        /// </summary>
        /// <param name="id">The announcement identifier.</param>
        /// <param name="dto">The announcement DTO.</param>
        /// <returns>The result of the update.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Announcements.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] AnnouncementDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                var updatedDto = await this.announcementService.UpdateAsync(dto);
                await this.clientForHubService.SendTargetedMessage(string.Empty, "announcements", "change-announcements");
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
            catch (OutdateException)
            {
                return this.Conflict();
            }
            catch (ForbiddenException)
            {
                return this.Forbid();
            }
        }

        /// <summary>
        /// Remove an announcement.
        /// </summary>
        /// <param name="id">The announcement identifier.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Announcements.Delete)]
        public async Task<IActionResult> Remove(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                await this.announcementService.RemoveAsync(id);
                await this.clientForHubService.SendTargetedMessage(string.Empty, "announcements", "change-announcements");
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Removes the specified announcement ids.
        /// </summary>
        /// <param name="ids">The announcement ids.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Announcements.Delete)]
        public async Task<IActionResult> Remove([FromQuery] List<int> ids)
        {
            if (ids?.Any() != true)
            {
                return this.BadRequest();
            }

            try
            {
                await this.announcementService.RemoveAsync(ids);
                await this.clientForHubService.SendTargetedMessage(string.Empty, "announcements", "change-announcements");
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Return the historical of an item by its id.
        /// </summary>
        /// <param name="id">ID of the item to update.</param>
        /// <returns>Item's historical.</returns>
        [HttpGet("{id}/historical")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Announcements.Read)]
        public async Task<IActionResult> GetHistorical(int id)
        {
            try
            {
                var dto = await this.announcementService.GetHistoricalAsync(id);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Return the actives announcements.
        /// </summary>
        /// <returns>Actives announcements.</returns>
        [HttpGet("actives")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetActives()
        {
            var actives = await this.announcementService.GetActives();
            return this.Ok(actives);
        }
    }
}
