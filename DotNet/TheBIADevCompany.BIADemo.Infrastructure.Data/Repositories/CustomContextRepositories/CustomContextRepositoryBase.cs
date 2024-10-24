// <copyright file="CustomContextRepositoryBase.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories.CustomContextRepositories
{
    using System;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.Infrastructure.Data.Repositories;

    /// <summary>
    /// Base class for custom context repositories.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TKey">Entity key type.</typeparam>
    public abstract class CustomContextRepositoryBase<TEntity, TKey> : TGenericRepositoryEF<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomContextRepositoryBase{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="dataContextFactory">The data context factory.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="queryableUnitOfWorkKey">The <see cref="IQueryableUnitOfWork"/> key.</param>
        protected CustomContextRepositoryBase(DataContextFactory dataContextFactory, IServiceProvider serviceProvider, string queryableUnitOfWorkKey)
            : base(dataContextFactory.GetQueryableUnitOfWork(queryableUnitOfWorkKey), serviceProvider)
        {
        }
    }
}
