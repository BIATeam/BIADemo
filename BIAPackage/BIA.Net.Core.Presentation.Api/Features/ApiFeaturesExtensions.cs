namespace BIA.Net.Core.WorkerService.Features
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Presentation.Api.Authentication;
    using BIA.Net.Core.Presentation.Common.Features.HubForClients;
    using Hangfire;
    using Hangfire.SqlServer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;

    /// <summary>
    /// Add the standard service.
    /// </summary>
    public static class ApiFeaturesExtensions
    {
        /// <summary>
        /// Start the api service feaures.
        /// </summary>
        /// <param name="services">the service collection.</param>
        /// <param name="optionsAction">the options.</param>
        /// <returns>the services collection.</returns>
        public static IServiceCollection AddBiaApiFeatures(
            [NotNull] this IServiceCollection services,
            Action<ApiFeaturesServiceOptions> optionsAction)
        {
            var options = new ApiFeaturesServiceOptions();
            optionsAction(options);

            var biaNetSection = new BiaNetSection();
            options.Configuration.GetSection("BiaNet").Bind(biaNetSection);


            // Authentication
            services.ConfigureAuthentication(biaNetSection);

            // Local memory cache
            services.AddMemoryCache();

            // Distributed Cache
            if (options.DistributedCache.IsActive)
            {
                services.AddDistributedSqlServerCache(config =>
                {
                    config.ConnectionString = options.Configuration.GetConnectionString(options.DistributedCache.ConnectionStringName);
                    config.TableName = "DistCache";
                    config.SchemaName = "dbo";
                });
            }

            // Swagger
            if (biaNetSection?.ApiFeatures?.Swagger?.IsActive == true)
            {
                options.Swagger.Activate();
            }
            if (options.Swagger.IsActive)
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
                    var securityRequirement = new OpenApiSecurityRequirement();
                    securityRequirement.Add(apiScheme, new[] { "Bearer" });

                    a.SwaggerDoc("BIAApi", new OpenApiInfo { Title = "BIAApi", Version = "v1.0" });
                    a.AddSecurityDefinition(
                        "Bearer",
                        apiScheme);
                    a.AddSecurityRequirement(securityRequirement);
                });
            }

            // Hub For Clients
            if (biaNetSection?.ApiFeatures?.HubForClients?.IsActive == true)
            {
                options.HubForClients.Activate(biaNetSection.ApiFeatures.HubForClients.RedisConnectionString, biaNetSection.ApiFeatures.HubForClients.RedisChannelPrefix);
            }
            if (options.HubForClients.IsActive)
            {
                if (String.IsNullOrEmpty(options.HubForClients.RedisConnectionString))
                {
                    services.AddSignalR();
                }
                else
                {
                    if (String.IsNullOrEmpty(options.HubForClients.RedisChannelPrefix))
                    {
                        services.AddSignalR().AddRedis(options.HubForClients.RedisConnectionString);
                    }
                    else { 
                        services.AddSignalR().AddRedis(options.HubForClients.RedisConnectionString,
                        rediOptions =>
                        {
                            rediOptions.Configuration.ChannelPrefix = options.HubForClients.RedisChannelPrefix;
                        });
                    }
                }
            }

            // Delegate Job Worker
            if (biaNetSection?.ApiFeatures?.DelegateJobToWorker?.IsActive == true)
            {
                options.DelegateJobToWorker.Activate(biaNetSection.ApiFeatures.DelegateJobToWorker.ConnectionStringName);
            }
            if (options.DelegateJobToWorker.IsActive)
            {
                var sqlStorage = new SqlServerStorage(options.Configuration.GetConnectionString(options.DelegateJobToWorker.ConnectionStringName));
                JobStorage.Current = sqlStorage;
            }

            return services;
        }


        public static IApplicationBuilder UseBiaApiFeatures([NotNull] this IApplicationBuilder app,
            Action<ApiFeaturesApplicationBuilderOptions> optionsAction)
        {
            var options = new ApiFeaturesApplicationBuilderOptions();
            optionsAction(options);


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                if (options.BiaNetSection?.ApiFeatures?.HubForClients?.IsActive == true)
                {
                    endpoints.MapHub<HubForClients>("/HubForClients");
                }
            });


            if (options.BiaNetSection?.ApiFeatures?.Swagger?.IsActive == true)
            {
                app.UseStaticFiles();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("BIAApi/swagger.json", "v1.0");
                    c.InjectJavascript("./jquery.min.js");
                    c.InjectJavascript("./AutoLogin.js");
                    c.InjectStylesheet("./AutoLogin.css");
                });
            }

            return app;
        }
    }
}
