// <copyright file="IQueryableUnitOfWorkNoTracking.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data
{
    using System;
    using System.Linq;
    using BIA.Net.Core.Common.Enum;
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

        /// <summary>
        /// Get the set of the entity by its <paramref name="entityType"/>.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <returns>The set of entity as <see cref="IQueryable"/>.</returns>
        IQueryable RetrieveSet(Type entityType);

        /// <inheritdoc cref="IModel.FindEntityType(Type)"/>
        IEntityType FindEntityType(Type entityType);

        /// <summary>
        /// Gets the database provider enum for the current context.
        /// </summary>
        /// <returns>The database provider enum.</returns>
        /// <exception cref="NotSupportedException">If the database provider is not supported.</exception>
        DbProvider GetDatabaseProviderEnum();
    }
}
