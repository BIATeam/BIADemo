namespace BIA.Net.Core.WorkerService.Features.Archive
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;

    public class ArchiveService : IHostedService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly List<IEntityArchiveConfiguration> entityArchiveConfigurations;
        private readonly BiaNetSection biaNetSection;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public ArchiveService(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory, IEnumerable<IEntityArchiveConfiguration> entityArchiveConfigurations)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.entityArchiveConfigurations = entityArchiveConfigurations.ToList();

            this.biaNetSection = new BiaNetSection();
            configuration.GetSection("BiaNet").Bind(this.biaNetSection);

            this.jsonSerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = this.serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IQueryableUnitOfWork>();

            var contextEntities = context
                .GetType()
                .GetProperties()
                .Where(x => x.PropertyType.IsGenericType
                    && x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .Select(x => x.PropertyType.GenericTypeArguments.FirstOrDefault())
                .Distinct()
                .ToList();

            foreach (var entityArchive in this.entityArchiveConfigurations)
            {
                var contextEntity = contextEntities.FirstOrDefault(ce => ce?.Name == entityArchive.EntityType.Name);
                if (contextEntity is null)
                    continue;

                var archiveMethod = this.GetType()
                    .GetMethod(nameof(ArchiveEntity), BindingFlags.NonPublic | BindingFlags.Instance)?
                    .MakeGenericMethod(contextEntity);

                if (archiveMethod != null)
                {
                    await (Task)archiveMethod.Invoke(this, [context, entityArchive]);
                }
            }
        }

        private async Task ArchiveEntity<TEntity>(IQueryableUnitOfWork context, IEntityArchiveConfiguration entityArchiveConfiguration) where TEntity : class
        {
            Debug.WriteLine($"### Start archiving {entityArchiveConfiguration.EntityType.Name} entity");

            var query = entityArchiveConfiguration.SetIncludes(context.RetrieveSet<TEntity>());
            await ArchiveEntityStep1(query, entityArchiveConfiguration);

            Debug.WriteLine($"### End archiving {entityArchiveConfiguration.EntityType.Name} entity");
        }

        private async Task ArchiveEntityStep1<TEntity>(IQueryable<TEntity> query, IEntityArchiveConfiguration entityArchiveConfiguration) where TEntity : class
        {
            var filter = CreateEntityFilterExpression<TEntity>(entityArchiveConfiguration.Step1Predicate());
            var entities = await query.Where(filter).ToListAsync();
            var resultJson = JsonConvert.SerializeObject(entities, this.jsonSerializerSettings);
            Debug.WriteLine($"Step1Entities: {resultJson}");
        }

        private static Expression<Func<TEntity, bool>> CreateEntityFilterExpression<TEntity>(Expression<Func<object, bool>> filter) where TEntity : class
        {
            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            var body = Expression.Invoke(filter, Expression.Convert(parameter, typeof(object)));
            return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
