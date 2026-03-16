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
#endif
#if BIA_FRONT_FEATURE
    using TheBIADevCompany.BIADemo.Infrastructure.Data.Features;
#endif
    using BIA.Net.Core.Application.File;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Ioc.Param;
#if BIA_FRONT_FEATURE
    using TheBIADevCompany.BIADemo.Application.File;
#endif
    using TheBIADevCompany.BIADemo.Crosscutting.Ioc.Bia.Param;
    using static TheBIADevCompany.BIADemo.Crosscutting.Common.Constants;

    /// <summary>
    /// The IoC Container.
    /// </summary>
    public static partial class IocContainer
    {
#if BIA_USE_DATABASE
        /// <summary>
        /// Configure infrastructure data container database context.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="dbKey">The database key.</param>
        /// <param name="enableRetryOnFailure">if set to <c>true</c> [enable retry on failure].</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <param name="fromDeployDB">if set to <c>true</c> [from deploy database].</param>
        public static void BiaConfigureInfrastructureDataContainerDbContext(
            ParamIocContainer param,
            string dbKey = BiaConstants.DatabaseConfiguration.DefaultKey,
            bool enableRetryOnFailure = true,
            int commandTimeout = default,
            bool fromDeployDB = false)
        {
            if (param.IsUnitTest)
            {
                return;
            }

            string connectionString = param.Configuration.GetDatabaseConnectionString(dbKey);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return;
            }

            DbProvider dbEngine = param.Configuration.GetProvider(dbKey);
            param.Collection.AddDbContext<IQueryableUnitOfWork, DataContext>(options =>
            {
                BiaConfigureDbContextOptions(enableRetryOnFailure, commandTimeout, options, connectionString, dbEngine);
                if (!fromDeployDB)
                {
                    options.AddInterceptors(new AuditSaveChangesInterceptor());
                }
            });

            if (!fromDeployDB)
            {
                param.Collection.AddDbContext<IQueryableUnitOfWorkNoTracking, DataContextNoTracking>(
                    options =>
                    {
                        BiaConfigureDbContextOptions(enableRetryOnFailure, commandTimeout, options, connectionString, dbEngine);
                    },
                    contextLifetime: ServiceLifetime.Transient);
                return;
            }

            BiaConfigureDbContextForDeployDB(param.Collection, connectionString, dbEngine);
        }

        private static void BiaConfigureInfrastructureDataContainer(ParamIocContainer param)
        {
            if (!param.IsUnitTest)
            {
                param.Collection.AddScoped<DataContextFactory>();
                param.Collection.AddSingleton<IAuditFeatureService, AuditFeatureService>();
            }

            param.Collection.AddScoped<IBiaHybridCache, BiaHybridCache>();

            param.Collection.AddSingleton<IAuditFeature, AuditFeature>();

            // Must specify the User type explicitly
            param.Collection.AddScoped<ICoreUserRepository, CoreUserRepository<User>>();
        }

        private static void BiaConfigureInfrastructureDataContainerAutoRegister(ParamAutoRegister param)
        {
            BiaIocContainer.RegisterServicesFromAssembly(
                collection: param.Collection,
                assemblyName: "TheBIADevCompany.BIADemo.Infrastructure.Data",
                interfaceAssemblyName: "TheBIADevCompany.BIADemo.Domain",
                serviceLifetime: param.ServiceLifetime,
                excludedServiceNames: param.ExcludedServiceNames,
                includedServiceNames: param.IncludedServiceNames);
        }

        private static void BiaConfigureDbContextOptions(bool enableRetryOnFailure, int commandTimeout, DbContextOptionsBuilder options, string connectionString, DbProvider dbEngine)
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
            }

            options.EnableSensitiveDataLogging();
        }

        private static void BiaConfigureDbContextForDeployDB(IServiceCollection collection, string connectionString, DbProvider dbEngine)
        {
            collection.Configure<BiaHistoryRepositoryOptions>(options =>
            {
                options.AppVersion = Constants.Application.BackEndVersion;
            });

            if (dbEngine == DbProvider.PostGreSql)
            {
                collection.AddDbContext<IDbContextDatabase, DataContextPostGreSql>(options =>
                {
                    options.UseNpgsql(connectionString, optionsBuilder =>
                    {
                        optionsBuilder.MigrationsAssembly(Constants.DatabaseMigrations.AssemblyNamePostgreSQL);
                    });
                    options.ReplaceService<IHistoryRepository, BiaNpgsqlHistoryRepository>();
                });
            }
            else
            {
                collection.AddDbContext<IDbContextDatabase, DataContext>(options =>
                {
                    options.UseSqlServer(connectionString, optionsBuilder =>
                    {
                        optionsBuilder.MigrationsAssembly(Constants.DatabaseMigrations.AssemblyNameSqlServer);
                    });
                    options.ReplaceService<IHistoryRepository, BiaSqlServerHistoryRepository>();
                });
            }
        }
#endif

        private static void BiaConfigureContainer(ParamIocContainer param)
        {
            if (param.Configuration == null && !param.IsUnitTest)
            {
                return;
            }

            param.BiaNetSection = new BiaNetSection();
            param.Configuration?.GetSection("BiaNet").Bind(param.BiaNetSection);

            ConfigureInfrastructureServiceContainer(param);
            ConfigureDomainContainer(param);
            ConfigureApplicationContainer(param);

#if BIA_USE_DATABASE
            ConfigureInfrastructureDataContainer(param);
#endif
            if (!param.IsUnitTest)
            {
                ConfigureCommonContainer(param);
                param.Collection.Configure<CommonFeatures>(param.Configuration.GetSection("BiaNet:CommonFeatures"));
                param.Collection.Configure<WorkerFeatures>(param.Configuration.GetSection("BiaNet:WorkerFeatures"));
                param.Collection.Configure<ApiFeatures>(param.Configuration.GetSection("BiaNet:ApiFeatures"));
            }
#if BIA_FRONT_FEATURE

            ErrorMessage.FillErrorTranslations();
#endif
        }

        private static void BiaConfigureApplicationContainer(ParamIocContainer param)
        {
            // Permissions
            param.Collection.AddSingleton<IPermissionProvider, PermissionProvider<BiaPermissionId>>();
            param.Collection.AddSingleton<IPermissionProvider, PermissionProvider<PermissionId>>();
            param.Collection.AddSingleton<IPermissionService, PermissionService>();
#if BIA_FRONT_FEATURE
            param.Collection.AddTransient(typeof(IBaseUserSynchronizeDomainService<User, UserFromDirectory>), typeof(UserSynchronizeDomainService));
            param.Collection.AddTransient(typeof(IBaseUserAppService<UserDto, User, UserFromDirectoryDto, UserFromDirectory, PagingFilterFormatDto>), typeof(UserAppService));
            param.Collection.AddTransient(typeof(IBaseTeamAppService<TeamTypeId>), typeof(TeamAppService));

            param.Collection.Configure<FileDownloaderOptions>(options =>
            {
                options.FrenchLanguageId = LanguageId.French;
                options.EnglishLanguageId = LanguageId.English;
                options.SpanishLanguageId = LanguageId.Spanish;
            });
            param.Collection.AddTransient<IFileDownloaderService, FileDownloaderService>();
#endif

            if (param.IsApi)
            {
                param.Collection.AddTransient(typeof(IAuthAppService), typeof(AuthAppService));
            }

            param.Collection.AddTransient<IBackgroundJobClient, BackgroundJobClient>();
        }

        private static void BiaConfigureApplicationContainerAutoRegister(ParamAutoRegister param)
        {
#if BIA_FRONT_FEATURE || BIA_USE_DATABASE
            BiaIocContainer.RegisterServicesFromAssembly(
                collection: param.Collection,
                assemblyName: "BIA.Net.Core.Application",
                serviceLifetime: ServiceLifetime.Transient,
                excludedServiceNames: []);
#endif

            List<string> excludedServiceNameList = param.ExcludedServiceNames?.ToList() ?? new List<string>();
            excludedServiceNameList.Add(nameof(AuthAppService));
            BiaIocContainer.RegisterServicesFromAssembly(
                collection: param.Collection,
                assemblyName: "TheBIADevCompany.BIADemo.Application",
                serviceLifetime: param.ServiceLifetime,
                includedServiceNames: param.IncludedServiceNames,
                excludedServiceNames: excludedServiceNameList);
        }

        private static void BiaConfigureDomainContainer(ParamIocContainer param)
        {
#if BIA_FRONT_FEATURE
            param.Collection.AddTransient(typeof(IUserFromDirectoryMapper<UserFromDirectoryDto, UserFromDirectory>), typeof(UserFromDirectoryMapper));
#endif

            Type templateType = typeof(BiaBaseMapper<,,>);
            Assembly assembly = Assembly.Load("TheBIADevCompany.BIADemo.Domain");
            List<Type> derivedTypes = ReflectiveEnumerator.GetDerivedTypes(assembly, templateType);
            foreach (var type in derivedTypes)
            {
                param.Collection.AddScoped(type);
            }
#if BIA_FRONT_FEATURE || BIA_USE_DATABASE

            param.Collection.AddSingleton<IAuditMapper, AnnouncementAuditMapper>();

            Type auditMapperType = typeof(IAuditMapper);
            List<Type> auditMapperDerivedTypes = ReflectiveEnumerator.GetDerivedTypes(assembly, auditMapperType);
            foreach (var auditMapperDerivedType in auditMapperDerivedTypes)
            {
                param.Collection.AddSingleton(auditMapperType, auditMapperDerivedType);
            }
#endif
        }

        private static void BiaConfigureDomainContainerAutoRegister(ParamAutoRegister param)
        {
            BiaIocContainer.RegisterServicesFromAssembly(
                collection: param.Collection,
                assemblyName: "TheBIADevCompany.BIADemo.Domain",
                serviceLifetime: param.ServiceLifetime,
                excludedServiceNames: param.ExcludedServiceNames,
                includedServiceNames: param.IncludedServiceNames);
        }

        private static void BiaConfigureInfrastructureServiceContainer(ParamIocContainer param)
        {
            param.Collection.AddSingleton<IUserDirectoryRepository<UserFromDirectoryDto, UserFromDirectory>, LdapRepository>();
            param.Collection.AddSingleton<IUserIdentityKeyDomainService, UserIdentityKeyDomainService>();
#if BIA_FRONT_FEATURE
            param.Collection.AddHttpClient<IIdentityProviderRepository<UserFromDirectory>, IdentityProviderRepository>().ConfigurePrimaryHttpMessageHandler(() => BiaIocContainer.CreateHttpClientHandler(param.BiaNetSection, false));

            if (param.BiaNetSection.CommonFeatures?.ClientForHub?.IsActive == true)
            {
                if (param.IsUnitTest || !string.IsNullOrEmpty(param.BiaNetSection.CommonFeatures?.ClientForHub.SignalRUrl))
                {
                    param.Collection.AddTransient<IClientForHubRepository, ExternalClientForSignalRRepository>();
                }
                else
                {
                    param.Collection.AddTransient<IClientForHubRepository, InternalClientForSignalRRepository<HubForClients>>();
                }
            }
#endif
        }
    }
}