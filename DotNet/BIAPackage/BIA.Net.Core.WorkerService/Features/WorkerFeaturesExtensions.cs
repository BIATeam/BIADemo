// <copyright file="WorkerFeaturesExtensions.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.WorkerService.Features
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Configuration.WorkerFeature;
    using BIA.Net.Core.Presentation.Common.Authentication;
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using Hangfire;
    using Hangfire.PerformContextAccessor;
    using Hangfire.PostgreSql;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Add the standard service.
    /// </summary>
    public static class WorkerFeaturesExtensions
    {
        /// <summary>
        /// Start the Worker service features.
        /// </summary>
        /// <param name="services">the service collection.</param>
        /// <param name="workerFeatures">the worker Features.</param>
        /// <param name="configuration">the application configuration.</param>
        /// <param name="databaseHandlerRepositories">the list of handler repositories.</param>
        /// <returns>the services collection.</returns>
        public static IServiceCollection AddBiaWorkerFeatures(
            [NotNull] this IServiceCollection services,
            WorkerFeatures workerFeatures,
            IConfiguration configuration,
            List<DatabaseHandlerRepository> databaseHandlerRepositories)
        {
            var biaNetSection = new BiaNetSection();
            configuration.GetSection("BiaNet").Bind(biaNetSection);

            // Authentication
            services.ConfigureAuthentication(biaNetSection);

            // Database Handler
            if (workerFeatures.DatabaseHandler.IsActive)
            {
                services.AddTransient<IHostedService, DataBaseHandlerService>(provider =>
                {
                    return new DataBaseHandlerService(databaseHandlerRepositories);
                });
            }

            // Hangfire Server
            if (workerFeatures.HangfireServer.IsActive)
            {
                // Log in hangfire dashboard
                if (workerFeatures.HangfireServer.LogsVisibleInDashboard)
                {
                    services.AddHangfirePerformContextAccessor();
                }

                services.AddHangfireServer(hfOptions =>
                {
                    hfOptions.ServerName = workerFeatures.HangfireServer.ServerName;
                });
                services.AddHangfire((serviceProvider, config) =>
                {
                    string dbEngine = configuration.GetDBEngine(workerFeatures.HangfireServer.ConnectionStringName);
                    if (dbEngine.ToLower().Equals("sqlserver"))
                    {
                        config.UseSimpleAssemblyNameTypeSerializer()
                              .UseRecommendedSerializerSettings()
                              .UseSqlServerStorage(configuration.GetConnectionString(workerFeatures.HangfireServer.ConnectionStringName));
                    }
                    else if (dbEngine.ToLower().Equals("postgresql"))
                    {
                        var optionsTime = new PostgreSqlStorageOptions
                        {
                            InvisibilityTimeout = TimeSpan.FromDays(5),
                        };

                        config.UseSimpleAssemblyNameTypeSerializer()
                              .UseRecommendedSerializerSettings()
                              .UsePostgreSqlStorage(configuration.GetConnectionString(workerFeatures.HangfireServer.ConnectionStringName), optionsTime);
                    }

                    // Log in hangfire dashboard
                    if (workerFeatures.HangfireServer.LogsVisibleInDashboard)
                    {
                        config.UsePerformContextAccessorFilter(serviceProvider);
                    }
                });
            }

            return services;
        }
    }
}
