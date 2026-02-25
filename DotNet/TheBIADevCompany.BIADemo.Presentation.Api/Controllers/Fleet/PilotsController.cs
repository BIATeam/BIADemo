// BIADemo only
// <copyright file="PilotsController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
// #define UseHubForClientInPilot
namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Fleet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
#if UseHubForClientInPilot
    using BIA.Net.Core.Application.Services;
#endif
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Fleet;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.Fleet;

    /// <summary>
    /// The API controller used to manage Pilots.
    /// </summary>
#if !UseHubForClientInPilot
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "UseHubForClientInPilot not set")]
#endif
    public class PilotsController : BiaControllerBase
    {
        /// <summary>
        /// The pilot application service.
        /// </summary>
        private readonly IPilotAppService pilotService;

#if UseHubForClientInPilot
        private readonly IClientForHubService clientForHubService;
#endif

#if UseHubForClientInPilot
        /// <summary>
        /// Initializes a new instance of the <see cref="PilotsController"/> class.
        /// </summary>
        /// <param name="pilotService">The pilot application service.</param>
        /// <param name="clientForHubService">The hub for client.</param>
        public PilotsController(
            IPilotAppService pilotService,
            IClientForHubService clientForHubService)
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="PilotsController"/> class.
        /// </summary>
        /// <param name="pilotService">The pilot application service.</param>
        public PilotsController(IPilotAppService pilotService)
#endif
        {
#if UseHubForClientInPilot
            this.clientForHubService = clientForHubService;
#endif
            this.pilotService = pilotService;
        }

        /// <summary>
        /// Get all pilots with filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of pilots.</returns>
        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(PermissionId.Pilot_List_Access))]
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            var (results, total) = await this.pilotService.GetRangeAsync(filters);
            this.HttpContext.Response.Headers.Append(BiaConstants.HttpHeaders.TotalCount, total.ToString());
            return this.Ok(results);
        }

        /// <summary>
        /// Get a pilot by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The pilot.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(PermissionId.Pilot_Read))]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return this.BadRequest();
            }

            try
            {
                var dto = await this.pilotService.GetAsync(id);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Add a pilot.
        /// </summary>
        /// <param name="dto">The pilot DTO.</param>
        /// <returns>The result of the creation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(PermissionId.Pilot_Create))]
        public async Task<IActionResult> Add([FromBody] PilotDto dto)
        {
            try
            {
                var createdDto = await this.pilotService.AddAsync(dto);
#if UseHubForClientInPilot
                await this.clientForHubService.SendTargetedMessage(createdDto.SiteId.ToString(), "pilots", "refresh-pilots");
#endif
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
        /// Update a pilot.
        /// </summary>
        /// <param name="id">The pilot identifier.</param>
        /// <param name="dto">The pilot DTO.</param>
        /// <returns>The result of the update.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(PermissionId.Pilot_Update))]
        public async Task<IActionResult> Update(Guid id, [FromBody] PilotDto dto)
        {
            if (id == Guid.Empty || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                var updatedDto = await this.pilotService.UpdateAsync(dto);
#if UseHubForClientInPilot
                await this.clientForHubService.SendTargetedMessage(updatedDto.SiteId.ToString(), "pilots", "refresh-pilots");
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
        /// Remove a pilot.
        /// </summary>
        /// <param name="id">The pilot identifier.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(PermissionId.Pilot_Delete))]
        public async Task<IActionResult> Remove(Guid id)
        {
            if (id == Guid.Empty)
            {
                return this.BadRequest();
            }

            try
            {
                var deletedDto = await this.pilotService.RemoveAsync(id);
#if UseHubForClientInPilot
                await this.clientForHubService.SendTargetedMessage(deletedDto.SiteId.ToString(), "pilots", "refresh-pilots");
#endif
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Removes the specified pilot ids.
        /// </summary>
        /// <param name="ids">The pilot ids.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(PermissionId.Pilot_Delete))]
        public async Task<IActionResult> Remove([FromQuery] List<Guid> ids)
        {
            if (ids?.Any() != true)
            {
                return this.BadRequest();
            }

            try
            {
                var deletedDtos = await this.pilotService.RemoveAsync(ids);
#if UseHubForClientInPilot
                deletedDtos.Select(m => m.SiteId).Distinct().ToList().ForEach(async parentId =>
                {
                    await this.clientForHubService.SendTargetedMessage(parentId.ToString(), "pilots", "refresh-pilots");
                });
#endif
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Save all pilots according to their state (added, updated or removed).
        /// </summary>
        /// <param name="dtos">The list of pilots.</param>
        /// <returns>The status code.</returns>
        [HttpPost("save")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(PermissionId.Pilot_Save))]
        public async Task<IActionResult> Save(IEnumerable<PilotDto> dtos)
        {
            var dtoList = dtos.ToList();
            if (!dtoList.Any())
            {
                return this.BadRequest();
            }

            try
            {
                var savedDtos = await this.pilotService.SaveAsync(dtoList);
#if UseHubForClientInPilot
                savedDtos.Select(m => m.SiteId).Distinct().ToList().ForEach(async parentId =>
                {
                    await this.clientForHubService.SendTargetedMessage(parentId.ToString(), "pilots", "refresh-pilots");
                });
#endif
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
            catch (ForbiddenException)
            {
                return this.Forbid();
            }
        }

        /// <summary>
        /// Generates a csv file according to the filters.
        /// </summary>
        /// <param name="filters">filters ( <see cref="PagingFilterFormatDto"/>).</param>
        /// <returns>a csv file.</returns>
        [HttpPost("csv")]
        [Authorize(Roles = nameof(PermissionId.Pilot_List_Access))]
        public virtual async Task<IActionResult> GetFile([FromBody] PagingFilterFormatDto filters)
        {
            byte[] buffer = await this.pilotService.GetCsvAsync(filters);
            return this.File(buffer, BiaConstants.Csv.ContentType + $";charset={BiaConstants.Csv.CharsetEncoding}", $"Pilots{BiaConstants.Csv.Extension}");
        }

        /// <summary>
        /// Update the fixed status of an item by its id.
        /// </summary>
        /// <param name="id">ID of the item to update.</param>
        /// <param name="isFixed">Fixed status.</param>
        /// <returns>Updated item.</returns>
        [HttpPut("{id}/[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(PermissionId.Pilot_Fix))]
        public virtual async Task<IActionResult> Fix(Guid id, [FromBody] bool isFixed)
        {
            try
            {
                var dto = await this.pilotService.UpdateFixedAsync(id, isFixed);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }
    }
}