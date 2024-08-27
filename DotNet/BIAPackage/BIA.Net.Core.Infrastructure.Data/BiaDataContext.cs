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
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Exceptions;
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
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occured while saving data.");
                this.RollbackChanges();
                throw this.ManageException(ex);
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

        /// <summary>
        /// Manage the handled <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception">The handled <see cref="Exception"/>.</param>
        /// <returns>A <see cref="FrontUserException"/> corresponding to the handled exception.</returns>
        protected virtual FrontUserException ManageException(Exception exception)
        {
            return exception.GetBaseException() switch
            {
                ValidationException validationEx => this.ManageException(validationEx),
                DbUpdateException dbUpdateEx => this.ManageException(dbUpdateEx),
                SqlException sqlEx => this.ManageException(sqlEx),
                _ => new FrontUserException(exception)
            };
        }

        /// <summary>
        /// Manage the handled <paramref name="validationException"/>.
        /// </summary>
        /// <param name="validationException">The handled <see cref="ValidationException"/>.</param>
        /// <returns>A <see cref="FrontUserException"/> corresponding to the handled exception.</returns>
        protected virtual FrontUserException ManageException(ValidationException validationException)
        {
            var memberNamesJoined = string.Join(", ", validationException.ValidationResult.MemberNames);
            return new FrontUserException(FrontUserExceptionErrorMessageKey.ValidationEntity, validationException, memberNamesJoined, validationException.ValidationResult.ErrorMessage);
        }

        /// <summary>
        /// Manage the handled <paramref name="dbUpdateException"/>.
        /// </summary>
        /// <param name="dbUpdateException">The handled <see cref="DbUpdateException"/>.</param>
        /// <returns>A <see cref="FrontUserException"/> corresponding to the handled exception.</returns>
        protected virtual FrontUserException ManageException(DbUpdateException dbUpdateException)
        {
            var entry = dbUpdateException.Entries.FirstOrDefault();
            if (entry != null)
            {
                var entityTypeName = entry.Entity.GetType().Name;
                return entry.State switch
                {
                    EntityState.Added => new FrontUserException(FrontUserExceptionErrorMessageKey.AddEntity, dbUpdateException, entityTypeName),
                    EntityState.Modified => new FrontUserException(FrontUserExceptionErrorMessageKey.EditEntity, dbUpdateException, entityTypeName),
                    EntityState.Deleted => new FrontUserException(FrontUserExceptionErrorMessageKey.DeleteEntity, dbUpdateException, entityTypeName),
                    _ => new FrontUserException(dbUpdateException),
                };
            }

            return new FrontUserException(dbUpdateException);
        }

        /// <summary>
        /// Manage the handled <paramref name="sqlException"/>.
        /// </summary>
        /// <param name="sqlException">The handled <see cref="SqlException"/>.</param>
        /// <returns>A <see cref="FrontUserException"/> corresponding to the handled exception.</returns>
        protected virtual FrontUserException ManageException(SqlException sqlException)
        {
            return sqlException.Number switch
            {
                208 => new FrontUserException(FrontUserExceptionErrorMessageKey.DatabaseObjectNotFound, sqlException),
                515 => new FrontUserException(FrontUserExceptionErrorMessageKey.DatabaseNullValueInsert, sqlException),
                547 => new FrontUserException(FrontUserExceptionErrorMessageKey.DatabaseForeignKeyConstraint, sqlException),
                2601 => new FrontUserException(FrontUserExceptionErrorMessageKey.DatabaseDuplicateKey, sqlException),
                2627 => new FrontUserException(FrontUserExceptionErrorMessageKey.DatabaseUniqueConstraint, sqlException),
                4060 => new FrontUserException(FrontUserExceptionErrorMessageKey.DatabaseOpen, sqlException),
                18456 => new FrontUserException(FrontUserExceptionErrorMessageKey.DatabaseLoginUser, sqlException),
                _ => new FrontUserException(sqlException)
            };
        }
    }
}
