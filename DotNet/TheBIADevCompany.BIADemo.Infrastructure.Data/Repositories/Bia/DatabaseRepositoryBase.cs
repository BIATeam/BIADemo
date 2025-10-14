// <copyright file="DatabaseRepositoryBase.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories.Bia
{
    using System;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.Infrastructure.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using TheBIADevCompany.BIADemo.Infrastructure.Data;

    /// <summary>
    /// Base class for database repositories.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TKey">Entity key type.</typeparam>
    public abstract class DatabaseRepositoryBase<TEntity, TKey> : TGenericRepositoryEF<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly DataContextFactory dataContextFactory;
        private readonly string databaseConfigurationKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseRepositoryBase{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="dataContextFactory">The data context factory.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="databaseConfigurationKey">The database configuration key.</param>
        protected DatabaseRepositoryBase(DataContextFactory dataContextFactory, IServiceProvider serviceProvider, string databaseConfigurationKey)
            : base(dataContextFactory.GetQueryableUnitOfWork(databaseConfigurationKey), serviceProvider)
        {
            this.dataContextFactory = dataContextFactory;
            this.databaseConfigurationKey = databaseConfigurationKey;
        }

        /// <summary>
        /// Retrieve a DBSet as no tracking from the <see cref="IQueryableUnitOfWorkNoTracking"/> created by <see cref="DataContextFactory"/> based on current <see cref="databaseConfigurationKey"/>.
        /// </summary>
        /// <returns><see cref="DbSet{TEntity}"/> as no tracking.</returns>
        protected override DbSet<TEntity> RetrieveSetNoTracking()
        {
            return this.dataContextFactory.GetQueryableUnitOfWorkNoTracking(this.databaseConfigurationKey).RetrieveSet<TEntity>();
        }
    }
}
