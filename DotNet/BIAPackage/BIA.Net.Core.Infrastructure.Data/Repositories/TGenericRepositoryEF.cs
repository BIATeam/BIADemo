// <copyright file="TGenericRepositoryEF.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Reflection.Metadata;
    using System.Threading.Tasks;
    using Audit.EntityFramework;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Audit;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using BIA.Net.Core.Domain.Specification;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
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
        private readonly IAuditFeature auditFeature;

        /// <summary>
        /// Initializes a new instance of the <see cref="TGenericRepositoryEF{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit Of Work.</param>
        /// <param name="serviceProvider">The service Provider.</param>
        public TGenericRepositoryEF(IQueryableUnitOfWork unitOfWork, IServiceProvider serviceProvider, IAuditFeature auditFeature)
        {
            this.unitOfWork = unitOfWork;
            this.serviceProvider = serviceProvider;
            this.auditFeature = auditFeature;
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
        public void Add(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            this.RetrieveSet().Add(item);
        }

        /// <summary>
        /// Add a list of item to the current context.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddRange(IEnumerable<TEntity> items)
        {
            if (items == null || !items.Any())
            {
                return;
            }

            this.RetrieveSet().AddRange(items);
        }

        /// <summary>
        /// Update an item in the current context.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="item">The item.</param>
        public void SetModified(TEntity item)
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
        public void Remove(TEntity item)
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
        /// Check if any element check rules.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>A value indicating whether at least one element match with condition.</returns>
        public async Task<bool> AnyAsync(Specification<TEntity> specification)
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
        public async Task<TEntity> GetEntityAsync(
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
        public async Task<TResult> GetResultAsync<TResult>(
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
        public async Task<IEnumerable<TEntity>> GetAllEntityAsync(
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
        public async Task<IEnumerable<TEntity>> GetAllEntityAsync<TOrderKey>(
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
        public async Task<IEnumerable<TResult>> GetAllResultAsync<TResult>(
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
        public async Task<IEnumerable<TResult>> GetAllResultAsync<TOrderKey, TResult>(
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
        public async Task<Tuple<IEnumerable<TResult>, int>> GetRangeResultAsync<TResult>(
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
        public void UpdateFixedAsync(TEntity item, bool isFixed)
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
        protected DbSet<TEntity> RetrieveSet()
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
        protected async Task<Tuple<IEnumerable<TResult>, int>> GetAllElementsAndCountAsync<TOrderKey, TResult>(Expression<Func<TEntity, TResult>> selectResult, TKey id, Specification<TEntity> specification, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderKey>> orderByExpression, bool ascending, int firstElement, int pageCount, QueryOrder<TEntity> queryOrder, Expression<Func<TEntity, object>>[] includes, string queryMode = null, bool isReadOnlyMode = false)
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
        protected IQueryable<TResult> GetAllElementsAsync<TOrderKey, TResult>(Expression<Func<TEntity, TResult>> selectResult, TKey id, Specification<TEntity> specification, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TOrderKey>> orderByExpression, bool ascending, int firstElement, int pageCount, QueryOrder<TEntity> queryOrder, Expression<Func<TEntity, object>>[] includes, string queryMode = null, bool isReadOnlyMode = false)
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
        protected IQueryable<TEntity> PrepareFilteredQuery(TKey id, Specification<TEntity> specification, Expression<Func<TEntity, bool>> filter, string queryMode, bool isNoTrackingMode = false)
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
        protected IQueryable<TResult> GetElements<TOrderKey, TResult>(IQueryable<TEntity> objectSet, Expression<Func<TEntity, TResult>> selectResult, Expression<Func<TEntity, TOrderKey>> orderByExpression, bool ascending, int firstElement, int pageCount, QueryOrder<TEntity> queryOrder, Expression<Func<TEntity, object>>[] includes, string queryMode)
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
        protected IQueryable<TEntity> CustomizeQueryAfter(IQueryable<TEntity> objectSet, Expression<Func<TEntity, object>>[] includes, string queryMode)
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

        public async Task<ImmutableList<IAudit>> GetAuditsAsync(TKey id)
        {
            var entityType = this.unitOfWork.FindEntityType(typeof(TEntity));
            var audits = new List<IAudit>();
            audits.AddRange(this.GetAudits(typeof(TEntity), id.ToString(), nameof(IEntity<TKey>.Id)));

            foreach (var navigation in entityType.GetNavigations())
            {
                if (navigation.PropertyInfo.CustomAttributes.Any(a => a.AttributeType == typeof(AuditIgnoreAttribute)))
                {
                    continue;
                }

                var navigationEntityType = navigation.TargetEntityType.ClrType;
                if (!navigationEntityType.CustomAttributes.Any(a => a.AttributeType == typeof(AuditIncludeAttribute)))
                {
                    continue;
                }

                var navigationFK = navigation.ForeignKey.Properties[0]?.Name;
                audits.AddRange(this.GetAudits(navigationEntityType, id.ToString(), navigationFK, true));
            }

            return [.. audits.OrderByDescending(x => x.AuditDate)];
        }

        private IEnumerable<IAudit> GetAudits(Type entityType, string entityIdValue, string entityIdProperty, bool hasParent = false)
        {
            if (entityType is null || string.IsNullOrWhiteSpace(entityIdValue) || string.IsNullOrWhiteSpace(entityIdProperty))
            {
                return [];
            }

            var entityAuditType = this.auditFeature.AuditTypeMapper(entityType);
            if (entityAuditType.BaseType != typeof(AuditEntity))
            {
                return [];
            }

            var query = this.unitOfWork.RetrieveSet(entityAuditType).Cast<AuditEntity>();

            if (!hasParent)
            {
                return query.Where(x => x.EntityId.Equals(entityIdValue));
            }

            var parentPropertyIdPattern = $"\"{entityIdProperty}\":\"{entityIdValue}\"";
            return query.Where(x => x.ParentEntityId != null && x.ParentEntityId.Contains(parentPropertyIdPattern));
        }
    }
}