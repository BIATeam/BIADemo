// BIADemo only
// <copyright file="IBiaDemoTestHangfireService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Application.Job
{
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.User;
    using Hangfire.Server;

    /// <summary>
    /// Sample class to use a hangfire task.
    /// </summary>
    public interface IBiaDemoTestHangfireService
    {
        /// <summary>
        /// Run a long task.
        /// </summary>
        /// <returns>The Task.</returns>
        Task RunLongTask();

        /// <summary>
        /// Run a long task and then sends a notification to audience.
        /// </summary>
        /// <param name="teamId">The team id.</param>
        /// <param name="currentSite">The current Site.</param>
        /// <param name="createdById">The creator user id.</param>
        /// <param name="context">The job context.</param>
        /// <returns>The Task.</returns>
        Task RandomReviewPlane(int teamId, CurrentTeamDto currentSite, int createdById, PerformContext context);
    }
}