// <copyright file="GenericRepository.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.Specification;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The class representing a GenericRepository.
    /// </summary>
    public class TGenericRepositoryEF<TEntity> : ITGenericRepository<TEntity>
        where TEntity : class, IEntity 
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        protected readonly IQueryableUnitOfWork unitOfWork;

        /// <summary>
        /// Get or set the Query customizer.
        /// </summary>
        public IQueryCustomizer<TEntity> QueryCustomizer { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepositoryEF"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit Of Work.</param>
        public TGenericRepositoryEF(IQueryableUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork => this.unitOfWork;

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
        public void Update(TEntity item)
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
            IQueryable<TEntity> objectSet = this.RetrieveSet();

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
        /// <returns>The <see cref="TEntity"/>.</returns>
        public async Task<TEntity> GetEntityAsync(
            int id = 0,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>>[] includes = null,
            string queryMode = null)
        {
            return (await this.GetAllEntityAsync(id: id, specification : specification, filter: filter, includes: includes, queryMode: queryMode)).SingleOrDefault();
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
        public async Task<TResult> GetResultAsync<TResult>(Expression<Func<TEntity, TResult>> selectResult, 
            int id = 0,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>>[] includes = null,
            string queryMode = null)
        {
            return (await this.GetAllResultAsync<TResult>(selectResult, id: id, specification: specification, filter: filter, includes: includes, queryMode: queryMode)).SingleOrDefault();
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
        /// <returns>All TEntity.</returns>
        public async Task<IEnumerable<TEntity>> GetAllEntityAsync(
            int id = 0,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            QueryOrder<TEntity> queryOrder= null,
            int firstElement = 0, 
            int pageCount = 0,
            Expression<Func<TEntity, object>>[] includes = null,
            string queryMode =null)
        {
            var result = this.GetAllElementsAsync<int, TEntity>(x => x, id, specification, filter, null, true, firstElement, pageCount, queryOrder, includes, queryMode);
            return await result.ToListAsync();
        }

        /// <summary>
        /// Get All Elements Ordered By.
        /// </summary>
        /// <typeparam name="TKey">Type of Ordered Field.</typeparam>
        /// <param name="orderByExpression">Ordered Expression.</param>
        /// <param name="ascending">Direction of sort.</param>
        /// <param name="specification">Specification Used to filter query.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <param name="includes">The list of includes.</param>
        /// <param name="queryMode">Mode of the query (optionnal).</param>
        /// <returns>List of Elements.</returns>
        public async Task<IEnumerable<TEntity>> GetAllEntityAsync<TKey>(Expression<Func<TEntity, TKey>> orderByExpression,
            bool ascending = true,
            int id = 0,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            int firstElement = 0,
            int pageCount = 0,
            Expression<Func<TEntity, object>>[] includes = null,
            string queryMode = null)
        {
            // Checking arguments for this query
            this.CheckArgument(orderByExpression);

            // Call Private Methods
            var result = this.GetAllElementsAsync(x => x, id, specification, filter, orderByExpression, ascending, firstElement, pageCount, null, includes, queryMode);
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
        /// <returns>All TEntity.</returns>
        public async Task<IEnumerable<TResult>> GetAllResultAsync<TResult>(Expression<Func<TEntity, TResult>> selectResult,
            int id = 0,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            QueryOrder<TEntity> queryOrder = null,
            int firstElement = 0,
            int pageCount = 0,
            Expression<Func<TEntity, object>>[] includes = null,
            string queryMode = null)
        {
            // Checking arguments for this query
            this.CheckArgument(selectResult);

            // Call Private Methods
            var result = this.GetAllElementsAsync<int, TResult>(selectResult, id, specification, filter, null, true, firstElement, pageCount, queryOrder, includes, queryMode);
            return await result.ToListAsync();
        }

        /// <summary>
        /// Get Elements with selected Columns of Entity By Specification Pattern, with Ordering and Includes.
        /// </summary>
        /// <typeparam name="TKey">Type of Ordering.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="orderByExpression">Lambda Expression for Ordering Query.</param>
        /// <param name="ascending">Direction of Ordering.</param>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <param name="ascending">Direction of Ordering.</param>
        /// <param name="queryMode">Mode of the query (optionnal).</param>
        /// <returns>List of Elements with selected Columns of Entity Object.</returns>
        public async Task<IEnumerable<TResult>> GetAllResultAsync<TKey, TResult>(Expression<Func<TEntity, TResult>> selectResult,
            Expression<Func<TEntity, TKey>> orderByExpression,
            bool ascending,
            int id = 0,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            int firstElement = 0,
            int pageCount = 0,
            Expression<Func<TEntity, object>>[] includes = null,
            string queryMode = null)
        {
            // Checking arguments for this query
            this.CheckArgument(selectResult);
            this.CheckArgument(orderByExpression);

            var result = this.GetAllElementsAsync(selectResult, id, specification, filter, orderByExpression, ascending, firstElement, pageCount, null, includes, queryMode);
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
        /// <returns>List of Elements with selected Columns of Entity Object and count.</returns>
        public async Task<Tuple<IEnumerable<TResult>, int>> GetRangeResultAsync<TResult>(
            Expression<Func<TEntity, TResult>> selectResult,
            int id = 0,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            QueryOrder<TEntity> queryOrder = null,
            int firstElement = 0,
            int pageCount = 0,
            Expression<Func<TEntity, object>>[] includes = null,
            string queryMode = null)
        {
            // Checking arguments for this query
            this.CheckArgument(selectResult);

            var result = await this.GetAllElementsAndCountAsync<int, TResult>(selectResult, id, specification, filter, null, true, firstElement, pageCount, queryOrder, includes, queryMode);

            return result;
        }

        /// <summary>
        /// Get Elements with selected Columns of Entity By Specification Pattern, with Ordering,
        /// Paging and Includes.
        /// </summary>
        /// <typeparam name="TKey">Type of Ordering.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="orderByExpression">Lambda Expression for Ordering Query.</param>
        /// <param name="ascending">Direction of Ordering.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param
        /// <param name="queryOrder">Order the Query.</param>
        /// <param name="includes">The list of includes.</param>
        /// <param name="queryMode">Mode of the query (optionnal).</param>
        /// If true, Launch a count on objectSet Query after filtering and before pagination.
        /// </param>
        /// <param name="queryOrder">The queryOrder.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>List of Selected column of Entity Object, Count of records (0 if not used).</returns>
        private async Task<Tuple<IEnumerable<TResult>, int>> GetAllElementsAndCountAsync<TKey, TResult>(Expression<Func<TEntity, TResult>> selectResult, int id, Specification<TEntity> specification, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, int firstElement, int pageCount, QueryOrder<TEntity> queryOrder, Expression<Func<TEntity, object>>[] includes, string queryMode = null)
        {
            IQueryable<TEntity> objectSet = PrepareFilteredQuery(id, specification, filter, queryMode);

            var countTask = objectSet.CountAsync();
            var count = await countTask; // cannot be donne in parallel on the same repo.

            IQueryable<TResult> listQuery = GetElements(objectSet, selectResult, orderByExpression, ascending, firstElement, pageCount, queryOrder, includes, queryMode);
            var listTask = listQuery.ToListAsync();

            var list = await listTask;

            // Return List of Entity Object and count
            return Tuple.Create(list.AsEnumerable(), count);
        }

        /// <summary>
        /// Get Elements with selected Columns of Entity By Specification Pattern, with Ordering,
        /// Paging and Includes.
        /// </summary>
        /// <typeparam name="TKey">Type of Ordering.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="orderByExpression">Lambda Expression for Ordering Query.</param>
        /// <param name="ascending">Direction of Ordering.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <param name="getFilteredCount">
        /// If true, Launch a count on objectSet Query after filtering and before pagination.
        /// </param>
        /// <param name="queryOrder">The queryOrder.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>List of Selected column of Entity Object, Count of records (0 if not used).</returns>
        private IQueryable<TResult> GetAllElementsAsync<TKey, TResult>(Expression<Func<TEntity, TResult>> selectResult, int id, Specification<TEntity> specification, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, int firstElement, int pageCount, QueryOrder<TEntity> queryOrder, Expression<Func<TEntity, object>>[] includes, string queryMode =null)
        {
            IQueryable<TEntity> objectSet = PrepareFilteredQuery(id, specification, filter, queryMode);

            // Return List of Entity Object and count
            return GetElements(objectSet, selectResult, orderByExpression, ascending, firstElement, pageCount, queryOrder, includes, queryMode);
        }


        private IQueryable<TEntity> PrepareFilteredQuery(int id, Specification<TEntity> specification, Expression<Func<TEntity, bool>> filter, string queryMode)
        {
            // Create IObjectSet for this particular type and query this
            IQueryable<TEntity> objectSet = this.RetrieveSet();

            if (QueryCustomizer != null)
            {
                objectSet = QueryCustomizer.CustomizeBefore(objectSet, queryMode);
            }

            // Add Specification condition
            if (specification != null || id != 0)
            {
                Specification<TEntity> spec = null;
                if (id != 0)
                {
                    spec = new DirectSpecification<TEntity>(x => x.Id == id);
                }
                if (specification != null)
                {
                    spec = (spec == null) ? ((Specification<TEntity>)specification) : spec & ((Specification<TEntity>)specification);
                }
                if (filter != null)
                {
                    var filterSpec = new DirectSpecification<TEntity>(filter);
                    spec = (spec == null) ? filterSpec : spec & filterSpec;
                }

                objectSet = objectSet.Where(spec.SatisfiedBy());
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
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="objectSet">The object set.</param>
        /// <param name="count">The count of element.</param>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="orderByExpression">Lambda Expression for Ordering Query.</param>
        /// <param name="ascending">Direction of Ordering.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <param name="queryOrder">The queryOrder.</param>
        /// <returns>List of Selected column of Entity Object, Count of records (0 if not used).</returns>
        private IQueryable<TResult> GetElements<TKey, TResult>(IQueryable<TEntity> objectSet, Expression<Func<TEntity, TResult>> selectResult, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, int firstElement, int pageCount, QueryOrder<TEntity> queryOrder, Expression<Func<TEntity, object>>[] includes, string queryMode)
        {
            objectSet = CustomizeQueryAfter(objectSet, includes, queryMode);

            // Ordering Query
            if (orderByExpression != null)
            {
                objectSet = ascending ? objectSet.OrderBy(orderByExpression) : objectSet.OrderByDescending(orderByExpression);
            }

            if (queryOrder != null)
            {
                objectSet = objectSet.ApplyQueryOrder(queryOrder);
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


        private IQueryable<TEntity> CustomizeQueryAfter(IQueryable<TEntity> objectSet, Expression<Func<TEntity, object>>[] includes, string queryMode)
        {
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    objectSet = objectSet.Include(include);
                }
            }

            if (QueryCustomizer != null)
            {
                objectSet = QueryCustomizer.CustomizeAfter(objectSet, queryMode);
            }

            return objectSet;
        }

        /// <summary>
        /// Retrieve a DBSet.
        /// </summary>
        /// <returns>The DBSet of TEntity.</returns>
        private DbSet<TEntity> RetrieveSet()
        {
            if (this.unitOfWork == null)
            {
                throw new DataException("The context must not be null");
            }

            return this.unitOfWork.RetrieveSet<TEntity>();
        }

        private void CheckArgument<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
        }
    }
}