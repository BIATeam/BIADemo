// <copyright file="Program.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.DeployDB
{
    using System;
    using System.Threading.Tasks;
    using Hangfire;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NLog;
    using NLog.Extensions.Hosting;
    using NLog.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Application.Job;
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
            await new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    IConfiguration configuration = hostingContext.Configuration;

                    // BIAToolKit - Begin ModelFirst
                    services.AddDbContext<DataContext>(options =>
                    {
                        options.UseSqlServer(configuration.GetConnectionString("BIADemoDatabase"));
                    });
                    services.AddHostedService<DeployDBService>();

                    // BIAToolKit - End ModelFirst

                    // Comment those lines if you do not use hangfire
                    services.AddHangfireServer(hfOptions =>
                    {
                        hfOptions.ServerName = "DeployHangfireDB";
                        hfOptions.Queues = new string[] { "Deploy" };
                    });
                    services.AddHangfire(config =>
                    {
                        config.UseSqlServerStorage(configuration.GetConnectionString("BIADemoDatabase"));
                        string projectName = configuration["Project:Name"];

                        // Initialize here the recuring jobs
                        RecurringJob.AddOrUpdate<WakeUpTask>($"{projectName}.{typeof(WakeUpTask).Name}", t => t.Run(), configuration["Tasks:WakeUp:CRON"]);
                        RecurringJob.AddOrUpdate<SynchronizeUserTask>($"{projectName}.{typeof(SynchronizeUserTask).Name}", t => t.Run(), configuration["Tasks:SynchronizeUser:CRON"]);

                        // Begin BIADemo
                        RecurringJob.AddOrUpdate<WithPermissionTask>($"{projectName}.{typeof(WithPermissionTask).Name}", t => t.Run(), Cron.Never);
                        RecurringJob.AddOrUpdate<EngineManageTask>($"{projectName}.{typeof(EngineManageTask).Name}", t => t.Run(), Cron.Never);

                        // End BIADemo
                    });
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    IConfiguration configuration = hostingContext.Configuration;
                    LogManager.Configuration = new NLogLoggingConfiguration(configuration.GetSection("NLog"));
                })
                .UseNLog()
                .RunConsoleAsync();
        }
    }
}
