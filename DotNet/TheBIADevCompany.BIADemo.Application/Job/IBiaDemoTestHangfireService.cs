// BIADemo only
// <copyright file="BiaDemoTestHangfire.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Application.Job
{
    using System.Threading.Tasks;
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
        /// <param name="settings">The notification settings.</param>
        /// <param name="context">The job context.</param>
        /// <returns>The Task.</returns>
        Task RunLongTaskWithNotification(int siteId, int createdById, PerformContext context);
    }
}