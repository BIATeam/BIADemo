namespace BIA.Net.Core.WorkerService.Features
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using BIA.Net.Core.WorkerService.Features.ClientForHub;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Hangfire;
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// Add the standard service.
    /// </summary>
    public static class WorkerFeaturesExtensions
    {
        /// <summary>
        /// Start the Worker service features.
        /// </summary>
        /// <param name="services">the service collection.</param>
        /// <param name="optionsAction">the options.</param>
        /// <returns>the services collection.</returns>
        public static IServiceCollection AddBiaWorkerFeatures(
            [NotNull] this IServiceCollection services,
            Action<WorkerFeaturesServiceOptions> optionsAction)
        {

            var options = new WorkerFeaturesServiceOptions();
            optionsAction(options);

            var biaNetSection = new BiaNetSection();
            options.Configuration.GetSection("BiaNet").Bind(biaNetSection);

            // Local memory cache
            services.AddMemoryCache();



            // Distributed Cache
            if (biaNetSection?.WorkerFeatures?.DistributedCache?.IsActive == true)
            {
                // Distributed Cache configuration copy from configuration file to options if defined
                options.DistributedCache = biaNetSection.WorkerFeatures.DistributedCache;
            }
            if (options.DistributedCache.IsActive)
            {
                services.AddDistributedSqlServerCache(config =>
                {
                    config.ConnectionString = options.Configuration.GetConnectionString(options.DistributedCache.ConnectionStringName);
                    config.TableName = "DistCache";
                    config.SchemaName = "dbo";
                });
                services.AddMemoryCache();
            }

            // Database Handler
            if (options.DatabaseHandler.IsActive)
            {
                services.AddTransient<IHostedService, DataBaseHandlerService>(provider =>
                {
                    return new DataBaseHandlerService(options.DatabaseHandler);
                });
            }

            // Client for hub
            if (biaNetSection?.WorkerFeatures?.ClientForHub?.IsActive == true)
            {
                options.ClientForHub.Activate(biaNetSection.WorkerFeatures.ClientForHub.SignalRUrl);
            }
            if (options.ClientForHub.IsActive)
            {
                services.AddTransient<IHostedService, ClientForHubService>(provider =>
                {
                    return new ClientForHubService(options.ClientForHub);
                });
            }

            // Hangfire Server
            if (biaNetSection?.WorkerFeatures?.HangfireServer?.IsActive == true)
            {
                // HangfireServer configuration copy from configuration file to options if defined
                options.Configuration.GetSection("BiaNet:WorkerFeatures:HangfireServer").Bind(options.HangfireServer);
                //options.HangfireServer.bind(biaNetSection.WorkerFeatures.HangfireServer);
            }
            if (options.HangfireServer.IsActive)
            {
                services.AddHangfireServer(hfOptions =>
                {
                    hfOptions.ServerName = options.HangfireServer.ServerName;
                });
                services.AddHangfire(config =>
                {
                    config
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseSqlServerStorage(
                           options.Configuration.GetConnectionString(options.HangfireServer.ConnectionStringName));
                });
            }

            return services;
        }

        public static IApplicationBuilder UseBiaWorkerFeatures([NotNull] this IApplicationBuilder app,
            Action<WorkerFeaturesServiceOptions> optionsAction)
        {
            var options = new WorkerFeaturesServiceOptions();
            optionsAction(options);

            var biaNetSection = new BiaNetSection();
            options.Configuration.GetSection("BiaNet").Bind(biaNetSection);
            // Hangfire Server
            if (biaNetSection?.WorkerFeatures?.HangfireServer?.IsActive == true)
            {
                // HangfireServer configuration copy from configuration file to options if defined
                options.Configuration.GetSection("BiaNet:WorkerFeatures:HangfireServer").Bind(options.HangfireServer);
                //options.HangfireServer = biaNetSection.WorkerFeatures.HangfireServer;
            }
            if (options.HangfireServer.IsActive)
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
                    DashboardTitle = () => options.HangfireServer.ServerName,
                });

                app.UseHangfireDashboard("/hangfire", new DashboardOptions
                {
                    Authorization = options.HangfireServer.Authorization
                });
            }
            return app;
        }
    }
}
