// BIADemo only
// <copyright file="AircraftMaintenanceCompaniesController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
// #define UseHubForClientInAircraftMaintenanceCompany
namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.AircraftMaintenanceCompany
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Base;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompanyModule.Aggregate;
#if UseHubForClientInAircraftMaintenanceCompany
    using BIA.Net.Core.Domain.RepoContract;
#endif
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
#if UseHubForClientInAircraftMaintenanceCompany
    using Microsoft.AspNetCore.SignalR;
#endif
    using TheBIADevCompany.BIADemo.Application.AircraftMaintenanceCompany;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The API controller used to manage AircraftMaintenanceCompanies.
    /// </summary>
#if !UseHubForClientInAircraftMaintenanceCompany
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "UseHubForClientInAircraftMaintenanceCompany not set")]
#endif
    public class AircraftMaintenanceCompaniesController : BiaControllerBase
    {
        /// <summary>
        /// The AircraftMaintenanceCompany application service.
        /// </summary>
        private readonly IAircraftMaintenanceCompanyAppService aircraftMaintenanceCompaniesService;

#if UseHubForClientInAircraftMaintenanceCompany
        private readonly IClientForHubRepository clientForHubService;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftMaintenanceCompaniesController"/> class.
        /// </summary>
        /// <param name="aircraftMaintenanceCompaniesService">The AircraftMaintenanceCompany application service.</param>
        /// <param name="clientForHubService">The hub for client.</param>
#if UseHubForClientInAircraftMaintenanceCompany
        public AircraftMaintenanceCompaniesController(
            IAircraftMaintenanceCompanyAppService aircraftMaintenanceCompaniesService, IClientForHubRepository clientForHubService)
#else
        public AircraftMaintenanceCompaniesController(IAircraftMaintenanceCompanyAppService aircraftMaintenanceCompaniesService)
#endif
        {
#if UseHubForClientInAircraftMaintenanceCompany
            this.clientForHubService = clientForHubService;
#endif
            this.aircraftMaintenanceCompaniesService = aircraftMaintenanceCompaniesService;
        }

        /// <summary>
        /// Get all AircraftMaintenanceCompanies with filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of AircraftMaintenanceCompanies.</returns>
        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.AircraftMaintenanceCompanies.ListAccess)]
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            var (results, total) = await this.aircraftMaintenanceCompaniesService.GetRangeAsync(filters, specification: TeamAdvancedFilterSpecification<AircraftMaintenanceCompany>.Filter(filters));
            this.HttpContext.Response.Headers.Append(BiaConstants.HttpHeaders.TotalCount, total.ToString());
            return this.Ok(results);
        }

        /// <summary>
        /// Get a AircraftMaintenanceCompany by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The AircraftMaintenanceCompany.</returns>
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
                var dto = await this.aircraftMaintenanceCompaniesService.GetAsync(id);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Add a AircraftMaintenanceCompany.
        /// </summary>
        /// <param name="dto">The AircraftMaintenanceCompany DTO.</param>
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
                var createdDto = await this.aircraftMaintenanceCompaniesService.AddAsync(dto);
#if UseHubForClientInAircraftMaintenanceCompany
                await this.clientForHubService.SendTargetedMessage(createdDto.SiteId.ToString(), "AircraftMaintenanceCompanies", "refresh-AircraftMaintenanceCompanies");
#endif
                return this.CreatedAtAction("Get", new { id = createdDto.Id }, createdDto);
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
        }

        /// <summary>
        /// Update a AircraftMaintenanceCompany.
        /// </summary>
        /// <param name="id">The AircraftMaintenanceCompany identifier.</param>
        /// <param name="dto">The AircraftMaintenanceCompany DTO.</param>
        /// <returns>The result of the update.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                var updatedDto = await this.aircraftMaintenanceCompaniesService.UpdateAsync(dto);
#if UseHubForClientInAircraftMaintenanceCompany
                _ = this.clientForHubService.SendTargetedMessage(updatedDto.SiteId.ToString(), "AircraftMaintenanceCompanies", "refresh-AircraftMaintenanceCompanies");
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
        }

        /// <summary>
        /// Remove a AircraftMaintenanceCompany.
        /// </summary>
        /// <param name="id">The AircraftMaintenanceCompany identifier.</param>
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
                var deletedDto = await this.aircraftMaintenanceCompaniesService.RemoveAsync(id);
#if UseHubForClientInAircraftMaintenanceCompany
                _ = this.clientForHubService.SendTargetedMessage(deletedDto.SiteId.ToString(), "AircraftMaintenanceCompanies", "refresh-AircraftMaintenanceCompanies");
#endif
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Removes the specified AircraftMaintenanceCompany ids.
        /// </summary>
        /// <param name="ids">The AircraftMaintenanceCompany ids.</param>
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
                var deletedDtos = await this.aircraftMaintenanceCompaniesService.RemoveAsync(ids);

#if UseHubForClientInAircraftMaintenanceCompany
                deletedDtos.Select(m => m.SiteId).Distinct().ToList().ForEach(parentId =>
                {
                    _ = this.clientForHubService.SendTargetedMessage(parentId.ToString(), "AircraftMaintenanceCompanies", "refresh-AircraftMaintenanceCompanies");
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
        /// Save all AircraftMaintenanceCompanies according to their state (added, updated or removed).
        /// </summary>
        /// <param name="dtos">The list of AircraftMaintenanceCompanies.</param>
        /// <returns>The status code.</returns>
        [HttpPost("save")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                var savedDtos = await this.aircraftMaintenanceCompaniesService.SaveAsync(dtoList);
#if UseHubForClientInAircraftMaintenanceCompany
                savedDtos.Select(m => m.SiteId).Distinct().ToList().ForEach(parentId =>
                {
                    _ = this.clientForHubService.SendTargetedMessage(parentId.ToString(), "AircraftMaintenanceCompanies", "refresh-AircraftMaintenanceCompanies");
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
            byte[] buffer = await this.aircraftMaintenanceCompaniesService.GetCsvAsync(filters);
            return this.File(buffer, BiaConstants.Csv.ContentType + ";charset=utf-8", $"AircraftMaintenanceCompanies{BiaConstants.Csv.Extension}");
        }
    }
}