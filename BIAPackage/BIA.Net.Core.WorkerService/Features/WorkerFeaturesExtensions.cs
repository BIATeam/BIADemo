namespace BIA.Net.Core.WorkerService.Features
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Hangfire;
    using Microsoft.AspNetCore.Builder;
    using Community.Microsoft.Extensions.Caching.PostgreSql;
    using Hangfire.PostgreSql;
    using Hangfire.Dashboard;
    using BIA.Net.Core.Common.Configuration.WorkerFeature;
    using System.Collections.Generic;
    using BIA.Net.Core.WorkerService.Features.HangfireServer;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Presentation.Common.Authentication;

    /// <summary>
    /// Add the standard service.
    /// </summary>
    public static class WorkerFeaturesExtensions
    {
        /// <summary>
        /// Start the Worker service features.
        /// </summary>
        /// <param name="services">the service collection.</param>
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

            // Local memory cache
            services.AddMemoryCache();

            // Distributed Cache
            if (workerFeatures?.DistributedCache?.IsActive == true)
            {
                string dbEngine = configuration.GetDBEngine(workerFeatures.DistributedCache.ConnectionStringName);
                if (dbEngine.ToLower().Equals("sqlserver"))
                {
                    services.AddDistributedSqlServerCache(config =>
                    {
                        config.ConnectionString = configuration.GetConnectionString(workerFeatures.DistributedCache.ConnectionStringName);
                        config.TableName = "DistCache";
                        config.SchemaName = "dbo";
                    });
                }
                else if (dbEngine.ToLower().Equals("postgresql"))
                {
                    services.AddDistributedPostgreSqlCache(config =>
                    {
                        config.ConnectionString = configuration.GetConnectionString(workerFeatures.DistributedCache.ConnectionStringName);
                        config.TableName = "DistCache";
                        config.SchemaName = "dbo";
                    });
                }

                services.AddMemoryCache();
            }

            // Database Handler
            if (workerFeatures.DatabaseHandler.IsActive)
            {
                services.AddTransient<IHostedService, DataBaseHandlerService>(provider =>
                {
                    return new DataBaseHandlerService(databaseHandlerRepositories);
                });
            }

            // Client for hub            
            /*if (ClientForHubOptions.IsActive)
            {
                services.AddTransient<IHostedService, ClientForHubService>(provider =>
                {
                    return new ClientForHubService(options.ClientForHub);
                });
            }*/

            // Hangfire Server            
            if (workerFeatures.HangfireServer.IsActive)
            {
                services.AddHangfireServer(hfOptions =>
                {
                    hfOptions.ServerName = workerFeatures.HangfireServer.ServerName;
                });
                services.AddHangfire(config =>
                {
                    string dbEngine = configuration.GetDBEngine(workerFeatures.DistributedCache.ConnectionStringName);
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
                });
            }

            return services;
        }

        public static IApplicationBuilder UseBiaWorkerFeatures<AuditFeature>([NotNull] this IApplicationBuilder app,
            WorkerFeatures workerFeatures, HangfireServerAuthorizations hangfireServerAuthorizations) where AuditFeature : IAuditFeature
        {
            // Hangfire Server
            if (workerFeatures?.HangfireServer?.IsActive == true)
            {
                //app.UseHangfireDashboard();

                //app.UseEndpoints(endpoints =>
                //{
                //    endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions()
                //    {
                //        Authorization =  new[] { new HangfireAuthorizationFilter() }
                //    })
                //    /*.RequireAuthorization(HangfirePolicyName)*/;
                //});

                app.UseHangfireDashboardCustomOptions(new HangfireDashboardCustomOptions
                {
                    DashboardTitle = () => workerFeatures.HangfireServer.ServerName,
                });
                app.UseHangfireDashboard("/hangfireAdmin", new DashboardOptions
                {
                    Authorization = hangfireServerAuthorizations.Authorization
                });
                app.UseHangfireDashboard("/hangfire", new DashboardOptions
                {
                    IsReadOnlyFunc = (DashboardContext context) => true,
                    Authorization = hangfireServerAuthorizations.AuthorizationReadOnly
                });
            }

            app.ApplicationServices.GetRequiredService<AuditFeature>().
                UseAuditFeatures(app.ApplicationServices);

            return app;
        }
    }
}
