using Microsoft.AspNetCore.Http;
// BIADemo only
// <copyright file="PlanesTypesController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
// #define UseHubForClientInPlaneType

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Plane
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
#if UseHubForClientInPlaneType
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
    /// The API controller used to manage planes.
    /// </summary>
    public class PlanesTypesController : BiaControllerBase
    {
        /// <summary>
        /// The plane application service.
        /// </summary>
        private readonly IPlaneTypeAppService planeTypeService;

#if UseHubForClientInPlaneType
        private readonly IClientForHubService clientForHubService;
#endif

#if UseHubForClientInPlaneType
        /// <summary>
        /// Initializes a new instance of the <see cref="PlanesTypesController"/> class.
        /// </summary>
        /// <param name="planeTypeService">The plane application service.</param>
        /// <param name="clientForHubService">The hub for client.</param>
        public PlanesTypesController(IPlaneTypeAppService planeTypeService, IClientForHubService clientForHubService)
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="PlanesTypesController"/> class.
        /// </summary>
        /// <param name="planeTypeService">The plane application service.</param>
        public PlanesTypesController(IPlaneTypeAppService planeTypeService)
#endif
        {
#if UseHubForClientInPlaneType
            this.clientForHubService = clientForHubService;
#endif
            this.planeTypeService = planeTypeService;
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
        [Authorize(Roles = Rights.PlanesTypes.ListAccess)]
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            var (results, total) = await this.planeTypeService.GetRangeAsync(filters);
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
        [Authorize(Roles = Rights.PlanesTypes.Read)]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var dto = await this.planeTypeService.GetAsync(id);
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
        [Authorize(Roles = Rights.PlanesTypes.Create)]
        public async Task<IActionResult> Add([FromBody] PlaneTypeDto dto)
        {
            try
            {
                var createdDto = await this.planeTypeService.AddAsync(dto);
#if UseHubForClientInPlaneType
                await this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "planetypes" }, "refresh -planetypes", string.Empty);
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
        [Authorize(Roles = Rights.PlanesTypes.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] PlaneTypeDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                var updatedDto = await this.planeTypeService.UpdateAsync(dto);
#if UseHubForClientInPlaneType
                await this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "planetypes" }, "refresh-planetypes", string.Empty);
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
        [Authorize(Roles = Rights.PlanesTypes.Delete)]
        public async Task<IActionResult> Remove(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                await this.planeTypeService.RemoveAsync(id);
#if UseHubForClientInPlaneType
                await this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "planetypes" }, "refresh-planetypes", string.Empty);
#endif
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Removes the specified planesType ids.
        /// </summary>
        /// <param name="ids">The planesType ids.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.PlanesTypes.Delete)]
        public async Task<IActionResult> Remove([FromQuery] List<int> ids)
        {
            if (ids?.Any() != true)
            {
                return this.BadRequest();
            }

            try
            {
                foreach (int id in ids)
                {
                    await this.planeTypeService.RemoveAsync(id);
                }

#if UseHubForClientInPlaneType
                await this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "planetypes" }, "refresh-planetypes", string.Empty);
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
        [Authorize(Roles = Rights.PlanesTypes.Save)]
        public async Task<IActionResult> Save(IEnumerable<PlaneTypeDto> dtos)
        {
            var dtoList = dtos.ToList();
            if (!dtoList.Any())
            {
                return this.BadRequest();
            }

            try
            {
                await this.planeTypeService.SaveAsync(dtoList);
#if UseHubForClientInPlaneType
                await this.clientForHubService.SendMessage(new TargetedFeatureDto { FeatureName = "planetypes" }, "refresh-planetypes", string.Empty);
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
            byte[] buffer = await this.planeTypeService.GetCsvAsync(filters);
            return this.File(buffer, BiaConstants.Csv.ContentType + ";charset=utf-8", $"Planes{BiaConstants.Csv.Extension}");
        }
    }
}