// <copyright file="ApiFeaturesExtensions.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Presentation.Api.Features
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Security.Principal;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Configuration.ApiFeature;
    using BIA.Net.Core.Common.Enum;
#pragma warning disable BIA001 // Forbidden reference to Domain layer in Presentation layer
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Service;
#pragma warning restore BIA001 // Forbidden reference to Domain layer in Presentation layer
    using BIA.Net.Core.Presentation.Api.Features.HangfireDashboard;
    using BIA.Net.Core.Presentation.Api.Features.OpenApiDocumentTransformer;
    using BIA.Net.Core.Presentation.Api.StartupConfiguration;
    using BIA.Net.Core.Presentation.Common.Features.HubForClients;
    using Hangfire;
    using Hangfire.Dashboard;
    using Hangfire.PostgreSql;
    using Hangfire.PostgreSql.Factories;
    using Hangfire.SqlServer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Options;
    using StackExchange.Redis;

    /// <summary>
    /// Add the standard service.
    /// </summary>
    public static class ApiFeaturesExtensions
    {
        /// <summary>
        /// Start the api service feaures.
        /// </summary>
        /// <param name="services">the service collection.</param>
        /// <param name="apiFeatures">the API features configuration.</param>
        /// <param name="configuration">the application configuration.</param>
        /// <returns>the services collection.</returns>
        public static IServiceCollection AddBiaApiFeatures(
            [NotNull] this IServiceCollection services,
            ApiFeatures apiFeatures,
            IConfiguration configuration)
        {
            var biaNetSection = new BiaNetSection();
            configuration.GetSection("BiaNet").Bind(biaNetSection);

            // Authentication
            services.ConfigureAuthentication(biaNetSection);

            // Identity
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPrincipal>(provider => new BiaClaimsPrincipal(provider.GetService<IHttpContextAccessor>().HttpContext.User));
            services.AddTransient(provider => new UserContext(provider.GetService<IHttpContextAccessor>().HttpContext.Request.Headers.AcceptLanguage.ToString(), biaNetSection.Cultures));

            // Client TimeZone
            services.AddScoped<IClientTimeZoneContext, HttpClientTimeZoneContext>();

            // Swagger
            if (apiFeatures.Swagger?.IsActive == true)
            {
                services.AddOpenApi(options =>
                {
                    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                    options.AddDocumentTransformer<SortTagsDocumentTransformer>();
                    options.AddDocumentTransformer<ApiInfoDocumentTransformer>(); // sets Title/Version
                });
            }

            // Hub For Clients
            if (apiFeatures.HubForClients?.IsActive == true)
            {
                if (string.IsNullOrEmpty(apiFeatures.HubForClients.RedisConnectionString))
                {
                    services.AddSignalR();
                }
                else
                {
                    if (string.IsNullOrEmpty(apiFeatures.HubForClients.RedisChannelPrefix))
                    {
                        services.AddSignalR().AddStackExchangeRedis(apiFeatures.HubForClients.RedisConnectionString);
                    }
                    else
                    {
                        services.AddSignalR().AddStackExchangeRedis(
                            apiFeatures.HubForClients.RedisConnectionString,
                            redisOptions =>
                            {
                                redisOptions.Configuration.ChannelPrefix = RedisChannel.Literal(apiFeatures.HubForClients.RedisChannelPrefix);
                            });
                    }
                }
            }

            // Delegate Job Worker
            if (apiFeatures.DelegateJobToWorker?.IsActive == true)
            {
                DbProvider provider = configuration.GetProvider(apiFeatures.DelegateJobToWorker.ConnectionStringName);

                if (provider == DbProvider.SqlServer)
                {
                    JobStorage.Current = new SqlServerStorage(configuration.GetDatabaseConnectionString(apiFeatures.DelegateJobToWorker.ConnectionStringName));
                }
                else if (provider == DbProvider.PostGreSql)
                {
                    var optionsTime = new PostgreSqlStorageOptions
                    {
                        InvisibilityTimeout = TimeSpan.FromDays(5),
                    };

                    JobStorage.Current = new PostgreSqlStorage(new NpgsqlConnectionFactory(configuration.GetDatabaseConnectionString(apiFeatures.DelegateJobToWorker.ConnectionStringName), optionsTime, null), optionsTime);
                }
            }

            if (apiFeatures.HangfireDashboard?.IsActive == true)
            {
                services.AddHangfire((config) =>
                {
                    DbProvider provider = configuration.GetProvider(apiFeatures.HangfireDashboard.ConnectionStringName);
                    if (provider == DbProvider.SqlServer)
                    {
                        config.UseSimpleAssemblyNameTypeSerializer()
                              .UseRecommendedSerializerSettings()
                              .UseSqlServerStorage(configuration.GetDatabaseConnectionString(apiFeatures.HangfireDashboard.ConnectionStringName));
                    }
                    else if (provider == DbProvider.PostGreSql)
                    {
                        var optionsTime = new PostgreSqlStorageOptions
                        {
                            InvisibilityTimeout = TimeSpan.FromDays(5),
                        };

                        config.UseSimpleAssemblyNameTypeSerializer()
                              .UseRecommendedSerializerSettings()
                              .UsePostgreSqlStorage(options => options.UseNpgsqlConnection(configuration.GetDatabaseConnectionString(apiFeatures.HangfireDashboard.ConnectionStringName)), optionsTime);
                    }

                    if (apiFeatures.HangfireDashboard.LogsVisibleInDashboard)
                    {
                        // Log in hangfire dashboard
                        config.UseDashboardJobLogs(apiFeatures.HangfireDashboard.LogFiles);
                    }
                });
            }

            return services;
        }

        /// <summary>
        /// Use Bia Api Features.
        /// </summary>
        /// <param name="app">the application builder.</param>
        /// <param name="apiFeatures">the Api feature.</param>
        /// <param name="hangfireServerAuthorizations">authorization for hangfire dashboard.</param>
        /// <returns>the application builder with bia feature.</returns>
        public static IApplicationBuilder UseBiaApiFeatures(
    [NotNull] this IApplicationBuilder app,
    ApiFeatures apiFeatures,
    HangfireDashboardAuthorizations hangfireServerAuthorizations)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                if (apiFeatures?.HubForClients?.IsActive == true)
                {
                    endpoints.MapHub<HubForClients>("/HubForClients");
                }
            });

            if (apiFeatures?.Swagger?.IsActive == true)
            {
                app.UseStaticFiles();

                string swaggerEndpoint = "BIAApi/swagger.json";

                if (app is WebApplication webApp)
                {
                    webApp.MapOpenApi(pattern: $"/swagger/{swaggerEndpoint}");
                }

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(swaggerEndpoint, "v1.0");
                    c.InjectJavascript("./AutoLogin.c0c914df432ec8edfa27c6c4d05ce98c.js");
                    c.InjectStylesheet("./AutoLogin.1379b731dd73c3456a0ce0aab0c01a83.css");
                });
            }

            // Hangfire Server
            if (apiFeatures?.HangfireDashboard?.IsActive == true)
            {
                app.UseHangfireDashboard("/hangfireAdmin", new DashboardOptions
                {
                    Authorization = hangfireServerAuthorizations.Authorization,
                });
                app.UseHangfireDashboard("/hangfire", new DashboardOptions
                {
                    IsReadOnlyFunc = (DashboardContext context) => true,
                    Authorization = hangfireServerAuthorizations.AuthorizationReadOnly,
                });
            }

            return app;
        }

        /// <summary>
        /// Register health checks (live).
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>A <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddBiaHealthChecksLiveness([NotNull] this IServiceCollection services)
        {
            services
                .AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "live" });

            return services;
        }

        /// <summary>
        /// Register health checks (database ready).
        /// </summary>
        /// <typeparam name="THealthCheck">The type of the health check.</typeparam>
        /// <param name="services">The services.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddBiaHealthChecksDbReadiness<THealthCheck>([NotNull] this IServiceCollection services)
            where THealthCheck : class, IHealthCheck
        {
            services
                .AddHealthChecks()
                .AddCheck<THealthCheck>("database", tags: new[] { "ready" });

            return services;
        }

        /// <summary>
        /// Adds a middleware that provides health check status.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns><see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseBiaHealthChecks([NotNull] this IApplicationBuilder app)
        {
            HealthCheckServiceOptions healthCheckOptions = app.ApplicationServices.GetService<IOptions<HealthCheckServiceOptions>>()?.Value;
            bool hasLive = healthCheckOptions?.Registrations?.Any(r => r.Tags.Contains("live")) == true;
            bool hasReady = healthCheckOptions?.Registrations?.Any(r => r.Tags.Contains("ready")) == true;

            if (hasLive)
            {
                app.UseHealthChecks("/health/live", new HealthCheckOptions
                {
                    Predicate = r => r.Tags.Contains("live"),
                });
            }

            if (hasReady)
            {
                app.UseHealthChecks("/health/ready", new HealthCheckOptions
                {
                    Predicate = r => r.Tags.Contains("ready"),
                });
            }

            return app;
        }
    }
}
