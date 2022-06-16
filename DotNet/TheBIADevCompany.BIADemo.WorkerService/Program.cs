// <copyright file="Program.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.WorkerService.Features;
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NLog;
    using NLog.Extensions.Logging;
    using NLog.Web;


    /// <summary>
    /// The base program class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main method that start the project.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The async task.</returns>
        public static async Task Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            try
            {
                IHostBuilder builder = Host.CreateDefaultBuilder(args);
                Startup startup;

                IHost host = builder.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile("bianetconfig.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"bianetconfig.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    IConfiguration configuration = hostingContext.Configuration;
                    LogManager.Configuration = new NLogLoggingConfiguration(configuration.GetSection("NLog"));
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    IConfiguration configuration = hostingContext.Configuration;
                    startup = new Startup(configuration);
                    startup.ConfigureServices(services);
                })
                .UseNLog()
                .UseWindowsService()
                .Build();

                Startup.Configure(host);

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                // NLog: catch setup errors
                var logger = NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit
                LogManager.Shutdown();
            }
        }
    }
}