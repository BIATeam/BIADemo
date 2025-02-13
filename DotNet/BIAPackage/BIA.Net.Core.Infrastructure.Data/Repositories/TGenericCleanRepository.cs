namespace BIA.Net.Core.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.EntityFrameworkCore;

    public abstract class TGenericCleanRepository<TEntity, TKey> : ITGenericCleanRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        protected TGenericCleanRepository(IQueryableUnitOfWork context)
        {
            this.Context = context;
        }

        protected IQueryableUnitOfWork Context { get; }

        public async Task<int> RemoveAll(Expression<Func<TEntity, bool>> rule)
        {
            var set = this.Context.RetrieveSet<TEntity>();
            var setWithIncludes = this.SetIncludes(set);

            var itemsToClean = await setWithIncludes.Where(rule).ToListAsync();

            set.RemoveRange(itemsToClean);
            await this.Context.CommitAsync();

            return itemsToClean.Count;
        }

        protected virtual IQueryable<TEntity> SetIncludes(IQueryable<TEntity> query)
        {
            return query;
        }
    }
}
