// BIADemo only
// <copyright file="BiaRemoteController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Utilities
{
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Plane;
    using TheBIADevCompany.BIADemo.Application.Utilities;

    /// <summary>
    /// Bia Remote Controller.
    /// </summary>
    public class BiaRemoteController : BiaControllerBase
    {
        private readonly IBiaRemoteService biaRemoteService;

        private readonly IRemotePlaneAppService remotePlaneAppService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaRemoteController"/> class.
        /// </summary>
        /// <param name="biaRemoteService">Document analysis service.</param>
        public BiaRemoteController(IBiaRemoteService biaRemoteService, IRemotePlaneAppService remotePlaneAppService)
        {
            this.biaRemoteService = biaRemoteService;
            this.remotePlaneAppService = remotePlaneAppService;
        }

        /// <summary>
        /// Ping.
        /// </summary>
        /// <returns>Return true if ok.</returns>
        [HttpGet("PingOne")]
        [AllowAnonymous]
        public async Task<IActionResult> PingOne()
        {
            bool isOk = await this.biaRemoteService.PingAsync();
            return this.Ok(isOk);
        }

        [HttpGet("PingTwo")]
        [AllowAnonymous]
        public async Task<IActionResult> PingTwo()
        {
            var obj = await this.remotePlaneAppService.ExampleCallApiAsync();
            return this.Ok(obj?.Id > 0);
        }
    }
}
