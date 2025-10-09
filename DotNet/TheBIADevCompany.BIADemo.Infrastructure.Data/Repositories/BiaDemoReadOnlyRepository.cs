// BIADemo only
// <copyright file="BiaDemoReadOnlyRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories
{
    using System;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Infrastructure.Data;
    using TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories.Bia;

    /// <summary>
    /// Repository for <typeparamref name="TEntity"/> on BIA Demo read only database.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TKey">Entity key type.</typeparam>
    public class BiaDemoReadOnlyRepository<TEntity, TKey> : DatabaseRepositoryBase<TEntity, TKey>, IBiaDemoReadOnlyRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BiaDemoReadOnlyRepository{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="dataContextFactory">The data context factory.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public BiaDemoReadOnlyRepository(DataContextFactory dataContextFactory, IServiceProvider serviceProvider)
            : base(dataContextFactory, serviceProvider, "ProjectDatabaseReadOnly")
        {
        }
    }
}
