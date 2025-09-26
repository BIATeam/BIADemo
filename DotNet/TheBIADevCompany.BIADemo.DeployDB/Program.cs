// <copyright file="Program.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.DeployDB
{
    using System;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Archive;
    using BIA.Net.Core.Application.Clean;
#if BIA_FRONT_FEATURE
    using BIA.Net.Core.Application.Job;
    using BIA.Net.Core.Common;
#endif
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Infrastructure.Data;
    using Hangfire;
    using Hangfire.PostgreSql;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NLog;
    using NLog.Extensions.Hosting;
    using NLog.Extensions.Logging;

#if BIA_FRONT_FEATURE
    using TheBIADevCompany.BIADemo.Application.Job;

#endif
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
            AppContext.SetSwitch(BiaConstants.AppContextSwitch.Npgsql.EnableLegacyTimestampBehavior, true);

            var env = Environment.GetEnvironmentVariable(Constants.Application.Environment);
            await new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile("bianetconfig.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"bianetconfig.{env}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    IConfiguration configuration = hostingContext.Configuration;

                    string connectionString = configuration.GetDatabaseConnectionString(BiaConstants.DatabaseConfiguration.DefaultKey);

                    if (!string.IsNullOrWhiteSpace(connectionString))
                    {
                        DbProvider dbEngine = configuration.GetProvider(BiaConstants.DatabaseConfiguration.DefaultKey);

                        if (dbEngine == DbProvider.PostGreSql)
                        {
                            services.AddDbContext<IDbContextDatabase, DataContextPostGreSql>(options =>
                            {
                                options.UseNpgsql(connectionString);
                            });
                        }
                        else
                        {
                            services.AddDbContext<IDbContextDatabase, DataContext>(options =>
                            {
                                options.UseSqlServer(connectionString);
                            });
                        }

                        services.AddDbContext<IQueryableUnitOfWork, DataContext>(options =>
                        {
                            if (dbEngine == DbProvider.PostGreSql)
                            {
                                options.UseNpgsql(connectionString);
                            }
                            else
                            {
                                options.UseSqlServer(connectionString);
                            }
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
                            if (connectionString != null)
                            {
                                if (dbEngine == DbProvider.PostGreSql)
                                {
                                    config.UsePostgreSqlStorage(options => options.UseNpgsqlConnection(connectionString));
                                }
                                else
                                {
                                    config.UseSqlServerStorage(connectionString);
                                }
                            }

                            // Initialize here the recuring jobs
#if BIA_FRONT_FEATURE
                            string projectName = configuration["Project:Name"];
                            RecurringJob.AddOrUpdate<WakeUpTask>($"{projectName}.{typeof(WakeUpTask).Name}", t => t.Run(), configuration["Tasks:WakeUp:CRON"]);
                            RecurringJob.AddOrUpdate<SynchronizeUserTask>($"{projectName}.{typeof(SynchronizeUserTask).Name}", t => t.Run(), configuration["Tasks:SynchronizeUser:CRON"]);

                            // Begin BIADemo
                            RecurringJob.AddOrUpdate<WithPermissionTask>($"{projectName}.{typeof(WithPermissionTask).Name}", t => t.Run(), Cron.Never);
                            RecurringJob.AddOrUpdate<EngineManageTask>($"{projectName}.{typeof(EngineManageTask).Name}", t => t.Run(), Cron.Never);
                            RecurringJob.AddOrUpdate<ArchiveTask>($"{projectName}.{typeof(ArchiveTask).Name}", t => t.Run(), configuration["Tasks:Archive:CRON"]);
                            RecurringJob.AddOrUpdate<CleanTask>($"{projectName}.{typeof(CleanTask).Name}", t => t.Run(), configuration["Tasks:Clean:CRON"]);

                            // End BIADemo
#endif
                        });
                    }
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
