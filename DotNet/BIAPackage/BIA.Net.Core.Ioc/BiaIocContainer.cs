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
    using BIA.Net.Core.Ioc.Param;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The IoC Container.
    /// </summary>
    public static class BiaIocContainer
    {
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
        /// This method scans the provided class assembly and identifies all concrete classes (non-generic, non-abstract).
        /// For each interface found in the interface assembly, it discovers all classes that implement that interface.
        /// Each interface-implementation pair is then registered to the IServiceCollection based on the specified ServiceLifetime,
        /// but only if the interface is not already registered in the collection.
        /// The included/excluded service names are taken into account during the filtering process.
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

            IEnumerable<Type> classTypes = classAssembly.GetTypes().Where(type => !type.IsGenericTypeDefinition && type.IsClass && !type.IsAbstract);
            IEnumerable<Type> interfaceTypes = interfaceAssembly.GetTypes().Where(type => !type.IsGenericTypeDefinition && type.IsInterface);

            if (excludedServiceNames != null)
            {
                classTypes = classTypes.Where(c => !excludedServiceNames.Contains(c.Name));
                interfaceTypes = interfaceTypes.Where(i => !excludedServiceNames.Contains(i.Name));
            }

            if (includedServiceNames != null)
            {
                classTypes = classTypes.Where(c => includedServiceNames.Contains(c.Name));
                interfaceTypes = interfaceTypes.Where(i => includedServiceNames.Contains(i.Name));
            }

            var classesImplementationsByInterface = new Dictionary<Type, List<Type>>();
            foreach (var interfaceType in interfaceTypes)
            {
                if (interfaceType.IsGenericTypeDefinition)
                {
                    classesImplementationsByInterface[interfaceType] = classTypes.Where(c =>
                        c.IsGenericTypeDefinition &&
                        c.GetGenericArguments().Length == interfaceType.GetGenericArguments().Length &&
                        c.GetInterfaces()
                            .Where(i => i.IsGenericType)
                            .Any(i => i.GetGenericTypeDefinition() == interfaceType)).ToList();
                }
                else
                {
                    classesImplementationsByInterface[interfaceType] = classTypes.Where(c =>
                        !c.IsGenericTypeDefinition &&
                        c.IsAssignableTo(interfaceType)).ToList();
                }
            }

            foreach (var kvp in classesImplementationsByInterface)
            {
                var interfaceType = kvp.Key;

                if (!collection.Any(s => s.ServiceType == interfaceType))
                {
                    foreach (var classType in kvp.Value)
                    {
                        switch (serviceLifetime)
                        {
                            case ServiceLifetime.Singleton:
                                collection.AddSingleton(interfaceType, classType);
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
        }

        /// <summary>
        /// Configures the domain container.
        /// </summary>
        /// <param name="param">The parameter.</param>
        public static void ConfigureDomainContainer(ParamIocContainer param)
        {
            // IT'S NOT NECESSARY TO DECLARE Services (They are automatically managed by the method BiaIocContainer.RegisterServicesFromAssembly)
            RegisterServicesFromAssembly(
                collection: param.Collection,
                assemblyName: "BIA.Net.Core.Domain",
                serviceLifetime: ServiceLifetime.Transient);

            // Domain
            Assembly assembly = Assembly.Load("BIA.Net.Core.Domain");

            Type biaBaseMapperType = typeof(BiaBaseMapper<,,>);
            List<Type> derivedBiaBaseMapperTypes = ReflectiveEnumerator.GetDerivedTypes(assembly, biaBaseMapperType);
            foreach (var type in derivedBiaBaseMapperTypes)
            {
                param.Collection.AddScoped(type);
            }

            Type biaBaseQueryModelMapperType = typeof(BiaBaseQueryModelMapper<,,,,,>);
            List<Type> derivedBiaBaseQueryModelMapperTypes = ReflectiveEnumerator.GetDerivedTypes(assembly, biaBaseQueryModelMapperType);
            foreach (var type in derivedBiaBaseQueryModelMapperTypes)
            {
                param.Collection.AddScoped(type);
            }
        }

        /// <summary>
        /// Configures the common container.
        /// </summary>
        /// <param name="param">The parameter.</param>
        public static void ConfigureCommonContainer(ParamIocContainer param)
        {
            if (param.IsUnitTest)
            {
                return;
            }

            // Configuration
            param.Collection.Configure<BiaNetSection>(options => param.Configuration.GetSection("BiaNet").Bind(options));
        }

        /// <summary>
        /// Configures the infrastructure data container.
        /// </summary>
        /// <param name="param">The parameter.</param>
        public static void ConfigureInfrastructureDataContainer(ParamIocContainer param)
        {
            if (param.IsUnitTest)
            {
                return;
            }

            param.Collection.AddScoped(typeof(ITGenericRepository<,>), typeof(TGenericRepositoryEF<,>));
            param.Collection.AddScoped(typeof(ITGenericArchiveRepository<,>), typeof(TGenericArchiveRepository<,>));
            param.Collection.AddScoped(typeof(ITGenericCleanRepository<,>), typeof(TGenericCleanRepository<,>));
            param.Collection.AddScoped<IViewQueryCustomizer, ViewQueryCustomizer>();
            param.Collection.AddScoped<IFileDownloadTokenRepository, FileDownloadTokenRepository>();
        }

        /// <summary>
        /// Configures the infrastructure service container.
        /// </summary>
        /// <param name="param">The parameter.</param>
        public static void ConfigureInfrastructureServiceContainer(ParamIocContainer param)
        {
            BiaNetSection biaNetSection = null;

            if (param.Configuration != null)
            {
                biaNetSection = new BiaNetSection();
                param.Configuration.GetSection("BiaNet").Bind(biaNetSection);
            }

            // Infrastructure Service
            param.Collection.AddTransient<ILdapRepositoryHelper, LdapRepositoryHelper>();
            param.Collection.AddTransient<IBiaLocalCache, BiaLocalCache>();

            if (biaNetSection?.CommonFeatures?.DistributedCache?.IsActive == true)
            {
                param.Collection.AddTransient<IBiaDistributedCache, BiaDistributedCache>();
            }
            else
            {
                param.Collection.AddTransient<IBiaDistributedCache, BiaLocalCache>();
            }

            param.Collection.AddHttpClient<IWakeUpWebApps, WakeUpWebApps>().ConfigurePrimaryHttpMessageHandler(() =>
                CreateHttpClientHandler(biaNetSection));

            param.Collection.AddTransient<IFileRepository, FileRepository>();
            param.Collection.AddHttpClient<IImageUrlRepository, ImageUrlRepository>().ConfigurePrimaryHttpMessageHandler(() =>
                GetHttpMessagehandler(biaNetSection.ProfileConfiguration?.AuthenticationConfiguration, biaNetSection));

            param.Collection.AddHttpClient<IBiaWebApiAuthRepository, BiaWebApiAuthRepository>().ConfigurePrimaryHttpMessageHandler(() =>
            CreateHttpClientHandler(biaNetSection));
        }

        private static HttpClientHandler GetHttpMessagehandler(AuthenticationConfiguration authenticationConfiguration, BiaNetSection biaNetSection)
        {
            return CreateHttpClientHandler(biaNetSection, authenticationConfiguration.Mode == AuthenticationMode.Default);
        }
    }
}
