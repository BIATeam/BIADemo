// <copyright file="BIAAbstractUnitTest.cs" company="BIA">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Test
{
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.Test.Data;
    using BIA.Net.Core.Test.Mock;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Base class for all unit tests.
    /// It forces the reset of the test context (database mock and IoC) before each test.
    /// </summary>
    /// <typeparam name="TMockEF">The type of the Entity Framework mock.</typeparam>
    /// <typeparam name="TDbContext">The type of the database context.</typeparam>
    public abstract class BIAAbstractUnitTest<TMockEF, TDbContext>
        where TDbContext : IQueryableUnitOfWork
        where TMockEF : class, IMockEntityFramework<TDbContext>
    {
        /// <summary>
        /// Shall we initialize the database with some default values?
        /// </summary>
        protected readonly bool isInitDB;

        /// <summary>
        /// The collection of services used for dependency injection.
        /// 
        /// Note: Call <see cref="RefreshContext"/> when all modifications have been done, otherwise your modifications will not be taken into account.
        /// </summary>
        protected IServiceCollection servicesCollection;

        /// <summary>
        /// The principal builder (used to mock authentication related data).
        /// </summary>
        protected PrincipalMockBuilder principalBuilder;

        /// <summary>
        /// The service provider.
        /// Used to manage dependency injection of services, controllers, etc.
        /// </summary>
        private ServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BIAAbstractUnitTest{TMockEF, TDbContext}"/> class.
        /// </summary>
        /// <param name="isInitDB">Shall we initialize the database with some default values?</param>
        protected BIAAbstractUnitTest(bool isInitDB)
        {
            this.isInitDB = isInitDB;
            this.principalBuilder = new PrincipalMockBuilder();
        }

        /// <summary>
        /// The database mock.
        /// It can be used to access tables and modify their content.
        /// </summary>
        protected TMockEF DbMock { get; private set; }

        /// <summary>
        /// Method used to reinitialize the context before each test:
        /// - Principal mock
        /// - List of services available for dependency injection
        /// - Database context
        /// </summary>
        [TestInitialize]
        public void InitTestBase()
        {
            InitServiceCollection();
            RefreshContext();
        }

        /// <summary>
        /// Refresh the context of the test.
        /// 
        /// WARNING! This method shall be called when <see cref="servicesCollection"/> has been modified. 
        /// It will:
        /// - Inject the principal mock (used to mock authentication related data)
        /// - Reinitialize the ServiceProvider based on the configured collection of services
        /// - Reinitialize the DbContext
        /// 
        /// WARNING! Since it resets the DbContext, any data added in DB before calling this method will be lost.
        /// </summary>
        protected void RefreshContext()
        {
            this.principalBuilder.BuildAndApply(this.servicesCollection);
            this.serviceProvider = this.servicesCollection.BuildServiceProvider();

            this.InitDbMock();
        }

        /// <summary>
        /// Initialize the database mock.
        /// </summary>
        protected void InitDbMock()
        {
            // Initialize database mock.
            this.DbMock = GetService<IMockEntityFramework<TDbContext>>() as TMockEF;

            if (this.isInitDB)
            {
                this.DbMock.InitDefaultData();
            }
        }

        /// <summary>
        /// Get an instance of a component of the given type (through dependency injection / IoC).
        /// </summary>
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <returns>An instance of a component of the given type.</returns>
        protected T GetService<T>()
        {
            return this.serviceProvider.GetService<T>();
        }

        /// <summary>
        /// Get a new instance of a controller of the given type (through dependency injection / IoC), with an initialized HTTP context.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <returns>A new instance of a controller of the given type, with an initialized HTTP context.</returns>
        protected TController GetControllerWithHttpContext<TController>()
            where TController : ControllerBase
        {
            TController controller = this.GetService<TController>();

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext(),
            };

            return controller;
        }

        /// <summary>
        /// Initialize the collection of services for dependency injection.
        /// </summary>
        protected abstract void InitServiceCollection();
    }
}
