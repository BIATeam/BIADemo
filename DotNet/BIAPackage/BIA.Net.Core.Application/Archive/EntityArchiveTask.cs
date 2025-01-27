namespace BIA.Net.Core.Application.Archive
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public abstract class EntityArchiveTask<TEntity, TKey> : IEntityArchiveTask where TEntity : class, IEntity<TKey>
    {
        private readonly ITGenericRepository<TEntity, TKey> repository;
        private readonly ILogger logger;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        protected EntityArchiveTask(ITGenericRepository<TEntity, TKey> repository, ILogger logger)
        {
            this.repository = repository;
            this.logger = logger;
            this.jsonSerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
        }

        protected abstract Expression<Func<TEntity, object>>[] Includes();
        protected abstract Expression<Func<TEntity, bool>> Step1Predicate();

        public async Task Run()
        {
            this.logger.Log(LogLevel.Information, $"Begin archive of {typeof(TEntity)} entity");
            var step1Entities = await this.repository.GetAllEntityAsync(filter: Step1Predicate(), includes: Includes());
            var step1Json = JsonConvert.SerializeObject(step1Entities, this.jsonSerializerSettings);
            this.logger.Log(LogLevel.Information, $"Step1Entities: {step1Json}");
            this.logger.Log(LogLevel.Information, $"End archive of {typeof(TEntity)} entity");
        }
    }
}
