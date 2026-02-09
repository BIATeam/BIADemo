// <copyright file="ViewsController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia.View
{
    using System;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Application.View;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Error;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Common.Helpers;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Dto.View;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia.Base;

    /// <summary>
    /// The API controller used to manage views.
    /// </summary>
    public class ViewsController : TeamLinkedControllerBase
    {
        /// <summary>
        /// The service role.
        /// </summary>
        private readonly IViewAppService viewAppService;
        private readonly IBiaClaimsPrincipalService biaClaimsPrincipalService;
        private readonly IEnumerable<IPermissionConverter> permissionConverters;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewsController" /> class.
        /// </summary>
        /// <param name="viewAppService">The view service.</param>
        /// <param name="teamAppService">The team service.</param>
        /// <param name="biaClaimsPrincipalService">The bia claims principal service.</param>
        public ViewsController(IViewAppService viewAppService, ITeamAppService teamAppService, IBiaClaimsPrincipalService biaClaimsPrincipalService, IEnumerable<IPermissionConverter> permissionConverters)
            : base(teamAppService)
        {
            this.viewAppService = viewAppService;
            this.biaClaimsPrincipalService = biaClaimsPrincipalService;
            this.permissionConverters = permissionConverters;
        }

        /// <summary>
        /// Get a view by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The view.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(BiaPermissionId.View_Read))]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var dto = await this.viewAppService.GetAsync(id);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Gets all views that I can see.
        /// </summary>
        /// <returns>The list of views.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = nameof(BiaPermissionId.View_List))]
        public async Task<IActionResult> GetAll()
        {
            var results = await this.viewAppService.GetAllAsync();

            return this.Ok(results);
        }

        /// <summary>
        /// Update a view.
        /// </summary>
        /// <param name="id">The userView identifier.</param>
        /// <param name="dto">The userView DTO.</param>
        /// <returns>The result of the update.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(BiaPermissionId.View_Update_UserView))]
        public async Task<IActionResult> UpdateView(int id, [FromBody] ViewDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                var (isAuthorized, permission) = await this.ValidateAndGetPermission(dto);
                if (!isAuthorized)
                {
                    return this.StatusCode(StatusCodes.Status403Forbidden);
                }

                var userData = this.biaClaimsPrincipalService.GetUserData<BaseUserDataDto>();
                if (dto.ViewTeams.Any(team => (team.DtoState == BIA.Net.Core.Domain.Dto.Base.DtoState.Added || team.DtoState == BIA.Net.Core.Domain.Dto.Base.DtoState.Deleted) && !userData.CrossTeamPermissions.Any(p => PermissionHelper.GetPermissionName(p.PermissionId, this.permissionConverters) == permission && (p.IsGlobal || p.TeamIds.Any(ti => ti == team.Id)))))
                {
                    throw new BusinessException("Can't update view for these teams.");
                }

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
            catch (BusinessException be)
            {
                throw new FrontUserException(be.Message);
            }
        }

        /// <summary>
        /// Add a view.
        /// </summary>
        /// <param name="dto">The view DTO.</param>
        /// <returns>The result of the creation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(BiaPermissionId.View_Add_UserView))]
        public async Task<IActionResult> AddView([FromBody] ViewDto dto)
        {
            try
            {
                var (isAuthorized, permission) = await this.ValidateAndGetPermission(dto);
                if (!isAuthorized)
                {
                    return this.StatusCode(StatusCodes.Status403Forbidden);
                }

                var userData = this.biaClaimsPrincipalService.GetUserData<BaseUserDataDto>();
                if (dto.ViewTeams.Any(team => !userData.CrossTeamPermissions.Any(p => p.IsGlobal || (PermissionHelper.GetPermissionName(p.PermissionId, this.permissionConverters) == permission && p.TeamIds.Any(ti => ti == team.Id)))))
                {
                    throw FrontUserException.Create(BiaErrorId.CannotAddViewForSelectedTeams);
                }

                var createdDto = await this.viewAppService.AddViewAsync(dto);
                return this.Ok(createdDto);
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
            catch (BusinessException be)
            {
                throw new FrontUserException(be.Message);
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
        [Authorize(Roles = nameof(BiaPermissionId.View_Delete_UserView))]
        public async Task<IActionResult> RemoveUserView(int id)
        {
            return await this.RemoveView(id);
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
        [Authorize(Roles = nameof(BiaPermissionId.View_Set_Default_UserView))]
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
        public async Task<IActionResult> UpdateTeamView(int id, [FromBody] ViewDto dto)
        {
            if (dto.ViewTeams.Count == 0 || !dto.ViewTeams.Any(vt => this.IsAuthorizeForTeam(vt.Id, BiaRights.Views.UpdateTeamViewSuffix).Result))
            {
                return this.StatusCode(StatusCodes.Status403Forbidden);
            }

            return await this.UpdateView(id, dto);
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
        [Authorize(Roles = nameof(BiaPermissionId.View_Delete_TeamView))]
        public async Task<IActionResult> RemoveTeamView(int id)
        {
            return await this.RemoveView(id);
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
            if (!this.IsAuthorizeForTeam(dto.TeamId, BiaRights.Views.SetDefaultTeamViewSuffix).Result)
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
            if (!this.IsAuthorizeForTeam(dto.TeamId, BiaRights.Views.AssignToTeamSuffix).Result)
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

        private async Task<(bool IsAuthorized, string Permission)> ValidateAndGetPermission(ViewDto dto)
        {
            if (dto.ViewType == 1
                && (dto.ViewTeams.Count == 0
                    || !dto.ViewTeams.Any(vt => this.IsAuthorizeForTeam(vt.Id, BiaRights.Views.UpdateTeamViewSuffix).Result)))
            {
                return (false, null);
            }

            string permission = null;
            if (dto.ViewType == 1 && dto.ViewTeams.Count > 0)
            {
                permission = await this.GetPermissionPrefixFromTeamId(dto.ViewTeams[0].Id) + BiaRights.Views.AssignToTeamSuffix;
            }

            return (true, permission);
        }

        private async Task<IActionResult> RemoveView(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                await this.viewAppService.RemoveViewAsync(id);
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
    }
}