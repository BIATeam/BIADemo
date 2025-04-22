// BIADemo only
// <copyright file="EnginesController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
#define UseHubForClientInEngine
namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Plane
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
#if UseHubForClientInEngine
    using BIA.Net.Core.Application.Services;
#endif
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Plane;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;

    /// <summary>
    /// The API controller used to manage Engines.
    /// </summary>
#if !UseHubForClientInEngine
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "UseHubForClientInEngine not set")]
#endif
    public class EnginesController : BiaControllerBase
    {
        /// <summary>
        /// The engine application service.
        /// </summary>
        private readonly IEngineAppService engineService;

#if UseHubForClientInEngine
        private readonly IClientForHubService clientForHubService;
#endif

#if UseHubForClientInEngine
        /// <summary>
        /// Initializes a new instance of the <see cref="EnginesController"/> class.
        /// </summary>
        /// <param name="engineService">The engine application service.</param>
        /// <param name="clientForHubService">The hub for client.</param>
        public EnginesController(
            IEngineAppService engineService, IClientForHubService clientForHubService)
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="EnginesController"/> class.
        /// </summary>
        /// <param name="engineService">The engine application service.</param>
        public EnginesController(IEngineAppService engineService)
#endif
        {
#if UseHubForClientInEngine
            this.clientForHubService = clientForHubService;
#endif
            this.engineService = engineService;
        }

        /// <summary>
        /// Get all engines with filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of engines.</returns>
        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Engines.ListAccess)]
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            var (results, total) = await this.engineService.GetRangeAsync(filters);
            this.HttpContext.Response.Headers.Append(BiaConstants.HttpHeaders.TotalCount, total.ToString());
            return this.Ok(results);
        }

        /// <summary>
        /// Get a engine by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The engine.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Engines.Read)]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var dto = await this.engineService.GetAsync(id);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Add a engine.
        /// </summary>
        /// <param name="dto">The engine DTO.</param>
        /// <returns>The result of the creation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Engines.Create)]
        public async Task<IActionResult> Add([FromBody] EngineDto dto)
        {
            try
            {
                var createdDto = await this.engineService.AddAsync(dto);
#if UseHubForClientInEngine
                _ = this.clientForHubService.SendTargetedMessage(createdDto.PlaneId.ToString(), "engines", "refresh-engines");
#endif
                return this.CreatedAtAction("Get", new { id = createdDto.Id }, createdDto);
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
        }

        /// <summary>
        /// Update a engine.
        /// </summary>
        /// <param name="id">The engine identifier.</param>
        /// <param name="dto">The engine DTO.</param>
        /// <returns>The result of the update.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Engines.Update)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] EngineDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                var updatedDto = await this.engineService.UpdateAsync(dto);
#if UseHubForClientInEngine
                _ = this.clientForHubService.SendTargetedMessage(updatedDto.PlaneId.ToString(), "engines", "refresh-engines");
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
        /// Remove a engine.
        /// </summary>
        /// <param name="id">The engine identifier.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Engines.Delete)]
        public async Task<IActionResult> Remove(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var deletedDto = await this.engineService.RemoveAsync(id);
#if UseHubForClientInEngine
                _ = this.clientForHubService.SendTargetedMessage(deletedDto.PlaneId.ToString(), "engines", "refresh-engines");
#endif
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Removes the specified engine ids.
        /// </summary>
        /// <param name="ids">The engine ids.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Engines.Delete)]
        public async Task<IActionResult> Remove([FromQuery] List<int> ids)
        {
            if (ids?.Any() != true)
            {
                return this.BadRequest();
            }

            try
            {
                var deletedDtos = await this.engineService.RemoveAsync(ids);

#if UseHubForClientInEngine
                deletedDtos.Select(m => m.PlaneId).Distinct().ToList().ForEach(parentId =>
                {
                    _ = this.clientForHubService.SendTargetedMessage(parentId.ToString(), "engines", "refresh-engines");
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
        /// Save all engines according to their state (added, updated or removed).
        /// </summary>
        /// <param name="dtos">The list of engines.</param>
        /// <returns>The status code.</returns>
        [HttpPost("save")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Engines.Save)]
        public async Task<IActionResult> Save(IEnumerable<EngineDto> dtos)
        {
            var dtoList = dtos.ToList();
            if (!dtoList.Any())
            {
                return this.BadRequest();
            }

            try
            {
                var savedDtos = await this.engineService.SaveAsync(dtoList);
#if UseHubForClientInEngine
                savedDtos.Select(m => m.PlaneId).Distinct().ToList().ForEach(parentId =>
                {
                    _ = this.clientForHubService.SendTargetedMessage(parentId.ToString(), "engines", "refresh-engines");
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
        [Authorize(Roles = Rights.Engines.ListAccess)]
        public virtual async Task<IActionResult> GetFile([FromBody] PagingFilterFormatDto filters)
        {
            byte[] buffer = await this.engineService.GetCsvAsync(filters);
            return this.File(buffer, BiaConstants.Csv.ContentType + ";charset=utf-8", $"Engines{BiaConstants.Csv.Extension}");
        }

        // Begin BIADemo

        /// <summary>
        /// Launches the job manually example.
        /// </summary>
        /// <returns>The status code.</returns>
        [HttpPost("LaunchJobManually")]
        public IActionResult LaunchJobManuallyExample()
        {
            this.engineService.LaunchJobManuallyExample();
            return this.Ok();
        }

        // End BIADemo
    }
}