// <copyright file="IocContainer.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;
    using TheBIADevCompany.BIADemo.Infrastructure.Data;
#if BIA_FRONT_FEATURE
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Infrastructure.Data.Features;
#endif
    using TheBIADevCompany.BIADemo.Application.User;
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
            RegisterServicesFromAssembly(
                collection: collection,
                assemblyName: "TheBIADevCompany.BIADemo.Application",
                excludedServiceNames: new List<string>() { nameof(AuthAppService) });

            if (isApi)
            {
                RegisterServicesFromAssembly(
                collection: collection,
                assemblyName: "TheBIADevCompany.BIADemo.Application",
                includedServiceNames: new List<string>() { nameof(AuthAppService) });
            }

            collection.AddTransient<IBackgroundJobClient, BackgroundJobClient>();
        }

        private static void ConfigureDomainContainer(IServiceCollection collection)
        {
            RegisterServicesFromAssembly(collection: collection, assemblyName: "TheBIADevCompany.BIADemo.Domain");

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

            RegisterServicesFromAssembly(
                collection: collection,
                assemblyName: "TheBIADevCompany.BIADemo.Infrastructure.Data",
                interfaceAssemblyName: "TheBIADevCompany.BIADemo.Domain");
#if BIA_FRONT_FEATURE
            collection.AddSingleton<AuditFeature>();
#endif
        }

#pragma warning disable S1172 // Unused method parameters should be removed
        private static void ConfigureInfrastructureServiceContainer(IServiceCollection collection, BiaNetSection biaNetSection)
#pragma warning restore S1172 // Unused method parameters should be removed
        {
            collection.AddSingleton<IUserDirectoryRepository<UserFromDirectory>, LdapRepository>();
#if BIA_FRONT_FEATURE
            collection.AddHttpClient<IIdentityProviderRepository, IdentityProviderRepository>().ConfigurePrimaryHttpMessageHandler(() => CreateHttpClientHandler(biaNetSection, false));
            collection.AddTransient<INotification, NotificationRepository>();
            collection.AddTransient<IClientForHubRepository, SignalRClientForHubRepository>();
            collection.AddHttpClient<IUserProfileRepository, UserProfileRepository>().ConfigurePrimaryHttpMessageHandler(() => CreateHttpClientHandler(biaNetSection));

            // Begin BIADemo
            collection.AddHttpClient<IRemotePlaneRepository, RemotePlaneRepository>().ConfigurePrimaryHttpMessageHandler(() => CreateHttpClientHandler(biaNetSection));

            // End BIADemo
#endif
        }

        /// <summary>
        /// Creates the HTTP client handler.
        /// </summary>
        /// <param name="biaNetSection">The bia net section.</param>
        /// <returns>HttpClientHandler object.</returns>
#pragma warning disable S1144 // Unused private types or members should be removed
        private static HttpClientHandler CreateHttpClientHandler(BiaNetSection biaNetSection, bool useDefaultCredentials = true)
#pragma warning restore S1144 // Unused private types or members should be removed
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

        private static void RegisterServicesFromAssembly(
            IServiceCollection collection,
            string assemblyName,
            string interfaceAssemblyName = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient,
            IEnumerable<string> excludedServiceNames = null,
            IEnumerable<string> includedServiceNames = null)
        {
            Assembly classAssembly = Assembly.Load(assemblyName);
            Assembly interfaceAssembly = !string.IsNullOrWhiteSpace(interfaceAssemblyName) ? Assembly.Load(interfaceAssemblyName) : classAssembly;

            IEnumerable<Type> classTypes = classAssembly.GetTypes().Where(type => type.IsClass && !type.IsAbstract);
            IEnumerable<Type> interfaceTypes = interfaceAssembly.GetTypes().Where(type => type.IsInterface);

            IEnumerable<(Type classType, Type interfaceType)> mappings = from classType in classTypes
                                                                         join interfaceType in interfaceTypes
                                                                         on "I" + classType.Name equals interfaceType.Name
                                                                         where (excludedServiceNames == null || !excludedServiceNames.Contains(classType.Name)) &&
                                                                               (includedServiceNames == null || includedServiceNames.Contains(classType.Name))
                                                                         select (classType, interfaceType);

            foreach (var (classType, interfaceType) in mappings)
            {
                switch (serviceLifetime)
                {
                    case ServiceLifetime.Singleton:
                        collection.AddSingleton(interfaceType, classType);
                        break;
                    case ServiceLifetime.Scoped:
                        collection.AddScoped(interfaceType, classType);
                        break;
                    case ServiceLifetime.Transient:
                        collection.AddTransient(interfaceType, classType);
                        break;
                    default:
                        collection.AddTransient(interfaceType, classType);
                        break;
                }
            }
        }
    }
}
