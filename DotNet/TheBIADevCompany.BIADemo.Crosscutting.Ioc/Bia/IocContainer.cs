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
    using BIA.Net.Core.Infrastructure.Data.Repositories;
    using BIA.Net.Core.Infrastructure.Data.Repositories.HistoryRepositories;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using BIA.Net.Core.Ioc;
    using BIA.Net.Core.Presentation.Common.Features.HubForClients;
    using Hangfire;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Migrations;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories;
#if BIA_FRONT_FEATURE
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Error;
#endif
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
#if BIA_FRONT_FEATURE
    using TheBIADevCompany.BIADemo.Domain.User.Entities;
    using TheBIADevCompany.BIADemo.Domain.User.Mappers;
#endif
    using TheBIADevCompany.BIADemo.Domain.User.Models;
#if BIA_USE_DATABASE
    using BIA.Net.Core.Infrastructure.Data.Helpers;
    using TheBIADevCompany.BIADemo.Infrastructure.Data;
    using TheBIADevCompany.BIADemo.Infrastructure.Data.Features;
#endif
    using BIA.Net.Core.Application.Services;
    using TheBIADevCompany.BIADemo.Application.Notification;
    using TheBIADevCompany.BIADemo.Domain.Dto.Notification;
    using TheBIADevCompany.BIADemo.Domain.Notification.Entities;

    /// <summary>
    /// The IoC Container.
    /// </summary>
    public static partial class IocContainer
    {
        /// <summary>
        /// The method used to register all instance.
        /// </summary>
        /// <param name="collection">The collection of service.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="isApi">true if it's an API, false if it's a Worker.</param>
        /// <param name="isUnitTest">Are we configuring IoC for unit tests? If so, some IoC shall not be performed here but replaced by
        /// specific ones in IocContainerTest.</param>
        private static void BiaConfigureContainer(IServiceCollection collection, IConfiguration configuration, bool isApi, bool isUnitTest = false)
        {
            if (configuration == null && !isUnitTest)
            {
                throw new ArgumentNullException(nameof(configuration));
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

        private static void BiaConfigureApplicationContainer(IServiceCollection collection, bool isApi)
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

            if (isApi)
            {
                collection.AddTransient(typeof(IAuthAppService), typeof(AuthAppService));
            }

            collection.AddTransient<IBackgroundJobClient, BackgroundJobClient>();
            collection.AddScoped<IBiaFileDownloaderService, BiaFileDownloaderService<INotificationAppService, Notification, NotificationDto, NotificationListItemDto>>();
        }

        private static void BiaConfigureApplicationContainerAutoRegister(
            IServiceCollection collection,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient,
            IEnumerable<string> excludedServiceNames = null,
            IEnumerable<string> includedServiceNames = null)
        {
#if BIA_FRONT_FEATURE || BIA_USE_DATABASE
            BiaIocContainer.RegisterServicesFromAssembly(
                collection: collection,
                assemblyName: "BIA.Net.Core.Application",
                serviceLifetime: ServiceLifetime.Transient,
                excludedServiceNames: [nameof(IBiaFileDownloaderService)]);
#endif

            List<string> excludedServiceNameList = excludedServiceNames?.ToList() ?? new List<string>();
            excludedServiceNameList.Add(nameof(AuthAppService));
            BiaIocContainer.RegisterServicesFromAssembly(
                collection: collection,
                assemblyName: "TheBIADevCompany.BIADemo.Application",
                serviceLifetime: serviceLifetime,
                includedServiceNames: includedServiceNames,
                excludedServiceNames: excludedServiceNameList);
        }

        private static void BiaConfigureDomainContainer(IServiceCollection collection)
        {
#if BIA_FRONT_FEATURE
            collection.AddTransient(typeof(IUserFromDirectoryMapper<UserFromDirectoryDto, UserFromDirectory>), typeof(UserFromDirectoryMapper));
#endif

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

        private static void BiaConfigureDomainContainerAutoRegister(
            IServiceCollection collection,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient,
            IEnumerable<string> excludedServiceNames = null,
            IEnumerable<string> includedServiceNames = null)
        {
            BiaIocContainer.RegisterServicesFromAssembly(
                collection: collection,
                assemblyName: "TheBIADevCompany.BIADemo.Domain",
                serviceLifetime: serviceLifetime,
                excludedServiceNames: excludedServiceNames,
                includedServiceNames: includedServiceNames);
        }

        private static void BiaConfigureInfrastructureDataContainer(IServiceCollection collection, bool isUnitTest)
        {
            if (!isUnitTest)
            {
                collection.Configure<BiaHistoryRepositoryOptions>(options =>
                {
                    options.AppVersion = Constants.Application.BackEndVersion;
                });

                collection.AddScoped<DataContextFactory>();
                collection.AddSingleton<BIA.Net.Core.Application.Services.IAuditFeatureService, BIA.Net.Core.Application.Services.AuditFeatureService>();
            }

            collection.AddSingleton<IAuditFeature, AuditFeature>();
            collection.AddScoped<IBiaHybridCache, BiaHybridCache>();

#if BIA_FRONT_FEATURE
            // Must specify the User type explicitly
            collection.AddScoped<ICoreUserRepository, CoreUserRepository<User>>();
#endif
        }

        private static void BiaConfigureInfrastructureDataContainerAutoRegister(
            IServiceCollection collection,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient,
            IEnumerable<string> excludedServiceNames = null,
            IEnumerable<string> includedServiceNames = null)
        {
            BiaIocContainer.RegisterServicesFromAssembly(
                collection: collection,
                assemblyName: "TheBIADevCompany.BIADemo.Infrastructure.Data",
                interfaceAssemblyName: "TheBIADevCompany.BIADemo.Domain",
                serviceLifetime: serviceLifetime,
                excludedServiceNames: excludedServiceNames,
                includedServiceNames: includedServiceNames);
        }

        private static void BiaConfigureInfrastructureDataContainerDbContext(
            IServiceCollection collection,
            IConfiguration configuration,
            bool isUnitTest,
            string dbKey = BiaConstants.DatabaseConfiguration.DefaultKey,
            bool enableRetryOnFailure = true,
            int commandTimeout = default)
        {
            if (!isUnitTest)
            {
                string connectionString = configuration.GetDatabaseConnectionString(dbKey);

                if (!string.IsNullOrWhiteSpace(connectionString))
                {
                    DbProvider dbEngine = configuration.GetProvider(dbKey);

                    // Infrastructure Data Layer
                    collection.AddDbContext<IQueryableUnitOfWork, DataContext>(
                        options =>
                        {
                            if (dbEngine == DbProvider.PostGreSql)
                            {
                                options.UseNpgsql(connectionString, npgsqlOptions =>
                                {
                                    if (enableRetryOnFailure)
                                    {
                                        npgsqlOptions.EnableRetryOnFailure();
                                    }

                                    if (commandTimeout > 0)
                                    {
                                        npgsqlOptions.CommandTimeout(commandTimeout);
                                    }
                                });
                                options.ReplaceService<IHistoryRepository, BiaNpgsqlHistoryRepository>();
                            }
                            else
                            {
                                options.UseSqlServer(connectionString, sqlServerOptions =>
                                {
                                    if (enableRetryOnFailure)
                                    {
                                        sqlServerOptions.EnableRetryOnFailure();
                                    }

                                    if (commandTimeout > 0)
                                    {
                                        sqlServerOptions.CommandTimeout(commandTimeout);
                                    }
                                });
                                options.ReplaceService<IHistoryRepository, BiaSqlServerHistoryRepository>();
                            }

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
                                    if (enableRetryOnFailure)
                                    {
                                        npgsqlOptions.EnableRetryOnFailure();
                                    }

                                    if (commandTimeout > 0)
                                    {
                                        npgsqlOptions.CommandTimeout(commandTimeout);
                                    }
                                });
                                options.ReplaceService<IHistoryRepository, BiaNpgsqlHistoryRepository>();
                            }
                            else
                            {
                                options.UseSqlServer(connectionString, sqlServerOptions =>
                                {
                                    if (enableRetryOnFailure)
                                    {
                                        sqlServerOptions.EnableRetryOnFailure();
                                    }

                                    if (commandTimeout > 0)
                                    {
                                        sqlServerOptions.CommandTimeout(commandTimeout);
                                    }
                                });
                                options.ReplaceService<IHistoryRepository, BiaSqlServerHistoryRepository>();
                            }

                            options.EnableSensitiveDataLogging();
                        },
                        contextLifetime: ServiceLifetime.Transient);
                }
            }
        }

        private static void BiaConfigureInfrastructureServiceContainer(IServiceCollection collection, BiaNetSection biaNetSection, bool isUnitTest = false)
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
#endif
        }
    }
}