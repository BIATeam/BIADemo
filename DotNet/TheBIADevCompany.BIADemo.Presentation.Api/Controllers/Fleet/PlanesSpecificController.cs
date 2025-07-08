// BIADemo only
// <copyright file="PlanesSpecificController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
#define UseHubForClientInPlane
namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Fleet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
#if UseHubForClientInPlane
    using BIA.Net.Core.Application.Services;
#endif
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Enum;
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
    /// The API controller used to manage Planes.
    /// </summary>
#if !UseHubForClientInPlane
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "UseHubForClientInPlane not set")]
#endif
    public class PlanesSpecificController : BiaControllerBase
    {
        /// <summary>
        /// The plane application service.
        /// </summary>
        private readonly IPlaneSpecificAppService planeSpecificService;

#if UseHubForClientInPlane
        private readonly IClientForHubService clientForHubService;
        private readonly IBiaClaimsPrincipalService biaClaimsPrincipalService;
#endif

#if UseHubForClientInPlane
        /// <summary>
        /// Initializes a new instance of the <see cref="PlanesSpecificController" /> class.
        /// </summary>
        /// <param name="planeSpecificService">The plane application service.</param>
        /// <param name="clientForHubService">The hub for client.</param>
        /// <param name="biaClaimsPrincipalService">The BIA claims principal service.</param>
        public PlanesSpecificController(
            IPlaneSpecificAppService planeSpecificService,
            IClientForHubService clientForHubService,
            IBiaClaimsPrincipalService biaClaimsPrincipalService)
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="PlanesSpecificController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="planeSpecificService">The plane application service.</param>
        /// <param name="biaClaimsPrincipalService">The BIA claims principal service.</param>
        public PlanesSpecificController(
            IPlaneSpecificAppService planeSpecificService,
            IBiaClaimsPrincipalService biaClaimsPrincipalService)
#endif
        {
#if UseHubForClientInPlane
            this.clientForHubService = clientForHubService;
#endif
            this.planeSpecificService = planeSpecificService;
            this.biaClaimsPrincipalService = biaClaimsPrincipalService;
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
        [Authorize(Roles = Rights.Planes.ListAccess)]
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            var (results, total) = await this.planeSpecificService.GetRangeAsync(filters);
            this.HttpContext.Response.Headers.Append(BiaConstants.HttpHeaders.TotalCount, total.ToString());
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
        [Authorize(Roles = Rights.Planes.Read)]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var dto = await this.planeSpecificService.GetAsync(id);
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
        [Authorize(Roles = Rights.Planes.Create)]
        public async Task<IActionResult> Add([FromBody] PlaneSpecificDto dto)
        {
            try
            {
                var createdDto = await this.planeSpecificService.AddAsync(dto);
#if UseHubForClientInPlane
                _ = this.clientForHubService.SendTargetedMessage(createdDto.SiteId.ToString(), "planes-specific", "refresh-planes-specific");
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
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Planes.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] PlaneSpecificDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                var updatedDto = await this.planeSpecificService.UpdateAsync(dto);
#if UseHubForClientInPlane
                _ = this.clientForHubService.SendTargetedMessage(updatedDto.SiteId.ToString(), "planes-specific", "refresh-planes-specific");
                _ = this.clientForHubService.SendTargetedMessage(updatedDto.SiteId.ToString(), "planes-specific", "refresh-planes-specific-plane-engines", new { updatedDto.Id, updatedDto.Engines });
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
        /// Remove a plane.
        /// </summary>
        /// <param name="id">The plane identifier.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Planes.Delete)]
        public async Task<IActionResult> Remove(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var deletedDto = await this.planeSpecificService.RemoveAsync(id);
#if UseHubForClientInPlane
                _ = this.clientForHubService.SendTargetedMessage(deletedDto.SiteId.ToString(), "planes-specific", "refresh-planes-specific");
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
        [Authorize(Roles = Rights.Planes.Delete)]
        public async Task<IActionResult> Remove([FromQuery] List<int> ids)
        {
            if (ids?.Any() != true)
            {
                return this.BadRequest();
            }

            try
            {
                var deletedDtos = await this.planeSpecificService.RemoveAsync(ids);

#if UseHubForClientInPlane
                deletedDtos.Select(m => m.SiteId).Distinct().ToList().ForEach(parentId =>
                {
                    _ = this.clientForHubService.SendTargetedMessage(parentId.ToString(), "planes-specific", "refresh-planes-specific");
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
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Planes.Save)]
        public async Task<IActionResult> Save(IEnumerable<PlaneSpecificDto> dtos)
        {
            var dtoList = dtos.ToList();
            if (!dtoList.Any())
            {
                return this.BadRequest();
            }

            try
            {
                var savedDtos = await this.planeSpecificService.SaveSafeAsync(
                    dtos: dtoList,
                    principal: this.biaClaimsPrincipalService.GetBiaClaimsPrincipal(),
                    rightAdd: Rights.Planes.Create,
                    rightUpdate: Rights.Planes.Update,
                    rightDelete: Rights.Planes.Delete);
#if UseHubForClientInPlane
                savedDtos.Select(m => m.SiteId).Distinct().ToList().ForEach(parentId =>
                {
                    _ = this.clientForHubService.SendTargetedMessage(parentId.ToString(), "planes-specific", "refresh-planes-specific");
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
            byte[] buffer = await this.planeSpecificService.GetCsvAsync(filters);
            return this.File(buffer, BiaConstants.Csv.ContentType + ";charset=utf-8", $"Planes{BiaConstants.Csv.Extension}");
        }
    }
}