// <copyright file="IocContainer.cs" company="BIA">
//  Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Core.IocContainer
{
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Helper;
    using BIA.Net.Core.Infrastructure.Service.Repositories.Ldap;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The IoC Container.
    /// </summary>
    public static class BIAIocContainer
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
            ConfigureInfrastructureServiceContainer(collection);
            ConfigureDomainContainer(collection);
            ConfigureApplicationContainer(collection);

            if (!isUnitTest)
            {
                ConfigureInfrastructureDataContainer(collection, configuration);
                ConfigureCommonContainer(collection, configuration);
            }
        }

        private static void ConfigureApplicationContainer(IServiceCollection collection)
        {

        }

        private static void ConfigureDomainContainer(IServiceCollection collection)
        {
            // Domain

        }

        private static void ConfigureCommonContainer(IServiceCollection collection, IConfiguration configuration)
        {
            // Configuration
            collection.Configure<BiaNetSection>(options => configuration.GetSection("BiaNet").Bind(options));
        }

        private static void ConfigureInfrastructureDataContainer(IServiceCollection collection, IConfiguration configuration)
        {
            // Infrastructure Data
        }

        private static void ConfigureInfrastructureServiceContainer(IServiceCollection collection)
        {
            // Infrastructure Service
            collection.AddTransient<ILdapRepositoryHelper, LdapRepositoryHelper>();
            collection.AddTransient<IBIALocalCache, BIALocalCache>();
            collection.AddTransient<IBIADistributedCache, BIADistributedCache>();
            collection.AddTransient<IBIAHybridCache, BIAHybridCache>();
        }
    }
}
