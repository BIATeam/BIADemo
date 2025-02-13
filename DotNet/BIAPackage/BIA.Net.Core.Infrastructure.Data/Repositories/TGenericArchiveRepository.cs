// <copyright file="TGenericArchiveRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;

    /// <summary>
    /// Generich archive repository of an entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The entity key type.</typeparam>
    public class TGenericArchiveRepository<TEntity, TKey> : ITGenericArchiveRepository<TEntity, TKey>
        where TEntity : class, IEntityArchivable<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TGenericArchiveRepository{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="context">The <see cref="IQueryableUnitOfWork"/> context.</param>
        public TGenericArchiveRepository(IQueryableUnitOfWork context)
        {
            this.Context = context;
        }

        /// <summary>
        /// The context.
        /// </summary>
        protected IQueryableUnitOfWork Context { get; }

        /// <inheritdoc/>
        public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> rule)
        {
            return await this.GetAllQuery().Where(rule).ToListAsync();
        }

        /// <inheritdoc/>
        public virtual async Task SetAsArchivedAsync(TEntity entity)
        {
            entity.IsArchived = true;
            entity.ArchivedDate = DateTime.UtcNow;

            this.Context.SetModified(entity);
            await this.Context.CommitAsync();
        }

        /// <summary>
        /// Return all the entities.
        /// </summary>
        /// <returns><see cref="IQueryable{TEntity}"/>.</returns>
        protected virtual IQueryable<TEntity> GetAllQuery()
        {
            var entityType = this.Context.FindEntityType(typeof(TEntity));
            return SetIncludesRecursive(this.Context.RetrieveSet<TEntity>(), entityType).AsSplitQuery();
        }

        /// <summary>
        /// Automatic add includes of entity's navigations at root level, then all cascade delete navigations recursively.
        /// </summary>
        /// <param name="query">Query to add the includes.</param>
        /// <param name="entityType">Entity type of the query.</param>
        /// <param name="parentNavigationPath">Include navigation of parent.</param>
        /// <param name="parentEntityType">Parent entity's type.</param>
        /// <returns><see cref="IQueryable{TEntity}"/>.</returns>
        private static IQueryable<TEntity> SetIncludesRecursive(IQueryable<TEntity> query, IEntityType entityType, string parentNavigationPath = null, IEntityType parentEntityType = null)
        {
            foreach (var navigation in entityType.GetNavigations())
            {
                var navigationEntityType = navigation.TargetEntityType;
                if (navigationEntityType == parentEntityType)
                {
                    continue;
                }

                string navigationPath = string.IsNullOrEmpty(parentNavigationPath) ? navigation.Name : $"{parentNavigationPath}.{navigation.Name}";
                query = query.Include(navigationPath);

                if (navigation.ForeignKey.PrincipalEntityType == entityType && (navigation.ForeignKey.DeleteBehavior == DeleteBehavior.Cascade || navigation.ForeignKey.DeleteBehavior == DeleteBehavior.ClientCascade))
                {
                    query = SetIncludesRecursive(query, navigationEntityType, navigationPath, entityType);
                }
            }

            return query;
        }
    }
}
