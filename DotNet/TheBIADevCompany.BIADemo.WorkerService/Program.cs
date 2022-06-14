// <copyright file="Program.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService
{
    using System;
    using System.Collections.Generic;
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

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.WorkerService.Features;

    // End BIADemo

    /// <summary>
    /// The base program class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main method that start the project.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            try
            {
                /// CreateWebHostBuilder(args).Build().Run();

                // If use as a windows service: CreateHostBuilder(args).Build().Run()


                WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
                builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
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
                .UseNLog();

                Startup startup = new Startup(builder.Configuration, builder.Environment);

                startup.ConfigureServices(builder.Services);

                WebApplication app = builder.Build();

                startup.Configure(app, app.Environment);

                app.Run();

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

        /// <summary>
        /// Create the web host builder.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The web host builder.</returns>
        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
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
                .UseNLog()
                .UseStartup<Startup>();

        /// <summary>
        /// Create the host builder.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The web host builder.</returns>
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
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
                .UseNLog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("https://*:8081", "http://*:8080");
                    webBuilder.UseStartup<Startup>();
                })
                .UseWindowsService();
    }
}