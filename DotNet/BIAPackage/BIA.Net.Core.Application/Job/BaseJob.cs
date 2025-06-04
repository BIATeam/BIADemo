// <copyright file="BaseJob.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Job
{
    using System;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Exceptions;
    using Hangfire;
    using Hangfire.Storage;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The base Job Service.
    /// </summary>
    public abstract class BaseJob
    {
        /// <summary>
        /// The project name.
        /// </summary>
        private readonly string projectName;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseJob"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        protected BaseJob(IConfiguration configuration, ILogger logger)
        {
            this.Configuration = configuration;
            this.Logger = logger;
            this.projectName = this.Configuration["Project:Name"];
        }

        /// <summary>
        /// The logger.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Application configuration.
        /// </summary>
        protected IConfiguration Configuration { get; }

        /// <summary>
        /// Execute the purpose of this job.
        /// </summary>
        public void Run()
        {
            string taskName = this.GetType().Name;
            string message = "{time}: Begin {taskName}";
            this.Logger.LogInformation(message, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), taskName);
            try
            {
                Task t = Task.Run(() => this.RunMonitoredTask());
                t.Wait();
                message = "Task {id} Status: {status}";
                this.Logger.LogInformation(message, t.Id, t.Status);
                message = "{time}: End {taskName} with Success.";
                this.Logger.LogInformation(message, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), taskName);
            }
            catch (Exception ex)
            {
                message = "{time}: End {taskName} with Fail.";
                this.Logger.LogError(ex, message, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), taskName);
                throw new JobException("Job Run Failed", ex);
            }
            finally
            {
                this.LogJobDetails($"{this.projectName}.{this.GetType().Name}");
            }
        }

        /// <summary>
        /// Run the monitored Task.
        /// </summary>
        /// <returns>the task async.</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected virtual async Task RunMonitoredTask()
        {
            throw new JobException($"Method RunMonitoredTask should be define in {this.GetType().Name}.");
        }

        /// <summary>
        /// Log details about every recurring job.
        /// </summary>
        protected void LogAllJobsDetails()
        {
            using (var connection = JobStorage.Current.GetConnection())
            {
                var recurringJobs = connection.GetRecurringJobs();

                if (recurringJobs == null || recurringJobs.Count == 0)
                {
                    this.Logger.LogInformation("No recurring jobs.");
                }
                else
                {
                    foreach (var job in recurringJobs)
                    {
                        this.LogJobDetails(job);
                    }
                }
            }
        }

        /// <summary>
        /// Log details about the given job.
        /// </summary>
        /// <param name="id">The job identifier.</param>
        private void LogJobDetails(string id)
        {
            using (var connection = JobStorage.Current.GetConnection())
            {
                var job = connection.GetRecurringJobs().Find(j => j.Id.Equals(id));

                if (job == null)
                {
                    string message = $"No recurring job '{id}'.";
                    this.Logger.LogInformation(message);
                }
                else
                {
                    this.LogJobDetails(job);
                }
            }
        }

        /// <summary>
        /// Log details about the given job.
        /// </summary>
        /// <param name="job">The job.</param>
        private void LogJobDetails(RecurringJobDto job)
        {
            if (!job.NextExecution.HasValue)
            {
                string message = $"Job '{job.Id}' has not been scheduled yet. Check again later.";
                this.Logger.LogInformation(message);
            }
            else
            {
                string message = $"Job '{job.Id}':{Environment.NewLine}" +
                    $"- Created at: {job.CreatedAt?.ToLocalTime()}{Environment.NewLine}" +
                    $"- CRON: {job.Cron}{Environment.NewLine}" +
                    $"- Error: {job.Error}{Environment.NewLine}" +
                    $"- LastExecution: {job.LastExecution?.ToLocalTime()}{Environment.NewLine}" +
                    $"- Next execution: {job.NextExecution.Value.ToLocalTime()}{Environment.NewLine}" +
                    $"- Queue: {job.Queue}";
                this.Logger.LogInformation(message);
            }
        }
    }
}