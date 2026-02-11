// <copyright file="AppSettingsController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia
{
    using System;
    using BIA.Net.Core.Application.Permission;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.Dto.App;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;

    /// <summary>
    /// Controller to provide setting define in back to the front.
    /// </summary>
    public class AppSettingsController : BiaControllerBase
    {
        /// <summary>
        /// The configuration of the BiaNet section.
        /// </summary>
        private readonly AppSettingsDto appSettings;

#if BIA_FRONT_FEATURE
        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettingsController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="teamAppService">The team app service.</param>
        /// <param name="permissionService">The permission service.</param>
        public AppSettingsController(IOptions<BiaNetSection> configuration, ITeamAppService teamAppService, IPermissionService permissionService)
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettingsController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="permissionService">The permission service.</param>
        public AppSettingsController(IOptions<BiaNetSection> configuration, IPermissionService permissionService)
#endif
        {
            this.appSettings = new AppSettingsDto
            {
                Keycloak = configuration.Value.Authentication?.Keycloak,
                Environment = configuration.Value.Environment,
                Cultures = configuration.Value.Cultures,
                MonitoringUrl = configuration.Value.ApiFeatures?.DelegateJobToWorker?.MonitoringUrl,
                ProfileConfiguration = configuration.Value.ProfileConfiguration,
                IframeConfiguration = configuration.Value.IframeConfiguration,
#if BIA_FRONT_FEATURE
                TeamsConfig = teamAppService.GetTeamsConfig(),
#endif
                Permissions = permissionService.Permissions,
            };
        }

        /// <summary>
        /// Get application settings.
        /// </summary>
        /// <returns>The Application settings.</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType<AppSettingsDto>(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return this.Ok(this.appSettings);
        }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        /// <returns>The Environment.</returns>
        [HttpGet("environment")]
        [AllowAnonymous]
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        public IActionResult GetEnvironment()
        {
            return this.Ok(Environment.GetEnvironmentVariable(Constants.Application.Environment));
        }
    }
}
