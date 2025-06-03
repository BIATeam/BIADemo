// BIADemo only
// <copyright file="AircraftMaintenanceCompanyOptionsController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Maintenance
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Maintenance;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;

    /// <summary>
    /// The API controller used to manage aircraftMaintenanceCompany options.
    /// </summary>
    public class AircraftMaintenanceCompanyOptionsController : BiaControllerBase
    {
        /// <summary>
        /// The aircraftMaintenanceCompany application service.
        /// </summary>
        private readonly IAircraftMaintenanceCompanyOptionAppService aircraftMaintenanceCompanyOptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftMaintenanceCompanyOptionsController"/> class.
        /// </summary>
        /// <param name="aircraftMaintenanceCompanyOptionService">The aircraftMaintenanceCompany application service.</param>
        public AircraftMaintenanceCompanyOptionsController(IAircraftMaintenanceCompanyOptionAppService aircraftMaintenanceCompanyOptionService)
        {
            this.aircraftMaintenanceCompanyOptionService = aircraftMaintenanceCompanyOptionService;
        }

        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// /// <returns>The list of aircraftMaintenanceCompanies.</returns>
        [HttpGet("allOptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.AircraftMaintenanceCompanyOptions.Options)]
        public async Task<IActionResult> GetAllOptions()
        {
            var results = await this.aircraftMaintenanceCompanyOptionService.GetAllOptionsAsync();
            return this.Ok(results);
        }
    }
}
