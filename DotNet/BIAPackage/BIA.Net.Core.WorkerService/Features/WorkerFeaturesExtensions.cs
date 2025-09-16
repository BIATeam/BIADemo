// <copyright file="WorkerFeaturesExtensions.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.WorkerService.Features
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Security.Principal;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Configuration.WorkerFeature;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.WorkerService.Features.DataBaseHandler;
    using Hangfire;
    using Hangfire.PerformContextAccessor;
    using Hangfire.PostgreSql;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using StackExchange.Redis;

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
        /// <returns>the services collection.</returns>
        public static IServiceCollection AddBiaWorkerFeatures(
            [NotNull] this IServiceCollection services,
            WorkerFeatures workerFeatures,
            IConfiguration configuration)
        {
            var biaNetSection = new BiaNetSection();
            configuration.GetSection("BiaNet").Bind(biaNetSection);

            // Hub For Clients
            if (biaNetSection.CommonFeatures.ClientForHub?.IsActive == true)
            {
                if (!string.IsNullOrEmpty(biaNetSection.CommonFeatures.ClientForHub.RedisConnectionString))
                {
                    if (string.IsNullOrEmpty(biaNetSection.CommonFeatures.ClientForHub.RedisChannelPrefix))
                    {
                        services.AddSignalR().AddStackExchangeRedis(biaNetSection.CommonFeatures.ClientForHub.RedisConnectionString);
                    }
                    else
                    {
                        services.AddSignalR().AddStackExchangeRedis(
                            biaNetSection.CommonFeatures.ClientForHub.RedisConnectionString,
                            redisOptions =>
                            {
                                redisOptions.Configuration.ChannelPrefix = RedisChannel.Literal(biaNetSection.CommonFeatures.ClientForHub.RedisChannelPrefix);
                            });
                    }
                }
                else
                {
                    services.AddSignalR();
                }
            }

            // Identity
            services.AddTransient<IPrincipal>(
                provider =>
                {
                    var claims = new List<Claim> { new(ClaimTypes.Name, Environment.UserName) };
                    var userIdentity = new ClaimsIdentity(claims, "NonEmptyAuthType");
                    return new BiaClaimsPrincipal(new ClaimsPrincipal(userIdentity));
                });
            services.AddTransient(provider => new UserContext("en-GB", biaNetSection.Cultures));

            // Database Handler
            if (workerFeatures.DatabaseHandler.IsActive)
            {
                services.AddHostedService<DataBaseHandlerService>();
            }

            // Hangfire Server
            if (workerFeatures.HangfireServer.IsActive)
            {
                // Log in hangfire dashboard
                if (workerFeatures.HangfireServer.LogsVisibleInDashboard)
                {
                    services.AddHangfirePerformContextAccessor();
                }

                GlobalJobFilters.Filters.Add(new ProlongExpirationTimeAttribute(workerFeatures.HangfireServer.SucceededTasksRetentionDays));

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
                              .UseSqlServerStorage(configuration.GetDatabaseConnectionString(workerFeatures.HangfireServer.ConnectionStringName));
                    }
                    else if (dbEngine.ToLower().Equals("postgresql"))
                    {
                        var optionsTime = new PostgreSqlStorageOptions
                        {
                            InvisibilityTimeout = TimeSpan.FromDays(5),
                        };

                        config.UseSimpleAssemblyNameTypeSerializer()
                              .UseRecommendedSerializerSettings()
                              .UsePostgreSqlStorage(options => options.UseNpgsqlConnection(configuration.GetDatabaseConnectionString(workerFeatures.HangfireServer.ConnectionStringName)), optionsTime);
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
