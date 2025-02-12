namespace BIA.Net.Core.Application.Clean
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.RepoContract;

    public abstract class CleanServiceBase<TEntity, TKey> : ICleanService where TEntity : class, IEntity<TKey>
    {
        private readonly ITGenericRepository<TEntity, TKey> repository;

        protected CleanServiceBase(ITGenericRepository<TEntity, TKey> repository)
        {
            this.repository = repository;
        }

        public async Task RunAsync()
        {
            var items = await this.repository.GetAllEntityAsync(filter: CleanRuleFilter());
            foreach(var item in items)
            {
                this.repository.Remove(item);
            }
            await this.repository.UnitOfWork.CommitAsync();
        }

        protected abstract Expression<Func<TEntity, bool>> CleanRuleFilter();
    }
}
