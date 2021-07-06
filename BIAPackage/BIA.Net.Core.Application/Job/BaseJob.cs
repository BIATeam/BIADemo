// <copyright file="BaseJob.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Job
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Helpers;
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
        /// The logger.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Application configuration.
        /// </summary>
        protected IConfiguration Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseJob"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        public BaseJob(IConfiguration configuration, ILogger logger)
        {
            this.Configuration = configuration;
            this.Logger = logger;
            this.projectName = this.Configuration["Project:Name"];
        }


        /// <summary>
        /// Execute the purpose of this job.
        /// </summary>
        /// <returns>The <see cref="Task"/> representing the operation to perform.</returns>
        public void Run()
        {
            string taskName = this.GetType().Name;
            this.Logger.LogInformation(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": Begin " + taskName);
            try
            {
                Task t = Task.Run(() => RunMonitoredTask());
                t.Wait();
                this.Logger.LogInformation("Task {0} Status: {1}", t.Id, t.Status);
                this.Logger.LogInformation(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": End " + taskName + " with Success.");
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + ": End " + taskName + " with Fail.");
                throw;
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
            throw new Exception("Method RunMonitoredTask should be define in " + this.GetType().Name + ".");
        }

        /// <summary>
        /// Log details about every recurring job.
        /// </summary>
        private void LogAllJobsDetails()
        {
            using (var connection = JobStorage.Current.GetConnection())
            {
                var recurringJobs = connection.GetRecurringJobs();

                if (recurringJobs == null || recurringJobs.Count() == 0)
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
                var job = connection.GetRecurringJobs().FirstOrDefault(j => j.Id.Equals(id));

                if (job == null)
                {
                    this.Logger.LogInformation($"No recurring job '{id}'.");
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
                this.Logger.LogInformation($"Job '{job.Id}' has not been scheduled yet. Check again later.");
            }
            else
            {
                this.Logger.LogInformation($"Job '{job.Id}':{Environment.NewLine}" +
                    $"- Created at: {job.CreatedAt?.ToLocalTime()}{Environment.NewLine}" +
                    $"- CRON: {job.Cron}{Environment.NewLine}" +
                    $"- Error: {job.Error}{Environment.NewLine}" +
                    $"- LastExecution: {job.LastExecution?.ToLocalTime()}{Environment.NewLine}" +
                    $"- Next execution: {job.NextExecution.Value.ToLocalTime()}{Environment.NewLine}" +
                    $"- Queue: {job.Queue}");
            }
        }
    }
}