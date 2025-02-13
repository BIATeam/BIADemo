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
    using Microsoft.Extensions.Logging;

    public abstract class CleanServiceBase<TEntity, TKey> : ICleanService where TEntity : class, IEntity<TKey>
    {
        private readonly ITGenericCleanRepository<TEntity, TKey> repository;

        protected CleanServiceBase(ITGenericCleanRepository<TEntity, TKey> repository, ILogger logger)
        {
            this.repository = repository;
            this.Logger = logger;
        }

        protected ILogger Logger { get; }

        public async Task RunAsync()
        {
            try
            {
                this.Logger.LogInformation("Cleaning {EntityName} entities...", typeof(TEntity).Name);
                var cleanedItemCount = await this.repository.RemoveAll(this.CleanRuleFilter());
                if (cleanedItemCount == 0)
                {
                    this.Logger.LogInformation("No {EntityName} entities to clean", typeof(TEntity).Name);
                    return;
                }

                this.Logger.LogInformation("Successfully clean {CleandItemCount} {EntityName} entities !", cleanedItemCount, typeof(TEntity).Name);
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, "Fail to clean {EntityName} entities : {Error}", typeof(TEntity).Name, ex.Message);
            }
        }

        protected abstract Expression<Func<TEntity, bool>> CleanRuleFilter();
    }
}
