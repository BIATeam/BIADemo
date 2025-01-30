namespace BIA.Net.Core.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Configuration.WorkerFeature;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Archive;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public abstract class TGenericArchiveRepository<TEntity, TKey> : ITGenericArchiveRepository<TEntity, TKey> where TEntity : class, IEntityArchivable<TKey>
    {
        protected readonly IQueryableUnitOfWork dataContext;

        protected TGenericArchiveRepository(IQueryableUnitOfWork dataContext)
        {
            this.dataContext = dataContext;
        }

        protected virtual Expression<Func<TEntity, bool>> ArchiveStep_ItemsSelector()
        {
            return x => x.ArchiveState == ArchiveStateEnum.Undefined;
        }

        protected virtual Expression<Func<TEntity, bool>> DeleteStep_ItemsSelector()
        {
            return x => x.ArchiveState == ArchiveStateEnum.Archived;
        }

        protected virtual IQueryable<TEntity> GetAllWithIncludes()
        {
            var entityType = dataContext.FindEntityType(typeof(TEntity));
            var visitedEntityTypes = new HashSet<string>
            {
                entityType.ClrType.FullName,
            };

            return IncludeRecursive(dataContext.RetrieveSet<TEntity>(), entityType, visitedEntityTypes);
        }

        public virtual async Task<IReadOnlyList<TEntity>> GetItemsToArchiveAsync()
        {
            return await GetAllWithIncludes().Where(ArchiveStep_ItemsSelector()).ToListAsync();
        }

        public virtual async Task<IReadOnlyList<TEntity>> GetItemsToDeleteAsync()
        {
            return await dataContext.RetrieveSet<TEntity>().Where(DeleteStep_ItemsSelector()).ToListAsync();
        }

        public async Task UpdateArchiveStateAsync(TEntity entity, ArchiveStateEnum archiveState)
        {
            entity.ArchiveState = archiveState;
            dataContext.SetModified(entity);
            await dataContext.CommitAsync();
        }

        public async Task RemoveAsync(TEntity entity)
        {
            dataContext.RetrieveSet<TEntity>().Remove(entity);
            await dataContext.CommitAsync();
        }

        private static IQueryable<TEntity> IncludeRecursive(IQueryable<TEntity> query, IEntityType entityType, HashSet<string> visitedEntityTypes, string parentPath = null)
        {
            foreach (var navigation in entityType.GetNavigations())
            {
                var targetEntityType = navigation.TargetEntityType;
                if (visitedEntityTypes.Contains(targetEntityType.ClrType.FullName))
                {
                    continue;
                }

                visitedEntityTypes.Add(targetEntityType.ClrType.FullName);

                string navigationPath = string.IsNullOrEmpty(parentPath) ? navigation.Name : $"{parentPath}.{navigation.Name}";
                query = query.Include(navigationPath);
                query = IncludeRecursive(query, targetEntityType, visitedEntityTypes, navigationPath);
            }

            return query;
        }
    }
}
