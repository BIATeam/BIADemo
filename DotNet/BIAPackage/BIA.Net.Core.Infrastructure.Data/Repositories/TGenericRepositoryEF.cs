// <copyright file="TGenericRepositoryEF.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using BIA.Net.Core.Domain.Specification;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The class representing a GenericRepository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public class TGenericRepositoryEF<TEntity, TKey> : ITGenericRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IQueryableUnitOfWork unitOfWork;

        /// <summary>
        /// The service provider.
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="TGenericRepositoryEF{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit Of Work.</param>
        /// <param name="serviceProvider">The service Provider.</param>
        public TGenericRepositoryEF(IQueryableUnitOfWork unitOfWork, IServiceProvider serviceProvider)
        {
            this.unitOfWork = unitOfWork;
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork => this.unitOfWork;

        /// <summary>
        /// Gets service provider.
        /// </summary>
        public IServiceProvider ServiceProvider => this.serviceProvider;

        /// <summary>
        /// Get or set the Query customizer.
        /// </summary>
        public IQueryCustomizer<TEntity> QueryCustomizer { get; set; }

        /// <summary>
        /// Add an item to the current context.
        /// </summary>
        /// <param name="item">The item.</param>
        public virtual void Add(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            this.RetrieveSet().Add(item);
        }

        /// <summary>
        /// Update an item in the current context.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="item">The item.</param>
        public virtual void SetModified(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            this.unitOfWork.SetModified(item);
        }

        /// <summary>
        /// Remove an item to the current context.
        /// </summary>
        /// <param name="item">The item.</param>
        public virtual void Remove(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var set = this.RetrieveSet();
            set.Attach(item);
            set.Remove(item);
        }

        /// <summary>
        /// Add a list of item to the current context.
        /// </summary>
        /// <param name="items">The items.</param>
        public virtual void AddRange(IEnumerable<TEntity> items)
        {
            if (items?.Any() != true)
            {
                return;
            }

            this.RetrieveSet().AddRange(items);
        }

        /// <inheritdoc />
        public virtual void UpdateRange(IEnumerable<TEntity> items)
        {
            if (items?.Any() != true)
            {
                return;
            }

            this.RetrieveSet().UpdateRange(items);
        }

        /// <inheritdoc />
        public virtual void RemoveRange(IEnumerable<TEntity> items)
        {
            if (items?.Any() != true)
            {
                return;
            }

            var set = this.RetrieveSet();
            set.AttachRange(items);
            set.RemoveRange(items);
        }

        /// <inheritdoc />
        public virtual async Task<int> DeleteByIdsAsync(IEnumerable<TKey> ids, int? batchSize = 100)
        {
            int deletedCount = 0;

            if (batchSize.HasValue && batchSize.Value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(batchSize));
            }

            List<TKey> idList = ids?.ToList();

            if (idList?.Any() != true)
            {
                return deletedCount;
            }

            DbSet<TEntity> dbset = this.RetrieveSet();

            if (batchSize.HasValue)
            {
                for (int i = 0; i < idList.Count; i = i + batchSize.Value)
                {
                    List<TKey> packages = idList.Skip(i).Take(batchSize.Value).ToList();
                    deletedCount += await dbset.Where(x => packages.Contains(x.Id)).ExecuteDeleteAsync();
                }
            }
            else
            {
                deletedCount = await dbset.Where(x => idList.Contains(x.Id)).ExecuteDeleteAsync();
            }

            return deletedCount;
        }

        /// <inheritdoc />
        public virtual async Task<int> ExecuteDeleteAsync(Expression<Func<TEntity, bool>> filter = default, int? batchSize = 100)
        {
            int deletedCount = 0;

            if (batchSize.HasValue && batchSize.Value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(batchSize));
            }

            IQueryable<TEntity> query = this.RetrieveSet();

            if (filter != default)
            {
                query = query.Where(filter);
            }

            if (batchSize.HasValue)
            {
                bool hasMore = true;
                while (hasMore)
                {
                    int deleted = await query.Take(batchSize.Value).ExecuteDeleteAsync();
                    deletedCount += deleted;

                    hasMore = deleted > 0;
                }
            }
            else
            {
                deletedCount = await query.ExecuteDeleteAsync();
            }

            return deletedCount;
        }

        /// <inheritdoc />
        public virtual async Task<int> ExecuteUpdateAsync(IDictionary<string, object> fieldUpdates, Expression<Func<TEntity, bool>> filter = default, int? batchSize = 100)
        {
            int updatedCount = 0;

            if (fieldUpdates?.Any() != true)
            {
                throw new ArgumentNullException(nameof(fieldUpdates));
            }

            if (batchSize.HasValue && batchSize.Value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(batchSize));
            }

            IQueryable<TEntity> query = this.RetrieveSet();

            if (filter != default)
            {
                query = query.Where(filter);
            }

            Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls = this.BuildSetPropertyCallsExpression(fieldUpdates);

            if (batchSize.HasValue)
            {
                // First check the number of entities to be processed
                var totalCount = await query.CountAsync();

                // If the number is small, no need for batching
                if (totalCount <= batchSize.Value)
                {
                    updatedCount = await query.ExecuteUpdateAsync(setPropertyCalls);
                }
                else
                {
                    // For large volumes, process in batches with progressive recovery of IDs
                    var processedCount = 0;
                    while (processedCount < totalCount)
                    {
                        var batchIds = await query
                            .Skip(processedCount)
                            .Take(batchSize.Value)
                            .Select(x => x.Id)
                            .ToListAsync();

                        if (!batchIds.Any())
                        {
                            break; // No more entities to process
                        }

                        var batchQuery = this.RetrieveSet().Where(x => batchIds.Contains(x.Id));
                        var batchUpdated = await batchQuery.ExecuteUpdateAsync(setPropertyCalls);
                        updatedCount += batchUpdated;
                        processedCount += batchIds.Count;
                    }
                }
            }
            else
            {
                updatedCount = await query.ExecuteUpdateAsync(setPropertyCalls);
            }

            return updatedCount;
        }

        /// <inheritdoc />
        public virtual async Task<int> MassAddAsync(IEnumerable<TEntity> items, int batchSize = 100, bool useBulk = false)
        {
            var itemsList = items?.ToList();
            if (itemsList?.Count == 0)
            {
                return 0;
            }

            if (useBulk && this.unitOfWork.IsAddBulkSupported())
            {
                return await this.unitOfWork.AddBulkAsync(itemsList);
            }

            return await this.ExecuteMassOperationAsync(itemsList, batchSize, this.AddRange);
        }

        /// <inheritdoc />
        public virtual async Task<int> MassUpdateAsync(IEnumerable<TEntity> items, int batchSize = 100, bool useBulk = false)
        {
            var itemsList = items?.ToList();
            if (itemsList?.Count == 0)
            {
                return 0;
            }

            if (useBulk && this.unitOfWork.IsUpdateBulkSupported())
            {
                return await this.unitOfWork.UpdateBulkAsync(itemsList);
            }

            return await this.ExecuteMassOperationAsync(itemsList, batchSize, this.UpdateRange);
        }

        /// <inheritdoc />
        public virtual async Task<int> MassDeleteAsync(IEnumerable<TEntity> items, int batchSize = 100, bool useBulk = false)
        {
            var itemsList = items?.ToList();
            if (itemsList?.Count == 0)
            {
                return 0;
            }

            if (useBulk && this.unitOfWork.IsRemoveBulkSupported())
            {
                return await this.unitOfWork.RemoveBulkAsync(itemsList);
            }

            return await this.ExecuteMassOperationAsync(itemsList, batchSize, this.RemoveRange);
        }

        /// <summary>
        /// Check if any element check rules.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>A value indicating whether at least one element match with condition.</returns>
        public virtual async Task<bool> AnyAsync(Specification<TEntity> specification)
        {
            // Create IObjectSet for this particular type and query this
            IQueryable<TEntity> objectSet = this.RetrieveSetNoTracking();

            // Return true if any match with filter condition
            return await objectSet.AnyAsync(specification.SatisfiedBy());
        }

        /// <summary>
        /// Get an entity with it's identifier.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="specification">Additionnal filter.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="includes">The list of includes.</param>
        /// <param name="queryMode">Mode of the query (optionnal).</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [This improves performance and enables parallel querying]. (optionnal, false by default).</param>
        /// <returns>The <see cref="TEntity"/>.</returns>
        public virtual async Task<TEntity> GetEntityAsync(
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>>[] includes = null,
            string queryMode = null,
            bool isReadOnlyMode = false)
        {
            return (await this.GetAllEntityAsync(id: id, specification: specification, filter: filter, includes: includes, queryMode: queryMode, isReadOnlyMode: isReadOnlyMode)).SingleOrDefault();
        }

        /// <summary>
        /// Get a dto with it's identifier.
        /// </summary>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="id">The id.</param>
        /// <param name="specification">Additionnal filter.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="includes">The list of includes.</param>
        /// <param name="queryMode">Mode of the query (optionnal).</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [This improves performance and enables parallel querying]. (optionnal, false by default).</param>
        /// <returns>All TEntity.</returns>
        public virtual async Task<TResult> GetResultAsync<TResult>(
            Expression<Func<TEntity, TResult>> selectResult,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>>[] includes = null,
            string queryMode = null,
            bool isReadOnlyMode = false)
        {
            return (await this.GetAllResultAsync<TResult>(selectResult, id: id, specification: specification, filter: filter, includes: includes, queryMode: queryMode, isReadOnlyMode: isReadOnlyMode)).SingleOrDefault();
        }

        /// <summary>
        /// Get All Elements.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="specification">Specification Used to filter query.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="queryOrder">Order the Query.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <param name="includes">The list of includes.</param>
        /// <param name="queryMode">Mode of the query (optionnal).</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [This improves performance and enables parallel querying]. (optionnal, false by default).</param>
        /// <returns>All TEntity.</returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllEntityAsync(
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            QueryOrder<TEntity> queryOrder = null,
            int firstElement = 0,
            int pageCount = 0,
            Expression<Func<TEntity, object>>[] includes = null,
            string queryMode = null,
            bool isReadOnlyMode = false)
        {
            var result = this.GetAllElementsAsync<int, TEntity>(x => x, id, specification, filter, null, true, firstElement, pageCount, queryOrder, includes, queryMode, isReadOnlyMode);
            return await result.ToListAsync();
        }

        /// <summary>
        /// Get All Elements Ordered By.
        /// </summary>
        /// <typeparam name="TOrderKey">Type of Ordered Field.</typeparam>
        /// <param name="orderByExpression">Ordered Expression.</param>
        /// <param name="ascending">Direction of sort.</param>
        /// <param name="id">The id.</param>
        /// <param name="specification">Specification Used to filter query.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <param name="includes">The list of includes.</param>
        /// <param name="queryMode">Mode of the query (optionnal).</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [This improves performance and enables parallel querying]. (optionnal, false by default).</param>
        /// <returns>List of Elements.</returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllEntityAsync<TOrderKey>(
            Expression<Func<TEntity, TOrderKey>> orderByExpression,
            bool ascending = true,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            int firstElement = 0,
            int pageCount = 0,
            Expression<Func<TEntity, object>>[] includes = null,
            string queryMode = null,
            bool isReadOnlyMode = false)
        {
            // Checking arguments for this query
            this.CheckArgument(orderByExpression);

            // Call protected Methods
            var result = this.GetAllElementsAsync(x => x, id, specification, filter, orderByExpression, ascending, firstElement, pageCount, null, includes, queryMode, isReadOnlyMode);
            return await result.ToListAsync();
        }

        /// <summary>
        /// Get All Elements.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="id">The id.</param>
        /// <param name="specification">Specification Used to filter query.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="queryOrder">Order the Query.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <param name="includes">The list of includes.</param>
        /// <param name="queryMode">Mode of the query (optionnal).</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [This improves performance and enables parallel querying]. (optionnal, false by default).</param>
        /// <returns>All TEntity.</returns>
        public virtual async Task<IEnumerable<TResult>> GetAllResultAsync<TResult>(
            Expression<Func<TEntity, TResult>> selectResult,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            QueryOrder<TEntity> queryOrder = null,
            int firstElement = 0,
            int pageCount = 0,
            Expression<Func<TEntity, object>>[] includes = null,
            string queryMode = null,
            bool isReadOnlyMode = false)
        {
            // Checking arguments for this query
            this.CheckArgument(selectResult);

            // Call protected Methods
            var result = this.GetAllElementsAsync<int, TResult>(selectResult, id, specification, filter, null, true, firstElement, pageCount, queryOrder, includes, queryMode, isReadOnlyMode);
            return await result.ToListAsync();
        }

        /// <summary>
        /// Get Elements with selected Columns of Entity By Specification Pattern, with Ordering and Includes.
        /// </summary>
        /// <typeparam name="TOrderKey">Type of Ordering.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="orderByExpression">Lambda Expression for Ordering Query.</param>
        /// <param name="ascending">Direction of Ordering.</param>
        /// <param name="id">The id.</param>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <param name="includes">The list of includes.</param>
        /// <param name="queryMode">Mode of the query (optionnal).</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [This improves performance and enables parallel querying]. (optionnal, false by default).</param>
        /// <returns>List of Elements with selected Columns of Entity Object.</returns>
        public virtual async Task<IEnumerable<TResult>> GetAllResultAsync<TOrderKey, TResult>(
            Expression<Func<TEntity, TResult>> selectResult,
            Expression<Func<TEntity, TOrderKey>> orderByExpression,
            bool ascending,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            int firstElement = 0,
            int pageCount = 0,
            Expression<Func<TEntity, object>>[] includes = null,
            string queryMode = null,
            bool isReadOnlyMode = false)
        {
            // Checking arguments for this query
            this.CheckArgument(selectResult);
            this.CheckArgument(orderByExpression);

            var result = this.GetAllElementsAsync(selectResult, id, specification, filter, orderByExpression, ascending, firstElement, pageCount, null, includes, queryMode, isReadOnlyMode);
            return await result.ToListAsync();
        }

        /// <summary>
        /// Gets the by spec and count.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="id">The id.</param>
        /// <param name="specification">Specification Used to filter query.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="queryOrder">Order the Query.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <param name="includes">The list of includes.</param>
        /// <param name="queryMode">Mode of the query (optionnal).</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [This improves performance and enables parallel querying]. (optionnal, false by default).</param>
        /// <returns>List of Elements with selected Columns of Entity Object and count.</returns>
        public virtual async Task<Tuple<IEnumerable<TResult>, int>> GetRangeResultAsync<TResult>(
            Expression<Func<TEntity, TResult>> selectResult,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            QueryOrder<TEntity> queryOrder = null,
            int firstElement = 0,
            int pageCount = 0,
            Expression<Func<TEntity, object>>[] includes = null,
            string queryMode = null,
            bool isReadOnlyMode = false)
        {
            // Checking arguments for this query
            this.CheckArgument(selectResult);

            var result = await this.GetAllElementsAndCountAsync<int, TResult>(selectResult, id, specification, filter, null, true, firstElement, pageCount, queryOrder, includes, queryMode, isReadOnlyMode);

            return result;
        }

        /// <inheritdoc/>
        public virtual void UpdateFixedAsync(TEntity item, bool isFixed)
        {
            if (item is not IEntityFixable fixableEntity)
            {
                throw new BadBiaFrameworkUsageException($"Entity {item.GetType()} is not a fixable entity");
            }

            fixableEntity.IsFixed = isFixed;
            fixableEntity.FixedDate = isFixed ? DateTime.UtcNow : null;
            this.SetModified(fixableEntity as TEntity);
        }

        /// <summary>
        /// Retrieve a DBSet.
        /// </summary>
        /// <returns>The DBSet of TEntity.</returns>
        protected virtual DbSet<TEntity> RetrieveSet()
        {
            if (this.unitOfWork == null)
            {
                throw new Common.Exceptions.DataException("The context must not be null");
            }

            return this.unitOfWork.RetrieveSet<TEntity>();
        }

        /// <summary>
        /// Retrieve a DBSet as no tracking from dependancy injection <see cref="IQueryableUnitOfWorkNoTracking"/>.
        /// </summary>
        /// <returns>The DBSet of TEntity with no tracking.</returns>
        protected virtual DbSet<TEntity> RetrieveSetNoTracking()
        {
            return this.serviceProvider.GetService<IQueryableUnitOfWorkNoTracking>().RetrieveSet<TEntity>();
        }

        /// <summary>
        /// Get Elements with selected Columns of Entity By Specification Pattern, with Ordering,
        /// Paging and Includes.
        /// </summary>
        /// <typeparam name="TOrderKey">Type of Ordering.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="id">the element id.</param>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="filter">filter lambda expression for Filtering Query.</param>
        /// <param name="orderByExpression">Lambda Expression for Ordering Query.</param>
        /// <param name="ascending">Direction of Ordering.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param
        /// <param name="queryOrder">Order the Query.</param>
        /// <param name="includes">The list of includes.</param>
        /// <param name="queryMode">Mode of the query (optionnal).</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [This improves performance and enables parallel querying]. (optionnal, false by default).</param>
        /// <returns>List of Selected column of Entity Object, Count of records (0 if not used).</returns>
        protected virtual async Task<Tuple<IEnumerable<TResult>, int>> GetAllElementsAndCountAsync<TOrderKey, TResult>(Expression<Func<TEntity, TResult>> selectResult, TKey id, Specification<TEntity> specification, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderKey>> orderByExpression, bool ascending, int firstElement, int pageCount, QueryOrder<TEntity> queryOrder, Expression<Func<TEntity, object>>[] includes, string queryMode = null, bool isReadOnlyMode = false)
        {
            if (isReadOnlyMode)
            {
                IQueryable<TEntity> objectSetCount = this.PrepareFilteredQuery(id, specification, filter, queryMode, isReadOnlyMode);
                IQueryable<TEntity> objectSetList = this.PrepareFilteredQuery(id, specification, filter, queryMode, isReadOnlyMode);
                IQueryable<TResult> listQuery = this.GetElements(objectSetList, selectResult, orderByExpression, ascending, firstElement, pageCount, queryOrder, includes, queryMode);

                var countTask = objectSetCount.CountAsync();
                var listTask = listQuery.ToListAsync();

                Task.WaitAll(countTask, listTask);
                var count = countTask.Result;
                var list = listTask.Result;

                // Return List of Entity Object and count
                return Tuple.Create(list.AsEnumerable(), count);
            }
            else
            {
                IQueryable<TEntity> objectSet = this.PrepareFilteredQuery(id, specification, filter, queryMode, isReadOnlyMode);

                var countTask = objectSet.CountAsync();
                var count = await countTask; // cannot be donne in parallel on the same repo.

                IQueryable<TResult> listQuery = this.GetElements(objectSet, selectResult, orderByExpression, ascending, firstElement, pageCount, queryOrder, includes, queryMode);
                var listTask = listQuery.ToListAsync();

                var list = await listTask;

                // Return List of Entity Object and count
                return Tuple.Create(list.AsEnumerable(), count);
            }
        }

        /// <summary>
        /// Get Elements with selected Columns of Entity By Specification Pattern, with Ordering,
        /// Paging and Includes.
        /// </summary>
        /// <typeparam name="TOrderKey">Type of Ordering.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderByExpression">Lambda Expression for Ordering Query.</param>
        /// <param name="ascending">Direction of Ordering.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <param name="queryOrder">The queryOrder.</param>
        /// <param name="includes">The list of includes.</param>
        /// <param name="queryMode">Mode of the query (optionnal).</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [This improves performance and enables parallel querying]. (optionnal, false by default).</param>
        /// <returns>List of Selected column of Entity Object, Count of records (0 if not used).</returns>
        protected virtual IQueryable<TResult> GetAllElementsAsync<TOrderKey, TResult>(Expression<Func<TEntity, TResult>> selectResult, TKey id, Specification<TEntity> specification, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderKey>> orderByExpression, bool ascending, int firstElement, int pageCount, QueryOrder<TEntity> queryOrder, Expression<Func<TEntity, object>>[] includes, string queryMode = null, bool isReadOnlyMode = false)
        {
            IQueryable<TEntity> objectSet = this.PrepareFilteredQuery(id, specification, filter, queryMode, isReadOnlyMode);

            // Return List of Entity Object and count
            return this.GetElements(objectSet, selectResult, orderByExpression, ascending, firstElement, pageCount, queryOrder, includes, queryMode);
        }

        /// <summary>
        /// Compare equality between 2 key.
        /// </summary>
        /// <param name="x">key1.</param>
        /// <param name="y">key2.</param>
        /// <returns>true if equals.</returns>
        protected virtual bool Compare(TKey x, TKey y)
        {
            return EqualityComparer<TKey>.Default.Equals(x, y);
        }

        /// <summary>
        /// Prepares the filtered query.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="specification">The specification.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="queryMode">The query mode.</param>
        /// <param name="isNoTrackingMode">if set to <c>true</c> [is no tracking mode].</param>
        /// <returns>A filtered query.</returns>
        protected virtual IQueryable<TEntity> PrepareFilteredQuery(TKey id, Specification<TEntity> specification, Expression<Func<TEntity, bool>> filter, string queryMode, bool isNoTrackingMode = false)
        {
            // Create IObjectSet for this particular type and query this
            IQueryable<TEntity> objectSet = isNoTrackingMode ? this.RetrieveSetNoTracking() : this.RetrieveSet();

            if (this.QueryCustomizer != null)
            {
                objectSet = this.QueryCustomizer.CustomizeBefore(objectSet, queryMode);
            }

            // Add Specification condition
            if (specification != null || !this.Compare(id, default))
            {
                Specification<TEntity> spec = null;
                if (!this.Compare(id, default))
                {
                    // Specific cast required for unitary test with Mock data
                    if (id is int)
                    {
                        spec = new DirectSpecification<TEntity>(x => (int)(object)x.Id == (int)(object)id);
                    }
                    else if (id is long)
                    {
                        spec = new DirectSpecification<TEntity>(x => (long)(object)x.Id == (long)(object)id);
                    }
                    else if (id is Guid)
                    {
                        spec = new DirectSpecification<TEntity>(x => (Guid)(object)x.Id == (Guid)(object)id);
                    }
                    else
                    {
                        spec = new DirectSpecification<TEntity>(x => (object)x.Id == (object)id);
                    }
                }

                if (specification != null)
                {
                    spec = (spec == null) ? specification : spec & specification;
                }

                if (filter != null)
                {
                    var filterSpec = new DirectSpecification<TEntity>(filter);
                    spec = (spec == null) ? filterSpec : spec & filterSpec;
                }

                objectSet = objectSet.Where(spec?.SatisfiedBy());
            }
            else if (filter != null)
            {
                objectSet = objectSet.Where(filter);
            }

            return objectSet;
        }

        /// <summary>
        /// Gets the elements.
        /// </summary>
        /// <typeparam name="TOrderKey">The type of the key for ordering.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="objectSet">The object set.</param>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="orderByExpression">Lambda Expression for Ordering Query.</param>
        /// <param name="ascending">Direction of Ordering.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <param name="queryOrder">The queryOrder.</param>
        /// <param name="includes">The includes.</param>
        /// <param name="queryMode">The queryMode.</param>
        /// <returns>List of Selected column of Entity Object, Count of records (0 if not used).</returns>
        protected virtual IQueryable<TResult> GetElements<TOrderKey, TResult>(IQueryable<TEntity> objectSet, Expression<Func<TEntity, TResult>> selectResult, Expression<Func<TEntity, TOrderKey>> orderByExpression, bool ascending, int firstElement, int pageCount, QueryOrder<TEntity> queryOrder, Expression<Func<TEntity, object>>[] includes, string queryMode)
        {
            objectSet = this.CustomizeQueryAfter(objectSet, includes, queryMode);

            // Ordering Query
            if (orderByExpression != null)
            {
                objectSet = ascending ? objectSet.OrderBy(orderByExpression) : objectSet.OrderByDescending(orderByExpression);
            }

            if (queryOrder != null)
            {
                objectSet = objectSet.ApplyQueryOrder<TEntity, TKey>(queryOrder);
            }

            // Cut Result for Paging
            if (firstElement > 0)
            {
                objectSet = objectSet.Skip(firstElement);
            }

            if (pageCount > 0)
            {
                objectSet = objectSet.Take(pageCount);
            }

            var result = objectSet.Select(selectResult);

            // Return List of Entity Object and count
            return result;
        }

        /// <summary>
        /// Customizes the query after.
        /// </summary>
        /// <param name="objectSet">The object set.</param>
        /// <param name="includes">The includes.</param>
        /// <param name="queryMode">The query mode.</param>
        /// <returns>The query customized after.</returns>
        protected virtual IQueryable<TEntity> CustomizeQueryAfter(IQueryable<TEntity> objectSet, Expression<Func<TEntity, object>>[] includes, string queryMode)
        {
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    objectSet = objectSet.Include(include);
                }
            }

            if (this.QueryCustomizer != null)
            {
                objectSet = this.QueryCustomizer.CustomizeAfter(objectSet, queryMode);
            }

            return objectSet;
        }

        /// <summary>
        /// Checks if the argument is null.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <exception cref="System.ArgumentNullException">expression.</exception>
        protected virtual void CheckArgument<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
        }

        /// <summary>
        /// Executes a mass operation on entities in batches.
        /// </summary>
        /// <param name="items">The items to process.</param>
        /// <param name="batchSize">The number of items to process per batch.</param>
        /// <param name="operation">The operation to perform on each batch.</param>
        /// <returns>Number of element affected.</returns>
        protected virtual async Task<int> ExecuteMassOperationAsync(IEnumerable<TEntity> items, int batchSize, Action<IEnumerable<TEntity>> operation)
        {
            int elementAffectedCount = 0;
            List<TEntity> itemList = items.ToList();

            if (itemList?.Any() != true)
            {
                return elementAffectedCount;
            }

            if (batchSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(batchSize));
            }

            if (itemList.Count <= batchSize)
            {
                operation(itemList);
                elementAffectedCount += await this.unitOfWork.CommitAsync();
                this.unitOfWork.Reset();
            }
            else
            {
                for (int i = 0; i < itemList.Count; i = i + batchSize)
                {
                    var packages = itemList.Skip(i).Take(batchSize).ToList();
                    operation(packages);
                    elementAffectedCount += await this.unitOfWork.CommitAsync();
                    this.unitOfWork.Reset();
                }
            }

            return elementAffectedCount;
        }

        /// <summary>
        /// Builds the SetProperty calls expression from the field updates dictionary.
        /// </summary>
        /// <param name="fieldUpdates">The field updates dictionary.</param>
        /// <returns>The SetProperty calls expression.</returns>
        protected virtual Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> BuildSetPropertyCallsExpression(IDictionary<string, object> fieldUpdates)
        {
            Type entityType = typeof(TEntity);
            Type setPropertyCallsType = typeof(SetPropertyCalls<TEntity>);
            MethodInfo setPropertyMethod = setPropertyCallsType.GetMethods()
                .FirstOrDefault(m => m.Name == nameof(SetPropertyCalls<TEntity>.SetProperty) && m.GetParameters().Length == 2);

            if (setPropertyMethod == null)
            {
                throw new InvalidOperationException("SetProperty method not found on SetPropertyCalls<TEntity>");
            }

            // Parameter for the lambda expression
            ParameterExpression parameter = Expression.Parameter(setPropertyCallsType, "s");
            Expression body = parameter;

            foreach (KeyValuePair<string, object> fieldUpdate in fieldUpdates)
            {
                string propertyName = fieldUpdate.Key;
                object propertyValue = fieldUpdate.Value;

                // Get the property info
                PropertyInfo propertyInfo = entityType.GetProperty(propertyName);
                if (propertyInfo == null)
                {
                    throw new ArgumentException($"Property '{propertyName}' not found on entity type '{entityType.Name}'");
                }

                // Create lambda expression for property access: entity => entity.PropertyName
                ParameterExpression entityParam = Expression.Parameter(entityType, "entity");
                MemberExpression propertyAccess = Expression.Property(entityParam, propertyInfo);
                LambdaExpression propertyLambda = Expression.Lambda(propertyAccess, entityParam);

                // Create lambda expression for the value: entity => value
                ConstantExpression valueExpression = Expression.Constant(propertyValue, propertyInfo.PropertyType);
                LambdaExpression valueLambda = Expression.Lambda(valueExpression, entityParam);

                // Create generic SetProperty method
                MethodInfo genericSetPropertyMethod = setPropertyMethod.MakeGenericMethod(propertyInfo.PropertyType);

                // Create method call: s.SetProperty(entity => entity.PropertyName, entity => value)
                MethodCallExpression setPropertyCall = Expression.Call(body, genericSetPropertyMethod, propertyLambda, valueLambda);
                body = setPropertyCall;
            }

            return Expression.Lambda<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>>(body, parameter);
        }
    }
}