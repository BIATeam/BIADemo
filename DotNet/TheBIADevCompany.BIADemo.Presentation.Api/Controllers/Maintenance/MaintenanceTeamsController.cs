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
        /// The MaintenanceTeam application service.
        /// </summary>
        private readonly IMaintenanceTeamAppService maintenanceTeamAppService;

#if UseHubForClientInMaintenanceTeam
        private readonly IClientForHubService clientForHubService;
#endif

#if UseHubForClientInMaintenanceTeam
        /// <summary>
        /// Initializes a new instance of the <see cref="MaintenanceTeamsController"/> class.
        /// </summary>
        /// <param name="maintenanceTeamAppService">The MaintenanceTeam application service.</param>
        /// <param name="clientForHubService">The hub for client.</param>
        public MaintenanceTeamsController(
            IMaintenanceTeamAppService maintenanceTeamAppService, IClientForHubService clientForHubService)
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="MaintenanceTeamsController"/> class.
        /// </summary>
        /// <param name="maintenanceTeamAppService">The MaintenanceTeam application service.</param>
        public MaintenanceTeamsController(IMaintenanceTeamAppService maintenanceTeamAppService)
#endif
        {
#if UseHubForClientInMaintenanceTeam
            this.clientForHubService = clientForHubService;
#endif
            this.maintenanceTeamAppService = maintenanceTeamAppService;
        }

        /// <summary>
        /// Get all MaintenanceTeams with filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of MaintenanceTeams.</returns>
        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceTeams.ListAccess)]
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            var (results, total) = await this.maintenanceTeamAppService.GetRangeAsync(filters);
            this.HttpContext.Response.Headers.Append(BiaConstants.HttpHeaders.TotalCount, total.ToString());
            return this.Ok(results);
        }

        /// <summary>
        /// Get a MaintenanceTeam by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The MaintenanceTeam.</returns>
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
                var dto = await this.maintenanceTeamAppService.GetAsync(id);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Add a MaintenanceTeam.
        /// </summary>
        /// <param name="dto">The MaintenanceTeam DTO.</param>
        /// <returns>The result of the creation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceTeams.Create)]
        public async Task<IActionResult> Add([FromBody] MaintenanceTeamDto dto)
        {
            try
            {
                var createdDto = await this.maintenanceTeamAppService.AddAsync(dto);
#if UseHubForClientInMaintenanceTeam
                await this.clientForHubService.SendTargetedMessage(createdDto.AircraftMaintenanceCompanyId.ToString(), "MaintenanceTeams", "refresh-MaintenanceTeams");
#endif
                return this.CreatedAtAction("Get", new { id = createdDto.Id }, createdDto);
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
        }

        /// <summary>
        /// Update a MaintenanceTeam.
        /// </summary>
        /// <param name="id">The MaintenanceTeam identifier.</param>
        /// <param name="dto">The MaintenanceTeam DTO.</param>
        /// <returns>The result of the update.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
                var updatedDto = await this.maintenanceTeamAppService.UpdateAsync(dto);
#if UseHubForClientInMaintenanceTeam
                _ = this.clientForHubService.SendTargetedMessage(updatedDto.AircraftMaintenanceCompanyId.ToString(), "MaintenanceTeams", "refresh-MaintenanceTeams");
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
        }

        /// <summary>
        /// Remove a MaintenanceTeam.
        /// </summary>
        /// <param name="id">The MaintenanceTeam identifier.</param>
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
                var deletedDto = await this.maintenanceTeamAppService.RemoveAsync(id);
#if UseHubForClientInMaintenanceTeam
                _ = this.clientForHubService.SendTargetedMessage(deletedDto.AircraftMaintenanceCompanyId.ToString(), "MaintenanceTeams", "refresh-MaintenanceTeams");
#endif
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Removes the specified MaintenanceTeam ids.
        /// </summary>
        /// <param name="ids">The MaintenanceTeam ids.</param>
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
                var deletedDtos = await this.maintenanceTeamAppService.RemoveAsync(ids);

#if UseHubForClientInMaintenanceTeam
                deletedDtos.Select(m => m.AircraftMaintenanceCompanyId).Distinct().ToList().ForEach(parentId =>
                {
                    _ = this.clientForHubService.SendTargetedMessage(parentId.ToString(), "MaintenanceTeams", "refresh-MaintenanceTeams");
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
        /// Save all MaintenanceTeams according to their state (added, updated or removed).
        /// </summary>
        /// <param name="dtos">The list of MaintenanceTeams.</param>
        /// <returns>The status code.</returns>
        [HttpPost("save")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                var savedDtos = await this.maintenanceTeamAppService.SaveAsync(dtoList);
#if UseHubForClientInMaintenanceTeam
                savedDtos.Select(m => m.AircraftMaintenanceCompanyId).Distinct().ToList().ForEach(parentId =>
                {
                    _ = this.clientForHubService.SendTargetedMessage(parentId.ToString(), "MaintenanceTeams", "refresh-MaintenanceTeams");
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
        public virtual async Task<IActionResult> GetFile([FromBody] PagingFilterFormatDto filters)
        {
            byte[] buffer = await this.maintenanceTeamAppService.GetCsvAsync(filters);
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
                var dto = await this.maintenanceTeamAppService.UpdateFixedAsync(id, isFixed);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }
    }
}