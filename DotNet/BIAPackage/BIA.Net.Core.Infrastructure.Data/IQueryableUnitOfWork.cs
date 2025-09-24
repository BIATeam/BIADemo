// <copyright file="IQueryableUnitOfWork.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data
{
    using System;
    using System.Linq;
    using BIA.Net.Core.Common;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;

    /// <summary>
    /// The interface base for Data context.
    /// </summary>
    public interface IQueryableUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Get the ObjectSet of the of type TEntity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>The set of entity.</returns>
        DbSet<TEntity> RetrieveSet<TEntity>()
            where TEntity : class;

        /// <summary>
        /// Set the item as modified.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <typeparam name="TEntity">The entity type of the item.</typeparam>
        void SetModified<TEntity>(TEntity item)
            where TEntity : class;

        /// <inheritdoc cref="IModel.FindEntityType(Type)"/>
        IEntityType FindEntityType(Type entityType);

        IQueryable RetrieveSet(Type entityType);
    }
}