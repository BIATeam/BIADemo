// <copyright file="IocContainer.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Ioc
{
    using BIA.Net.Core.Common.Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

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
        public static void ConfigureContainer(IServiceCollection collection, IConfiguration configuration, bool isApi, bool isUnitTest = false)
        {
            BiaConfigureContainer(collection, configuration, isApi, isUnitTest);
        }

        private static void ConfigureApplicationContainer(IServiceCollection collection, bool isApi)
        {
            BiaConfigureApplicationContainer(collection, isApi);
        }

        private static void ConfigureDomainContainer(IServiceCollection collection)
        {
            BiaConfigureDomainContainer(collection);
            BiaConfigureDomainContainerAutoRegister(collection);
        }

        private static void ConfigureCommonContainer(IServiceCollection collection, IConfiguration configuration)
        {
            // Common Layer
        }

#if BIA_USE_DATABASE
        private static void ConfigureInfrastructureDataContainer(IServiceCollection collection, IConfiguration configuration, bool isUnitTest)
        {
            BiaConfigureInfrastructureDataContainer(collection, isUnitTest);
            BiaConfigureInfrastructureDataContainerAutoRegister(collection);
            BiaConfigureInfrastructureDataContainerDbContext(collection, configuration, isUnitTest);
        }
#endif

        private static void ConfigureInfrastructureServiceContainer(IServiceCollection collection, BiaNetSection biaNetSection, bool isUnitTest = false)
        {
            BiaConfigureInfrastructureServiceContainer(collection, biaNetSection, isUnitTest);
        }
    }
}