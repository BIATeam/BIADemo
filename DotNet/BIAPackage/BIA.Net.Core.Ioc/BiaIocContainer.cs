// <copyright file="BiaIocContainer.cs" company="BIA">
//  Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Application.Translation;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Configuration.AuthenticationSection;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.RepoContract.BiaApi;
    using BIA.Net.Core.Infrastructure.Data.Repositories;
    using BIA.Net.Core.Infrastructure.Data.Repositories.QueryCustomizer;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using BIA.Net.Core.Infrastructure.Service.Repositories.BiaApi;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Ldap;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The IoC Container.
    /// </summary>
    public static class BiaIocContainer
    {
        /// <summary>
        /// The method used to register all instances.
        /// </summary>
        /// <param name="collection">The collection of services.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="isUnitTest">Are we configuring IoC for unit tests? If so, some IoC shall not be performed here but replaced by
        /// specific ones in BIAIocContainerTest.</param>
        public static void ConfigureContainer(IServiceCollection collection, IConfiguration configuration, bool isUnitTest = false)
        {
            ConfigureInfrastructureServiceContainer(collection, configuration);
            ConfigureDomainContainer(collection);

            if (!isUnitTest)
            {
                ConfigureInfrastructureDataContainer(collection);
                ConfigureCommonContainer(collection, configuration);
            }
        }

        /// <summary>
        /// Creates the HTTP client handler.
        /// </summary>
        /// <param name="biaNetSection">The bia net section.</param>
        /// <param name="useDefaultCredentials">if set to <c>true</c> [use default credentials].</param>
        /// <returns>
        /// HttpClientHandler object.
        /// </returns>
        public static HttpClientHandler CreateHttpClientHandler(BiaNetSection biaNetSection, bool useDefaultCredentials = true)
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

        /// <summary>
        /// This method dynamically registers services from an assembly based on provided parameters.
        /// </summary>
        /// <param name="collection">The IServiceCollection to add services to.</param>
        /// <param name="assemblyName">The name of the assembly containing the classes to register services for.</param>
        /// <param name="interfaceAssemblyName">The name of the assembly containing the interfaces. If not provided, uses the class assembly.</param>
        /// <param name="serviceLifetime">The lifecycle scope of the services (Singleton, Scoped, Transient). Scoped by default.</param>
        /// <param name="excludedServiceNames">A list of class type names to be excluded from service registration, if any.</param>
        /// <param name="includedServiceNames">A list of class type names to be included for service registration, if any.</param>
        /// <remarks>
        ///  This method scans the provided class assembly and maps each class to its interface based on naming convention ("I" + ClassName).
        ///  The mapped pairs are then registered to the IServiceCollection based on the specified ServiceLifetime.
        ///  The included/excluded service names are taken into account during this process.
        /// </remarks>
        public static void RegisterServicesFromAssembly(
            IServiceCollection collection,
            string assemblyName,
            string interfaceAssemblyName = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped,
            IEnumerable<string> excludedServiceNames = null,
            IEnumerable<string> includedServiceNames = null)
        {
            Assembly classAssembly = Assembly.Load(assemblyName);
            Assembly interfaceAssembly = !string.IsNullOrWhiteSpace(interfaceAssemblyName) ? Assembly.Load(interfaceAssemblyName) : classAssembly;

            IEnumerable<Type> classTypes = classAssembly.GetTypes().Where(type => type.IsClass && !type.IsAbstract);
            IEnumerable<Type> interfaceTypes = interfaceAssembly.GetTypes().Where(type => type.IsInterface);

            IEnumerable<(Type ClassType, Type InterfaceType)> mappings = from classType in classTypes
                                                                         join interfaceType in interfaceTypes
                                                                         on "I" + classType.Name equals interfaceType.Name
                                                                         where (excludedServiceNames == null || !excludedServiceNames.Contains(classType.Name)) &&
                                                                               (includedServiceNames == null || includedServiceNames.Contains(classType.Name))
                                                                         select (classType, interfaceType);

            foreach (var (classType, interfaceType) in mappings)
            {
                if (!collection.Any(s => s.ServiceType == interfaceType))
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
                            collection.AddScoped(interfaceType, classType);
                            break;
                    }
                }
            }
        }

        private static void ConfigureDomainContainer(IServiceCollection collection)
        {
            // IT'S NOT NECESSARY TO DECLARE Services (They are automatically managed by the method BiaIocContainer.RegisterServicesFromAssembly)
            RegisterServicesFromAssembly(
                collection: collection,
                assemblyName: "BIA.Net.Core.Domain",
                serviceLifetime: ServiceLifetime.Transient);

            // Domain
            Type templateType = typeof(BiaBaseMapper<,,>);
            Assembly assembly = Assembly.Load("BIA.Net.Core.Domain");
            List<Type> derivedTypes = ReflectiveEnumerator.GetDerivedTypes(assembly, templateType);
            foreach (var type in derivedTypes)
            {
                collection.AddScoped(type);
            }
        }

        private static void ConfigureCommonContainer(IServiceCollection collection, IConfiguration configuration)
        {
            // Configuration
            collection.Configure<BiaNetSection>(options => configuration.GetSection("BiaNet").Bind(options));
        }

        private static void ConfigureInfrastructureDataContainer(IServiceCollection collection)
        {
            collection.AddScoped(typeof(ITGenericRepository<,>), typeof(TGenericRepositoryEF<,>));
            collection.AddScoped(typeof(ITGenericArchiveRepository<,>), typeof(TGenericArchiveRepository<,>));
            collection.AddScoped(typeof(ITGenericCleanRepository<,>), typeof(TGenericCleanRepository<,>));
            collection.AddScoped<IViewQueryCustomizer, ViewQueryCustomizer>();

            // Infrastructure Data
        }

        private static void ConfigureInfrastructureServiceContainer(IServiceCollection collection, IConfiguration configuration)
        {
            BiaNetSection biaNetSection = null;

            if (configuration != null)
            {
                biaNetSection = new BiaNetSection();
                configuration.GetSection("BiaNet").Bind(biaNetSection);
            }

            // Infrastructure Service
            collection.AddTransient<ILdapRepositoryHelper, LdapRepositoryHelper>();
            collection.AddTransient<IBiaLocalCache, BiaLocalCache>();
            collection.AddHttpClient<IRoleApiRepository, RoleApiRepository>().ConfigurePrimaryHttpMessageHandler(() => CreateHttpClientHandler(biaNetSection));

            if (biaNetSection?.CommonFeatures?.DistributedCache?.IsActive == true)
            {
                collection.AddTransient<IBiaDistributedCache, BiaDistributedCache>();
            }
            else
            {
                collection.AddTransient<IBiaDistributedCache, BiaLocalCache>();
            }

            collection.AddTransient<IBiaHybridCache, BiaHybridCache>();

            collection.AddHttpClient<IWakeUpWebApps, WakeUpWebApps>().ConfigurePrimaryHttpMessageHandler(() =>
                CreateHttpClientHandler(biaNetSection));

            collection.AddTransient<IFileRepository, FileRepository>();
            collection.AddHttpClient<IImageUrlRepository, ImageUrlRepository>().ConfigurePrimaryHttpMessageHandler(() =>
                GetHttpMessagehandler(biaNetSection.ProfileConfiguration?.AuthenticationConfiguration, biaNetSection));

            collection.AddHttpClient<IBiaWebApiAuthRepository, BiaWebApiAuthRepository>().ConfigurePrimaryHttpMessageHandler(() =>
            CreateHttpClientHandler(biaNetSection));
        }

        private static HttpClientHandler GetHttpMessagehandler(AuthenticationConfiguration authenticationConfiguration, BiaNetSection biaNetSection)
        {
            return CreateHttpClientHandler(biaNetSection, authenticationConfiguration.Mode == AuthenticationMode.Default);
        }
    }
}
