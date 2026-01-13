// BIADemo only
// <copyright file="BiaRemoteController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Utilities
{
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Fleet;
    using TheBIADevCompany.BIADemo.Application.Utilities;

    /// <summary>
    /// Bia Remote Controller.
    /// </summary>
    public class BiaRemoteController : BiaControllerBase
    {
        /// <summary>
        /// The remote bia API rw service.
        /// </summary>
        private readonly IRemoteBiaApiRwService remoteBiaApiRwService;

        /// <summary>
        /// The remote plane service.
        /// </summary>
        private readonly IRemotePlaneAppService remotePlaneService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaRemoteController"/> class.
        /// </summary>
        /// <param name="remoteBiaApiRwService">The remote bia API rw service.</param>
        /// <param name="remotePlaneService">The remote plane service.</param>
        public BiaRemoteController(IRemoteBiaApiRwService remoteBiaApiRwService, IRemotePlaneAppService remotePlaneService)
        {
            this.remoteBiaApiRwService = remoteBiaApiRwService;
            this.remotePlaneService = remotePlaneService;
        }

        /// <summary>
        /// Ping.
        /// </summary>
        /// <returns>Return true if ok.</returns>
        [HttpGet("ping")]
        [AllowAnonymous]
        public async Task<IActionResult> Ping()
        {
            bool isOk = await this.remoteBiaApiRwService.PingAsync();
            return this.Ok(isOk);
        }

        /// <summary>
        /// Check if remote plane exist.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Return true if plane exist.</returns>
        [HttpGet("planes/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRemotePlane(int id)
        {
            bool isOk = await this.remotePlaneService.CheckExistAsync(id);
            return this.Ok(isOk);
        }

        /// <summary>
        /// Creates the remote plane.
        /// </summary>
        /// <returns>Return true if plane created.</returns>
        [HttpPost("planes/test")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateRemotePlane()
        {
            bool isOk = await this.remotePlaneService.CreateAsync();
            return this.Ok(isOk);
        }
    }
}
