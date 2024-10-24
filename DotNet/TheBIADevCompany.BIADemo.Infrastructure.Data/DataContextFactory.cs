// <copyright file="DataContextFactory.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using Audit.EntityFramework;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The factory for <see cref="DataContext"/> and <see cref="DataContextReadOnly"/>.
    /// </summary>
    public class DataContextFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration configuration;
        private readonly Dictionary<string, IQueryableUnitOfWork> queryableUnitOfWorks = new ();
        private readonly List<DatabaseConfiguration> databaseConfigurations;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContextFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">The injected service provider.</param>
        /// <param name="configuration">The injected configuration.</param>
        public DataContextFactory(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.serviceProvider = serviceProvider;
            this.configuration = configuration;
            this.databaseConfigurations = configuration.GetSection("DatabaseConfigurations").Get<List<DatabaseConfiguration>>();
            this.FillQueryableUnitOfWorks();
        }

        /// <summary>
        /// Retrieve the corresponding <see cref="IQueryableUnitOfWork"/> by key.
        /// </summary>
        /// <param name="key">The <see cref="IQueryableUnitOfWork"/> key.</param>
        /// <returns><see cref="IQueryableUnitOfWork"/>.</returns>
        /// <exception cref="InvalidOperationException">Element not found.</exception>
        public IQueryableUnitOfWork GetQueryableUnitOfWork(string key)
        {
            if (this.queryableUnitOfWorks.TryGetValue(key, out IQueryableUnitOfWork unitOfWork))
            {
                return unitOfWork;
            }

            throw new InvalidOperationException($"Unable to find {nameof(IQueryableUnitOfWork)} with key {key}");
        }

        /// <summary>
        /// Creates the corresponding <see cref="IQueryableUnitOfWorkReadOnly"/> by key.
        /// </summary>
        /// <param name="key">The <see cref="IQueryableUnitOfWorkReadOnly"/> key.</param>
        /// <returns><see cref="IQueryableUnitOfWorkReadOnly"/>.</returns>
        /// <exception cref="InvalidOperationException">Element not found.</exception>
        public IQueryableUnitOfWorkReadOnly GetQueryableUnitOfWorkReadOnly(string key)
        {
            var databaseConfiguration = this.databaseConfigurations.Find(x => x.Key == key)
                ?? throw new InvalidOperationException($"Unable to find {nameof(DatabaseConfiguration)} with key {key}");

            return this.CreateDataContextReadOnly(databaseConfiguration);
        }

        private static void ConfigureDataContextOptionsProvider(DatabaseConfiguration databaseConfiguration, DbContextOptionsBuilder<DataContext> dataContextOptions)
        {
            switch (databaseConfiguration.Provider.ToLower())
            {
                case "sqlserver":
                    dataContextOptions.UseSqlServer(databaseConfiguration.ConnectionString);
                    break;
                case "postgresql":
                    dataContextOptions.UseNpgsql(databaseConfiguration.ConnectionString);
                    break;
                default:
                    throw new NotImplementedException($"Provider {databaseConfiguration.Provider} not handled.");
            }
        }

        private void FillQueryableUnitOfWorks()
        {
            foreach (var databaseConfiguration in this.databaseConfigurations)
            {
                var queryableUnitOfWork = this.CreateDataContext(databaseConfiguration);
                this.queryableUnitOfWorks.TryAdd(databaseConfiguration.Key, queryableUnitOfWork);
            }
        }

        private DataContext CreateDataContext(DatabaseConfiguration databaseConfiguration)
        {
            var dataContextLogger = this.serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<DataContext>();
            var dataContextOptions = new DbContextOptionsBuilder<DataContext>();
            ConfigureDataContextOptionsProvider(databaseConfiguration, dataContextOptions);
            dataContextOptions.EnableSensitiveDataLogging();
            dataContextOptions.AddInterceptors(new AuditSaveChangesInterceptor());
            return new DataContext(dataContextOptions.Options, dataContextLogger, this.configuration);
        }

        private DataContextReadOnly CreateDataContextReadOnly(DatabaseConfiguration databaseConfiguration)
        {
            var dataContextLogger = this.serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<DataContextReadOnly>();
            var dataContextOptions = new DbContextOptionsBuilder<DataContext>();
            ConfigureDataContextOptionsProvider(databaseConfiguration, dataContextOptions);
            dataContextOptions.EnableSensitiveDataLogging();
            return new DataContextReadOnly(dataContextOptions.Options, dataContextLogger, this.configuration);
        }
    }
}
