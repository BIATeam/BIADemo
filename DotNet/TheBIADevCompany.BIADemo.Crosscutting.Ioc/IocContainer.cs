// <copyright file="IocContainer.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Audit.Core;
    using Audit.EntityFramework;
#if BIA_FRONT_FEATURE
    using BIA.Net.Core.Application.User;
#endif
    using BIA.Net.Core.Application.Permission;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Configuration.ApiFeature;
    using BIA.Net.Core.Common.Configuration.CommonFeature;
    using BIA.Net.Core.Common.Configuration.WorkerFeature;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Announcement.Mappers;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.User.Mappers;
    using BIA.Net.Core.Domain.User.Services;
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.Infrastructure.Data.DateTimeConversion;
    using BIA.Net.Core.Infrastructure.Data.DateTimeConversion.PostgreSql;
    using BIA.Net.Core.Infrastructure.Data.DateTimeConversion.SqlServer;
    using BIA.Net.Core.Infrastructure.Data.Repositories;
    using BIA.Net.Core.Infrastructure.Data.Repositories.HistoryRepositories;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using BIA.Net.Core.Ioc;
    using BIA.Net.Core.Presentation.Common.Features.HubForClients;
    using Hangfire;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Migrations;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
#if BIA_FRONT_FEATURE
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Error;
#endif
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
#if BIA_FRONT_FEATURE

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.RepoContract.DocumentAnalysis;

    // End BIADemo
    using TheBIADevCompany.BIADemo.Domain.User.Entities;
    using TheBIADevCompany.BIADemo.Domain.User.Mappers;
#endif
    using TheBIADevCompany.BIADemo.Domain.User.Models;
#if BIA_USE_DATABASE
    using TheBIADevCompany.BIADemo.Infrastructure.Data;
    using TheBIADevCompany.BIADemo.Infrastructure.Data.Features;
    using TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories;
#endif
    using TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories;

    /// <summary>
    /// The IoC Container.
    /// </summary>
    public static class IocContainer
    {
        /// <summary>
        /// The method used to register all instance.
        /// </summary>
        /// <param name="collection">The collection of service.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="isApi">true if it's an API, false if it's a Worker.</param>
        /// <param name="isUnitTest">Are we configuring IoC for unit tests? If so, some IoC shall not be performed here but replaced by
        /// specific ones in IocContainerTest.</param>
        public static void ConfigureContainer(IServiceCollection collection, IConfiguration configuration, bool isApi, bool isUnitTest = false)
        {
            if (configuration == null && !isUnitTest)
            {
                throw Exception("Configuration cannot be null");
            }

            BiaNetSection biaNetSection = new BiaNetSection();
            configuration?.GetSection("BiaNet").Bind(biaNetSection);

            ConfigureInfrastructureServiceContainer(collection, biaNetSection, isUnitTest);
            ConfigureDomainContainer(collection);
            ConfigureApplicationContainer(collection, isApi);

            BiaIocContainer.ConfigureContainer(collection, configuration, isUnitTest);

#if BIA_USE_DATABASE
            ConfigureInfrastructureDataContainer(collection, configuration, isUnitTest);
#endif
            if (!isUnitTest)
            {
                ConfigureCommonContainer(collection, configuration);
                collection.Configure<CommonFeatures>(configuration.GetSection("BiaNet:CommonFeatures"));
                collection.Configure<WorkerFeatures>(configuration.GetSection("BiaNet:WorkerFeatures"));
                collection.Configure<ApiFeatures>(configuration.GetSection("BiaNet:ApiFeatures"));
            }
#if BIA_FRONT_FEATURE

            ErrorMessage.FillErrorTranslations();
#endif
        }

        private static Exception Exception(string v)
        {
            throw new NotImplementedException();
        }

        private static void ConfigureApplicationContainer(IServiceCollection collection, bool isApi)
        {
            // Permissions
            collection.AddSingleton<IPermissionProvider, PermissionProvider<BiaPermissionId>>();
            collection.AddSingleton<IPermissionProvider, PermissionProvider<PermissionId>>();
            collection.AddSingleton<IPermissionService, PermissionService>();

#if BIA_FRONT_FEATURE
            collection.AddTransient(typeof(IBaseUserSynchronizeDomainService<User, UserFromDirectory>), typeof(UserSynchronizeDomainService));
            collection.AddTransient(typeof(IBaseUserAppService<UserDto, User, UserFromDirectoryDto, UserFromDirectory>), typeof(UserAppService));
            collection.AddTransient(typeof(IBaseTeamAppService<TeamTypeId>), typeof(TeamAppService));
#endif
#if BIA_FRONT_FEATURE || BIA_USE_DATABASE

            // IT'S NOT NECESSARY TO DECLARE Services (They are automatically managed by the method BiaIocContainer.RegisterServicesFromAssembly)
            BiaIocContainer.RegisterServicesFromAssembly(
                collection: collection,
                assemblyName: "BIA.Net.Core.Application",
                serviceLifetime: ServiceLifetime.Transient);
#endif

            // IT'S NOT NECESSARY TO DECLARE Services (They are automatically managed by the method BiaIocContainer.RegisterServicesFromAssembly)
            BiaIocContainer.RegisterServicesFromAssembly(
                collection: collection,
                assemblyName: "TheBIADevCompany.BIADemo.Application",
                excludedServiceNames: new List<string>() { nameof(AuthAppService) },
                serviceLifetime: ServiceLifetime.Transient);

            if (isApi)
            {
                collection.AddTransient(typeof(IAuthAppService), typeof(AuthAppService));
            }

            collection.AddTransient<IBackgroundJobClient, BackgroundJobClient>();
        }

        private static void ConfigureDomainContainer(IServiceCollection collection)
        {
#if BIA_FRONT_FEATURE
            collection.AddTransient(typeof(IUserFromDirectoryMapper<UserFromDirectoryDto, UserFromDirectory>), typeof(UserFromDirectoryMapper));
#endif

            // IT'S NOT NECESSARY TO DECLARE Services (They are automatically managed by the method BiaIocContainer.RegisterServicesFromAssembly)
            BiaIocContainer.RegisterServicesFromAssembly(
                collection: collection,
                assemblyName: "TheBIADevCompany.BIADemo.Domain",
                serviceLifetime: ServiceLifetime.Transient);

            Type templateType = typeof(BiaBaseMapper<,,>);
            Assembly assembly = Assembly.Load("TheBIADevCompany.BIADemo.Domain");
            List<Type> derivedTypes = ReflectiveEnumerator.GetDerivedTypes(assembly, templateType);
            foreach (var type in derivedTypes)
            {
                collection.AddScoped(type);
            }
#if BIA_FRONT_FEATURE || BIA_USE_DATABASE

            collection.AddSingleton<IAuditMapper, AnnouncementAuditMapper>();

            Type auditMapperType = typeof(IAuditMapper);
            List<Type> auditMapperDerivedTypes = ReflectiveEnumerator.GetDerivedTypes(assembly, auditMapperType);
            foreach (var auditMapperDerivedType in auditMapperDerivedTypes)
            {
                collection.AddSingleton(auditMapperType, auditMapperDerivedType);
            }
#endif
        }

        private static void ConfigureCommonContainer(IServiceCollection collection, IConfiguration configuration)
        {
            // Common Layer
        }

#if BIA_USE_DATABASE
        private static void ConfigureInfrastructureDataContainer(IServiceCollection collection, IConfiguration configuration, bool isUnitTest)
        {
            if (!isUnitTest)
            {
                collection.Configure<BiaHistoryRepositoryOptions>(options =>
                {
                    options.AppVersion = Constants.Application.BackEndVersion;
                });

                string connectionString = configuration.GetDatabaseConnectionString(BiaConstants.DatabaseConfiguration.DefaultKey);

                if (!string.IsNullOrWhiteSpace(connectionString))
                {
                    DbProvider dbEngine = configuration.GetProvider(BiaConstants.DatabaseConfiguration.DefaultKey);

                    // Infrastructure Data Layer
                    collection.AddDbContext<IQueryableUnitOfWork, DataContext>(
                        options =>
                        {
                            if (dbEngine == DbProvider.PostGreSql)
                            {
                                options.UseNpgsql(connectionString, npgsqlOptions =>
                                {
                                    npgsqlOptions.EnableRetryOnFailure();
                                });
                                options.ReplaceService<IHistoryRepository, BiaNpgsqlHistoryRepository>();
                            }
                            else
                            {
                                options.UseSqlServer(connectionString, sqlServerOptions =>
                                {
                                    sqlServerOptions.EnableRetryOnFailure();
                                });
                                options.ReplaceService<IHistoryRepository, BiaSqlServerHistoryRepository>();
                            }

                            ((IDbContextOptionsBuilderInfrastructure)options).AddOrUpdateExtension<IDbContextOptionsExtension>(
                                dbEngine == DbProvider.SqlServer
                                    ? new SqlServerDateTimeConversionDbContextOptionsExtension(ServiceLifetime.Scoped)
                                    : new PostgreSqlDateTimeConversionDbContextOptionsExtension(ServiceLifetime.Scoped));

                            options.EnableSensitiveDataLogging();
                            options.AddInterceptors(new AuditSaveChangesInterceptor());
                        });

                    collection.AddDbContext<IQueryableUnitOfWorkNoTracking, DataContextNoTracking>(
                        options =>
                        {
                            if (dbEngine == DbProvider.PostGreSql)
                            {
                                options.UseNpgsql(connectionString, npgsqlOptions =>
                                {
                                    npgsqlOptions.EnableRetryOnFailure();
                                });
                                options.ReplaceService<IHistoryRepository, BiaNpgsqlHistoryRepository>();
                            }
                            else
                            {
                                options.UseSqlServer(connectionString, sqlServerOptions =>
                                {
                                    sqlServerOptions.EnableRetryOnFailure();
                                });
                                options.ReplaceService<IHistoryRepository, BiaSqlServerHistoryRepository>();
                            }

                            ((IDbContextOptionsBuilderInfrastructure)options).AddOrUpdateExtension<IDbContextOptionsExtension>(
                                dbEngine == DbProvider.SqlServer
                                    ? new SqlServerDateTimeConversionDbContextOptionsExtension(ServiceLifetime.Transient)
                                    : new PostgreSqlDateTimeConversionDbContextOptionsExtension(ServiceLifetime.Transient));

                            options.EnableSensitiveDataLogging();
                        },
                        contextLifetime: ServiceLifetime.Transient);
                }

                collection.AddScoped<DataContextFactory>();
                collection.AddSingleton<BIA.Net.Core.Application.Services.IAuditFeatureService, BIA.Net.Core.Application.Services.AuditFeatureService>();
            }

            collection.AddSingleton<IAuditFeature, AuditFeature>();

            // IT'S NOT NECESSARY TO DECLARE QueryCustomizer/Repository (They are automatically managed by the method BiaIocContainer.RegisterServicesFromAssembly)
            BiaIocContainer.RegisterServicesFromAssembly(
                collection: collection,
                assemblyName: "TheBIADevCompany.BIADemo.Infrastructure.Data",
                interfaceAssemblyName: "TheBIADevCompany.BIADemo.Domain",
                serviceLifetime: ServiceLifetime.Transient);

#if BIA_FRONT_FEATURE
            // Must specify the User type explicitly
            collection.AddScoped<ICoreUserRepository, CoreUserRepository<User>>();
#endif
        }
#endif

#pragma warning disable S1172 // Unused method parameters should be removed
        private static void ConfigureInfrastructureServiceContainer(IServiceCollection collection, BiaNetSection biaNetSection, bool isUnitTest = false)
#pragma warning restore S1172 // Unused method parameters should be removed
        {
            collection.AddSingleton<IUserDirectoryRepository<UserFromDirectoryDto, UserFromDirectory>, LdapRepository>();
            collection.AddSingleton<IUserIdentityKeyDomainService, UserIdentityKeyDomainService>();
            collection.AddTransient<IMailRepository, MailRepository>();
#if BIA_FRONT_FEATURE
            collection.AddHttpClient<IIdentityProviderRepository<UserFromDirectory>, IdentityProviderRepository>().ConfigurePrimaryHttpMessageHandler(() => BiaIocContainer.CreateHttpClientHandler(biaNetSection, false));

            if (biaNetSection.CommonFeatures?.ClientForHub?.IsActive == true)
            {
                if (isUnitTest || !string.IsNullOrEmpty(biaNetSection.CommonFeatures?.ClientForHub.SignalRUrl))
                {
                    collection.AddTransient<IClientForHubRepository, ExternalClientForSignalRRepository>();
                }
                else
                {
                    collection.AddTransient<IClientForHubRepository, InternalClientForSignalRRepository<HubForClients>>();
                }
            }

            // Begin BIADemo
            collection.AddSingleton<Infrastructure.Service.Repositories.DocumentAnalysis.PdfAnalysisRepository>();
            collection.AddSingleton<IDocumentAnalysisRepositoryFactory, Infrastructure.Service.Repositories.DocumentAnalysis.DocumentAnalysisRepositoryFactory>();

            collection.AddHttpClient<IRemoteBiaApiRwRepository, RemoteBiaApiRwRepository>()
                .ConfigurePrimaryHttpMessageHandler(() => BiaIocContainer.CreateHttpClientHandler(biaNetSection)).AddStandardResilienceHandler();
            collection.AddHttpClient<IRemotePlaneRepository, RemotePlaneRepository>()
                .ConfigurePrimaryHttpMessageHandler(() => BiaIocContainer.CreateHttpClientHandler(biaNetSection, false)).AddStandardResilienceHandler();
            collection.AddHttpClient<IBiaDemoRoleApiRepository, BiaDemoRoleApiRepository>()
                .ConfigurePrimaryHttpMessageHandler(() => BiaIocContainer.CreateHttpClientHandler(biaNetSection));

            // End BIADemo
#endif
        }
    }
}