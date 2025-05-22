// <copyright file="CleanServiceBase.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Clean
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.RepoContract;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Abstract class for all clean services of an entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TKey">Entity key type.</typeparam>
    public abstract class CleanServiceBase<TEntity, TKey> : ICleanService
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CleanServiceBase{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="cleanRepository">The clean repository.</param>
        /// <param name="logger">The logger.</param>
        protected CleanServiceBase(ITGenericCleanRepository<TEntity, TKey> cleanRepository, ILogger logger)
        {
            this.CleanRepository = cleanRepository;
            this.Logger = logger;
        }

        /// <summary>
        /// The clean repository.
        /// </summary>
        protected ITGenericCleanRepository<TEntity, TKey> CleanRepository { get; }

        /// <summary>
        /// Logger.
        /// </summary>
        protected ILogger Logger { get; }

        /// <inheritdoc/>
        public virtual async Task RunAsync()
        {
            try
            {
                this.Logger.LogInformation("Cleaning {EntityName} entities...", typeof(TEntity).Name);
                var cleanedItemCount = await this.CleanRepository.RemoveAll(this.CleanRuleFilter());
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

        /// <summary>
        /// The rule to filter the entities to clean.
        /// </summary>
        /// <returns><see cref="Expression"/>.</returns>
        protected abstract Expression<Func<TEntity, bool>> CleanRuleFilter();
    }
}
