// <copyright file="Program.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.DeployDB
{
    using System;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Archive;
    using BIA.Net.Core.Common.Configuration;
    using Hangfire;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NLog;
    using NLog.Extensions.Hosting;
    using NLog.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Application.Job;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Infrastructure.Data;

    /// <summary>
    /// The base program class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main method that start the project.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>Task.</returns>
        public static async Task Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable(Constants.Application.Environment);
            await new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile("bianetconfig.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"bianetconfig.{env}.json", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    IConfiguration configuration = hostingContext.Configuration;

                    services.AddDbContext<DataContext>(options =>
                    {
                        options.UseSqlServer(configuration.GetDatabaseConnectionString("ProjectDatabase"));
                    });
                    services.AddHostedService<DeployDBService>();

                    // Comment those lines if you do not use hangfire
                    services.AddHangfireServer(hfOptions =>
                    {
                        hfOptions.ServerName = "DeployHangfireDB";
                        hfOptions.Queues = new string[] { "Deploy" };
                    });
                    services.AddHangfire(config =>
                    {
                        config.UseSqlServerStorage(configuration.GetDatabaseConnectionString("ProjectDatabase"));

                        // Initialize here the recuring jobs
#if BIA_FRONT_FEATURE
                        string projectName = configuration["Project:Name"];
                        RecurringJob.AddOrUpdate<WakeUpTask>($"{projectName}.{typeof(WakeUpTask).Name}", t => t.Run(), configuration["Tasks:WakeUp:CRON"]);
                        RecurringJob.AddOrUpdate<SynchronizeUserTask>($"{projectName}.{typeof(SynchronizeUserTask).Name}", t => t.Run(), configuration["Tasks:SynchronizeUser:CRON"]);

                        // Begin BIADemo
                        RecurringJob.AddOrUpdate<WithPermissionTask>($"{projectName}.{typeof(WithPermissionTask).Name}", t => t.Run(), Cron.Never);
                        RecurringJob.AddOrUpdate<EngineManageTask>($"{projectName}.{typeof(EngineManageTask).Name}", t => t.Run(), Cron.Never);
                        RecurringJob.AddOrUpdate<ArchiveTask>($"{projectName}.{typeof(ArchiveTask).Name}", t => t.Run(), configuration["Tasks:Archive:CRON"]);

                        // End BIADemo
#endif
                    });
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    IConfiguration configuration = hostingContext.Configuration;
                    LogManager.Configuration = new NLogLoggingConfiguration(configuration.GetSection("NLog"));
                    LogManager.GetCurrentClassLogger().Info($"{Constants.Application.Environment}: {Environment.GetEnvironmentVariable(Constants.Application.Environment)}");
                })
                .UseNLog()
                .RunConsoleAsync();
        }
    }
}
