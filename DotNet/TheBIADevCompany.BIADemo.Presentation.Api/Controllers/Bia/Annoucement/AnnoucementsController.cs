// <copyright file="AnnoucementsController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia.Annoucement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Annoucement;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Annoucement;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;

    /// <summary>
    /// The API controller used to manage Annoucements.
    /// </summary>
    public class AnnoucementsController : BiaControllerBase
    {
        /// <summary>
        /// The annoucement application service.
        /// </summary>
        private readonly IAnnoucementAppService annoucementService;

        private readonly IClientForHubService clientForHubService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnoucementsController"/> class.
        /// </summary>
        /// <param name="annoucementService">The annoucement application service.</param>
        /// <param name="clientForHubService">The hub for client.</param>
        public AnnoucementsController(IAnnoucementAppService annoucementService, IClientForHubService clientForHubService)
        {
            this.annoucementService = annoucementService;
            this.clientForHubService = clientForHubService;
        }

        /// <summary>
        /// Get all annoucements with filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of annoucements.</returns>
        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Annoucements.ListAccess)]
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            var (results, total) = await this.annoucementService.GetRangeAsync(filters);
            this.HttpContext.Response.Headers.Append(BiaConstants.HttpHeaders.TotalCount, total.ToString());
            return this.Ok(results);
        }

        /// <summary>
        /// Get a annoucement by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The annoucement.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Annoucements.Read)]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var dto = await this.annoucementService.GetAsync(id);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Add a annoucement.
        /// </summary>
        /// <param name="dto">The annoucement DTO.</param>
        /// <returns>The result of the creation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Annoucements.Create)]
        public async Task<IActionResult> Add([FromBody] AnnoucementDto dto)
        {
            try
            {
                var createdDto = await this.annoucementService.AddAsync(dto);
                await this.clientForHubService.SendTargetedMessage(string.Empty, "annoucements", "change-annoucements");
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
        /// Update a annoucement.
        /// </summary>
        /// <param name="id">The annoucement identifier.</param>
        /// <param name="dto">The annoucement DTO.</param>
        /// <returns>The result of the update.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Annoucements.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] AnnoucementDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                var updatedDto = await this.annoucementService.UpdateAsync(dto);
                await this.clientForHubService.SendTargetedMessage(string.Empty, "annoucements", "change-annoucements");
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
        /// Remove a annoucement.
        /// </summary>
        /// <param name="id">The annoucement identifier.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Annoucements.Delete)]
        public async Task<IActionResult> Remove(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                await this.annoucementService.RemoveAsync(id);
                await this.clientForHubService.SendTargetedMessage(string.Empty, "annoucements", "change-annoucements");
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Removes the specified annoucement ids.
        /// </summary>
        /// <param name="ids">The annoucement ids.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Annoucements.Delete)]
        public async Task<IActionResult> Remove([FromQuery] List<int> ids)
        {
            if (ids?.Any() != true)
            {
                return this.BadRequest();
            }

            try
            {
                await this.annoucementService.RemoveAsync(ids);
                await this.clientForHubService.SendTargetedMessage(string.Empty, "annoucements", "change-annoucements");
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
        [Authorize(Roles = Rights.Annoucements.Read)]
        public async Task<IActionResult> GetHistorical(int id)
        {
            try
            {
                var dto = await this.annoucementService.GetHistoricalAsync(id);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        [HttpGet("actives")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActives()
        {
            var actives = await this.annoucementService.GetActives();
            return this.Ok(actives);
        }
    }
}
