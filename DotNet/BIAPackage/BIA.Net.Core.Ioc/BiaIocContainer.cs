// <copyright file="BiaIocContainer.cs" company="BIA">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.IocContainer
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Reflection;
    using BIA.Net.Core.Application.Translation;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Data.Repositories;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Ldap;
    using BIA.Net.Core.Ioc;
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
            ConfigureApplicationContainer(collection);

            if (!isUnitTest)
            {
                ConfigureInfrastructureDataContainer(collection);
                ConfigureCommonContainer(collection, configuration);
            }
        }

        private static void ConfigureApplicationContainer(IServiceCollection collection)
        {
            collection.AddTransient<ILanguageAppService, LanguageAppService>();
        }

        private static void ConfigureDomainContainer(IServiceCollection collection)
        {
            // Domain
            Type templateType = typeof(BaseMapper<,,>);
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

            if (biaNetSection?.CommonFeatures?.DistributedCache?.IsActive == true)
            {
                collection.AddTransient<IBiaDistributedCache, BiaDistributedCache>();
            }
            else
            {
                collection.AddTransient<IBiaDistributedCache, BiaLocalCache>();
            }

            collection.AddTransient<IBiaHybridCache, BiaHybridCache>();

            collection.AddHttpClient<IWakeUpWebApps, WakeUpWebApps>().ConfigurePrimaryHttpMessageHandler(() => CreateHttpClientHandler(biaNetSection));
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
