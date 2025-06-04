// BIADemo only
// <copyright file="MaintenanceTeamsController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
// #define UseHubForClientInMaintenanceTeam
namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Maintenance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
#if UseHubForClientInMaintenanceTeam
    using BIA.Net.Core.Application.Services;
#endif
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Maintenance;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.Maintenance;

    /// <summary>
    /// The API controller used to manage MaintenanceTeams.
    /// </summary>
#if !UseHubForClientInMaintenanceTeam
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "UseHubForClientInMaintenanceTeam not set")]
#endif
    public class MaintenanceTeamsController : BiaControllerBase
    {
        /// <summary>
        /// The maintenanceTeam application service.
        /// </summary>
        private readonly IMaintenanceTeamAppService maintenanceTeamService;

#if UseHubForClientInMaintenanceTeam
        private readonly IClientForHubService clientForHubService;
#endif

#if UseHubForClientInMaintenanceTeam
        /// <summary>
        /// Initializes a new instance of the <see cref="MaintenanceTeamsController"/> class.
        /// </summary>
        /// <param name="maintenanceTeamService">The maintenanceTeam application service.</param>
        /// <param name="clientForHubService">The hub for client.</param>
        public MaintenanceTeamsController(
            IMaintenanceTeamAppService maintenanceTeamService,
            IClientForHubService clientForHubService)
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="MaintenanceTeamsController"/> class.
        /// </summary>
        /// <param name="maintenanceTeamService">The maintenanceTeam application service.</param>
        public MaintenanceTeamsController(IMaintenanceTeamAppService maintenanceTeamService)
#endif
        {
#if UseHubForClientInMaintenanceTeam
            this.clientForHubService = clientForHubService;
#endif
            this.maintenanceTeamService = maintenanceTeamService;
        }

        /// <summary>
        /// Get all maintenanceTeams with filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of maintenanceTeams.</returns>
        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceTeams.ListAccess)]
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            var (results, total) = await this.maintenanceTeamService.GetRangeAsync(filters);
            this.HttpContext.Response.Headers.Append(BiaConstants.HttpHeaders.TotalCount, total.ToString());
            return this.Ok(results);
        }

        /// <summary>
        /// Get a maintenanceTeam by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The maintenanceTeam.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceTeams.Read)]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var dto = await this.maintenanceTeamService.GetAsync(id);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Add a maintenanceTeam.
        /// </summary>
        /// <param name="dto">The maintenanceTeam DTO.</param>
        /// <returns>The result of the creation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceTeams.Create)]
        public async Task<IActionResult> Add([FromBody] MaintenanceTeamDto dto)
        {
            try
            {
                var createdDto = await this.maintenanceTeamService.AddAsync(dto);
#if UseHubForClientInMaintenanceTeam
                await this.clientForHubService.SendTargetedMessage(createdDto.AircraftMaintenanceCompanyId.ToString(), "maintenanceTeams", "refresh-maintenanceTeams");
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
        /// Update a maintenanceTeam.
        /// </summary>
        /// <param name="id">The maintenanceTeam identifier.</param>
        /// <param name="dto">The maintenanceTeam DTO.</param>
        /// <returns>The result of the update.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceTeams.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] MaintenanceTeamDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                var updatedDto = await this.maintenanceTeamService.UpdateAsync(dto);
#if UseHubForClientInMaintenanceTeam
                await this.clientForHubService.SendTargetedMessage(updatedDto.AircraftMaintenanceCompanyId.ToString(), "maintenanceTeams", "refresh-maintenanceTeams");
#endif
                return this.Ok(updatedDto);
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
            catch (ForbiddenException ex)
            {
                return this.Problem(
                        type: "/docs/errors/forbidden",
                        title: "User is not authorized to make this action.",
                        detail: ex.Message,
                        statusCode: StatusCodes.Status403Forbidden);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
            catch (OutdateException)
            {
                return this.Conflict();
            }
        }

        /// <summary>
        /// Remove a maintenanceTeam.
        /// </summary>
        /// <param name="id">The maintenanceTeam identifier.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceTeams.Delete)]
        public async Task<IActionResult> Remove(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var deletedDto = await this.maintenanceTeamService.RemoveAsync(id);
#if UseHubForClientInMaintenanceTeam
                await this.clientForHubService.SendTargetedMessage(deletedDto.AircraftMaintenanceCompanyId.ToString(), "maintenanceTeams", "refresh-maintenanceTeams");
#endif
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Removes the specified maintenanceTeam ids.
        /// </summary>
        /// <param name="ids">The maintenanceTeam ids.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceTeams.Delete)]
        public async Task<IActionResult> Remove([FromQuery] List<int> ids)
        {
            if (ids?.Any() != true)
            {
                return this.BadRequest();
            }

            try
            {
                var deletedDtos = await this.maintenanceTeamService.RemoveAsync(ids);
#if UseHubForClientInMaintenanceTeam
                deletedDtos.Select(m => m.AircraftMaintenanceCompanyId).Distinct().ToList().ForEach(async parentId =>
                {
                    await this.clientForHubService.SendTargetedMessage(parentId.ToString(), "maintenanceTeams", "refresh-maintenanceTeams");
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
        /// Save all maintenanceTeams according to their state (added, updated or removed).
        /// </summary>
        /// <param name="dtos">The list of maintenanceTeams.</param>
        /// <returns>The status code.</returns>
        [HttpPost("save")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceTeams.Save)]
        public async Task<IActionResult> Save(IEnumerable<MaintenanceTeamDto> dtos)
        {
            var dtoList = dtos.ToList();
            if (!dtoList.Any())
            {
                return this.BadRequest();
            }

            try
            {
                var savedDtos = await this.maintenanceTeamService.SaveAsync(dtoList);
#if UseHubForClientInMaintenanceTeam
                savedDtos.Select(m => m.AircraftMaintenanceCompanyId).Distinct().ToList().ForEach(async parentId =>
                {
                    await this.clientForHubService.SendTargetedMessage(parentId.ToString(), "maintenanceTeams", "refresh-maintenanceTeams");
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
        }

        /// <summary>
        /// Generates a csv file according to the filters.
        /// </summary>
        /// <param name="filters">filters ( <see cref="PagingFilterFormatDto"/>).</param>
        /// <returns>a csv file.</returns>
        [HttpPost("csv")]
        [Authorize(Roles = Rights.MaintenanceTeams.ListAccess)]
        public virtual async Task<IActionResult> GetFile([FromBody] PagingFilterFormatDto filters)
        {
            byte[] buffer = await this.maintenanceTeamService.GetCsvAsync(filters);
            return this.File(buffer, BiaConstants.Csv.ContentType + ";charset=utf-8", $"MaintenanceTeams{BiaConstants.Csv.Extension}");
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
        [Authorize(Roles = Rights.MaintenanceTeams.Fix)]
        public virtual async Task<IActionResult> Fix(int id, [FromBody] bool isFixed)
        {
            try
            {
                var dto = await this.maintenanceTeamService.UpdateFixedAsync(id, isFixed);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }
    }
}