// BIADemo only
// <copyright file="HangfiresController.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Presentation.Api.Controllers.Example
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
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;

    /// <summary>
    /// The API controller used to manage hangfire.
    /// </summary>
    public class HangfiresController : BiaControllerBase
    {
        private readonly BiaClaimsPrincipal principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="HangfiresController"/> class.
        /// </summary>
        /// <param name="principal">The principal.</param>
        public HangfiresController(
            IPrincipal principal)
        {
            this.principal = principal as BiaClaimsPrincipal;
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
        }

        /// <summary>
        /// Call a hangfire task.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <returns>Return the statut.</returns>
        [HttpPut("randomReviewPlane/{teamId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = Rights.Hangfires.RunWorker)]
        public IActionResult RandomReviewPlane(int teamId)
        {
            try
            {
                var client = new BackgroundJobClient();
                client.Create<BiaDemoTestHangfireService>(x => x.RandomReviewPlane(teamId, this.principal.GetUserData<UserDataDto>().GetCurrentTeam((int)TeamTypeId.Site), this.principal.GetUserId(), null), new EnqueuedState());

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
        }
    }
}