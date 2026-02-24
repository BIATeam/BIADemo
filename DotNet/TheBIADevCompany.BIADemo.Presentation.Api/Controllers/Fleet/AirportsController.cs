// BIADemo only
// <copyright file="AirportsController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
#define UseHubForClientInAirport
namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Fleet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
#if UseHubForClientInAirport
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
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.Fleet;

    /// <summary>
    /// The API controller used to manage Airports.
    /// </summary>
#if !UseHubForClientInAirport
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "UseHubForClientInAirport not set")]
#endif
    public class AirportsController : BiaControllerBase
    {
        /// <summary>
        /// The airport application service.
        /// </summary>
        private readonly IAirportAppService airportService;

#if UseHubForClientInAirport
        private readonly IClientForHubService clientForHubService;
#endif

#if UseHubForClientInAirport
        /// <summary>
        /// Initializes a new instance of the <see cref="AirportsController"/> class.
        /// </summary>
        /// <param name="airportService">The airport application service.</param>
        /// <param name="clientForHubService">The hub for client.</param>
        public AirportsController(
            IAirportAppService airportService,
            IClientForHubService clientForHubService)
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="AirportsController"/> class.
        /// </summary>
        /// <param name="airportService">The airport application service.</param>
        public AirportsController(IAirportAppService airportService)
#endif
        {
#if UseHubForClientInAirport
            this.clientForHubService = clientForHubService;
            this.EntityName = "airport";
            this.EntityNamePlural = "airports";
#endif
            this.airportService = airportService;
        }

        /// <summary>
        /// Get all airports with filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of airports.</returns>
        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(PermissionId.Airport_List_Access))]
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            var (results, total) = await this.airportService.GetRangeAsync(filters);
            this.HttpContext.Response.Headers.Append(BiaConstants.HttpHeaders.TotalCount, total.ToString());
            return this.Ok(results);
        }

        /// <summary>
        /// Get a airport by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The airport.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(PermissionId.Airport_Read))]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var dto = await this.airportService.GetAsync(id);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Add a airport.
        /// </summary>
        /// <param name="dto">The airport DTO.</param>
        /// <returns>The result of the creation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(PermissionId.Airport_Create))]
        public async Task<IActionResult> Add([FromBody] AirportDto dto)
        {
            try
            {
                var createdDto = await this.airportService.AddAsync(dto);
#if UseHubForClientInAirport
                await this.SendEntityChangedAsync();
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
        /// Update a airport.
        /// </summary>
        /// <param name="id">The airport identifier.</param>
        /// <param name="dto">The airport DTO.</param>
        /// <returns>The result of the update.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(PermissionId.Airport_Update))]
        public async Task<IActionResult> Update(int id, [FromBody] AirportDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                var updatedDto = await this.airportService.UpdateAsync(dto);
#if UseHubForClientInAirport
                await this.clientForHubService.SendTargetedMessage(string.Empty, this.EntityNamePlural, "update-" + this.EntityName, updatedDto);
                await this.SendEntityChangedAsync();
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
        /// Remove a airport.
        /// </summary>
        /// <param name="id">The airport identifier.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(PermissionId.Airport_Delete))]
        public async Task<IActionResult> Remove(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                await this.airportService.RemoveAsync(id);
#if UseHubForClientInAirport
                await this.SendEntityChangedAsync();
#endif
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Removes the specified airport ids.
        /// </summary>
        /// <param name="ids">The airport ids.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(PermissionId.Airport_Delete))]
        public async Task<IActionResult> Remove([FromQuery] List<int> ids)
        {
            if (ids?.Any() != true)
            {
                return this.BadRequest();
            }

            try
            {
                await this.airportService.RemoveAsync(ids);
#if UseHubForClientInAirport
                await this.SendEntityChangedAsync();
#endif
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Save all airports according to their state (added, updated or removed).
        /// </summary>
        /// <param name="dtos">The list of airports.</param>
        /// <returns>The status code.</returns>
        [HttpPost("save")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(PermissionId.Airport_Save))]
        public async Task<IActionResult> Save(IEnumerable<AirportDto> dtos)
        {
            var dtoList = dtos.ToList();
            if (!dtoList.Any())
            {
                return this.BadRequest();
            }

            try
            {
                await this.airportService.SaveAsync(dtoList);
#if UseHubForClientInAirport
                await this.SendEntityChangedAsync();
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
        [Authorize(Roles = nameof(PermissionId.Airport_List_Access))]
        public virtual async Task<IActionResult> GetFile([FromBody] PagingFilterFormatDto filters)
        {
            byte[] buffer = await this.airportService.GetCsvAsync(filters);
            return this.File(buffer, BiaConstants.Csv.ContentType + $";charset={BiaConstants.Csv.CharsetEncoding}", $"Airports{BiaConstants.Csv.Extension}");
        }

#if UseHubForClientInAirport
        /// <summary>
        /// Notifies clients that entity have changed.
        /// </summary>
        /// <param name="parentKey">The parent key.</param>
        /// <param name="parentKeys">The parent keys.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        private async Task SendEntityChangedAsync(string parentKey = null, List<string> parentKeys = null)
        {
            await this.SendEntityChangedAsync(
                clientForHubService: this.clientForHubService,
                parentKey: parentKey,
                parentKeys: parentKeys);
        }
#endif
    }
}