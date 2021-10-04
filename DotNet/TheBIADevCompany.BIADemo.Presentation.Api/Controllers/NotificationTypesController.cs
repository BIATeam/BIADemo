// <copyright file="NotificationTypesController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers
{
    using System.Threading.Tasks;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Service;

    /// <summary>
    /// The API controller used to manage planes.
    /// </summary>
    public class NotificationTypesController : BiaControllerBase
    {
        /// <summary>
        /// The plane application service.
        /// </summary>
        private readonly INotificationTypeDomainService notificationTypeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTypesController"/> class.
        /// </summary>
        /// <param name="notificationTypeService">The plane application service.</param>
        /// <param name="hubForClients">The hub for client.</param>
        public NotificationTypesController(INotificationTypeDomainService notificationTypeService)
        {
            this.notificationTypeService = notificationTypeService;
        }

        /// <summary>
        /// Gets all option that I can see.
        /// </summary>
        /// /// <returns>The list of production sites.</returns>
        [HttpGet("allOptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.NotificationTypes.Options)]
        public async Task<IActionResult> GetAllOptions()
        {
            var results = await this.notificationTypeService.GetAllOptionsAsync();
            return this.Ok(results);
        }
    }
}