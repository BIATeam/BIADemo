// <copyright file="BiaDataContext.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.DistCacheModule.Aggregate;
    using BIA.Net.Core.Domain.TranslationModule.Aggregate;
    using BIA.Net.Core.Infrastructure.Data.Helpers;
    using BIA.Net.Core.Infrastructure.Data.ModelBuilders;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Bia Data Context.
    /// </summary>
    public class BiaDataContext : DbContext, IQueryableUnitOfWork
    {
        /// <summary>
        /// The current logger.
        /// </summary>
        private readonly ILogger<BiaDataContext> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaDataContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger.</param>
        public BiaDataContext(DbContextOptions options, ILogger<BiaDataContext> logger)
            : base(options)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Gets or sets the language DBSet.
        /// </summary>
        public DbSet<Language> Languages { get; set; }

        /// <summary>
        /// Gets or sets the distibued cache DBSet.
        /// </summary>
        public virtual DbSet<DistCache> DistCache { get; set; }

        /// <summary>
        /// Save Change on DataBase.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken"/> to observe while waiting for the
        /// task to complete.
        /// </param>
        /// <returns>Number of Modified Element.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var entities = from e in this.ChangeTracker.Entries()
                               where e.State == EntityState.Added
                                     || e.State == EntityState.Modified
                               select e.Entity;
                foreach (var entity in entities)
                {
                    var validationContext = new ValidationContext(entity);
                    Validator.ValidateObject(entity, validationContext);
                }

                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (ValidationException exception)
            {
                this.logger.LogError(exception, "An error occured on entity validation.");
                this.RollbackChanges();
                throw new DataException(exception.Message, exception);
            }
            catch (DbUpdateException exception)
            {
                this.logger.LogError(exception, "An error occured while saving data.");
                this.RollbackChanges();
                throw new DataException("An error occured while saving data.", exception);
            }
        }

        /// <inheritdoc cref="IQueryableUnitOfWork.CommitAsync"/>
        public async Task<int> CommitAsync()
        {
            return await this.SaveChangesAsync();
        }

        /// <summary>
        /// Rollback changes in the current context.
        /// </summary>
        public void RollbackChanges()
        {
            this.ChangeTracker.Entries().ToList().ForEach(entry => entry.State = EntityState.Unchanged);
        }

        /// <summary>
        /// Reset tracking.
        /// </summary>
        public void Reset()
        {
            this.ChangeTracker.Clear();
        }

        /// <summary>
        /// Bulk function to add entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="items">List of the items to add.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task AddBulkAsync<TEntity>(IEnumerable<TEntity> items)
            where TEntity : class
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (this.Database.ProviderName.EndsWith(".SqlServer"))
            {
                SqlServerBulkHelper.Insert(this, items?.ToList());
            }
            else
            {
                throw new NotImplementedException("this.Database.ProviderName: " + this.Database.ProviderName);
            }
        }

        /// <summary>
        /// Bulk function to update entities. Obsolete in V3.9.0.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="items">List of the items to update.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
#pragma warning disable S1133 // Deprecated code should be removed
        [Obsolete(message: "UpdateBulkAsync is deprecated, please use a custom repository instead and use the Entity Framework's ExecuteUpdateAsync method (See the example with the EngineRepository in BIADemo).", error: true)]
#pragma warning restore S1133 // Deprecated code should be removed
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task UpdateBulkAsync<TEntity>(IEnumerable<TEntity> items)
            where TEntity : class
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Bulk function to delete entities. Obsolete in V3.9.0.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="items">List of the items to delete.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
#pragma warning disable S1133 // Deprecated code should be removed
        [Obsolete(message: "RemoveBulkAsync is deprecated, please use a custom repository instead and use the Entity Framework's ExecuteDeleteAsync method (See the example with the EngineRepository in BIADemo).", error: true)]
#pragma warning restore S1133 // Deprecated code should be removed
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task RemoveBulkAsync<TEntity>(IEnumerable<TEntity> items)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            where TEntity : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Attach the item to the current context.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <typeparam name="TEntity">The entity type of the item.</typeparam>
        public new void Attach<TEntity>(TEntity item)
            where TEntity : class
        {
            base.Attach(item);
        }

        /// <summary>
        /// Get the ObjectSet of the of type TEntity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>The set of entity.</returns>
        public DbSet<TEntity> RetrieveSet<TEntity>()
            where TEntity : class
        {
            return this.Set<TEntity>();
        }

        /// <summary>
        /// Set the item as modified.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <typeparam name="TEntity">The entity type of the item.</typeparam>
        public void SetModified<TEntity>(TEntity item)
            where TEntity : class
        {
            this.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// OnModelCreating.
        /// </summary>
        /// <param name="modelBuilder">the model Builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DistCacheModelBuilder.CreateDistCacheModel(modelBuilder);
        }

        /// <summary>
        /// OnEndModelCreating.
        /// </summary>
        /// <param name="modelBuilder">the model Builder.</param>
        protected virtual void OnEndModelCreating(ModelBuilder modelBuilder)
        {
            RowVersionBuilder.CreateRowVersion(modelBuilder);
        }
    }
}
