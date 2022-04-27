// <copyright file="IocContainer.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Crosscutting.Ioc
{
    using Audit.Core;
    using Audit.EntityFramework;
    using BIA.Net.Core.Common.Configuration.CommonFeature;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using BIA.Net.Core.IocContainer;
    using Hangfire;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TheBIADevCompany.BIADemo.Application.AircraftMaintenanceCompany;

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Application.Job;
    using TheBIADevCompany.BIADemo.Application.Plane;

    // End BIADemo
    using TheBIADevCompany.BIADemo.Application.Site;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Application.View;
    using TheBIADevCompany.BIADemo.Domain.NotificationModule.Service;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Service;
    using TheBIADevCompany.BIADemo.Infrastructure.Data;
    using TheBIADevCompany.BIADemo.Infrastructure.Data.Features;
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
        /// <param name="isUnitTest">Are we configuring IoC for unit tests? If so, some IoC shall not be performed here but replaced by
        /// specific ones in IocContainerTest.</param>
        public static void ConfigureContainer(IServiceCollection collection, IConfiguration configuration, bool isUnitTest = false)
        {
            BIAIocContainer.ConfigureContainer(collection, configuration, isUnitTest);

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
            // Application Layer
            collection.AddTransient<ITeamAppService, TeamAppService>();
            collection.AddTransient<ISiteAppService, SiteAppService>();
            collection.AddTransient<IMemberAppService, MemberAppService>();
            collection.AddTransient<IRoleAppService, RoleAppService>();
            collection.AddTransient<IUserAppService, UserAppService>();
            collection.AddTransient<IViewAppService, ViewAppService>();
            collection.AddTransient<IBackgroundJobClient, BackgroundJobClient>();

            // Begin BIADemo
            collection.AddTransient<IAircraftMaintenanceCompanyAppService, AircraftMaintenanceCompanyAppService>();
            collection.AddTransient<IMaintenanceTeamAppService, MaintenanceTeamAppService>();
            collection.AddTransient<IPlaneAppService, PlaneAppService>();
            collection.AddTransient<IPlaneTypeAppService, PlaneTypeAppService>();
            collection.AddTransient<IAirportAppService, AirportAppService>();
            collection.AddTransient<IBiaDemoTestHangfireService, BiaDemoTestHangfireService>();

            // End BIADemo
        }

        private static void ConfigureDomainContainer(IServiceCollection collection)
        {
            // Domain Layer
            collection.AddTransient<IUserPermissionDomainService, UserPermissionDomainService>();
            collection.AddTransient<IUserSynchronizeDomainService, UserSynchronizeDomainService>();
            collection.AddTransient<INotificationDomainService, NotificationDomainService>();
            collection.AddTransient<INotificationTypeDomainService, NotificationTypeDomainService>();
        }

        private static void ConfigureCommonContainer(IServiceCollection collection, IConfiguration configuration)
        {
            // Common Layer
        }

        private static void ConfigureInfrastructureDataContainer(IServiceCollection collection, IConfiguration configuration)
        {
            // Infrastructure Data Layer
            collection.AddDbContext<IQueryableUnitOfWork, DataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("BIADemoDatabase"));
                options.EnableSensitiveDataLogging();
                options.AddInterceptors(new AuditSaveChangesInterceptor());
            });
            collection.AddDbContext<IQueryableUnitOfWorkReadOnly, DataContextReadOnly>(
                options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("BIADemoDatabase"));
                    options.EnableSensitiveDataLogging();
                },
                contextLifetime: ServiceLifetime.Transient);

            collection.AddTransient<IMemberQueryCustomizer, MemberQueryCustomizer>();
            collection.AddTransient<IViewQueryCustomizer, ViewQueryCustomizer>();
            collection.AddTransient<INotificationQueryCustomizer, NotificationQueryCustomizer>();
            collection.Configure<AuditConfiguration>(
               configuration.GetSection("BiaNet:ApiFeatures:AuditConfiguration"));
            collection.AddSingleton<AuditFeature>();
        }

        private static void ConfigureInfrastructureServiceContainer(IServiceCollection collection)
        {
            // Infrastructure Service Layer
            collection.AddSingleton<IUserDirectoryRepository<UserFromDirectory>, LdapRepository>();
            collection.AddTransient<INotification, NotificationRepository>();
            collection.AddTransient<IClientForHubRepository, SignalRClientForHubRepository>();
        }
    }
}
