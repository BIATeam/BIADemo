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
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.Specification;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The class representing a GenericRepository.
    /// </summary>
    [Obsolete("Not used any more. Use TGenericRepositoryEF", false)]
    public class GenericRepositoryEF : IGenericRepository
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        protected readonly IQueryableUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepositoryEF"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit Of Work.</param>
        public GenericRepositoryEF(IQueryableUnitOfWork unitOfWork)
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
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="item">The item.</param>
        public void Add<TEntity>(TEntity item)
            where TEntity : class, IEntity
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            this.RetrieveSet<TEntity>().Add(item);
        }

        /// <summary>
        /// Add a list of item to the current context.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="items">The items.</param>
        public void AddRange<TEntity>(IEnumerable<TEntity> items)
            where TEntity : class, IEntity
        {
            if (items == null || !items.Any())
            {
                return;
            }

            this.RetrieveSet<TEntity>().AddRange(items);
        }

        /// <summary>
        /// Update an item in the current context.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="item">The item.</param>
        public void Update<TEntity>(TEntity item)
            where TEntity : class, IEntity
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
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="item">The item.</param>
        public void Remove<TEntity>(TEntity item)
            where TEntity : class, IEntity
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var set = this.RetrieveSet<TEntity>();
            set.Attach(item);
            set.Remove(item);
        }

        /// <summary>
        /// Check if any element check rules.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="filter">Lambda Expression for filtering Query in where parameters.</param>
        /// <returns>A value indicating whether at least one element match with condition.</returns>
        public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : class, IEntity
        {
            // Create IObjectSet for this particular type and query this
            IQueryable<TEntity> objectSet = this.RetrieveSet<TEntity>();

            // Return true if any match with filter condition
            return await objectSet.AnyAsync(filter);
        }

        /// <summary>
        /// Check if any element check rules.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="specification">The specification.</param>
        /// <returns>A value indicating whether at least one element match with condition.</returns>
        public async Task<bool> AnyAsync<TEntity>(ISpecification<TEntity> specification)
            where TEntity : class, IEntity
        {
            // Create IObjectSet for this particular type and query this
            IQueryable<TEntity> objectSet = this.RetrieveSet<TEntity>();

            // Return true if any match with filter condition
            return await objectSet.AnyAsync(specification.SatisfiedBy());
        }

        /// <summary>
        /// Get an entity with it's identifier.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>The <see cref="TEntity"/>.</returns>
        public async Task<TEntity> GetAsync<TEntity>(int id, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity
        {
            return (await this.GetResultsWithFiltersAsync<TEntity, int, TEntity>(x => x, x => x.Id == id, null, true, 0, 0, false, null, includes)).Item1.SingleOrDefault();
        }

        /// <summary>
        /// Get an entity with it's identifier.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="filter">Additionnal filter.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>The <see cref="TEntity"/>.</returns>
        public async Task<TEntity> GetAsync<TEntity>(int id, Specification<TEntity> filter, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity
        {
            return (await this.GetBySpecAsync<TEntity>(new DirectSpecification<TEntity>(x => x.Id == id) & filter, includes)).SingleOrDefault();
        }

        /// <summary>
        /// Get an entity with it's identifier.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="filter">Additionnal filter.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>The <see cref="TEntity"/>.</returns>
        public async Task<TResult> GetAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selectResult, int id, Specification<TEntity> filter)
            where TEntity : class, IEntity
        {
            return (await this.GetBySpecAsync<TEntity, TResult>(selectResult, new DirectSpecification<TEntity>(x => x.Id == id) & filter)).SingleOrDefault();
        }


        /// <summary>
        /// Get All Elements.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="includes">The list of includes.</param>
        /// <returns>All TEntity.</returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity
        {
            // Call Private Methods
            var result = await this.GetResultsWithFiltersAsync<TEntity, int, TEntity>(x => x, null, null, true, 0, 0, false, null, includes);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get All Elements.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <returns>All TEntity.</returns>
        public async Task<IEnumerable<TResult>> GetAllAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selectResult)
            where TEntity : class, IEntity
        {
            // Checking arguments for this query
            this.CheckArgument(selectResult);

            // Call Private Methods
            var result = await this.GetResultsWithFiltersAsync<TEntity, int, TResult>(selectResult, null, null, true, 0, 0, false, null);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get All Elements Ordered By.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">Type of Ordered Field.</typeparam>
        /// <param name="orderByExpression">Ordered Expression.</param>
        /// <param name="ascending">Direction of sort.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>List of Elements.</returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity, TKey>(Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity
        {
            // Checking arguments for this query
            this.CheckArgument(orderByExpression);

            // Call Private Methods
            var result = await this.GetResultsWithFiltersAsync(x => x, null, orderByExpression, ascending, 0, 0, false, null, includes);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get Elements of Entity By filter, with Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="filter">Lambda Expression for filtering Query in where parameters.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>List of Entity Object.</returns>
        public async Task<IEnumerable<TEntity>> GetByFilterAsync<TEntity>(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity
        {
            // Checking query arguments
            this.CheckArgument(filter);

            // Call Private Method
            var result = await this.GetResultsWithFiltersAsync<TEntity, int, TEntity>(x => x, filter, null, true, 0, 0, false, null, includes);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get Elements with Selected Columns of Entity By filter, with Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="filter">Lambda Expression for filtering Query in where parameters.</param>
        /// <returns>List of Selected column of Entity Object.</returns>
        public async Task<IEnumerable<TResult>> GetByFilterAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selectResult, Expression<Func<TEntity, bool>> filter)
            where TEntity : class, IEntity
        {
            // Checking query arguments
            this.CheckArgument(selectResult);
            this.CheckArgument(filter);

            // Call Private Method
            var result = await this.GetResultsWithFiltersAsync<TEntity, int, TResult>(selectResult, filter, null, true, 0, 0, false, null);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get Elements of Entity By filter, with Ordering and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">Type of Ordering.</typeparam>
        /// <param name="filter">Lambda Expression for filtering Query in where parameters.</param>
        /// <param name="orderByExpression">Lambda Expression for Ordering Query.</param>
        /// <param name="ascending">Direction of Ordering.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>List of Entity Object.</returns>
        public async Task<IEnumerable<TEntity>> GetByFilterAsync<TEntity, TKey>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity
        {
            // Checking query arguments
            this.CheckArgument(filter);
            this.CheckArgument(orderByExpression);

            // Call Private Method
            var result = await this.GetResultsWithFiltersAsync(x => x, filter, orderByExpression, ascending, 0, 0, false, null, includes);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get Elements of Entity By filter, with Ordering and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="filter">Lambda Expression for filtering Query in where parameters.</param>
        /// <param name="order">Lambda Expression for Ordering Query.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>List of Entity Object.</returns>
        public async Task<IEnumerable<TEntity>> GetByFilterAsync<TEntity>(Expression<Func<TEntity, bool>> filter, QueryOrder<TEntity> order, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity
        {
            // Checking query arguments
            this.CheckArgument(filter);
            this.CheckArgument(order);

            // Call Private Method
            var result = await this.GetResultsWithFiltersAsync<TEntity, int, TEntity>(x => x, filter, null, false, 0, 0, false, order, includes);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get Elements with Selected Columns of Entity By filter, with Ordering and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">Type of Ordering.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="filter">Lambda Expression for filtering Query in where parameters.</param>
        /// <param name="orderByExpression">Lambda Expression for Ordering Query.</param>
        /// <param name="ascending">Direction of Ordering.</param>
        /// <returns>List of Selected column of Entity Object.</returns>
        public async Task<IEnumerable<TResult>> GetByFilterAsync<TEntity, TKey, TResult>(Expression<Func<TEntity, TResult>> selectResult, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending)
            where TEntity : class, IEntity
        {
            // Checking query arguments
            this.CheckArgument(selectResult);
            this.CheckArgument(filter);
            this.CheckArgument(orderByExpression);

            // Call Private Method
            var result = await this.GetResultsWithFiltersAsync(selectResult, filter, orderByExpression, ascending, 0, 0, false, null);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get Elements with Selected Columns of Entity By filter, with Ordering and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="filter">Lambda Expression for filtering Query in where parameters.</param>
        /// <param name="order">Lambda Expression for Ordering Query.</param>
        /// <returns>List of Selected column of Entity Object.</returns>
        public async Task<IEnumerable<TResult>> GetByFilterAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selectResult, Expression<Func<TEntity, bool>> filter, QueryOrder<TEntity> order)
            where TEntity : class, IEntity
        {
            // Checking query arguments
            this.CheckArgument(selectResult);
            this.CheckArgument(filter);
            this.CheckArgument(order);

            // Call Private Method
            var result = await this.GetResultsWithFiltersAsync<TEntity, int, TResult>(selectResult, filter, null, false, 0, 0, false, order);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get Elements Entity By filter, with Ordering, Paging and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">Type of Ordering.</typeparam>
        /// <param name="filter">Lambda Expression for filtering Query in where parameters.</param>
        /// <param name="orderByExpression">Lambda Expression for Ordering Query.</param>
        /// <param name="ascending">Direction of Ordering.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>List of Entity Object.</returns>
        public async Task<IEnumerable<TEntity>> GetByFilterAsync<TEntity, TKey>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, int firstElement, int pageCount, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity
        {
            // Checking query arguments
            this.CheckArgument(filter);
            this.CheckArgument(orderByExpression);

            // Call Private Method
            var result = await this.GetResultsWithFiltersAsync(x => x, filter, orderByExpression, ascending, firstElement, pageCount, false, null, includes);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get Elements Entity By filter, with Ordering, Paging and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="filter">Lambda Expression for filtering Query in where parameters.</param>
        /// <param name="order">Lambda Expression for Ordering Query.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>List of Entity Object.</returns>
        public async Task<IEnumerable<TEntity>> GetByFilterAsync<TEntity>(Expression<Func<TEntity, bool>> filter, QueryOrder<TEntity> order, int firstElement, int pageCount, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity
        {
            // Checking query arguments
            this.CheckArgument(filter);
            this.CheckArgument(order);

            // Call Private Method
            var result = await this.GetResultsWithFiltersAsync<TEntity, int, TEntity>(x => x, filter, null, false, firstElement, pageCount, false, order, includes);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get Elements with Selected Columns of Entity By filter, with Ordering, Paging and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">Type of Ordering.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="filter">Lambda Expression for filtering Query in where parameters.</param>
        /// <param name="orderByExpression">Lambda Expression for Ordering Query.</param>
        /// <param name="ascending">Direction of Ordering.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <returns>List of Selected column of Entity Object.</returns>
        public async Task<IEnumerable<TResult>> GetByFilterAsync<TEntity, TKey, TResult>(Expression<Func<TEntity, TResult>> selectResult, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, int firstElement, int pageCount)
            where TEntity : class, IEntity
        {
            // Checking query arguments
            this.CheckArgument(selectResult);
            this.CheckArgument(filter);
            this.CheckArgument(orderByExpression);

            // Call Private Method
            var result = await this.GetResultsWithFiltersAsync(selectResult, filter, orderByExpression, ascending, firstElement, pageCount, false, null);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get Elements with Selected Columns of Entity By filter, with Ordering, Paging and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="filter">Lambda Expression for filtering Query in where parameters.</param>
        /// <param name="order">Lambda Expression for Ordering Query.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <returns>List of Selected column of Entity Object.</returns>
        public async Task<IEnumerable<TResult>> GetByFilterAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selectResult, Expression<Func<TEntity, bool>> filter, QueryOrder<TEntity> order, int firstElement, int pageCount)
            where TEntity : class, IEntity
        {
            // Checking query arguments
            this.CheckArgument(selectResult);
            this.CheckArgument(filter);
            this.CheckArgument(order);

            // Call Private Method
            var result = await this.GetResultsWithFiltersAsync<TEntity, int, TResult>(selectResult, filter, null, false, firstElement, pageCount, false, order);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get Elements of Entity By Specification Pattern, with Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>List of Elements of Entity Object.</returns>
        public async Task<IEnumerable<TEntity>> GetBySpecAsync<TEntity>(ISpecification<TEntity> specification, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity
        {
            // Checking query argument
            this.CheckArgument(specification);

            var result = await this.GetBySpecElementsAsync<TEntity, int, TEntity>(x => x, specification, null, true, 0, 0, false, null, includes);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get Elements with selected Columns of Entity By Specification Pattern, with Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <returns>List of Elements with selected Columns of Entity Object.</returns>
        public async Task<IEnumerable<TResult>> GetBySpecAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selectResult, ISpecification<TEntity> specification)
            where TEntity : class, IEntity
        {
            // Checking query arguments
            this.CheckArgument(selectResult);
            this.CheckArgument(specification);

            var result = await this.GetBySpecElementsAsync<TEntity, int, TResult>(selectResult, specification, null, true, 0, 0, false, null);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get Elements of Entity By Specification Pattern, with Ordering and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">Type of Ordering.</typeparam>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="orderByExpression">Lambda Expression for Ordering Query.</param>
        /// <param name="ascending">Direction of Ordering.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>List of Elements of Entity Object.</returns>
        public async Task<IEnumerable<TEntity>> GetBySpecAsync<TEntity, TKey>(ISpecification<TEntity> specification, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity
        {
            // Checking arguments for this query
            this.CheckArgument(specification);
            this.CheckArgument(orderByExpression);

            var result = await this.GetBySpecElementsAsync(x => x, specification, orderByExpression, ascending, 0, 0, false, null, includes);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get Elements of Entity By Specification Pattern, with Ordering and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="order">Lambda Expression for Ordering Query.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>List of Elements of Entity Object.</returns>
        public async Task<IEnumerable<TEntity>> GetBySpecAsync<TEntity>(ISpecification<TEntity> specification, QueryOrder<TEntity> order, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity
        {
            // Checking arguments for this query
            this.CheckArgument(specification);
            this.CheckArgument(order);

            var result = await this.GetBySpecElementsAsync<TEntity, int, TEntity>(x => x, specification, null, false, 0, 0, false, order, includes);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get Elements with selected Columns of Entity By Specification Pattern, with Ordering and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">Type of Ordering.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="orderByExpression">Lambda Expression for Ordering Query.</param>
        /// <param name="ascending">Direction of Ordering.</param>
        /// <returns>List of Elements with selected Columns of Entity Object.</returns>
        public async Task<IEnumerable<TResult>> GetBySpecAsync<TEntity, TKey, TResult>(Expression<Func<TEntity, TResult>> selectResult, ISpecification<TEntity> specification, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending)
            where TEntity : class, IEntity
        {
            // Checking arguments for this query
            this.CheckArgument(selectResult);
            this.CheckArgument(specification);
            this.CheckArgument(orderByExpression);

            var result = await this.GetBySpecElementsAsync(selectResult, specification, orderByExpression, ascending, 0, 0, false, null);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get Elements with selected Columns of Entity By Specification Pattern, with Ordering and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="order">Lambda Expression for Ordering Query.</param>
        /// <returns>List of Elements with selected Columns of Entity Object.</returns>
        public async Task<IEnumerable<TResult>> GetBySpecAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selectResult, ISpecification<TEntity> specification, QueryOrder<TEntity> order)
            where TEntity : class, IEntity
        {
            // Checking arguments for this query
            this.CheckArgument(selectResult);
            this.CheckArgument(specification);
            this.CheckArgument(order);

            var result = await this.GetBySpecElementsAsync<TEntity, int, TResult>(selectResult, specification, null, false, 0, 0, false, order);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get Elements of Entity By Specification Pattern, with Ordering, Paging and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">Type of Ordering.</typeparam>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="orderByExpression">Lambda Expression for Ordering Query.</param>
        /// <param name="ascending">Direction of Ordering.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>List of Elements of Entity Object.</returns>
        public async Task<IEnumerable<TEntity>> GetBySpecAsync<TEntity, TKey>(ISpecification<TEntity> specification, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, int firstElement, int pageCount, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity
        {
            // Checking arguments for this query
            this.CheckArgument(specification);
            this.CheckArgument(orderByExpression);

            var result = await this.GetBySpecElementsAsync(x => x, specification, orderByExpression, ascending, firstElement, pageCount, false, null, includes);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get Elements of Entity By Specification Pattern, with Ordering, Paging and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="order">Lambda Expression for Ordering Query.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>List of Elements of Entity Object.</returns>
        public async Task<IEnumerable<TEntity>> GetBySpecAsync<TEntity>(ISpecification<TEntity> specification, QueryOrder<TEntity> order, int firstElement, int pageCount, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity
        {
            // Checking arguments for this query
            this.CheckArgument(specification);
            this.CheckArgument(order);

            var result = await this.GetBySpecElementsAsync<TEntity, int, TEntity>(x => x, specification, null, false, firstElement, pageCount, false, order, includes);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get Elements with selected Columns of Entity By Specification Pattern, with Ordering,
        /// Paging and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">Type of Ordering.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="orderByExpression">Lambda Expression for Ordering Query.</param>
        /// <param name="ascending">Direction of Ordering.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <returns>List of Elements with selected Columns of Entity Object.</returns>
        public async Task<IEnumerable<TResult>> GetBySpecAsync<TEntity, TKey, TResult>(Expression<Func<TEntity, TResult>> selectResult, ISpecification<TEntity> specification, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, int firstElement, int pageCount)
            where TEntity : class, IEntity
        {
            // Checking arguments for this query
            this.CheckArgument(selectResult);
            this.CheckArgument(specification);
            this.CheckArgument(orderByExpression);

            var result = await this.GetBySpecElementsAsync(selectResult, specification, orderByExpression, ascending, firstElement, pageCount, false, null);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Get Elements with selected Columns of Entity By Specification Pattern, with Ordering,
        /// Paging and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="order">Lambda Expression for Ordering Query.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <returns>List of Elements with selected Columns of Entity Object.</returns>
        public async Task<IEnumerable<TResult>> GetBySpecAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selectResult, ISpecification<TEntity> specification, QueryOrder<TEntity> order, int firstElement, int pageCount)
            where TEntity : class, IEntity
        {
            // Checking arguments for this query
            this.CheckArgument(selectResult);
            this.CheckArgument(specification);
            this.CheckArgument(order);

            var result = await this.GetBySpecElementsAsync<TEntity, int, TResult>(selectResult, specification, null, false, firstElement, pageCount, false, order);
            return await result.Item1.ToListAsync();
        }

        /// <summary>
        /// Gets the by spec and count.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selectResult">The select result.</param>
        /// <param name="specification">The specification.</param>
        /// <param name="order">The order.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">The page count.</param>
        /// <returns>List of Elements with selected Columns of Entity Object and count.</returns>
        public async Task<Tuple<IEnumerable<TResult>, int>> GetBySpecAndCountAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selectResult, ISpecification<TEntity> specification, QueryOrder<TEntity> order, int firstElement, int pageCount)
            where TEntity : class, IEntity
        {
            // Checking arguments for this query
            this.CheckArgument(selectResult);
            this.CheckArgument(specification);
            this.CheckArgument(order);

            var result = await this.GetBySpecElementsAsync<TEntity, int, TResult>(selectResult, specification, null, false, firstElement, pageCount, true, order);

            return Tuple.Create((await result.Item1.ToListAsync()).AsEnumerable(), result.Item2);
        }

        /// <summary>
        /// Gets the elements.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="objectSet">The object set.</param>
        /// <param name="count">The count of element.</param>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="orderByExpression">Lambda Expression for Ordering Query.</param>
        /// <param name="ascending">Direction of Ordering.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <param name="order">The order.</param>
        /// <returns>List of Selected column of Entity Object, Count of records (0 if not used).</returns>
        private static Tuple<IQueryable<TResult>, int> GetElements<TEntity, TKey, TResult>(IQueryable<TEntity> objectSet, int count, Expression<Func<TEntity, TResult>> selectResult, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, int firstElement, int pageCount, QueryOrder<TEntity> order)
            where TEntity : class, IEntity
        {
            // Ordering Query
            if (orderByExpression != null)
            {
                objectSet = ascending ? objectSet.OrderBy(orderByExpression) : objectSet.OrderByDescending(orderByExpression);
            }

            if (order != null)
            {
                objectSet = objectSet.ApplyQueryOrder(order);
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
            return Tuple.Create(result, count);
        }

        /// <summary>
        /// Get Elements with selected Columns of Entity By Specification Pattern, with Ordering,
        /// Paging and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
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
        /// <param name="order">The order.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>List of Selected column of Entity Object, Count of records (0 if not used).</returns>
        private async Task<Tuple<IQueryable<TResult>, int>> GetBySpecElementsAsync<TEntity, TKey, TResult>(Expression<Func<TEntity, TResult>> selectResult, ISpecification<TEntity> specification, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, int firstElement, int pageCount, bool getFilteredCount, QueryOrder<TEntity> order, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity
        {
            // Create IObjectSet for this particular type and query this
            IQueryable<TEntity> objectSet = this.RetrieveSet<TEntity>();

            // Add Specification condition
            if (specification != null)
            {
                objectSet = objectSet.Where(specification.SatisfiedBy());
            }

            objectSet = AddIncludes( objectSet, includes);

            var count = 0;
            if (getFilteredCount)
            {
                count = await objectSet.CountAsync();
            }

            // Return List of Entity Object and count
            return GetElements(objectSet, count, selectResult, orderByExpression, ascending, firstElement, pageCount, order);
        }

        protected virtual IQueryable<TEntity> AddIncludes<TEntity>(IQueryable<TEntity> objectSet, params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity
        {
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    objectSet = objectSet.Include(include);
                }
            }

            return objectSet;
        }

        /// <summary>
        /// Get Elements with selected Columns of Entity By filter, with Ordering, Paging and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">Type of Ordering.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="filter">Lambda Expression for filtering Query in where parameters.</param>
        /// <param name="orderByExpression">Lambda Expression for Ordering Query.</param>
        /// <param name="ascending">Direction of Ordering.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <param name="getFilteredCount">
        /// If true, Launch a count on objectSet Query after filtering and before pagination.
        /// </param>
        /// <param name="order">The order.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>List of Selected column of Entity Object, Count of records (0 if not used).</returns>
        private async Task<Tuple<IQueryable<TResult>, int>> GetResultsWithFiltersAsync<TEntity, TKey, TResult>(Expression<Func<TEntity, TResult>> selectResult, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, int firstElement, int pageCount, bool getFilteredCount, QueryOrder<TEntity> order, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity
        {
            // Create IObjectSet for this particular type and query this
            IQueryable<TEntity> objectSet = this.RetrieveSet<TEntity>();

            // Add Filter condition
            if (filter != null)
            {
                objectSet = objectSet.Where(filter);
            }

            objectSet = AddIncludes(objectSet, includes);

            var count = 0;
            if (getFilteredCount)
            {
                count = await objectSet.Select(selectResult).CountAsync();
            }

            // Return List of Entity Object and count
            return GetElements(objectSet, count, selectResult, orderByExpression, ascending, firstElement, pageCount, order);
        }

        /// <summary>
        /// Retrieve a DBSet.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>The DBSet of TEntity.</returns>
        private DbSet<TEntity> RetrieveSet<TEntity>()
            where TEntity : class, IEntity
        {
            if (this.unitOfWork == null)
            {
                throw new DataException("The context must not be null");
            }

            return this.unitOfWork.RetrieveSet<TEntity>();
        }

        private void CheckArgument<TEntity, TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
        }

        private void CheckArgument<TEntity>(QueryOrder<TEntity> order)
            where TEntity : class, IEntity
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
        }

        private void CheckArgument<TEntity>(ISpecification<TEntity> specification)
            where TEntity : class, IEntity
        {
            if (specification == null)
            {
                throw new ArgumentNullException(nameof(specification));
            }
        }
    }
}