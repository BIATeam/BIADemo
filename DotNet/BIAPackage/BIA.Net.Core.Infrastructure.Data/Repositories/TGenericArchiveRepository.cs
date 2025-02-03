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
    using BIA.Net.Core.Domain.Archive;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;

    /// <summary>
    /// Generich archive repository of an entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The entity key type.</typeparam>
    public abstract class TGenericArchiveRepository<TEntity, TKey> : ITGenericArchiveRepository<TEntity, TKey>
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
        protected TGenericArchiveRepository(IQueryableUnitOfWork dataContext)
        {
            this.dataContext = dataContext;
        }

        /// <inheritdoc/>
        public virtual async Task<IReadOnlyList<TEntity>> GetItemsToArchiveAsync()
        {
            return await this.GetAllWithIncludes().Where(this.ArchiveStep_ItemsSelector()).ToListAsync();
        }

        /// <inheritdoc/>
        public virtual async Task<IReadOnlyList<TEntity>> GetItemsToBlockAsync()
        {
            return await this.GetAllWithIncludes().Where(this.ArchiveStep_BlockedItemsSelector()).ToListAsync();
        }

        /// <inheritdoc/>
        public virtual async Task<IReadOnlyList<TEntity>> GetItemsToDeleteAsync()
        {
            return await this.GetAllWithIncludes().Where(this.DeleteStep_ItemsSelector()).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateArchiveStateAsync(TEntity entity, ArchiveState archiveState)
        {
            entity.ArchiveState = archiveState;
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
        protected virtual Expression<Func<TEntity, bool>> ArchiveStep_ItemsSelector()
        {
            return x => x.ArchiveState == ArchiveState.Undefined;
        }

        /// <summary>
        /// Selector of items to set as blocked at archive step.
        /// </summary>
        /// <returns>Selector expression.</returns>
        protected virtual Expression<Func<TEntity, bool>> ArchiveStep_BlockedItemsSelector()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selector of items to delete.
        /// </summary>
        /// <returns>Selector expression.</returns>
        protected virtual Expression<Func<TEntity, bool>> DeleteStep_ItemsSelector()
        {
            return x => x.ArchiveState == ArchiveState.Archived;
        }

        /// <summary>
        /// Return all the entities with includes.
        /// </summary>
        /// <returns><see cref="IQueryable{TEntity}"/>.</returns>
        protected virtual IQueryable<TEntity> GetAllWithIncludes()
        {
            var entityType = this.dataContext.FindEntityType(typeof(TEntity));
            return IncludeRecursive(this.dataContext.RetrieveSet<TEntity>(), entityType);
        }

        private static IQueryable<TEntity> IncludeRecursive(IQueryable<TEntity> query, IEntityType entityType, string parentPath = null, IEntityType parentEntityType = null)
        {
            foreach (var navigation in entityType.GetNavigations())
            {
                var navigationEntityType = navigation.TargetEntityType;
                if (navigationEntityType == parentEntityType)
                {
                    continue;
                }

                string navigationPath = string.IsNullOrEmpty(parentPath) ? navigation.Name : $"{parentPath}.{navigation.Name}";
                query = query.Include(navigationPath);

                if (navigation.ForeignKey.PrincipalEntityType == entityType && (navigation.ForeignKey.DeleteBehavior == DeleteBehavior.Cascade || navigation.ForeignKey.DeleteBehavior == DeleteBehavior.ClientCascade))
                {
                    query = IncludeRecursive(query, navigationEntityType, navigationPath, entityType);
                }
            }

            return query;
        }
    }
}
