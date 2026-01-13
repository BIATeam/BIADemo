// <copyright file="RecuringJobsHelper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.WorkerService.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Hangfire;
    using Hangfire.Storage;
    using Hangfire.Storage.Monitoring;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Helper for recurring jobs.
    /// </summary>
    public static class RecuringJobsHelper
    {
        /// <summary>
        /// Initialize the Background server.
        /// </summary>
        /// <param name="queueName">name of the queue (use the short project name to Lower).</param>
        public static void CleanHangfireServerQueue(string queueName = null)
        {
            using (var connection = JobStorage.Current.GetConnection())
            {
                var recurringJobs = connection.GetRecurringJobs();

                if (recurringJobs != null)
                {
                    foreach (var item in recurringJobs)
                    {
                        if (string.IsNullOrEmpty(queueName) || item.Queue == queueName)
                        {
                            RecurringJob.RemoveIfExists(item.Id);
                        }
                    }
                }
            }

            var mon = JobStorage.Current.GetMonitoringApi();
            List<string> jobsToCheck = new();

            mon.ProcessingJobs(0, int.MaxValue).ToList().ForEach(x => jobsToCheck.Add(x.Key));

            List<QueueWithTopEnqueuedJobsDto> queues;
            if (string.IsNullOrEmpty(queueName))
            {
                queues = mon.Queues().ToList();
            }
            else
            {
                queues = mon.Queues().Where(x => x.Name == queueName).ToList();
            }

            foreach (var queue in queues)
            {
                for (var i = 0; i < Math.Ceiling(queue.Length / 1000d); i++)
                {
                    mon.EnqueuedJobs(queue.Name, 1000 * i, 1000)
                        .ForEach(x => jobsToCheck.Add(x.Key));
                }
            }

            var jobInstanceIdsToDelete = new List<string>();

            foreach (var jobKey in jobsToCheck)
            {
                if (string.IsNullOrEmpty(queueName))
                {
                    jobInstanceIdsToDelete.Add(jobKey);
                }
                else
                {
                    var job = mon.JobDetails(jobKey);

                    if (job != null)
                    {
                        var history = job.History.FirstOrDefault(h => h.StateName == "Enqueued");
                        if (history != null)
                        {
                            var queueHisto = history.Data["Queue"];
                            if (queueHisto == queueName)
                            {
                                jobInstanceIdsToDelete.Add(jobKey);
                            }
                        }
                    }
                }
            }

            foreach (var id in jobInstanceIdsToDelete)
            {
                BackgroundJob.Delete(id);
            }
        }

        /// <summary>
        /// Purge all pending Job in Queue.
        /// </summary>
        /// <param name="queueName">name of the queue.</param>
        public static void PurgeQueue(string queueName)
        {
            var toDelete = new List<string>();
            var monitor = JobStorage.Current.GetMonitoringApi();

            var queue = monitor.Queues().First(x => x.Name == queueName);
            for (var i = 0; i < Math.Ceiling(queue.Length / 1000d); i++)
            {
                monitor.EnqueuedJobs(queue.Name, 1000 * i, 1000)
                    .ForEach(x => toDelete.Add(x.Key));
            }

            foreach (var jobId in toDelete)
            {
                BackgroundJob.Delete(jobId);
            }
        }
    }
}
