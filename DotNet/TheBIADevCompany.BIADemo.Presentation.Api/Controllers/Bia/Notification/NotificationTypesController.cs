// <copyright file="NotificationTypesController.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Bia.Notification
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Notification;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Presentation.Api.Controller.Base;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The API controller used to manage notification type.
    /// </summary>
    public class NotificationTypesController : BiaControllerBase
    {
        /// <summary>
        /// The notification type application service.
        /// </summary>
        private readonly INotificationTypeAppService notificationTypeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTypesController"/> class.
        /// </summary>
        /// <param name="notificationTypeService">The notification type application service.</param>
        public NotificationTypesController(INotificationTypeAppService notificationTypeService)
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
        [Authorize(Roles = BiaRights.NotificationTypes.Options)]
        public async Task<IActionResult> GetAllOptions()
        {
            var results = await this.notificationTypeService.GetAllOptionsAsync();
            return this.Ok(results);
        }
    }
}