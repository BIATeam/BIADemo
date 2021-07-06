// <copyright file="IGenericRepository.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.RepoContract
{
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.Specification;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// The interface base for IGenericRepository.
    /// </summary>
    [Obsolete("Not used any more. Use ITGenericRepository", false)]
    public interface IGenericRepository
    {
        /// <summary>
        /// Gets Unit of Work.
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Add an item to the current context.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="item">The item.</param>
        void Add<TEntity>(TEntity item)
            where TEntity : class, IEntity;

        /// <summary>
        /// Add a list of item to the current context.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="items">The items.</param>
        void AddRange<TEntity>(IEnumerable<TEntity> items)
            where TEntity : class, IEntity;

        /// <summary>
        /// Update an item in the current context.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="item">The item.</param>
        void Update<TEntity>(TEntity item)
            where TEntity : class, IEntity;

        /// <summary>
        /// Remove an item to the current context.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="item">The item.</param>
        void Remove<TEntity>(TEntity item)
            where TEntity : class, IEntity;

        /// <summary>
        /// Get an entity with it's identifier.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>The <see cref="TEntity"/>.</returns>
        Task<TEntity> GetAsync<TEntity>(int id, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity;

        /// <summary>
        /// Get an entity with it's identifier.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="filter">Additionnal filter.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>The <see cref="TEntity"/>.</returns>
        Task<TEntity> GetAsync<TEntity>(int id, Specification<TEntity> filter, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity;


        /// <summary>
        /// Get an entity with it's identifier.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="filter">Additionnal filter.</param>
        /// <returns>The <see cref="TEntity"/>.</returns>
        Task<TResult> GetAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selectResult, int id, Specification<TEntity> filter)
            where TEntity : class, IEntity;

        /// <summary>
        /// Check if any element check rules.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="filter">Lambda Expression for filtering Query in where parameters.</param>
        /// <returns>A value indicating whether at least one element match with condition.</returns>
        Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> filter)
        where TEntity : class, IEntity;

        /// <summary>
        /// Check if any element check rules.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="specification">The specification.</param>
        /// <returns>A value indicating whether at least one element match with condition.</returns>
        Task<bool> AnyAsync<TEntity>(ISpecification<TEntity> specification)
            where TEntity : class, IEntity;

        /// <summary>
        /// Get all entities of type TEntity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="includes">The list of includes.</param>
        /// <returns>The list of entities.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity;

        /// <summary>
        /// Get all entities of type TEntity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="selectResult">The select result.</param>
        /// <typeparam name="TResult">The select result type.</typeparam>
        /// <returns>The list of entities.</returns>
        Task<IEnumerable<TResult>> GetAllAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selectResult)
            where TEntity : class, IEntity;

        /// <summary>
        /// Get All Elements Ordered By.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">Type of Ordered Field.</typeparam>
        /// <param name="orderByExpression">Ordered Expression.</param>
        /// <param name="ascending">Direction of sort.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>List of Elements.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity, TKey>(Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity;

        /// <summary>
        /// Get Elements of Entity By filter, with Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="filter">Lambda Expression for filtering Query in where parameters.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>List of Entity Object.</returns>
        Task<IEnumerable<TEntity>> GetByFilterAsync<TEntity>(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity;

        /// <summary>
        /// Get Elements with Selected Columns of Entity By filter, with Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="filter">Lambda Expression for filtering Query in where parameters.</param>
        /// <returns>List of Selected column of Entity Object.</returns>
        Task<IEnumerable<TResult>> GetByFilterAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selectResult, Expression<Func<TEntity, bool>> filter)
            where TEntity : class, IEntity;

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
        Task<IEnumerable<TEntity>> GetByFilterAsync<TEntity, TKey>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity;

        /// <summary>
        /// Get Elements of Entity By filter, with Ordering and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="filter">Lambda Expression for filtering Query in where parameters.</param>
        /// <param name="order">Lambda Expression for Ordering Query.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>List of Entity Object.</returns>
        Task<IEnumerable<TEntity>> GetByFilterAsync<TEntity>(Expression<Func<TEntity, bool>> filter, QueryOrder<TEntity> order, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity;

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
        Task<IEnumerable<TResult>> GetByFilterAsync<TEntity, TKey, TResult>(Expression<Func<TEntity, TResult>> selectResult, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending)
            where TEntity : class, IEntity;

        /// <summary>
        /// Get Elements with Selected Columns of Entity By filter, with Ordering and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="filter">Lambda Expression for filtering Query in where parameters.</param>
        /// <param name="order">Lambda Expression for Ordering Query.</param>
        /// <returns>List of Selected column of Entity Object.</returns>
        Task<IEnumerable<TResult>> GetByFilterAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selectResult, Expression<Func<TEntity, bool>> filter, QueryOrder<TEntity> order)
            where TEntity : class, IEntity;

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
        Task<IEnumerable<TEntity>> GetByFilterAsync<TEntity, TKey>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, int firstElement, int pageCount, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity;

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
        Task<IEnumerable<TEntity>> GetByFilterAsync<TEntity>(Expression<Func<TEntity, bool>> filter, QueryOrder<TEntity> order, int firstElement, int pageCount, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity;

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
        Task<IEnumerable<TResult>> GetByFilterAsync<TEntity, TKey, TResult>(Expression<Func<TEntity, TResult>> selectResult, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, int firstElement, int pageCount)
            where TEntity : class, IEntity;

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
        Task<IEnumerable<TResult>> GetByFilterAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selectResult, Expression<Func<TEntity, bool>> filter, QueryOrder<TEntity> order, int firstElement, int pageCount)
            where TEntity : class, IEntity;

        /// <summary>
        /// Get Elements of Entity By Specification Pattern, with Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>List of Elements of Entity Object.</returns>
        Task<IEnumerable<TEntity>> GetBySpecAsync<TEntity>(ISpecification<TEntity> specification, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity;

        /// <summary>
        /// Get Elements with selected Columns of Entity By Specification Pattern, with Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <returns>List of Elements with selected Columns of Entity Object.</returns>
        Task<IEnumerable<TResult>> GetBySpecAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selectResult, ISpecification<TEntity> specification)
            where TEntity : class, IEntity;

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
        Task<IEnumerable<TEntity>> GetBySpecAsync<TEntity, TKey>(ISpecification<TEntity> specification, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity;

        /// <summary>
        /// Get Elements of Entity By Specification Pattern, with Ordering and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="order">Lambda Expression for Ordering Query.</param>
        /// <param name="includes">The list of includes.</param>
        /// <returns>List of Elements of Entity Object.</returns>
        Task<IEnumerable<TEntity>> GetBySpecAsync<TEntity>(ISpecification<TEntity> specification, QueryOrder<TEntity> order, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity;

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
        Task<IEnumerable<TResult>> GetBySpecAsync<TEntity, TKey, TResult>(Expression<Func<TEntity, TResult>> selectResult, ISpecification<TEntity> specification, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending)
            where TEntity : class, IEntity;

        /// <summary>
        /// Get Elements with selected Columns of Entity By Specification Pattern, with Ordering and Includes.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TResult">Type of Selected return.</typeparam>
        /// <param name="selectResult">Lambda Expression for Select on query.</param>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="order">Lambda Expression for Ordering Query.</param>
        /// <returns>List of Elements with selected Columns of Entity Object.</returns>
        Task<IEnumerable<TResult>> GetBySpecAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selectResult, ISpecification<TEntity> specification, QueryOrder<TEntity> order)
            where TEntity : class, IEntity;

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
        Task<IEnumerable<TEntity>> GetBySpecAsync<TEntity, TKey>(ISpecification<TEntity> specification, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, int firstElement, int pageCount, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity;

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
        Task<IEnumerable<TEntity>> GetBySpecAsync<TEntity>(ISpecification<TEntity> specification, QueryOrder<TEntity> order, int firstElement, int pageCount, params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class, IEntity;

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
        Task<IEnumerable<TResult>> GetBySpecAsync<TEntity, TKey, TResult>(Expression<Func<TEntity, TResult>> selectResult, ISpecification<TEntity> specification, Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, int firstElement, int pageCount)
            where TEntity : class, IEntity;

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
        Task<IEnumerable<TResult>> GetBySpecAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selectResult, ISpecification<TEntity> specification, QueryOrder<TEntity> order, int firstElement, int pageCount)
            where TEntity : class, IEntity;

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
        Task<Tuple<IEnumerable<TResult>, int>> GetBySpecAndCountAsync<TEntity, TResult>(Expression<Func<TEntity, TResult>> selectResult, ISpecification<TEntity> specification, QueryOrder<TEntity> order, int firstElement, int pageCount)
            where TEntity : class, IEntity;
    }
}