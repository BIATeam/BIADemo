// BIADemo only
// <copyright file="AircraftMaintenanceCompaniesController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
// #define UseHubForClientInAircraftMaintenanceCompany
namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Maintenance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Base;
#if UseHubForClientInAircraftMaintenanceCompany
    using BIA.Net.Core.Application.Services;
#endif
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Maintenance;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.Maintenance;

    /// <summary>
    /// The API controller used to manage AircraftMaintenanceCompanies.
    /// </summary>
#if !UseHubForClientInAircraftMaintenanceCompany
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "UseHubForClientInAircraftMaintenanceCompany not set")]
#endif
    public class AircraftMaintenanceCompaniesController : BiaControllerBase
    {
        /// <summary>
        /// The aircraftMaintenanceCompany application service.
        /// </summary>
        private readonly IAircraftMaintenanceCompanyAppService aircraftMaintenanceCompanyService;

#if UseHubForClientInAircraftMaintenanceCompany
        private readonly IClientForHubService clientForHubService;
#endif

#if UseHubForClientInAircraftMaintenanceCompany
        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftMaintenanceCompaniesController"/> class.
        /// </summary>
        /// <param name="aircraftMaintenanceCompanyService">The aircraftMaintenanceCompany application service.</param>
        /// <param name="clientForHubService">The hub for client.</param>
        public AircraftMaintenanceCompaniesController(
            IAircraftMaintenanceCompanyAppService aircraftMaintenanceCompanyService,
            IClientForHubService clientForHubService)
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftMaintenanceCompaniesController"/> class.
        /// </summary>
        /// <param name="aircraftMaintenanceCompanyService">The aircraftMaintenanceCompany application service.</param>
        public AircraftMaintenanceCompaniesController(IAircraftMaintenanceCompanyAppService aircraftMaintenanceCompanyService)
#endif
        {
#if UseHubForClientInAircraftMaintenanceCompany
            this.clientForHubService = clientForHubService;
#endif
            this.aircraftMaintenanceCompanyService = aircraftMaintenanceCompanyService;
        }

        /// <summary>
        /// Get all aircraftMaintenanceCompanies with filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of aircraftMaintenanceCompanies.</returns>
        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.AircraftMaintenanceCompanies.ListAccess)]
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            var (results, total) = await this.aircraftMaintenanceCompanyService.GetRangeAsync(filters);
            this.HttpContext.Response.Headers.Append(BiaConstants.HttpHeaders.TotalCount, total.ToString());
            return this.Ok(results);
        }

        /// <summary>
        /// Get a aircraftMaintenanceCompany by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The aircraftMaintenanceCompany.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.AircraftMaintenanceCompanies.Read)]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var dto = await this.aircraftMaintenanceCompanyService.GetAsync(id);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Add a aircraftMaintenanceCompany.
        /// </summary>
        /// <param name="dto">The aircraftMaintenanceCompany DTO.</param>
        /// <returns>The result of the creation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.AircraftMaintenanceCompanies.Create)]
        public async Task<IActionResult> Add([FromBody] AircraftMaintenanceCompanyDto dto)
        {
            try
            {
                var createdDto = await this.aircraftMaintenanceCompanyService.AddAsync(dto);
#if UseHubForClientInAircraftMaintenanceCompany
                await this.clientForHubService.SendTargetedMessage(createdDto.ParentTeamId.ToString(), "aircraftMaintenanceCompanies", "refresh-aircraftMaintenanceCompanies");
#endif
                return this.CreatedAtAction("Get", new { id = createdDto.Id }, createdDto);
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
        }

        /// <summary>
        /// Update a aircraftMaintenanceCompany.
        /// </summary>
        /// <param name="id">The aircraftMaintenanceCompany identifier.</param>
        /// <param name="dto">The aircraftMaintenanceCompany DTO.</param>
        /// <returns>The result of the update.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.AircraftMaintenanceCompanies.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] AircraftMaintenanceCompanyDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                var updatedDto = await this.aircraftMaintenanceCompanyService.UpdateAsync(dto);
#if UseHubForClientInAircraftMaintenanceCompany
                await this.clientForHubService.SendTargetedMessage(updatedDto.ParentTeamId.ToString(), "aircraftMaintenanceCompanies", "refresh-aircraftMaintenanceCompanies");
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
        /// Remove a aircraftMaintenanceCompany.
        /// </summary>
        /// <param name="id">The aircraftMaintenanceCompany identifier.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.AircraftMaintenanceCompanies.Delete)]
        public async Task<IActionResult> Remove(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var deletedDto = await this.aircraftMaintenanceCompanyService.RemoveAsync(id);
#if UseHubForClientInAircraftMaintenanceCompany
                await this.clientForHubService.SendTargetedMessage(deletedDto.ParentTeamId.ToString(), "aircraftMaintenanceCompanies", "refresh-aircraftMaintenanceCompanies");
#endif
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Removes the specified aircraftMaintenanceCompany ids.
        /// </summary>
        /// <param name="ids">The aircraftMaintenanceCompany ids.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.AircraftMaintenanceCompanies.Delete)]
        public async Task<IActionResult> Remove([FromQuery] List<int> ids)
        {
            if (ids?.Any() != true)
            {
                return this.BadRequest();
            }

            try
            {
                var deletedDtos = await this.aircraftMaintenanceCompanyService.RemoveAsync(ids);
#if UseHubForClientInAircraftMaintenanceCompany
                deletedDtos.Select(m => m.ParentTeamId).Distinct().ToList().ForEach(async parentId =>
                {
                    await this.clientForHubService.SendTargetedMessage(parentId.ToString(), "aircraftMaintenanceCompanies", "refresh-aircraftMaintenanceCompanies");
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
        /// Save all aircraftMaintenanceCompanies according to their state (added, updated or removed).
        /// </summary>
        /// <param name="dtos">The list of aircraftMaintenanceCompanies.</param>
        /// <returns>The status code.</returns>
        [HttpPost("save")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.AircraftMaintenanceCompanies.Save)]
        public async Task<IActionResult> Save(IEnumerable<AircraftMaintenanceCompanyDto> dtos)
        {
            var dtoList = dtos.ToList();
            if (!dtoList.Any())
            {
                return this.BadRequest();
            }

            try
            {
                var savedDtos = await this.aircraftMaintenanceCompanyService.SaveAsync(dtoList);
#if UseHubForClientInAircraftMaintenanceCompany
                savedDtos.Select(m => m.ParentTeamId).Distinct().ToList().ForEach(async parentId =>
                {
                    await this.clientForHubService.SendTargetedMessage(parentId.ToString(), "aircraftMaintenanceCompanies", "refresh-aircraftMaintenanceCompanies");
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
        [Authorize(Roles = Rights.AircraftMaintenanceCompanies.ListAccess)]
        public virtual async Task<IActionResult> GetFile([FromBody] PagingFilterFormatDto filters)
        {
            byte[] buffer = await this.aircraftMaintenanceCompanyService.GetCsvAsync(filters);
            return this.File(buffer, BiaConstants.Csv.ContentType + ";charset=utf-8", $"AircraftMaintenanceCompanies{BiaConstants.Csv.Extension}");
        }
    }
}