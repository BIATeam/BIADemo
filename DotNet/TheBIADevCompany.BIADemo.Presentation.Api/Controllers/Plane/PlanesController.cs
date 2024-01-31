// BIADemo only
// <copyright file="PlanesController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
#define UseHubForClientInPlane
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
#if UseHubForClientInPlane
    using BIA.Net.Core.Domain.RepoContract;
#endif
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Hangfire;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
#if UseHubForClientInPlane
    using Microsoft.AspNetCore.SignalR;
#endif
    using TheBIADevCompany.BIADemo.Application.Plane;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;

    /// <summary>
    /// The API controller used to manage Planes.
    /// </summary>
#if !UseHubForClientInPlane
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "UseHubForClientInPlane not set")]
#endif
    public class PlanesController : BiaControllerBase
    {
        /// <summary>
        /// The plane application service.
        /// </summary>
        private readonly IPlaneAppService planeService;

#if UseHubForClientInPlane
        private readonly IClientForHubRepository clientForHubService;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanesController"/> class.
        /// </summary>
        /// <param name="planeService">The plane application service.</param>
        /// <param name="clientForHubService">The hub for client.</param>
#if UseHubForClientInPlane
        public PlanesController(
            IPlaneAppService planeService, IClientForHubRepository clientForHubService)
#else
        public PlanesController(IPlaneAppService planeService)
#endif
        {
#if UseHubForClientInPlane
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
        [Authorize(Roles = Rights.Planes.ListAccess)]
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            var (results, total) = await this.planeService.GetRangeAsync(filters);
            this.HttpContext.Response.Headers.Add(BIAConstants.HttpHeaders.TotalCount, total.ToString());
            return this.Ok(results);
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Planes.ListAccess)]
        public async Task<IActionResult> RetrieveAll()
        {
            var results = await this.planeService.GetAllAsync();
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
        [Authorize(Roles = Rights.Planes.Create)]
        public async Task<IActionResult> Add([FromBody] PlaneDto dto)
        {
            try
            {
                var createdDto = await this.planeService.AddAsync(dto);
#if UseHubForClientInPlane
                /// BIAToolKit - Begin Parent siteId
                _ = this.clientForHubService.SendTargetedMessage(createdDto.SiteId.ToString(), "planes", "refresh-planes");
                /// BIAToolKit - End Parent siteId
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
        [Authorize(Roles = Rights.Planes.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] PlaneDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                var updatedDto = await this.planeService.UpdateAsync(dto);
#if UseHubForClientInPlane
                /// BIAToolKit - Begin Parent siteId
                _ = this.clientForHubService.SendTargetedMessage(updatedDto.SiteId.ToString(), "planes", "refresh-planes");
                /// BIAToolKit - End Parent siteId
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
        [Authorize(Roles = Rights.Planes.Delete)]
        public async Task<IActionResult> Remove(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var deletedDto = await this.planeService.RemoveAsync(id);
#if UseHubForClientInPlane
                /// BIAToolKit - Begin Parent siteId
                _ = this.clientForHubService.SendTargetedMessage(deletedDto.SiteId.ToString(), "planes", "refresh-planes");
                /// BIAToolKit - End Parent siteId
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
                var deletedDtos = await this.planeService.RemoveAsync(ids);

#if UseHubForClientInPlane
                /// BIAToolKit - Begin Parent siteId
                deletedDtos.Select(m => m.SiteId).Distinct().ToList().ForEach(parentId =>
                {
                    _ = this.clientForHubService.SendTargetedMessage(parentId.ToString(), "planes", "refresh-planes");
                });
                /// BIAToolKit - End Parent siteId
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
        [Authorize(Roles = Rights.Planes.Save)]
        public async Task<IActionResult> Save(IEnumerable<PlaneDto> dtos)
        {
            var dtoList = dtos.ToList();
            if (!dtoList.Any())
            {
                return this.BadRequest();
            }

            try
            {
                var savedDtos = await this.planeService.SaveAsync(dtoList);
#if UseHubForClientInPlane
                /// BIAToolKit - Begin Parent siteId
                savedDtos.Select(m => m.SiteId).Distinct().ToList().ForEach(parentId =>
                {
                    _ = this.clientForHubService.SendTargetedMessage(parentId.ToString(), "planes", "refresh-planes");
                });
                /// BIAToolKit - End Parent siteId
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
            byte[] buffer = await this.planeService.GetCsvAsync(filters);
            return this.File(buffer, BIAConstants.Csv.ContentType + ";charset=utf-8", $"Planes{BIAConstants.Csv.Extension}");
        }

        /// <summary>
        /// Adds planes.
        /// </summary>
        /// <param name="dtos">List of planes.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPost("bulk")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Planes.Create)]
        public async Task<IActionResult> AddBulkAsync([FromBody] IEnumerable<PlaneDto> dtos)
        {
            // JSON test swagger.
            // [{ "id":0,"msn":"BULK1","isActive":true,"lastFlightDate":"2022-04-17T15:21:28.997Z","deliveryDate":"2021-04-17T15:21:28.997Z","capacity":1,"siteId":1,"planeType":{ "id":1} },{ "id":0,"msn":"BULK2","isActive":true,"lastFlightDate":"2022-04-18T15:21:28.997Z","deliveryDate":"2021-04-18T15:21:28.997Z","capacity":2,"siteId":1,"planeType":{ "id":1} },{ "id":0,"msn":"BULK3","isActive":true,"lastFlightDate":"2022-04-19T15:21:28.997Z","deliveryDate":"2021-04-19T15:21:28.997Z","capacity":3,"siteId":1,"planeType":{ "id":1} },{ "id":0,"msn":"BULK4","isActive":true,"lastFlightDate":"2022-04-20T15:21:28.997Z","deliveryDate":"2021-04-20T15:21:28.997Z","capacity":4,"siteId":1,"planeType":{ "id":1} },{ "id":0,"msn":"BULK5","isActive":true,"lastFlightDate":"2022-04-21T15:21:28.997Z","deliveryDate":"2021-04-21T15:21:28.997Z","capacity":5,"siteId":1,"planeType":{ "id":1} }]
            var dtoList = dtos.ToList();
            if (!dtoList.Any())
            {
                return this.BadRequest();
            }

            try
            {
                await this.planeService.AddBulkAsync(dtoList);
                return this.Ok();
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
        }
    }
}