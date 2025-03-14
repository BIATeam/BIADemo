﻿// <copyright file="ApiFeaturesExtensions.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Presentation.Api.Features
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Reflection;
    using System.Security.Principal;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Configuration.ApiFeature;
#pragma warning disable BIA001 // Forbidden reference to Domain layer in Presentation layer
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Service;
#pragma warning restore BIA001 // Forbidden reference to Domain layer in Presentation layer
    using BIA.Net.Core.Presentation.Api.Features.HangfireDashboard;
    using BIA.Net.Core.Presentation.Api.StartupConfiguration;
    using BIA.Net.Core.Presentation.Common.Features.HubForClients;
    using Hangfire;
    using Hangfire.Dashboard;
    using Hangfire.Dashboard.JobLogs;
    using Hangfire.PostgreSql;
    using Hangfire.PostgreSql.Factories;
    using Hangfire.SqlServer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
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

            // Swagger
            if (apiFeatures.Swagger?.IsActive == true)
            {
                services.AddSwaggerGen(a =>
                {
                    var apiScheme = new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                    };
                    var securityRequirement = new OpenApiSecurityRequirement
                    {
                        { apiScheme, new[] { "Bearer" } },
                    };

                    a.SwaggerDoc("BIAApi", new OpenApiInfo { Title = "BIAApi", Version = "v1.0" });
                    a.AddSecurityDefinition(
                        "Bearer",
                        apiScheme);
                    a.AddSecurityRequirement(securityRequirement);

                    string filePath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetEntryAssembly().GetName().Name}.xml");
                    a.IncludeXmlComments(filePath, true);
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
                string dbEngine = configuration.GetDBEngine(apiFeatures.DelegateJobToWorker.ConnectionStringName);

                if (dbEngine.ToLower().Equals("sqlserver"))
                {
                    JobStorage.Current = new SqlServerStorage(configuration.GetConnectionString(apiFeatures.DelegateJobToWorker.ConnectionStringName));
                }
                else if (dbEngine.ToLower().Equals("postgresql"))
                {
                    var optionsTime = new PostgreSqlStorageOptions
                    {
                        InvisibilityTimeout = TimeSpan.FromDays(5),
                    };

                    JobStorage.Current = new PostgreSqlStorage(new NpgsqlConnectionFactory(configuration.GetConnectionString(apiFeatures.DelegateJobToWorker.ConnectionStringName), optionsTime, null), optionsTime);
                }
            }

            if (apiFeatures.HangfireDashboard.IsActive)
            {
                services.AddHangfire((config) =>
                {
                    string dbEngine = configuration.GetDBEngine(apiFeatures.HangfireDashboard.ConnectionStringName);
                    if (dbEngine.ToLower().Equals("sqlserver"))
                    {
                        config.UseSimpleAssemblyNameTypeSerializer()
                              .UseRecommendedSerializerSettings()
                              .UseSqlServerStorage(configuration.GetConnectionString(apiFeatures.HangfireDashboard.ConnectionStringName));
                    }
                    else if (dbEngine.ToLower().Equals("postgresql"))
                    {
                        var optionsTime = new PostgreSqlStorageOptions
                        {
                            InvisibilityTimeout = TimeSpan.FromDays(5),
                        };

                        config.UseSimpleAssemblyNameTypeSerializer()
                              .UseRecommendedSerializerSettings()
                              .UsePostgreSqlStorage(options => options.UseNpgsqlConnection(configuration.GetConnectionString(apiFeatures.HangfireDashboard.ConnectionStringName)), optionsTime);
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
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("BIAApi/swagger.json", "v1.0");
                    c.InjectJavascript("./jquery.min.js");
                    c.InjectJavascript("./AutoLoginV3.8.0.js");
                    c.InjectStylesheet("./AutoLoginV3.8.0.css");
                });
            }

            // Hangfire Server
            if (apiFeatures.HangfireDashboard.IsActive)
            {
                app.UseHangfireDashboardCustomOptions(new HangfireDashboardCustomOptions
                {
                    DashboardTitle = () => apiFeatures.HangfireDashboard.ServerName,
                });
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
    }
}
