// <copyright file="IocContainer.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Reflection;
    using Audit.Core;
    using Audit.EntityFramework;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Configuration.ApiFeature;
    using BIA.Net.Core.Common.Configuration.CommonFeature;
    using BIA.Net.Core.Common.Configuration.WorkerFeature;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using BIA.Net.Core.Ioc;
    using BIA.Net.Core.IocContainer;
    using Hangfire;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    // BIAToolKit - Begin Dependency 1
    // BIAToolKit - End Dependency 1
    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Application.AircraftMaintenanceCompany;
    using TheBIADevCompany.BIADemo.Application.Job;

    // BIAToolKit - Begin Partial Dependency 1 Plane
    using TheBIADevCompany.BIADemo.Application.Plane;

    // BIAToolKit - End Partial Dependency 1 Plane
    // End BIADemo
    using TheBIADevCompany.BIADemo.Application.Site;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Application.View;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Service;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Service;
    using TheBIADevCompany.BIADemo.Infrastructure.Data;
    using TheBIADevCompany.BIADemo.Infrastructure.Data.Features;
    using TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories;
    using TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories.QueryCustomizer;
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

            BiaIocContainer.ConfigureContainer(collection, configuration, isUnitTest);

            ConfigureInfrastructureServiceContainer(collection, biaNetSection);
            ConfigureDomainContainer(collection);
            ConfigureApplicationContainer(collection, isApi);

            if (!isUnitTest)
            {
                ConfigureInfrastructureDataContainer(collection, configuration);
                ConfigureCommonContainer(collection, configuration);
                collection.Configure<CommonFeatures>(configuration.GetSection("BiaNet:CommonFeatures"));
                collection.Configure<WorkerFeatures>(configuration.GetSection("BiaNet:WorkerFeatures"));
                collection.Configure<ApiFeatures>(configuration.GetSection("BiaNet:ApiFeatures"));
            }
        }

        private static Exception Exception(string v)
        {
            throw new NotImplementedException();
        }

        private static void ConfigureApplicationContainer(IServiceCollection collection, bool isApi)
        {
            // Application Layer
            collection.AddTransient<ITeamAppService, TeamAppService>();
            collection.AddTransient<ISiteAppService, SiteAppService>();
            collection.AddTransient<IMemberAppService, MemberAppService>();
            collection.AddTransient<IRoleAppService, RoleAppService>();
            collection.AddTransient<IUserAppService, UserAppService>();
            collection.AddTransient<IViewAppService, ViewAppService>();
            collection.AddTransient<IBackgroundJobClient, BackgroundJobClient>();

            if (isApi)
            {
                collection.AddTransient<IAuthAppService, AuthAppService>();
            }

            // Begin BIADemo
            collection.AddTransient<IAircraftMaintenanceCompanyAppService, AircraftMaintenanceCompanyAppService>();
            collection.AddTransient<IMaintenanceTeamAppService, MaintenanceTeamAppService>();

            // BIAToolKit - Begin Partial Dependency 2 Plane
            collection.AddTransient<IPlaneAppService, PlaneAppService>();

            // BIAToolKit - End Partial Dependency 2 Plane
            collection.AddTransient<IPlaneTypeAppService, PlaneTypeAppService>();

            // BIAToolKit - Begin Partial Dependency 2 Airport
            collection.AddTransient<IAirportAppService, AirportAppService>();

            // BIAToolKit - End Partial Dependency 2 Airport
            collection.AddTransient<IEngineAppService, EngineAppService>();
            collection.AddTransient<IBiaDemoTestHangfireService, BiaDemoTestHangfireService>();
            collection.AddTransient<IRemotePlaneAppService, RemotePlaneAppService>();

            // End BIADemo

            // BIAToolKit - Begin Dependency 2
            // BIAToolKit - End Dependency 2
        }

        private static void ConfigureDomainContainer(IServiceCollection collection)
        {
            // Domain Layer
            collection.AddTransient<IUserIdentityKeyDomainService, UserIdentityKeyDomainService>();
            collection.AddTransient<IUserPermissionDomainService, UserPermissionDomainService>();
            collection.AddTransient<IUserSynchronizeDomainService, UserSynchronizeDomainService>();
            collection.AddTransient<INotificationDomainService, NotificationDomainService>();
            collection.AddTransient<INotificationTypeDomainService, NotificationTypeDomainService>();

            Type templateType = typeof(BaseMapper<,,>);
            Assembly assembly = Assembly.Load("TheBIADevCompany.BIADemo.Domain");
            List<Type> derivedTypes = ReflectiveEnumerator.GetDerivedTypes(assembly, templateType);
            foreach (var type in derivedTypes)
            {
                collection.AddScoped(type);
            }
        }

        private static void ConfigureCommonContainer(IServiceCollection collection, IConfiguration configuration)
        {
            // Common Layer
        }

        private static void ConfigureInfrastructureDataContainer(IServiceCollection collection, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("BIADemoDatabase");

            // Infrastructure Data Layer
            collection.AddDbContext<IQueryableUnitOfWork, DataContext>(options =>
            {
                if (connectionString != null)
                {
                    options.UseSqlServer(connectionString);
                }

                options.EnableSensitiveDataLogging();
                options.AddInterceptors(new AuditSaveChangesInterceptor());
            });
            collection.AddDbContext<IQueryableUnitOfWorkReadOnly, DataContextReadOnly>(
                options =>
                {
                    if (connectionString != null)
                    {
                        options.UseSqlServer(connectionString);
                    }

                    options.EnableSensitiveDataLogging();
                },
                contextLifetime: ServiceLifetime.Transient);

            collection.AddTransient<IMemberQueryCustomizer, MemberQueryCustomizer>();
            collection.AddTransient<IViewQueryCustomizer, ViewQueryCustomizer>();
            collection.AddTransient<INotificationQueryCustomizer, NotificationQueryCustomizer>();
            collection.AddSingleton<AuditFeature>();

            // Begin BIADemo
            collection.AddTransient<IEngineRepository, EngineRepository>();

            // End BIADemo
        }

        private static void ConfigureInfrastructureServiceContainer(IServiceCollection collection, BiaNetSection biaNetSection)
        {
            // Infrastructure Service Layer
            collection.AddSingleton<IUserDirectoryRepository<UserFromDirectory>, LdapRepository>();
            collection.AddTransient<INotification, NotificationRepository>();
            collection.AddTransient<IClientForHubRepository, SignalRClientForHubRepository>();

            collection.AddHttpClient<IUserProfileRepository, UserProfileRepository>().ConfigurePrimaryHttpMessageHandler(() => CreateHttpClientHandler(biaNetSection));

            collection.AddHttpClient<IIdentityProviderRepository, IdentityProviderRepository>().ConfigurePrimaryHttpMessageHandler(() => CreateHttpClientHandler(biaNetSection, false));

            // Begin BIADemo
            collection.AddHttpClient<IRemotePlaneRepository, RemotePlaneRepository>().ConfigurePrimaryHttpMessageHandler(() => CreateHttpClientHandler(biaNetSection));

            // End BIADemo
        }

        /// <summary>
        /// Creates the HTTP client handler.
        /// </summary>
        /// <param name="biaNetSection">The bia net section.</param>
        /// <returns>HttpClientHandler object.</returns>
        private static HttpClientHandler CreateHttpClientHandler(BiaNetSection biaNetSection, bool useDefaultCredentials = true)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler
            {
                UseDefaultCredentials = useDefaultCredentials,
                AllowAutoRedirect = false,
                UseProxy = false,
            };

            if (biaNetSection?.Security?.DisableTlsVerify == true)
            {
#pragma warning disable S4830 // Server certificates should be verified during SSL/TLS connections
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
#pragma warning restore S4830 // Server certificates should be verified during SSL/TLS connections
            }

            return httpClientHandler;
        }
    }
}
