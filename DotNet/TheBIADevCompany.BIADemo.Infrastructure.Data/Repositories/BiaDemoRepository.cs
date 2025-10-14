// BIADemo only
// <copyright file="BiaDemoRepository.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories
{
    using System;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Infrastructure.Data;
    using TheBIADevCompany.BIADemo.Infrastructure.Data.Repositories.Bia;

    /// <summary>
    /// Repository for <typeparamref name="TEntity"/> on BIA Demo database.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TKey">Entity key type.</typeparam>
    public class BiaDemoRepository<TEntity, TKey> : DatabaseRepositoryBase<TEntity, TKey>, IBiaDemoRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BiaDemoRepository{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="dataContextFactory">The data context factory.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public BiaDemoRepository(DataContextFactory dataContextFactory, IServiceProvider serviceProvider)
            : base(dataContextFactory, serviceProvider, BiaConstants.DatabaseConfiguration.DefaultKey)
        {
        }
    }
}
