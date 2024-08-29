namespace BIA.Net.Core.WorkerService.Features.DataBaseHandler
{
    using System;
    using System.Buffers.Binary;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Infrastructure.Data;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class PollingDatabaseHandlerRepository<TEntity, TEntityKeyType> : IDatabaseHandlerRepository
        where TEntity : VersionedTable, IEntity<TEntityKeyType>
    {
        private readonly TimeSpan timeout = TimeSpan.FromSeconds(5);
        private readonly IServiceProvider serviceProvider;
        private readonly EntityChangeHandler onChange;
        private readonly ILogger<PollingDatabaseHandlerRepository<TEntity, TEntityKeyType>> logger;
        private CancellationTokenSource cancellationToken;

        public delegate void EntityChangeHandler(TEntity changedEntity);

        public PollingDatabaseHandlerRepository(IServiceProvider serviceProvider, EntityChangeHandler onChange)
        {
            this.logger = serviceProvider.GetService<ILogger<PollingDatabaseHandlerRepository<TEntity, TEntityKeyType>>>();
            this.serviceProvider = serviceProvider;
            this.onChange = onChange;
        }

        public virtual void Start(IServiceProvider serviceProvider)
        {
            this.Polling();
        }

        public virtual void Stop()
        {
            this.cancellationToken.Cancel();
            this.cancellationToken.Dispose();
        }

        private async Task Polling()
        {
            this.logger.LogInformation($"{this.GetType().Name}<{typeof(TEntity).Name}>.{nameof(Polling)}() : start");

            this.cancellationToken = new ();
            var previousData = await this.RetrievePreviousData();

            while (!this.cancellationToken.IsCancellationRequested)
            {
                try
                {
                    this.logger.LogInformation($"{this.GetType().Name}<{typeof(TEntity).Name}>.{nameof(Polling)}() : getting latest entities");
                    var newData = await this.RetrievePreviousData();

                    this.logger.LogInformation($"{this.GetType().Name}<{typeof(TEntity).Name}>.{nameof(Polling)}() : comparing previous entities");
                    var changedEntities = this.GetChangedEntities(previousData, newData);
                    foreach (var entity in changedEntities)
                    {
                        this.onChange(entity);
                    }

                    previousData = newData;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.ToString());
                }
                finally
                {
                    this.logger.LogInformation($"{this.GetType().Name}<{typeof(TEntity).Name}>.{nameof(Polling)}() : wait for next iteration...");
                    await Task.Delay(this.timeout);
                }
            }
        }

        private List<TEntity> GetChangedEntities(List<TEntity> previousData, List<TEntity> newData)
        {
            var deletedEntities = previousData.Where(pe => !newData.Exists(ne => ne.Id.Equals(pe.Id))).ToList();
            var addedEntities = newData.Where(ne => !previousData.Exists(pe => pe.Id.Equals(ne.Id))).ToList();
            var modifiedEntities = previousData.Except(deletedEntities).Where(pe => newData.Exists(ne => ne.Id.Equals(pe.Id) && !ne.RowVersion.SequenceEqual(pe.RowVersion))).ToList();

            var changedEntities = deletedEntities.Concat(addedEntities).Concat(modifiedEntities).OrderByDescending(x => BinaryPrimitives.ReadUInt64BigEndian(x.RowVersion)).ToList();
            this.logger.LogInformation($"{this.GetType().Name}<{typeof(TEntity).Name}>.{nameof(GetChangedEntities)}() : entities changed count = {changedEntities.Count}");

            return changedEntities;
        }

        private async Task<List<TEntity>> RetrievePreviousData()
        {
            try
            {
                var dataContext = this.serviceProvider.GetRequiredService<IQueryableUnitOfWorkReadOnly>();
                return await dataContext.RetrieveSet<TEntity>().ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
