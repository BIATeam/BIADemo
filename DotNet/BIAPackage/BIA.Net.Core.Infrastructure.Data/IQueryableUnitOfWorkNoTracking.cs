// <copyright file="IQueryableUnitOfWorkNoTracking.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data
{
    using Microsoft.EntityFrameworkCore;

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
    }
}
