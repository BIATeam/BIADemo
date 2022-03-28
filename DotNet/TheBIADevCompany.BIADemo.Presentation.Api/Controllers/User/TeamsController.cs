// BIADemo only
// <copyright file="TeamsController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
#define UseHubForClientInTeam

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto;
    using BIA.Net.Core.Domain.Dto.Base;
#if UseHubForClientInTeam
    using BIA.Net.Core.Domain.RepoContract;
#endif
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
#if UseHubForClientInTeam
    using Microsoft.AspNetCore.SignalR;
#endif
    using TheBIADevCompany.BIADemo.Application.Plane;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;

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
        /// The plane application service.
        /// </summary>
        private readonly ITeamAppService teamService;

#if UseHubForClientInTeam
        private readonly IClientForHubRepository clientForHubService;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamsController"/> class.
        /// </summary>
        /// <param name="teamService">The team application service.</param>
        /// <param name="memberService">The member application service.</param>
        /// <param name="clientForHubService">The hub for client.</param>
#if UseHubForClientInTeam
        public TeamsController(ITeamAppService teamService, IMemberAppService memberService, IClientForHubRepository clientForHubService)
#else
        public TeamsController(ITeamAppService teamService)
#endif
        {
#if UseHubForClientInTeam
            this.clientForHubService = clientForHubService;
#endif
            this.memberService = memberService;
            this.teamService = teamService;
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
                await this.memberService.SetDefaultTeamAsync(teamId, teamTypeId);
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
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
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
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
            }
        }
    }
}