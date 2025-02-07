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
        /// Datacontext.
        /// </summary>
#pragma warning disable SA1401 // Fields should be private
        protected readonly IQueryableUnitOfWork dataContext;
#pragma warning restore SA1401 // Fields should be private

        /// <summary>
        /// Initializes a new instance of the <see cref="TGenericArchiveRepository{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="dataContext">The <see cref="IQueryableUnitOfWork"/> context.</param>
        public TGenericArchiveRepository(IQueryableUnitOfWork dataContext)
        {
            this.dataContext = dataContext;
        }

        /// <inheritdoc/>
        public virtual async Task<IReadOnlyList<TEntity>> GetItemsToArchiveAsync()
        {
            var query = this.GetAllQuery().Where(this.ArchiveStepItemsSelector());
            return await query.ToListAsync();
        }

        /// <inheritdoc/>
        public virtual async Task<IReadOnlyList<TEntity>> GetItemsToDeleteAsync(double? archiveDateMaxDays = 365)
        {
            var query = this.GetAllQuery().Where(this.DeleteStepItemsSelector(archiveDateMaxDays));
            return await query.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task SetAsArchivedAsync(TEntity entity)
        {
            entity.IsArchived = true;
            entity.ArchivedDate = DateTime.UtcNow;

            this.dataContext.SetModified(entity);
            await this.dataContext.CommitAsync();
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(TEntity entity)
        {
            this.dataContext.RetrieveSet<TEntity>().Remove(entity);
            await this.dataContext.CommitAsync();
        }

        /// <summary>
        /// Selector of items to archive.
        /// </summary>
        /// <returns>Selector expression.</returns>
        protected virtual Expression<Func<TEntity, bool>> ArchiveStepItemsSelector()
        {
            return x => x.IsFixed && x.FixedDate != null && (x.ArchivedDate == null || x.ArchivedDate.Value < x.FixedDate.Value);
        }

        /// <summary>
        /// Selector of items to delete.
        /// </summary>
        /// <param name="archiveDateMaxDays">The maximum days of archive date of item to select.</param>
        /// <returns>Selector expression.</returns>
        protected virtual Expression<Func<TEntity, bool>> DeleteStepItemsSelector(double? archiveDateMaxDays = 365)
        {
            var currentDateTime = DateTime.UtcNow;
            var maxDays = archiveDateMaxDays.GetValueOrDefault();

            return x => x.IsArchived && x.ArchivedDate != null && (archiveDateMaxDays == null || x.ArchivedDate.Value.AddDays(maxDays) < currentDateTime);
        }

        /// <summary>
        /// Return all the entities.
        /// </summary>
        /// <returns><see cref="IQueryable{TEntity}"/>.</returns>
        protected virtual IQueryable<TEntity> GetAllQuery()
        {
            var entityType = this.dataContext.FindEntityType(typeof(TEntity));
            return GetQueryWithIncludesRecursive(this.dataContext.RetrieveSet<TEntity>(), entityType).AsSplitQuery();
        }

        /// <summary>
        /// Automatic add includes of entity's navigations at root level, then all cascade delete navigations recursively.
        /// </summary>
        /// <param name="query">Query to add the includes.</param>
        /// <param name="entityType">Entity type of the query.</param>
        /// <param name="parentNavigationPath">Include navigation of parent.</param>
        /// <param name="parentEntityType">Parent entity's type.</param>
        /// <returns><see cref="IQueryable{TEntity}"/>.</returns>
        private static IQueryable<TEntity> GetQueryWithIncludesRecursive(IQueryable<TEntity> query, IEntityType entityType, string parentNavigationPath = null, IEntityType parentEntityType = null)
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
                    query = GetQueryWithIncludesRecursive(query, navigationEntityType, navigationPath, entityType);
                }
            }

            return query;
        }
    }
}
