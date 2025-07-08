// BIADemo only
// <copyright file="MaintenanceContractsController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
#define UseHubForClientInMaintenanceContract
namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Maintenance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
#if UseHubForClientInMaintenanceContract
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
    using TheBIADevCompany.BIADemo.Application.Maintenance;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.Maintenance;

    /// <summary>
    /// The API controller used to manage MaintenanceContracts.
    /// </summary>
#if !UseHubForClientInMaintenanceContract
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "UseHubForClientInMaintenanceContract not set")]
#endif
    public class MaintenanceContractsController : BiaControllerBase
    {
        /// <summary>
        /// The maintenanceContract application service.
        /// </summary>
        private readonly IMaintenanceContractAppService maintenanceContractService;

        /// <summary>
        /// The BIA claims principal service.
        /// </summary>
        private readonly IBiaClaimsPrincipalService biaClaimsPrincipalService;

#if UseHubForClientInMaintenanceContract
        private readonly IClientForHubService clientForHubService;
#endif

#if UseHubForClientInMaintenanceContract
        /// <summary>
        /// Initializes a new instance of the <see cref="MaintenanceContractsController" /> class.
        /// </summary>
        /// <param name="maintenanceContractService">The maintenanceContract application service.</param>
        /// <param name="clientForHubService">The hub for client.</param>
        /// <param name="biaClaimsPrincipalService">The BIA claims principal service.</param>
        public MaintenanceContractsController(
            IMaintenanceContractAppService maintenanceContractService,
            IClientForHubService clientForHubService,
            IBiaClaimsPrincipalService biaClaimsPrincipalService)
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="MaintenanceContractsController" /> class.
        /// </summary>
        /// <param name="maintenanceContractService">The maintenanceContract application service.</param>
        /// <param name="biaClaimsPrincipalService">The BIA claims principal service.</param>
        public MaintenanceContractsController(
            IMaintenanceContractAppService maintenanceContractService,
            IBiaClaimsPrincipalService biaClaimsPrincipalService)
#endif
        {
#if UseHubForClientInMaintenanceContract
            this.clientForHubService = clientForHubService;
#endif
            this.maintenanceContractService = maintenanceContractService;
            this.biaClaimsPrincipalService = biaClaimsPrincipalService;
        }

        /// <summary>
        /// Get all maintenanceContracts with filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of maintenanceContracts.</returns>
        [HttpPost("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceContracts.ListAccess)]
        public async Task<IActionResult> GetAll([FromBody] PagingFilterFormatDto filters)
        {
            var (results, total) = await this.maintenanceContractService.GetRangeAsync(filters);
            this.HttpContext.Response.Headers.Append(BiaConstants.HttpHeaders.TotalCount, total.ToString());
            return this.Ok(results);
        }

        /// <summary>
        /// Get a maintenanceContract by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The maintenanceContract.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceContracts.Read)]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                var dto = await this.maintenanceContractService.GetAsync(id);
                return this.Ok(dto);
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Add a maintenanceContract.
        /// </summary>
        /// <param name="dto">The maintenanceContract DTO.</param>
        /// <returns>The result of the creation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceContracts.Create)]
        public async Task<IActionResult> Add([FromBody] MaintenanceContractDto dto)
        {
            try
            {
                var createdDto = await this.maintenanceContractService.AddAsync(dto);
#if UseHubForClientInMaintenanceContract
                await this.clientForHubService.SendTargetedMessage(string.Empty, "maintenance-contracts", "refresh-maintenance-contracts");
#endif
                return this.CreatedAtAction("Get", new { id = createdDto.Id }, createdDto);
            }
            catch (ArgumentNullException)
            {
                return this.ValidationProblem();
            }
            catch (ForbiddenException ex)
            {
                return this.Problem(
                        type: "/docs/errors/forbidden",
                        title: "User is not authorized to make this action.",
                        detail: ex.Message,
                        statusCode: StatusCodes.Status403Forbidden);
            }
        }

        /// <summary>
        /// Update a maintenanceContract.
        /// </summary>
        /// <param name="id">The maintenanceContract identifier.</param>
        /// <param name="dto">The maintenanceContract DTO.</param>
        /// <returns>The result of the update.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceContracts.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] MaintenanceContractDto dto)
        {
            if (id == 0 || dto == null || dto.Id != id)
            {
                return this.BadRequest();
            }

            try
            {
                var updatedDto = await this.maintenanceContractService.UpdateAsync(dto);
#if UseHubForClientInMaintenanceContract
                await this.clientForHubService.SendTargetedMessage(string.Empty, "maintenance-contracts", "refresh-maintenance-contracts");
                await this.clientForHubService.SendTargetedMessage(string.Empty, "maintenance-contracts", "update-maintenance-contract", updatedDto);
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
            catch (ForbiddenException ex)
            {
                return this.Problem(
                        type: "/docs/errors/forbidden",
                        title: "User is not authorized to make this action.",
                        detail: ex.Message,
                        statusCode: StatusCodes.Status403Forbidden);
            }
        }

        /// <summary>
        /// Remove a maintenanceContract.
        /// </summary>
        /// <param name="id">The maintenanceContract identifier.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceContracts.Delete)]
        public async Task<IActionResult> Remove(int id)
        {
            if (id == 0)
            {
                return this.BadRequest();
            }

            try
            {
                await this.maintenanceContractService.RemoveAsync(id);
#if UseHubForClientInMaintenanceContract
                await this.clientForHubService.SendTargetedMessage(string.Empty, "maintenance-contracts", "refresh-maintenance-contracts");
#endif
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Removes the specified maintenanceContract ids.
        /// </summary>
        /// <param name="ids">The maintenanceContract ids.</param>
        /// <returns>The result of the remove.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceContracts.Delete)]
        public async Task<IActionResult> Remove([FromQuery] List<int> ids)
        {
            if (ids?.Any() != true)
            {
                return this.BadRequest();
            }

            try
            {
                await this.maintenanceContractService.RemoveAsync(ids);

#if UseHubForClientInMaintenanceContract
                await this.clientForHubService.SendTargetedMessage(string.Empty, "maintenance-contracts", "refresh-maintenance-contracts");
#endif
                return this.Ok();
            }
            catch (ElementNotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Save all maintenanceContracts according to their state (added, updated or removed).
        /// </summary>
        /// <param name="dtos">The list of maintenanceContracts.</param>
        /// <returns>The status code.</returns>
        [HttpPost("save")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.MaintenanceContracts.Save)]
        public async Task<IActionResult> Save(IEnumerable<MaintenanceContractDto> dtos)
        {
            var dtoList = dtos.ToList();
            if (!dtoList.Any())
            {
                return this.BadRequest();
            }

            try
            {
                await this.maintenanceContractService.SaveSafeAsync(
                   dtos: dtoList,
                   principal: this.biaClaimsPrincipalService.GetBiaClaimsPrincipal(),
                   rightAdd: Rights.MaintenanceContracts.Create,
                   rightUpdate: Rights.MaintenanceContracts.Update,
                   rightDelete: Rights.MaintenanceContracts.Delete);
#if UseHubForClientInMaintenanceContract
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
            catch (ForbiddenException ex)
            {
                return this.Problem(
                        type: "/docs/errors/forbidden",
                        title: "User is not authorized to make this action.",
                        detail: ex.Message,
                        statusCode: StatusCodes.Status403Forbidden);
            }
        }

        /// <summary>
        /// Generates a csv file according to the filters.
        /// </summary>
        /// <param name="filters">filters ( <see cref="PagingFilterFormatDto"/>).</param>
        /// <returns>a csv file.</returns>
        [HttpPost("csv")]
        [Authorize(Roles = Rights.MaintenanceContracts.ListAccess)]
        public virtual async Task<IActionResult> GetFile([FromBody] PagingFilterFormatDto filters)
        {
            byte[] buffer = await this.maintenanceContractService.GetCsvAsync(filters);
            return this.File(buffer, BiaConstants.Csv.ContentType + ";charset=utf-8", $"MaintenanceContracts{BiaConstants.Csv.Extension}");
        }
    }
}
