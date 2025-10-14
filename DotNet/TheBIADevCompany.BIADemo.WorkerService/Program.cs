// <copyright file="Program.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.WorkerService
{
    using System;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NLog;
    using NLog.Extensions.Hosting;
    using NLog.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;

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
                AppContext.SetSwitch(BiaConstants.AppContextSwitch.Npgsql.EnableLegacyTimestampBehavior, true);

                IHostBuilder builder = Host.CreateDefaultBuilder(args);
                Startup startup;

                IHost host = builder.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    string environment = Environment.GetEnvironmentVariable(Constants.Application.Environment);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile("bianetconfig.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"bianetconfig.{environment}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    IConfiguration configuration = hostingContext.Configuration;
                    LogManager.Configuration = new NLogLoggingConfiguration(configuration.GetSection("NLog"));
                    LogManager.GetCurrentClassLogger().Info($"{Constants.Application.Environment}: {Environment.GetEnvironmentVariable(Constants.Application.Environment)}");
                })
                .ConfigureServices((hostingContext, services) =>
                {
#if BIA_FRONT_FEATURE
                    // Begin BIADemo
                    services.AddHostedService<Worker>();

                    // End BIADemo
#endif
                    IConfiguration configuration = hostingContext.Configuration;
                    startup = new Startup(configuration);
                    startup.ConfigureServices(services);
                })
                .UseNLog()
                .UseWindowsService()
                .Build();

                var config = host.Services.GetRequiredService<IConfiguration>();
                Startup.Configure(host, config);

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                // NLog: catch setup errors
                var logger = LogManager.GetCurrentClassLogger();
                logger.Error(ex, "Stopped program because of exception");
                throw new BIA.Net.Core.Common.Exceptions.SystemException("Stopped program because of exception. See log file", ex);
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit
                LogManager.Shutdown();
            }
        }
    }
}