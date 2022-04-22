namespace BIA.Net.Core.Presentation.Api.Features
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Configuration.ApiFeature;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Presentation.Common.Authentication;
    using BIA.Net.Core.Presentation.Common.Features.HubForClients;
    using Community.Microsoft.Extensions.Caching.PostgreSql;
    using Hangfire;
    using Hangfire.PostgreSql;
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

            // Local memory cache
            services.AddMemoryCache();

            // Distributed Cache
            if (apiFeatures.DistributedCache.IsActive)
            {
                string dbEngine = configuration.GetDBEngine(apiFeatures.DistributedCache.ConnectionStringName);
                if (dbEngine.ToLower().Equals("sqlserver"))
                {
                    services.AddDistributedSqlServerCache(config =>
                    {
                        config.ConnectionString = configuration.GetConnectionString(apiFeatures.DistributedCache.ConnectionStringName);
                        config.TableName = "DistCache";
                        config.SchemaName = "dbo";
                    });
                }
                else if (dbEngine.ToLower().Equals("postgresql"))
                {
                    services.AddDistributedPostgreSqlCache(config =>
                    {
                        config.ConnectionString = configuration.GetConnectionString(apiFeatures.DistributedCache.ConnectionStringName);
                        config.SchemaName = "dbo";
                        config.TableName = "DistCache";
                    });
                }
            }

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
                        { apiScheme, new[] { "Bearer" } }
                    };

                    a.SwaggerDoc("BIAApi", new OpenApiInfo { Title = "BIAApi", Version = "v1.0" });
                    a.AddSecurityDefinition(
                        "Bearer",
                        apiScheme);
                    a.AddSecurityRequirement(securityRequirement);
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
                        services.AddSignalR().AddRedis(apiFeatures.HubForClients.RedisConnectionString);
                    }
                    else
                    {
                        services.AddSignalR().AddRedis(apiFeatures.HubForClients.RedisConnectionString,
                        redisOptions =>
                        {
                            redisOptions.Configuration.ChannelPrefix = apiFeatures.HubForClients.RedisChannelPrefix;
                        });
                    }
                }
            }

            // Delegate Job Worker
            if (apiFeatures.DelegateJobToWorker?.IsActive == true)
            {
                string dbEngine = configuration.GetDBEngine(apiFeatures.DistributedCache.ConnectionStringName);

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

                    JobStorage.Current = new PostgreSqlStorage(configuration.GetConnectionString(apiFeatures.DelegateJobToWorker.ConnectionStringName), optionsTime);
                }
            }

            return services;
        }


        public static IApplicationBuilder UseBiaApiFeatures<AuditFeature>([NotNull] this IApplicationBuilder app,
            ApiFeatures apiFeatures) where AuditFeature : IAuditFeature
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
                    c.InjectJavascript("./AutoLogin.js");
                    c.InjectStylesheet("./AutoLogin.css");
                });
            }

            app.ApplicationServices.GetRequiredService<AuditFeature>().
                UseAuditFeatures(app.ApplicationServices);

            return app;
        }
    }
}
