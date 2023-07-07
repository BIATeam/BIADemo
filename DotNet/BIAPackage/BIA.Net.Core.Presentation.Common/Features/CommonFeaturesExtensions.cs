// <copyright file="CommonFeaturesExtensions.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Presentation.Common.Features
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using BIA.Net.Core.Common.Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Community.Microsoft.Extensions.Caching.PostgreSql;
    using BIA.Net.Core.Presentation.Common.Authentication;
    using BIA.Net.Core.Common.Configuration.CommonFeature;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// Add the standard service.
    /// </summary>
    public static class CommonFeaturesExtensions
    {
        /// <summary>
        /// Start the Worker service features.
        /// </summary>
        /// <param name="services">the service collection.</param>
        /// <param name="configuration">the application configuration.</param>
        /// <param name="databaseHandlerRepositories">the list of handler repositories.</param>
        /// <returns>the services collection.</returns>
        public static IServiceCollection AddBiaCommonFeatures(
            [NotNull] this IServiceCollection services,
            CommonFeatures commonFeatures,
            IConfiguration configuration)
        {
            var biaNetSection = new BiaNetSection();
            configuration.GetSection("BiaNet").Bind(biaNetSection);

            // Authentication
            services.ConfigureAuthentication(biaNetSection);

            // Distributed Cache
            if (commonFeatures?.DistributedCache?.IsActive == true)
            {
                string dbEngine = configuration.GetDBEngine(commonFeatures.DistributedCache.ConnectionStringName);
                if (dbEngine.ToLower().Equals("sqlserver"))
                {
                    services.AddDistributedSqlServerCache(config =>
                    {
                        config.ConnectionString = configuration.GetConnectionString(commonFeatures.DistributedCache.ConnectionStringName);
                        config.TableName = "DistCache";
                        config.SchemaName = "dbo";
                    });
                }
                else if (dbEngine.ToLower().Equals("postgresql"))
                {
                    services.AddDistributedPostgreSqlCache(config =>
                    {
                        config.ConnectionString = configuration.GetConnectionString(commonFeatures.DistributedCache.ConnectionStringName);
                        config.TableName = "DistCache";
                        config.SchemaName = "dbo";
                    });
                }

                services.AddMemoryCache();
            }

            // Client for hub
            /*if (ClientForHubOptions.IsActive)
            {
                services.AddTransient<IHostedService, ClientForHubService>(provider =>
                {
                    return new ClientForHubService(options.ClientForHub);
                });
            }*/

            return services;
        }

        public static void UseBiaCommonFeatures<AuditFeature>(IServiceProvider services) where AuditFeature : IAuditFeature
        {
            //services = app.ApplicationServices for Api
            // for worker
            //using (var scope = host.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //}

            services.GetRequiredService<AuditFeature>().UseAuditFeatures(services);
        }
    }
}
