// <copyright file="BiaDataContext.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Audit.EntityFramework;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Common.Helpers;
    using BIA.Net.Core.Domain.DistCache.Entities;
    using BIA.Net.Core.Domain.Translation.Entities;
    using BIA.Net.Core.Infrastructure.Data.Helpers;
    using BIA.Net.Core.Infrastructure.Data.ModelBuilders;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiaDataContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        public BiaDataContext(DbContextOptions options, ILogger<BiaDataContext> logger, IConfiguration configuration)
            : base(options)
        {
            this.logger = logger;
            this.configuration = configuration;
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
                if (this.configuration.GetEntityModelStateValidation())
                {
                    this.ValidateEntities();
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

        /// <inheritdoc />
        public async Task<int> CommitAsync()
        {
            return await this.SaveChangesAsync();
        }

        /// <summary>
        /// Rollback changes in the current context.
        /// </summary>
        public void RollbackChanges()
        {
            try
            {
                var trackedEntities = this.ChangeTracker.Entries().ToList();
                foreach (var entity in trackedEntities)
                {
                    switch (entity.State)
                    {
                        case EntityState.Added:
                            entity.State = EntityState.Detached;
                            break;

                        case EntityState.Modified:
                            entity.CurrentValues.SetValues(entity.OriginalValues);
                            entity.State = EntityState.Unchanged;
                            break;

                        case EntityState.Deleted:
                            entity.State = EntityState.Unchanged;
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Unable to rollback changes");
            }
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
                await SqlServerBulkHelper.InsertAsync(this, items?.ToList());
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

        /// <inheritdoc/>
        public IQueryable RetrieveSet(Type entityType)
        {
            return this.Set(entityType) as IQueryable;
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

        /// <inheritdoc />
        public IEntityType FindEntityType(Type entityType)
        {
            return this.Model.FindEntityType(entityType);
        }

        /// <summary>
        /// Run SQL scripts stored in an assembly embedded resources folder.
        /// </summary>
        /// <param name="assembly">Assembly that contains the embedded resources.</param>
        /// <param name="relativeResourcesFolderPath">Relative path to SQL scripts folder in embedded resources.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RunScriptsFromAssemblyEmbeddedResourcesFolder(Assembly assembly, string relativeResourcesFolderPath)
        {
            foreach (var scriptPath in EmbeddedResourceHelper.GetEmbeddedResourcesPath(assembly, relativeResourcesFolderPath, "sql"))
            {
                var script = await EmbeddedResourceHelper.ReadEmbeddedResourceAsync(assembly, scriptPath);
                if (!string.IsNullOrWhiteSpace(script))
                {
                    try
                    {
                        await this.Database.ExecuteSqlRawAsync(script);
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogError(ex, "Error while executing script {ScriptPath}", scriptPath);
                    }
                }
            }
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
                _ => new FrontUserException(exception),
            };
        }

        /// <summary>
        /// Manage the handled <paramref name="validationException"/>.
        /// </summary>
        /// <param name="validationException">The handled <see cref="ValidationException"/>.</param>
        /// <returns>A <see cref="FrontUserException"/> corresponding to the handled exception.</returns>
        protected virtual FrontUserException ManageException(ValidationException validationException)
        {
            return new FrontUserException(FrontUserExceptionErrorMessageKey.ValidationEntity, validationException, validationException.ValidationResult.ErrorMessage);
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
                    EntityState.Modified => new FrontUserException(FrontUserExceptionErrorMessageKey.ModifyEntity, dbUpdateException, entityTypeName),
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
                515 => new FrontUserException(FrontUserExceptionErrorMessageKey.DatabaseNullValueInsert, sqlException, GetColumnNameFromSqlExceptionNullInsert(sqlException.Message)),
                547 => new FrontUserException(FrontUserExceptionErrorMessageKey.DatabaseForeignKeyConstraint, sqlException),
                2601 => new FrontUserException(FrontUserExceptionErrorMessageKey.DatabaseDuplicateKey, sqlException),
                2627 => new FrontUserException(FrontUserExceptionErrorMessageKey.DatabaseUniqueConstraint, sqlException),
                4060 => new FrontUserException(FrontUserExceptionErrorMessageKey.DatabaseOpen, sqlException),
                18456 => new FrontUserException(FrontUserExceptionErrorMessageKey.DatabaseLoginUser, sqlException),
                _ => new FrontUserException(sqlException),
            };
        }

        /// <summary>
        /// Validate the model of all changed entities currently tracked.
        /// </summary>
        /// <exception cref="ValidationException">Occurs when model state is invalid.</exception>
        protected virtual void ValidateEntities()
        {
            var changedEntities = this.ChangeTracker.Entries()
                                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                                    .Select(e => e.Entity);

            foreach (var entity in changedEntities)
            {
                if (!Array.Exists(entity.GetType().GetProperties(), p => p.GetCustomAttributes<ValidationAttribute>().Any()))
                {
                    continue;
                }

                try
                {
                    var validationContext = new ValidationContext(entity);
                    Validator.ValidateObject(entity, validationContext);
                }
                catch (Exception ex)
                {
                    throw new ValidationException(ex.Message, ex);
                }
            }
        }

        private static string GetColumnNameFromSqlExceptionNullInsert(string sqlExceptionMessage)
        {
            string pattern = @"column\s'([^']*)'";
            Match match = Regex.Match(sqlExceptionMessage, pattern);
            return match.Success ? match.Groups[1].Value : string.Empty;
        }
    }
}
