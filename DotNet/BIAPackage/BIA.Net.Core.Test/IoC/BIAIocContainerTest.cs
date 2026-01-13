// <copyright file="BIAIocContainerTest.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Test.IoC
{
    using System;
    using System.Security.Principal;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.Infrastructure.Data.Repositories;
    using BIA.Net.Core.Presentation.Common.Features.HubForClients;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;

    /// <summary>
    /// IoC container used for unit tests.
    /// </summary>
    public static class BIAIocContainerTest
    {
        private static Mock<IHubClients> mockClients;
        private static Mock<IClientProxy> mockClientProxy;
        private static Mock<IHubContext<HubForClients>> hubContext;

        /// <summary>
        /// The method used to register all instances for unit test purposes.
        /// </summary>
        /// <param name="services">The collection of services to update.</param>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <typeparam name="TDbContextReadOnly">The type of the database context read only.</typeparam>
        public static void ConfigureContainerTest<TDbContext, TDbContextReadOnly>(IServiceCollection services)
            where TDbContext : DbContext, IQueryableUnitOfWork
            where TDbContextReadOnly : DbContext, IQueryableUnitOfWorkNoTracking
        {
            services.AddLogging();

            ConfigureInfrastructureDataContainerTest<TDbContext, TDbContextReadOnly>(services);
            ConfigureInfrastructureServiceContainerTest(services);
        }

        /// <summary>
        /// Configure the database IoC.
        /// </summary>
        /// <param name="services">The collection of services to update.</param>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <typeparam name="TDbContextReadOnly">The type of the database context read only.</typeparam>
        public static void ConfigureInfrastructureDataContainerTest<TDbContext, TDbContextReadOnly>(IServiceCollection services)
            where TDbContext : DbContext, IQueryableUnitOfWork
            where TDbContextReadOnly : DbContext, IQueryableUnitOfWorkNoTracking
        {
            services.AddDbContext<IQueryableUnitOfWork, TDbContext>(
                options =>
                {
                    options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                    options.EnableSensitiveDataLogging();
                });

            services.AddDbContext<IQueryableUnitOfWorkNoTracking, TDbContextReadOnly>(
               options =>
               {
                   options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                   options.EnableSensitiveDataLogging();
               },
               contextLifetime: ServiceLifetime.Transient);

            services.AddScoped(typeof(ITGenericRepository<,>), typeof(TGenericRepositoryEF<,>));
        }

        /// <summary>
        /// Configure the database IoC.
        /// </summary>
        /// <param name="services">The collection of services to update.</param>
        public static void ConfigureInfrastructureServiceContainerTest(IServiceCollection services)
        {
            mockClients = new Mock<IHubClients>();
            mockClientProxy = new Mock<IClientProxy>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            hubContext = new Mock<IHubContext<HubForClients>>();
            hubContext.Setup(x => x.Clients).Returns(() => mockClients.Object);

            services.AddSingleton(hubContext.Object);
        }

        /// <summary>
        /// Apply the given principal mock to the dependency injection system.
        /// </summary>
        /// <param name="services">The collection of services to update.</param>
        /// <param name="principal">The principal mock to apply.</param>
        public static void ApplyPrincipalMock(IServiceCollection services, BiaClaimsPrincipal principal)
        {
            services.AddTransient<IPrincipal>(p => principal);
        }
    }
}
