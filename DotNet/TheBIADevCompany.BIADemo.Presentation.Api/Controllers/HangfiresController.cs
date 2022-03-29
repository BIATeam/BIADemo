// BIADemo only
// <copyright file="HangfiresController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers
{
    using System;
    using System.Security.Principal;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Hangfire;
    using Hangfire.States;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Job;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Service;

    /// <summary>
    /// The API controller used to manage hangfire.
    /// </summary>
    public class HangfiresController : BiaControllerBase
    {
        private readonly INotificationDomainService notificationAppService;
        private readonly IBiaDemoTestHangfireService demoTestHangfireService;
        private readonly BIAClaimsPrincipal principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="HangfiresController"/> class.
        /// </summary>
        /// <param name="demoTestHangfireService">The demo service.</param>
        /// <param name="notificationAppService">The notification service.</param>
        /// <param name="principal">The principal.</param>
        public HangfiresController(
            IBiaDemoTestHangfireService demoTestHangfireService,
            INotificationDomainService notificationAppService,
            IPrincipal principal)
        {
            this.notificationAppService = notificationAppService;
            this.demoTestHangfireService = demoTestHangfireService;
            this.principal = principal as BIAClaimsPrincipal;
        }

        /// <summary>
        /// Call a hangfire task.
        /// </summary>
        /// <returns>Return the statut.</returns>
        [HttpPut("callworker")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Hangfires.RunWorker)]
        public IActionResult CallWorker()
        {
            try
            {
                var client = new BackgroundJobClient();
                client.Create<BiaDemoTestHangfireService>(x => x.Run(), new EnqueuedState(/*BIAQueueAttribute.QueueName*/));

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
        /// Call a hangfire task.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <returns>Return the statut.</returns>
        [HttpPut("callworkerwithnotification/{teamId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Hangfires.RunWorker)]
        public IActionResult CallWorkerWithNotification(int teamId)
        {
            try
            {
                var client = new BackgroundJobClient();
                client.Create<BiaDemoTestHangfireService>(x => x.RunLongTaskWithNotification(teamId, this.principal.GetUserId(), null), new EnqueuedState());

                return this.Ok("Operation being processed in background...");
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