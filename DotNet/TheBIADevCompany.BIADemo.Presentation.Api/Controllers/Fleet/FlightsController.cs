// BIADemo only
// <copyright file="FlightsController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
// #define UseHubForClientInFlight
namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Fleet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
#if UseHubForClientInFlight
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
    using TheBIADevCompany.BIADemo.Domain.Dto.Fleet;

    /// <summary>
    /// The API controller used to manage Flights.
    /// </summary>
#if !UseHubForClientInFlight
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "UseHubForClientInFlight not set")]
#endif
    public class FlightsController : BiaControllerBase
    {
        /// <summary>
        /// The flight application service.
        /// </summary>
        private readonly IFlightAppService flightService;

#if UseHubForClientInFlight
        private readonly IClientForHubService clientForHubService;
#endif

#if UseHubForClientInFlight
        /// <summary>
        /// Initializes a new instance of the <see cref="FlightsController"/> class.
        /// </summary>
        /// <param name="flightService">The flight application service.</param>
        /// <param name="clientForHubService">The hub for client.</param>
        public FlightsController(
            IFlightAppService flightService,
            IClientForHubService clientForHubService)
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="FlightsController"/> class.
        /// </summary>
        /// <param name="flightService">The flight application service.</param>
        public FlightsController(IFlightAppService flightService)
#endif
        {
#if UseHubForClientInFlight
            this.clientForHubService = clientForHubService;
#endif
            this.flightService = flightService;
        }

        /// <summary>
        /// Get all flights with filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of flights.</returns>
        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Flights.ListAccess)]
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            var (results, total) = await this.flightService.GetRangeAsync(filters);
            this.HttpContext.Response.Headers.Append(BiaConstants.HttpHeaders.TotalCount, total.ToString());
            return this.Ok(results);
        }

        /// <summary>
        /// Get a flight by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The flight.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Flights.Read)]
        public async Task<IActionResult> Get(string id)
        {
            if (id == string.Empty)
            {
                return this.BadRequest();
            }

            try
            {
                var dto = await this.flightService.GetAsync(id);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Add a flight.
        /// </summary>
        /// <param name="dto">The flight DTO.</param>
        /// <returns>The result of the creation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Flights.Create)]
        public async Task<IActionResult> Add([FromBody] FlightDto dto)
        {
            try
            {
                var createdDto = await this.flightService.AddAsync(dto);
#if UseHubForClientInFlight
                await this.clientForHubService.SendTargetedMessage(createdDto.SiteId.ToString(), "flights", "refresh-flights");
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
        /// Update a flight.
        /// </summary>
        /// <param name="id">The flight identifier.</param>
        /// <param name="dto">The flight DTO.</param>
        /// <returns>The result of the update.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Flights.Update)]
        public async Task<IActionResult> Update(string id, [FromBody] FlightDto dto)
        {
            if (id == string.Empty || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                var updatedDto = await this.flightService.UpdateAsync(dto);
#if UseHubForClientInFlight
                await this.clientForHubService.SendTargetedMessage(updatedDto.SiteId.ToString(), "flights", "refresh-flights");
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
        /// Remove a flight.
        /// </summary>
        /// <param name="id">The flight identifier.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Flights.Delete)]
        public async Task<IActionResult> Remove(string id)
        {
            if (id == string.Empty)
            {
                return this.BadRequest();
            }

            try
            {
                var deletedDto = await this.flightService.RemoveAsync(id);
#if UseHubForClientInFlight
                await this.clientForHubService.SendTargetedMessage(deletedDto.SiteId.ToString(), "flights", "refresh-flights");
#endif
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Removes the specified flight ids.
        /// </summary>
        /// <param name="ids">The flight ids.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Flights.Delete)]
        public async Task<IActionResult> Remove([FromQuery] List<string> ids)
        {
            if (ids?.Any() != true)
            {
                return this.BadRequest();
            }

            try
            {
                var deletedDtos = await this.flightService.RemoveAsync(ids);
#if UseHubForClientInFlight
                deletedDtos.Select(m => m.SiteId).Distinct().ToList().ForEach(async parentId =>
                {
                    await this.clientForHubService.SendTargetedMessage(parentId.ToString(), "flights", "refresh-flights");
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
        /// Save all flights according to their state (added, updated or removed).
        /// </summary>
        /// <param name="dtos">The list of flights.</param>
        /// <returns>The status code.</returns>
        [HttpPost("save")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Flights.Save)]
        public async Task<IActionResult> Save(IEnumerable<FlightDto> dtos)
        {
            var dtoList = dtos.ToList();
            if (!dtoList.Any())
            {
                return this.BadRequest();
            }

            try
            {
                var savedDtos = await this.flightService.SaveAsync(dtoList);
#if UseHubForClientInFlight
                savedDtos.Select(m => m.SiteId).Distinct().ToList().ForEach(async parentId =>
                {
                    await this.clientForHubService.SendTargetedMessage(parentId.ToString(), "flights", "refresh-flights");
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
        [Authorize(Roles = Rights.Flights.ListAccess)]
        public virtual async Task<IActionResult> GetFile([FromBody] PagingFilterFormatDto filters)
        {
            byte[] buffer = await this.flightService.GetCsvAsync(filters);
            return this.File(buffer, BiaConstants.Csv.ContentType + ";charset=utf-8", $"Flights{BiaConstants.Csv.Extension}");
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
        [Authorize(Roles = Rights.Flights.Fix)]
        public virtual async Task<IActionResult> Fix(string id, [FromBody] bool isFixed)
        {
            try
            {
                var dto = await this.flightService.UpdateFixedAsync(id, isFixed);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }
    }
}