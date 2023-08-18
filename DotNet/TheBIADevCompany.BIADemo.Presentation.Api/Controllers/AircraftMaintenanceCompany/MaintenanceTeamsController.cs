// BIADemo only
// <copyright file="MaintenanceTeamsController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
// #define UseHubForClientInMaintenanceTeam
namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.AircraftMaintenanceCompany
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
#if UseHubForClientInMaintenanceTeam
    using BIA.Net.Core.Domain.RepoContract;
#endif
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Hangfire;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
#if UseHubForClientInMaintenanceTeam
    using Microsoft.AspNetCore.SignalR;
#endif
    using TheBIADevCompany.BIADemo.Application.AircraftMaintenanceCompany;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;

    /// <summary>
    /// The API controller used to manage MaintenanceTeams.
    /// </summary>
#if !UseHubForClientInMaintenanceTeam
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "UseHubForClientInMaintenanceTeam not set")]
#endif
    public class MaintenanceTeamsController : BiaControllerBase
    {
        /// <summary>
        /// The MaintenanceTeam application service.
        /// </summary>
        private readonly IMaintenanceTeamAppService aircraftMaintenanceCompanieservice;

#if UseHubForClientInMaintenanceTeam
        private readonly IClientForHubRepository clientForHubService;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintenanceTeamsController"/> class.
        /// </summary>
        /// <param name="aircraftMaintenanceCompanieservice">The MaintenanceTeam application service.</param>
        /// <param name="clientForHubService">The hub for client.</param>
        /// <param name="principal">The BIAClaimsPrincipal.</param>
#if UseHubForClientInMaintenanceTeam
        public MaintenanceTeamsController(
            IMaintenanceTeamAppService MaintenanceTeamservice, IClientForHubRepository clientForHubService)
#else
        public MaintenanceTeamsController(IMaintenanceTeamAppService aircraftMaintenanceCompanieservice)
#endif
        {
#if UseHubForClientInMaintenanceTeam
            this.clientForHubService = clientForHubService;
#endif
            this.aircraftMaintenanceCompanieservice = aircraftMaintenanceCompanieservice;
        }

        /// <summary>
        /// Get all MaintenanceTeams with filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of MaintenanceTeams.</returns>
        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceTeams.ListAccess)]
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            var (results, total) = await this.aircraftMaintenanceCompanieservice.GetRangeAsync(filters);
            this.HttpContext.Response.Headers.Add(BiaConstants.HttpHeaders.TotalCount, total.ToString());
            return this.Ok(results);
        }

        /// <summary>
        /// Get a MaintenanceTeam by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The MaintenanceTeam.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceTeams.Read)]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var dto = await this.aircraftMaintenanceCompanieservice.GetAsync(id);
                return this.Ok(dto);
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
        /// Add a MaintenanceTeam.
        /// </summary>
        /// <param name="dto">The MaintenanceTeam DTO.</param>
        /// <returns>The result of the creation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceTeams.Create)]
        public async Task<IActionResult> Add([FromBody] MaintenanceTeamDto dto)
        {
            try
            {
                var createdDto = await this.aircraftMaintenanceCompanieservice.AddAsync(dto);
#if UseHubForClientInMaintenanceTeam
                await this.clientForHubService.SendTargetedMessage(createdDto.SiteId.ToString(), "MaintenanceTeams", "refresh-MaintenanceTeams");
#endif
                return this.CreatedAtAction("Get", new { id = createdDto.Id }, createdDto);
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
            catch (Exception)
            {
                // BE CAREFULL on messages in output consol because the exception not always contains the error in case of inheritance.
                return this.StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Update a MaintenanceTeam.
        /// </summary>
        /// <param name="id">The MaintenanceTeam identifier.</param>
        /// <param name="dto">The MaintenanceTeam DTO.</param>
        /// <returns>The result of the update.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceTeams.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] MaintenanceTeamDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                var updatedDto = await this.aircraftMaintenanceCompanieservice.UpdateAsync(dto);
#if UseHubForClientInMaintenanceTeam
                _ = this.clientForHubService.SendTargetedMessage(updatedDto.SiteId.ToString(), "MaintenanceTeams", "refresh-MaintenanceTeams");
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
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Remove a MaintenanceTeam.
        /// </summary>
        /// <param name="id">The MaintenanceTeam identifier.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceTeams.Delete)]
        public async Task<IActionResult> Remove(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var deletedDto = await this.aircraftMaintenanceCompanieservice.RemoveAsync(id);
#if UseHubForClientInMaintenanceTeam
                _ = this.clientForHubService.SendTargetedMessage(deletedDto.SiteId.ToString(), "MaintenanceTeams", "refresh-MaintenanceTeams");
#endif
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
        /// Removes the specified MaintenanceTeam ids.
        /// </summary>
        /// <param name="ids">The MaintenanceTeam ids.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceTeams.Delete)]
        public async Task<IActionResult> Remove([FromQuery] List<int> ids)
        {
            if (ids == null || ids?.Any() != true)
            {
                return this.BadRequest();
            }

            try
            {
                var deletedDtos = await this.aircraftMaintenanceCompanieservice.RemoveAsync(ids);

#if UseHubForClientInMaintenanceTeam
                deletedDtos.Select(m => m.SiteId).Distinct().ToList().ForEach(parentId =>
                {
                    _ = this.clientForHubService.SendTargetedMessage(parentId.ToString(), "MaintenanceTeams", "refresh-MaintenanceTeams");
                });
#endif
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
        /// Save all MaintenanceTeams according to their state (added, updated or removed).
        /// </summary>
        /// <param name="dtos">The list of MaintenanceTeams.</param>
        /// <returns>The status code.</returns>
        [HttpPost("save")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceTeams.Save)]
        public async Task<IActionResult> Save(IEnumerable<MaintenanceTeamDto> dtos)
        {
            var dtoList = dtos.ToList();
            if (!dtoList.Any())
            {
                return this.BadRequest();
            }

            try
            {
                var savedDtos = await this.aircraftMaintenanceCompanieservice.SaveAsync(dtoList);
#if UseHubForClientInMaintenanceTeam
                savedDtos.Select(m => m.SiteId).Distinct().ToList().ForEach(parentId =>
                {
                    _ = this.clientForHubService.SendTargetedMessage(parentId.ToString(), "MaintenanceTeams", "refresh-MaintenanceTeams");
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
            catch (Exception)
            {
                return this.StatusCode(500, "Internal server error");
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
            byte[] buffer = await this.aircraftMaintenanceCompanieservice.GetCsvAsync(filters);
            return this.File(buffer, BiaConstants.Csv.ContentType + ";charset=utf-8", $"MaintenanceTeams{BiaConstants.Csv.Extension}");
        }
    }
}