// <copyright file="BannerMessagesController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia.Banner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Banner;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.Banner;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;

    /// <summary>
    /// The API controller used to manage BannerMessages.
    /// </summary>
    public class BannerMessagesController : BiaControllerBase
    {
        /// <summary>
        /// The bannerMessage application service.
        /// </summary>
        private readonly IBannerMessageAppService bannerMessageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerMessagesController"/> class.
        /// </summary>
        /// <param name="bannerMessageService">The bannerMessage application service.</param>
        public BannerMessagesController(IBannerMessageAppService bannerMessageService)
        {
            this.bannerMessageService = bannerMessageService;
        }

        /// <summary>
        /// Get all bannerMessages with filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of bannerMessages.</returns>
        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.BannerMessages.ListAccess)]
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            var (results, total) = await this.bannerMessageService.GetRangeAsync(filters);
            this.HttpContext.Response.Headers.Append(BiaConstants.HttpHeaders.TotalCount, total.ToString());
            return this.Ok(results);
        }

        /// <summary>
        /// Get a bannerMessage by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The bannerMessage.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.BannerMessages.Read)]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var dto = await this.bannerMessageService.GetAsync(id);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Add a bannerMessage.
        /// </summary>
        /// <param name="dto">The bannerMessage DTO.</param>
        /// <returns>The result of the creation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.BannerMessages.Create)]
        public async Task<IActionResult> Add([FromBody] BannerMessageDto dto)
        {
            try
            {
                var createdDto = await this.bannerMessageService.AddAsync(dto);
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
        /// Update a bannerMessage.
        /// </summary>
        /// <param name="id">The bannerMessage identifier.</param>
        /// <param name="dto">The bannerMessage DTO.</param>
        /// <returns>The result of the update.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.BannerMessages.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] BannerMessageDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                var updatedDto = await this.bannerMessageService.UpdateAsync(dto);
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
        /// Remove a bannerMessage.
        /// </summary>
        /// <param name="id">The bannerMessage identifier.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.BannerMessages.Delete)]
        public async Task<IActionResult> Remove(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                await this.bannerMessageService.RemoveAsync(id);
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Removes the specified bannerMessage ids.
        /// </summary>
        /// <param name="ids">The bannerMessage ids.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.BannerMessages.Delete)]
        public async Task<IActionResult> Remove([FromQuery] List<int> ids)
        {
            if (ids?.Any() != true)
            {
                return this.BadRequest();
            }

            try
            {
                await this.bannerMessageService.RemoveAsync(ids);
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Return the historical of an item by its id.
        /// </summary>
        /// <param name="id">ID of the item to update.</param>
        /// <returns>Item's historical.</returns>
        [HttpGet("{id}/historical")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.BannerMessages.Read)]
        public virtual async Task<IActionResult> GetHistorical(int id)
        {
            try
            {
                var dto = await this.bannerMessageService.GetHistoricalAsync(id);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }
    }
}
