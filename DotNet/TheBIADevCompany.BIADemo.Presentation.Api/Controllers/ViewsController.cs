// <copyright file="ViewsController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.View;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.SiteView;
    using TheBIADevCompany.BIADemo.Domain.Dto.View;

    /// <summary>
    /// The API controller used to manage views.
    /// </summary>
    public class ViewsController : BiaControllerBase
    {
        /// <summary>
        /// The service role.
        /// </summary>
        private readonly IViewAppService viewService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewsController"/> class.
        /// </summary>
        /// <param name="viewService">The view service.</param>
        public ViewsController(IViewAppService viewService)
        {
            this.viewService = viewService;
        }

        /// <summary>
        /// Gets all views that I can see.
        /// </summary>
        /// <returns>The list of views.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Rights.Views.List)]
        public async Task<IActionResult> GetAll()
        {
            var results = await this.viewService.GetAllAsync();

            return this.Ok(results);
        }

        /// <summary>
        /// Update a userView.
        /// </summary>
        /// <param name="id">The userView identifier.</param>
        /// <param name="dto">The userView DTO.</param>
        /// <returns>The result of the update.</returns>
        [HttpPut("UserViews/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Views.UpdateUserView)]
        public async Task<IActionResult> UpdateUserView(int id, [FromBody]ViewDto dto)
        {
            return await this.UpdateView(id, dto);
        }

        /// <summary>
        /// Add a view.
        /// </summary>
        /// <param name="dto">The view DTO.</param>
        /// <returns>The result of the creation.</returns>
        [HttpPost("UserViews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Views.AddUserView)]
        public async Task<IActionResult> AddUserView([FromBody]ViewDto dto)
        {
            try
            {
                var createdDto = await this.viewService.AddUserViewAsync(dto);
                return this.Ok(createdDto);
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Remove a view.
        /// </summary>
        /// <param name="id">The view identifier.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete("UserViews/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Views.DeleteUserView)]
        public async Task<IActionResult> RemoveUserView(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                await this.viewService.RemoveUserViewAsync(id);
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
            catch (BusinessException)
            {
                return this.BadRequest();
            }
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Set a view as the default one for the current user.
        /// </summary>
        /// <param name="id">The view identifier.</param>
        /// <param name="dto">The defaultView Dto.</param>
        /// <returns>
        /// The result of the action.
        /// </returns>
        [HttpPut("UserViews/{id}/setDefault")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Views.SetDefaultUserView)]
        public async Task<IActionResult> SetDefaultUserView(int id, [FromBody]DefaultViewDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                await this.viewService.SetDefaultUserViewAsync(dto);
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Update a siteView.
        /// </summary>
        /// <param name="id">The siteView identifier.</param>
        /// <param name="dto">The siteView DTO.</param>
        /// <returns>
        /// The result of the update.
        /// </returns>
        [HttpPut("SiteViews/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Views.UpdateSiteView)]
        public async Task<IActionResult> UpdateSiteView(int id, [FromBody]ViewDto dto)
        {
            return await this.UpdateView(id, dto);
        }

        /// <summary>
        /// Add a view.
        /// </summary>
        /// <param name="dto">The view DTO.</param>
        /// <returns>The result of the creation.</returns>
        [HttpPost("SiteViews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Views.AddSiteView)]
        public async Task<IActionResult> AddSiteView([FromBody]SiteViewDto dto)
        {
            try
            {
                var createdDto = await this.viewService.AddSiteViewAsync(dto);
                return this.Ok(createdDto);
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Remove a view.
        /// </summary>
        /// <param name="id">The view identifier.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete("SiteViews/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Views.DeleteSiteView)]
        public async Task<IActionResult> RemoveSiteView(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                await this.viewService.RemoveSiteViewAsync(id);
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
            catch (BusinessException)
            {
                return this.BadRequest();
            }
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Set a view as the default one for the current site.
        /// </summary>
        /// <param name="id">The view identifier.</param>
        /// <param name="dto">The defaultSiteView dto.</param>
        /// <returns>
        /// The result of the action.
        /// </returns>
        [HttpPut("SiteViews/{id}/setDefault")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Views.SetDefaultSiteView)]
        public async Task<IActionResult> SetDefaultSiteView(int id, [FromBody]DefaultSiteViewDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                await this.viewService.SetDefaultSiteViewAsync(dto);
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Set a view as the default one for the current site.
        /// </summary>
        /// <param name="id">The view identifier.</param>
        /// <param name="dto">The assignViewToSite dto.</param>
        /// <returns>
        /// The result of the action.
        /// </returns>
        [HttpPut("{id}/AssignViewToSite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Views.AssignToSite)]
        public async Task<IActionResult> AssignViewToSite(int id, [FromBody]AssignViewToSiteDto dto)
        {
            if (id < 1 || dto == null || dto.ViewId != id || dto.SiteId < 1)
            {
                return this.BadRequest();
            }

            try
            {
                await this.viewService.AssignViewToSiteAsync(dto);
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates the view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dto">The dto.</param>
        /// <returns>the view updated.</returns>
        private async Task<IActionResult> UpdateView(int id, ViewDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                var updatedDto = await this.viewService.UpdateViewAsync(dto);
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
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
            }
        }
    }
}