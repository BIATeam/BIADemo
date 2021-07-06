// <copyright file="BIAIocContainerTest.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Test.IoC
{
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.Infrastructure.Data.Repositories;
    using BIA.Net.Core.Presentation.Common.Features.HubForClients;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using System;
    using System.Security.Principal;

    /// <summary>
    /// IoC container used for unit tests.
    /// </summary>
    public class BIAIocContainerTest
    {
        static Mock<IHubClients> mockClients;
        static Mock<IClientProxy> mockClientProxy;
        static Mock<IHubContext<HubForClients>> hubContext;


        /// <summary>
        /// The method used to register all instances for unit test purposes.
        /// </summary>
        /// <param name="services">The collection of services to update.</param>
        public static void ConfigureContainerTest<TDbContext>(IServiceCollection services)
            where TDbContext : DbContext, IQueryableUnitOfWork
        {
            services.AddLogging();

            ConfigureInfrastructureDataContainerTest<TDbContext>(services);
            ConfigureInfrastructureServiceContainerTest<TDbContext>(services);


        }

        /// <summary>
        /// Configure the database IoC.
        /// </summary>
        /// <param name="services">The collection of services to update.</param>
        public static void ConfigureInfrastructureDataContainerTest<TDbContext>(IServiceCollection services)
            where TDbContext : DbContext, IQueryableUnitOfWork
        {
            services.AddDbContext<IQueryableUnitOfWork, TDbContext>(
                options =>
                {
                    options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                    options.EnableSensitiveDataLogging();
                });
            services.AddScoped(typeof(ITGenericRepository<>), typeof(TGenericRepositoryEF<>));
        }

        /// <summary>
        /// Configure the database IoC.
        /// </summary>
        /// <param name="services">The collection of services to update.</param>
        public static void ConfigureInfrastructureServiceContainerTest<TDbContext>(IServiceCollection services)
            where TDbContext : DbContext, IQueryableUnitOfWork
        {
            mockClients = new Mock<IHubClients>();
            mockClientProxy = new Mock<IClientProxy>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            hubContext = new Mock<IHubContext<HubForClients>>();
            hubContext.Setup(x => x.Clients).Returns(() => mockClients.Object);

            services.AddSingleton<IHubContext<HubForClients>>(hubContext.Object);
        }

        /// <summary>
        /// Apply the given principal mock to the dependency injection system.
        /// </summary>
        /// <param name="services">The collection of services to update.</param>
        /// <param name="principal">The principal mock to apply.</param>
        public static void ApplyPrincipalMock(IServiceCollection services, BIAClaimsPrincipal principal)
        {
            services.AddTransient<IPrincipal>(p => principal);
        }
    }
}
