// <copyright file="IQueryableUnitOfWorkNoTracking.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data
{
    using System.Linq;
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;

    /// <summary>
    /// The interface base for Data context with no tracking.
    /// </summary>
    public interface IQueryableUnitOfWorkNoTracking
    {
        /// <summary>
        /// Get the ObjectSet of the of type TEntity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>The set of entity.</returns>
        DbSet<TEntity> RetrieveSet<TEntity>()
            where TEntity : class;

        /// <inheritdoc cref="IModel.FindEntityType(Type)"/>
        IEntityType FindEntityType(Type entityType);

        IQueryable RetrieveSet(Type entityType);
    }
}
