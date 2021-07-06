// <copyright file="Program.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService
{
    using System;

    // Begin BIADemo
    using System.Collections.Generic;
    using BIA.Net.Core.Common.Configuration;

    // End BIADemo
    using BIA.Net.Core.WorkerService.Features;

    // Begin BIADemo
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;

    // End BIADemo
    using Microsoft.AspNetCore;
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
                CreateWebHostBuilder(args).Build().Run();

                // If use as a windows service: CreateHostBuilder(args).Build().Run()
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
                .UseStartup<Startup>()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddBiaWorkerFeatures(config =>
                    {
                        config.Configuration = hostContext.Configuration;

                        // Begin BIADemo
                        var biaNetSection = new BiaNetSection();
                        config.Configuration.GetSection("BiaNet").Bind(biaNetSection);

                        if (biaNetSection.WorkerFeatures.DatabaseHandler.IsActive)
                        {
                            config.DatabaseHandler.Activate(new List<DatabaseHandlerRepository>()
                            {
                                new PlaneHandlerRepository(hostContext.Configuration),
                            });
                        }

                        // End BIADemo
                    });

                    services.AddHostedService<Worker>();
                });

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
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddBiaWorkerFeatures(config =>
                    {
                        config.Configuration = hostContext.Configuration;

                        // Begin BIADemo
                        var biaNetSection = new BiaNetSection();
                        config.Configuration.GetSection("BiaNet").Bind(biaNetSection);

                        if (biaNetSection.WorkerFeatures.DatabaseHandler.IsActive)
                        {
                            config.DatabaseHandler.Activate(new List<DatabaseHandlerRepository>()
                            {
                                new PlaneHandlerRepository(hostContext.Configuration),
                            });
                        }

                        // End BIADemo
                    });

                    services.AddHostedService<Worker>();
                })
                .UseWindowsService();
    }
}