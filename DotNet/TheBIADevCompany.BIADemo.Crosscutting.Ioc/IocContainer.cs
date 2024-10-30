// <copyright file="IocContainer.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Ioc
{
    using System;
    using System.Collections.Generic;
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
    using System.Security.Principal;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Service;
    using Microsoft.AspNetCore.Http;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Domain.RepoContract.DocumentAnalysis;
    using TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories;
    using TheBIADevCompany.BIADemo.Infrastructure.Service.Repositories.DocumentAnalysis;

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
            ConfigureDomainContainer(collection, biaNetSection);
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
            // IT'S NOT NECESSARY TO DECLARE Services (They are automatically managed by the method BiaIocContainer.RegisterServicesFromAssembly)
            BiaIocContainer.RegisterServicesFromAssembly(
                collection: collection,
                assemblyName: "TheBIADevCompany.BIADemo.Application",
                excludedServiceNames: new List<string>() { nameof(AuthAppService) },
                serviceLifetime: ServiceLifetime.Transient);

            if (isApi)
            {
                BiaIocContainer.RegisterServicesFromAssembly(
                collection: collection,
                assemblyName: "TheBIADevCompany.BIADemo.Application",
                includedServiceNames: new List<string>() { nameof(AuthAppService) },
                serviceLifetime: ServiceLifetime.Transient);
            }

            collection.AddTransient<IBackgroundJobClient, BackgroundJobClient>();
        }

        private static void ConfigureDomainContainer(IServiceCollection collection, BiaNetSection biaNetSection)
        {
            // IT'S NOT NECESSARY TO DECLARE Services (They are automatically managed by the method BiaIocContainer.RegisterServicesFromAssembly)
            BiaIocContainer.RegisterServicesFromAssembly(
                collection: collection,
                assemblyName: "TheBIADevCompany.BIADemo.Domain",
                serviceLifetime: ServiceLifetime.Transient);

            Type templateType = typeof(BaseMapper<,,>);
            Assembly assembly = Assembly.Load("TheBIADevCompany.BIADemo.Domain");
            List<Type> derivedTypes = ReflectiveEnumerator.GetDerivedTypes(assembly, templateType);
            foreach (var type in derivedTypes)
            {
                collection.AddScoped(type);
            }

            collection.AddTransient<IPrincipal>(provider => new BiaClaimsPrincipal(provider.GetService<IHttpContextAccessor>().HttpContext.User));
            collection.AddTransient(provider => new UserContext(provider.GetService<IHttpContextAccessor>().HttpContext.Request.Headers["Accept-Language"].ToString(), biaNetSection.Cultures));
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
            collection.AddDbContext<IQueryableUnitOfWorkNoTracking, DataContextNoTracking>(
                options =>
                {
                    if (connectionString != null)
                    {
                        options.UseSqlServer(connectionString);
                    }

                    options.EnableSensitiveDataLogging();
                },
                contextLifetime: ServiceLifetime.Transient);

            // IT'S NOT NECESSARY TO DECLARE QueryCustomizer/Repository (They are automatically managed by the method BiaIocContainer.RegisterServicesFromAssembly)
            BiaIocContainer.RegisterServicesFromAssembly(
                collection: collection,
                assemblyName: "TheBIADevCompany.BIADemo.Infrastructure.Data",
                interfaceAssemblyName: "TheBIADevCompany.BIADemo.Domain",
                serviceLifetime: ServiceLifetime.Transient);

            collection.AddScoped<DataContextFactory>();
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
            collection.AddHttpClient<IIdentityProviderRepository, IdentityProviderRepository>().ConfigurePrimaryHttpMessageHandler(() => BiaIocContainer.CreateHttpClientHandler(biaNetSection, false));
            collection.AddTransient<IMailRepository, MailRepository>();

            collection.AddTransient<IClientForHubRepository, SignalRClientForHubRepository>();

            collection.AddHttpClient<IIdentityProviderRepository, IdentityProviderRepository>().ConfigurePrimaryHttpMessageHandler(() => BiaIocContainer.CreateHttpClientHandler(biaNetSection, false));

            collection.AddSingleton<PdfAnalysisRepository>();
            collection.AddSingleton<IDocumentAnalysisRepositoryFactory, DocumentAnalysisRepositoryFactory>();

            // Begin BIADemo
            collection.AddHttpClient<IRemotePlaneRepository, RemotePlaneRepository>().ConfigurePrimaryHttpMessageHandler(() => BiaIocContainer.CreateHttpClientHandler(biaNetSection));

            // End BIADemo
#endif
        }
    }
}
