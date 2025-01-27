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

    public abstract class ArchiveServiceBase<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        private readonly ILogger logger;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        protected ArchiveServiceBase(ILogger logger)
        {
            this.logger = logger;
            this.jsonSerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
        }

        protected abstract Expression<Func<TEntity, bool>> Step1Predicate();
        protected abstract Task<IEnumerable<object>> GetStep1ItemsAsync();

        public async Task RunAsync()
        {
            this.logger.Log(LogLevel.Information, $"Begin archive of {typeof(TEntity).Name} entity");
            await RunStep1Async();
            this.logger.Log(LogLevel.Information, $"End archive of {typeof(TEntity).Name} entity");
        }

        private async Task RunStep1Async()
        {
            var items = await GetStep1ItemsAsync();
            foreach (var item in items)
            {
                var jsonItem = GetJson(item);
                this.logger.Log(LogLevel.Information, $"Archiving item {jsonItem}");
            }
        }

        private string GetJson(object element)
        {
            return JsonConvert.SerializeObject(element, this.jsonSerializerSettings);
        }
    }
}
