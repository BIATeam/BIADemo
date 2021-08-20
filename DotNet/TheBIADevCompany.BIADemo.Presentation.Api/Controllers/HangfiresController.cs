// BIADemo only
// <copyright file="HangfiresController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers
{
    using System;
    using System.DirectoryServices.AccountManagement;
    using System.Security.Principal;
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Presentation.Api.Controllers.Base;
    using Hangfire;
    using Hangfire.States;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using TheBIADevCompany.BIADemo.Application.Job;
    using TheBIADevCompany.BIADemo.Application.Notification;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Domain.Dto.Notification;

    /// <summary>
    /// The API controller used to manage planes.
    /// </summary>
    public class HangfiresController : BiaControllerBase
    {
        private readonly IBackgroundJobClient backgroundJobClient;
        private readonly INotificationAppService notificationAppService;
        private readonly IBiaDemoTestHangfireService demoTestHangfireService;
        private readonly BIAClaimsPrincipal principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="HangfiresController"/> class.
        /// </summary>
        public HangfiresController(
            IBackgroundJobClient backgroundJobClient,
            IBiaDemoTestHangfireService demoTestHangfireService,
            INotificationAppService notificationAppService,
            IPrincipal principal)
        {
            this.backgroundJobClient = backgroundJobClient;
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
        /// <returns>Return the statut.</returns>
        [HttpPut("callworkerwithnotification")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Hangfires.RunWorker)]
        public IActionResult CallWorkerWithNotification()
        {
            try
            {
                var userId = this.principal.GetUserId();

                var jobId = this.backgroundJobClient.Enqueue(() => this.demoTestHangfireService.RunLongTask());
                var notification = new NotificationDto
                {
                    JobId = jobId,
                    CreatedById = userId,
                    CreatedDate = DateTime.Now,
                    Description = "Description",
                    SiteId = this.principal.GetUserData<UserDataDto>().CurrentSiteId,
                    Title = "Title",
                };

                this.notificationAppService.AddAsync(notification);

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
    }
}