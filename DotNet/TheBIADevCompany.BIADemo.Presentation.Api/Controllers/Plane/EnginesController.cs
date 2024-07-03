// BIADemo only
// <copyright file="EnginesController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
#define UseHubForClientInEngine
namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Plane
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
#if UseHubForClientInEngine
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Specification;
#endif
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Hangfire;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
#if UseHubForClientInEngine
    using Microsoft.AspNetCore.SignalR;
#endif
    using TheBIADevCompany.BIADemo.Application.Plane;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;

    /// <summary>
    /// The API controller used to manage Engines.
    /// </summary>
#if !UseHubForClientInEngine
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "UseHubForClientInEngine not set")]
#endif
    public class EnginesController : BiaControllerBase
    {
        /// <summary>
        /// The plane application service.
        /// </summary>
        private readonly IEngineAppService planeService;

#if UseHubForClientInEngine
        private readonly IClientForHubRepository clientForHubService;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="EnginesController"/> class.
        /// </summary>
        /// <param name="planeService">The plane application service.</param>
        /// <param name="clientForHubService">The hub for client.</param>
        /// <param name="principal">The BIAClaimsPrincipal.</param>
#if UseHubForClientInEngine
        public EnginesController(
            IEngineAppService planeService, IClientForHubRepository clientForHubService)
#else
        public EnginesController(IEngineAppService planeService)
#endif
        {
#if UseHubForClientInEngine
            this.clientForHubService = clientForHubService;
#endif
            this.planeService = planeService;
        }

        /// <summary>
        /// Get all planes with filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of planes.</returns>
        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Engines.ListAccess)]
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            var (results, total) = await this.planeService.GetRangeAsync(filters, specification: EngineSpecification.SearchGetAll(filters));
            this.HttpContext.Response.Headers.Append(BIAConstants.HttpHeaders.TotalCount, total.ToString());
            return this.Ok(results);
        }

        /// <summary>
        /// Get a plane by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The plane.</returns>
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
                var dto = await this.planeService.GetAsync(id);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Add a plane.
        /// </summary>
        /// <param name="dto">The plane DTO.</param>
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
                var createdDto = await this.planeService.AddAsync(dto);
#if UseHubForClientInEngine
                _ = this.clientForHubService.SendTargetedMessage(createdDto.PlaneId.ToString(), "planes", "refresh-planes");
#endif
                return this.CreatedAtAction("Get", new { id = createdDto.Id }, createdDto);
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
        }

        /// <summary>
        /// Update a plane.
        /// </summary>
        /// <param name="id">The plane identifier.</param>
        /// <param name="dto">The plane DTO.</param>
        /// <returns>The result of the update.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Engines.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] EngineDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                var updatedDto = await this.planeService.UpdateAsync(dto);
#if UseHubForClientInEngine
                _ = this.clientForHubService.SendTargetedMessage(updatedDto.PlaneId.ToString(), "planes", "refresh-planes");
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
        /// Remove a plane.
        /// </summary>
        /// <param name="id">The plane identifier.</param>
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
                var deletedDto = await this.planeService.RemoveAsync(id);
#if UseHubForClientInEngine
                _ = this.clientForHubService.SendTargetedMessage(deletedDto.PlaneId.ToString(), "planes", "refresh-planes");
#endif
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Removes the specified plane ids.
        /// </summary>
        /// <param name="ids">The plane ids.</param>
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
                var deletedDtos = await this.planeService.RemoveAsync(ids);

#if UseHubForClientInEngine
                deletedDtos.Select(m => m.PlaneId).Distinct().ToList().ForEach(parentId =>
                {
                    _ = this.clientForHubService.SendTargetedMessage(parentId.ToString(), "planes", "refresh-planes");
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
        /// Save all planes according to their state (added, updated or removed).
        /// </summary>
        /// <param name="dtos">The list of planes.</param>
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
                var savedDtos = await this.planeService.SaveAsync(dtoList);
#if UseHubForClientInEngine
                savedDtos.Select(m => m.PlaneId).Distinct().ToList().ForEach(parentId =>
                {
                    _ = this.clientForHubService.SendTargetedMessage(parentId.ToString(), "planes", "refresh-planes");
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
            byte[] buffer = await this.planeService.GetCsvAsync(filters, specification: EngineSpecification.SearchGetAll(filters));
            return this.File(buffer, BIAConstants.Csv.ContentType + ";charset=utf-8", $"Engines{BIAConstants.Csv.Extension}");
        }

        /// <summary>
        /// Launches the job manually example.
        /// </summary>
        /// <returns>The status code.</returns>
        [HttpPost("LaunchJobManually")]
        public IActionResult LaunchJobManuallyExample()
        {
            this.planeService.LaunchJobManuallyExample();
            return this.Ok();
        }
    }
}