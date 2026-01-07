// <copyright file="BiaDataContext.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>
namespace BIA.Net.Core.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Audit.EntityFramework;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Configuration;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Error;
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
    public class BiaDataContext : DbContext, IQueryableUnitOfWork, IDbContextDatabase
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
        /// <returns>The number of elements added.</returns>
        public async Task<int> AddBulkAsync<TEntity>(IEnumerable<TEntity> items)
            where TEntity : class
        {
            var itemsList = items?.ToList();
            if (itemsList?.Count == 0)
            {
                return 0;
            }

            if (this.Database.IsSqlServer())
            {
                await SqlServerBulkHelper.InsertAsync(this, itemsList);
                return itemsList.Count;
            }

            if (this.Database.IsNpgsql())
            {
                await PostgreSqlBulkHelper.InsertAsync(this, itemsList);
                return itemsList.Count;
            }

            throw new NotSupportedException($"Bulk add operations are not supported for provider: {this.Database.ProviderName}");
        }

        /// <summary>
        /// Bulk function to update entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="items">List of the items to update.</param>
        /// <returns>The number of elements updated.</returns>
        public async Task<int> UpdateBulkAsync<TEntity>(IEnumerable<TEntity> items)
            where TEntity : class
        {
            var itemsList = items?.ToList();
            if (itemsList?.Count == 0)
            {
                return 0;
            }

            if (this.Database.IsSqlServer())
            {
                await SqlServerBulkHelper.UpdateAsync(this, itemsList);
                return itemsList.Count;
            }

            if (this.Database.IsNpgsql())
            {
                await PostgreSqlBulkHelper.UpdateAsync(this, itemsList);
                return itemsList.Count;
            }

            throw new NotSupportedException($"Bulk update operations are not supported for provider: {this.Database.ProviderName}");
        }

        /// <summary>
        /// Bulk function to delete entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="items">List of the items to delete.</param>
        /// <returns>The number of elements deleted.</returns>
        public async Task<int> RemoveBulkAsync<TEntity>(IEnumerable<TEntity> items)
            where TEntity : class
        {
            var itemsList = items?.ToList();
            if (itemsList?.Count == 0)
            {
                return 0;
            }

            if (this.Database.IsSqlServer())
            {
                await SqlServerBulkHelper.DeleteAsync(this, itemsList);
                return itemsList.Count;
            }

            if (this.Database.IsNpgsql())
            {
                await PostgreSqlBulkHelper.DeleteAsync(this, itemsList);
                return itemsList.Count;
            }

            throw new NotSupportedException($"Bulk remove operations are not supported for provider: {this.Database.ProviderName}");
        }

        /// <summary>
        /// Determines whether bulk add operations are supported by the current database provider.
        /// </summary>
        /// <returns><c>true</c> if bulk add operations are supported; otherwise, <c>false</c>.</returns>
        public bool IsAddBulkSupported()
        {
            return this.Database.IsSqlServer() || this.Database.IsNpgsql();
        }

        /// <summary>
        /// Determines whether bulk update operations are supported by the current database provider.
        /// </summary>
        /// <returns><c>true</c> if bulk update operations are supported; otherwise, <c>false</c>.</returns>
        public bool IsUpdateBulkSupported()
        {
            return this.Database.IsSqlServer() || this.Database.IsNpgsql();
        }

        /// <summary>
        /// Determines whether bulk remove operations are supported by the current database provider.
        /// </summary>
        /// <returns><c>true</c> if bulk remove operations are supported; otherwise, <c>false</c>.</returns>
        public bool IsRemoveBulkSupported()
        {
            return this.Database.IsSqlServer() || this.Database.IsNpgsql();
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

        /// <inheritdoc />
        public void Migrate()
        {
            this.Database.Migrate();
        }

        /// <inheritdoc />
        public DbConnection GetDbConnection()
        {
            return this.Database.GetDbConnection();
        }

        /// <inheritdoc/>
        public DbProvider GetDatabaseProviderEnum()
        {
            return this.Database switch
            {
                var db when db.IsSqlServer() => DbProvider.SqlServer,
                var db when db.IsNpgsql() => DbProvider.PostGreSql,
                _ => throw new NotSupportedException($"Database provider {this.Database.ProviderName} is not supported yet"),
            };
        }

        /// <inheritdoc />
        public void SetCommandTimeout(TimeSpan timeout)
        {
            this.Database.SetCommandTimeout(timeout);
        }

        /// <summary>
        /// OnModelCreating.
        /// </summary>
        /// <param name="modelBuilder">the model Builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DistCacheModelBuilder.CreateDistCacheModel(modelBuilder);
            AnnouncementModelBuilder.CreateModel(modelBuilder);
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
            return FrontUserException.Create(BiaErrorId.ValidationEntity, validationException, validationException.ValidationResult.ErrorMessage);
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
                    EntityState.Added => FrontUserException.Create(BiaErrorId.AddEntity, dbUpdateException, entityTypeName),
                    EntityState.Modified => FrontUserException.Create(BiaErrorId.ModifyEntity, dbUpdateException, entityTypeName),
                    EntityState.Deleted => FrontUserException.Create(BiaErrorId.DeleteEntity, dbUpdateException, entityTypeName),
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
                515 => FrontUserException.Create(BiaErrorId.DatabaseNullValueInsert, sqlException, GetColumnNameFromSqlExceptionNullInsert(sqlException.Message)),
                547 => FrontUserException.Create(BiaErrorId.DatabaseForeignKeyConstraint, sqlException),
                2601 => FrontUserException.Create(BiaErrorId.DatabaseDuplicateKey, sqlException),
                2627 => FrontUserException.Create(BiaErrorId.DatabaseUniqueConstraint, sqlException),
                4060 => FrontUserException.Create(BiaErrorId.DatabaseOpen, sqlException),
                18456 => FrontUserException.Create(BiaErrorId.DatabaseLoginUser, sqlException),
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
