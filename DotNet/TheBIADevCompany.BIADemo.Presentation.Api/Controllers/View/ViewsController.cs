// <copyright file="ViewsController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.View
{
    using System;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Exceptions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Application.View;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.View;
    using TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Base;

    /// <summary>
    /// The API controller used to manage views.
    /// </summary>
    public class ViewsController : TeamLinkedControllerBase
    {
        /// <summary>
        /// The service role.
        /// </summary>
        private readonly IViewAppService viewAppService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewsController"/> class.
        /// </summary>
        /// <param name="viewAppService">The view service.</param>
        /// <param name="teamAppService">The team service.</param>
        public ViewsController(IViewAppService viewAppService, ITeamAppService teamAppService)
            : base(teamAppService)
        {
            this.viewAppService = viewAppService;
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
            var results = await this.viewAppService.GetAllAsync();

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
        public async Task<IActionResult> UpdateUserView(int id, [FromBody] ViewDto dto)
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
        public async Task<IActionResult> AddUserView([FromBody] ViewDto dto)
        {
            try
            {
                var createdDto = await this.viewAppService.AddUserViewAsync(dto);
                return this.Ok(createdDto);
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
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
                await this.viewAppService.RemoveUserViewAsync(id);
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
        public async Task<IActionResult> SetDefaultUserView(int id, [FromBody] DefaultViewDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                await this.viewAppService.SetDefaultUserViewAsync(dto);
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
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
        [HttpPut("TeamViews/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTeamView(int id, [FromBody] TeamViewDto dto)
        {
            if (!this.IsAuthorizeForTeam(dto.TeamId, Rights.Views.UpdateTeamViewSuffix).Result)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden);
            }

            return await this.UpdateView(id, dto);
        }

        /// <summary>
        /// Add a view.
        /// </summary>
        /// <param name="dto">The view DTO.</param>
        /// <returns>The result of the creation.</returns>
        [HttpPost("TeamViews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddTeamView([FromBody] TeamViewDto dto)
        {
            if (!this.IsAuthorizeForTeam(dto.TeamId, Rights.Views.AddTeamViewSuffix).Result)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden);
            }

            try
            {
                var createdDto = await this.viewAppService.AddTeamViewAsync(dto);
                return this.Ok(createdDto);
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
        }

        /// <summary>
        /// Remove a view.
        /// </summary>
        /// <param name="id">The view identifier.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete("TeamViews/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Views.DeleteTeamView)]
        public async Task<IActionResult> RemoveTeamView(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                await this.viewAppService.RemoveTeamViewAsync(id);
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
        }

        /// <summary>
        /// Set a view as the default one for the current site.
        /// </summary>
        /// <param name="id">The view identifier.</param>
        /// <param name="dto">The defaultTeamView dto.</param>
        /// <returns>
        /// The result of the action.
        /// </returns>
        [HttpPut("TeamViews/{id}/setDefault")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetDefaultTeamView(int id, [FromBody] DefaultTeamViewDto dto)
        {
            if (!this.IsAuthorizeForTeam(dto.TeamId, Rights.Views.SetDefaultTeamViewSuffix).Result)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden);
            }

            if (id == 0 || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                await this.viewAppService.SetDefaultTeamViewAsync(dto);
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Set a view as the default one for the current site.
        /// </summary>
        /// <param name="id">The view identifier.</param>
        /// <param name="dto">The assignViewToTeam dto.</param>
        /// <returns>
        /// The result of the action.
        /// </returns>
        [HttpPut("{id}/AssignViewToTeam")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AssignViewToTeam(int id, [FromBody] AssignViewToTeamDto dto)
        {
            if (!this.IsAuthorizeForTeam(dto.TeamId, Rights.Views.AssignToTeamSuffix).Result)
            {
                return this.StatusCode(StatusCodes.Status403Forbidden);
            }

            if (id < 1 || dto.ViewId != id || dto.TeamId < 1)
            {
                return this.BadRequest();
            }

            try
            {
                await this.viewAppService.AssignViewToTeamAsync(dto);
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
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
                var updatedDto = await this.viewAppService.UpdateViewAsync(dto);
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
    }
}