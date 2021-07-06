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
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    IConfiguration configuration = hostingContext.Configuration;
                    services.AddDbContext<DataContext>(options =>
                    {
                        options.UseSqlServer(configuration.GetConnectionString("BIADemoDatabase"));
                    });
                    services.AddHostedService<DeployDBService>();

                    // Comment those lines if you do not use hangfire
                    services.AddHangfireServer(hfOptions =>
                    {
                        hfOptions.ServerName = "DeployHangfireDB";
                    });
                    services.AddHangfire(config =>
                    {
                        config
                            .UseSqlServerStorage(
                               configuration.GetConnectionString("BIADemoDatabase"));
                    });
                })
                .RunConsoleAsync();
        }
    }
}
