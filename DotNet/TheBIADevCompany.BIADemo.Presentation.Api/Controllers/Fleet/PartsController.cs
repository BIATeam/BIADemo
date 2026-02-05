// BIADemo only
// <copyright file="PartsController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Fleet
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Fleet;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;

    /// <summary>
    /// The API controller used to manage Parts.
    /// </summary>
    public class PartsController : BiaControllerBase
    {
        /// <summary>
        /// The part option application service.
        /// </summary>
        private readonly IPartOptionAppService partOptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartsController"/> class.
        /// </summary>
        /// <param name="partService">The part application service.</param>
        public PartsController(IPartOptionAppService partService)
        {
            this.partOptionService = partService;
        }

        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// /// <returns>The list of parts.</returns>
        [HttpGet("allOptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = nameof(OptionPermissionId.Part_Options))]
        public async Task<IActionResult> GetAllOptions()
        {
            var results = await this.partOptionService.GetAllOptionsAsync();
            return this.Ok(results);
        }
    }
}