namespace BIA.Net.Core.Application.Archive
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Archive;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public abstract class ArchiveServiceBase<TEntity, TKey> where TEntity : class, IEntityArchivable<TKey>
    {
        protected readonly ITGenericRepository<TEntity, TKey> entityRepository;
        private readonly ILogger logger;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        protected ArchiveServiceBase(ITGenericRepository<TEntity, TKey> entityRepository, ILogger logger)
        {
            this.entityRepository = entityRepository;
            this.logger = logger;
            this.jsonSerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
        }

        protected virtual Expression<Func<TEntity, bool>> ArchiveStepItemsSelector()
        {
            return x => x.ArchiveState == ArchiveStateEnum.Undefined;
        }

        protected virtual Expression<Func<TEntity, bool>> DeleteStepItemsSelector()
        {
            return x => x.ArchiveState == ArchiveStateEnum.Archived;
        }

        protected virtual Task<IEnumerable<TEntity>> GetDeleteStepItemsAsync()
        {
            return this.entityRepository.GetAllEntityAsync(filter: DeleteStepItemsSelector());
        }

        protected virtual Task<IEnumerable<TEntity>> GetArchiveStepItemsAsync()
        {
            return this.entityRepository.GetAllEntityAsync(filter: ArchiveStepItemsSelector());
        }

        public async Task RunAsync()
        {
            this.logger.Log(LogLevel.Information, $"Begin archive of {typeof(TEntity).Name} entity");
            await RunArchiveStepAsync();
            await RunDeleteStepAsync();
            this.logger.Log(LogLevel.Information, $"End archive of {typeof(TEntity).Name} entity");
        }

        protected virtual async Task RunDeleteStepAsync()
        {
            var items = await GetDeleteStepItemsAsync();
            foreach (var item in items)
            {
                await DeleteItemAsync(item);
            }
        }

        protected async Task DeleteItemAsync(TEntity item)
        {
            this.logger.Log(LogLevel.Information, $"Item {item.Id} : deleting");
            this.entityRepository.Remove(item);
            await this.entityRepository.UnitOfWork.CommitAsync();
        }

        protected virtual async Task RunArchiveStepAsync()
        {
            var items = await GetArchiveStepItemsAsync();
            foreach (var item in items)
            {
                await ArchiveItemAsync(item);
            }
        }

        protected async Task ArchiveItemAsync(TEntity item)
        {
            this.logger.Log(LogLevel.Information, $"Item {item.Id} : archiving");

            try
            {
                if (await SaveItemToServerAsync(item))
                {
                    item.ArchiveState = ArchiveStateEnum.Archived;
                    this.entityRepository.SetModified(item);
                    await this.entityRepository.UnitOfWork.CommitAsync();
                    this.logger.Log(LogLevel.Information, $"Item {item.Id} : archived successfully");
                }
            }
            catch (Exception ex)
            {
                this.logger.Log(LogLevel.Error, $"Item {item.Id} : failed to archive : {ex.Message}");
            }
        }

        private async Task<bool> SaveItemToServerAsync(TEntity item)
        {
            this.logger.Log(LogLevel.Information, $"Item {item.Id} : saving to server");
            try
            {
                var jsonItem = GetJson(item);
                return true;
            }
            catch(Exception ex)
            {
                this.logger.Log(LogLevel.Error, $"Item {item.Id} : failed to save to server : {ex.Message}");
                return false;
            }
        }

        private string GetJson(object element)
        {
            return JsonConvert.SerializeObject(element, this.jsonSerializerSettings);
        }
    }
}
