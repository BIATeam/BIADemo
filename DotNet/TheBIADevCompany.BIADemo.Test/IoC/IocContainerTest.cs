// <copyright file="IocContainerTest.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Test.IoC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using BIA.Net.Core.Test.Data;
    using BIA.Net.Core.Test.IoC;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TheBIADevCompany.BIADemo.Crosscutting.Ioc;
    using TheBIADevCompany.BIADemo.Infrastructure.Data;
    using TheBIADevCompany.BIADemo.Test.Data;

    /// <summary>
    /// Specific IoC container used for unit tests.
    ///
    /// Note: Add IoC for components specific to your project in <see cref="ConfigureContainerTest(IServiceCollection)"/> method.
    /// </summary>
    /// <seealso cref="BIAIocContainerTest"/>
    /// <seealso cref="IocContainer"/>
    public static class IocContainerTest
    {
        /// <summary>
        /// Method used to register all instances for unit test purposes.
        ///
        /// Note: Add IoC for components specific to your project in this method.
        /// </summary>
        /// <param name="services">The collection of services to update.</param>
        public static void ConfigureContainerTest(IServiceCollection services)
        {
            IConfiguration configuration = BuildConfiguration();
            services.AddSingleton(configuration);

            IocContainer.ConfigureContainer(services, null, true, true);
            BIAIocContainerTest.ConfigureContainerTest<DataContext, DataContextNoTracking>(services);

            services.AddTransient<IMockEntityFramework<DataContext, DataContextNoTracking>, MockEntityFrameworkInMemory>();

            RegisterControllersFromAssembly(services, "TheBIADevCompany.BIADemo.Presentation.Api");
        }

        /// <summary>
        /// Registers the controllers from assembly.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="assemblyName">Name of the assembly.</param>
        private static void RegisterControllersFromAssembly(
            IServiceCollection collection,
            string assemblyName)
        {
            Assembly classAssembly = Assembly.Load(assemblyName);

            IEnumerable<Type> classTypes = classAssembly.GetTypes().Where(type => type.IsClass && !type.IsAbstract && type.Name.EndsWith("Controller"));

            foreach (Type classType in classTypes)
            {
                collection.AddTransient(classType, classType);
            }
        }

        /// <summary>
        /// Builds the configuration.
        /// </summary>
        /// <returns>The configuration.</returns>
        private static IConfiguration BuildConfiguration()
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            return configurationBuilder.Build();
        }
    }
}
