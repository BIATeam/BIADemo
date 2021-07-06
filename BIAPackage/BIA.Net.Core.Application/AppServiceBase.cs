// <copyright file="AppServiceBase.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application
{
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.RepoContract;

    /// <summary>
    /// The base class for all application service.
    /// </summary>
    public abstract class AppServiceBase<TEntity>
                where TEntity : class, IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppServiceBase"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        protected AppServiceBase(ITGenericRepository<TEntity> repository)
        {
            this.Repository = repository;
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        protected ITGenericRepository<TEntity> Repository { get; }
    }
}