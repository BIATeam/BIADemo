// <copyright file="TeamsController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
// #define UseHubForClientInTeam

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia.User
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
#if UseHubForClientInTeam
    using BIA.Net.Core.Application.Services;
#endif
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Bia.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

    /// <summary>
    /// The API controller used to manage Teams.
    /// </summary>
    public class TeamsController : BiaControllerBase
    {
        /// <summary>
        /// The member application service.
        /// </summary>
        private readonly IMemberAppService memberService;

        /// <summary>
        /// The user application service.
        /// </summary>
        private readonly IUserAppService<UserExtendedDto, UserExtended> userService;

        /// <summary>
        /// The plane application service.
        /// </summary>
        private readonly ITeamAppService teamService;

#if UseHubForClientInTeam
        private readonly IClientForHubService clientForHubService;
#endif

#if UseHubForClientInTeam
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamsController"/> class.
        /// </summary>
        /// <param name="teamService">The team application service.</param>
        /// <param name="memberService">The member application service.</param>
        /// <param name="clientForHubService">The hub for client.</param>
        public TeamsController(ITeamAppService teamService, IMemberAppService memberService, IClientForHubService clientForHubService)
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamsController"/> class.
        /// </summary>
        /// <param name="teamService">The team application service.</param>
        /// <param name="memberService">The member application service.</param>
        /// <param name="userService">The user application service.</param>
        public TeamsController(ITeamAppService teamService, IMemberAppService memberService, IUserAppService<UserExtendedDto, UserExtended> userService)
#endif
        {
#if UseHubForClientInTeam
            this.clientForHubService = clientForHubService;
#endif
            this.memberService = memberService;
            this.userService = userService;
            this.teamService = teamService;
        }

        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// <returns>The list of production sites.</returns>
        [HttpGet("allOptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Teams.Options)]
        public async Task<IActionResult> GetAllOptions()
        {
            var results = await this.teamService.GetAllOptionsAsync();
            return this.Ok(results);
        }

        /// <summary>
        /// Get all teams.
        /// </summary>
        /// <returns>The list of planes.</returns>
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Teams.ListAccess)]
        public async Task<IActionResult> GetAll()
        {
            var results = await this.teamService.GetAllAsync();
            return this.Ok(results);
        }

        /// <summary>
        /// Sets the default site.
        /// </summary>
        /// <param name="teamTypeId">The team type.</param>
        /// <param name="teamId">The team identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPut("TeamType/{teamTypeId}/setDefault/{teamId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Teams.SetDefaultTeam)]
        public async Task<IActionResult> SetDefaultTeam(int teamTypeId, int teamId)
        {
            if (teamId == 0 || teamTypeId == 0)
            {
                return this.BadRequest();
            }

            try
            {
                await this.userService.SetDefaultTeamAsync(teamId, teamTypeId);
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
        /// Reset the default site.
        /// </summary>
        /// <param name="teamTypeId">The team type.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPut("TeamType/{teamTypeId}/resetDefault")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Teams.SetDefaultTeam)]
        public async Task<IActionResult> ResetDefaultTeam(int teamTypeId)
        {
            if (teamTypeId == 0)
            {
                return this.BadRequest();
            }

            try
            {
                await this.userService.ResetDefaultTeamAsync(teamTypeId);
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
        /// Sets the default role.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <param name="roleIds">The roles identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPut("Team/{teamId}/setDefaultRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Teams.SetDefaultRoles)]
        public async Task<IActionResult> SetDefaultRoles(int teamId, List<int> roleIds)
        {
            if (teamId == 0)
            {
                return this.BadRequest();
            }

            try
            {
                await this.memberService.SetDefaultRoleAsync(teamId, roleIds);
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
        /// Resets the default role.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPut("Team/{teamId}/resetDefaultRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Teams.SetDefaultRoles)]
        public async Task<IActionResult> ResetDefaultRoles(int teamId)
        {
            if (teamId == 0)
            {
                return this.BadRequest();
            }

            try
            {
                await this.memberService.ResetDefaultRoleAsync(teamId);
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
    }
}