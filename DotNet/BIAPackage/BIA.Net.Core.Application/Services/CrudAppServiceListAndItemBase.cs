// <copyright file="CrudAppServiceListAndItemBase.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;

    /// <summary>
    /// The base class for all CRUD application service.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TDtoListItem">The DTO of item in list type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TFilterDto">The filter DTO type.</typeparam>
    /// <typeparam name="TMapper">The mapper used between entity and DTO.</typeparam>
    /// <typeparam name="TMapperListItem">The mapper used between entity and DTO for item in list.</typeparam>
    public abstract class CrudAppServiceListAndItemBase<TDto, TDtoListItem, TEntity, TKey, TFilterDto, TMapper, TMapperListItem> : CrudAppServiceBase<TDto, TEntity, TKey, TFilterDto, TMapper>
        where TDto : BaseDto<TKey>, new()
        where TDtoListItem : BaseDto<TKey>, new()
        where TEntity : class, IEntity<TKey>, new()
        where TFilterDto : LazyLoadDto, new()
        where TMapper : BaseMapper<TDto, TEntity, TKey>
        where TMapperListItem : BaseMapper<TDtoListItem, TEntity, TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CrudAppServiceListAndItemBase{TDto, TDtoListItem, TEntity, TKey, TFilterDto, TMapper, TMapperListItem}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        protected CrudAppServiceListAndItemBase(ITGenericRepository<TEntity, TKey> repository)
            : base(repository)
        {
        }

        /// <inheritdoc cref="CrudAppServiceListAndItemBase{TDto,TFilterDto}.GetRangeAsync"/>
        public new virtual async Task<(IEnumerable<TDtoListItem> Results, int Total)> GetRangeAsync(
            TFilterDto filters = null,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null,
            bool isReadOnlyMode = false)
        {
            return await this.GetRangeAsync<TDtoListItem, TMapperListItem, TFilterDto>(filters: filters, id: id, specification: specification, filter: filter, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode, isReadOnlyMode: isReadOnlyMode);
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="specification">The specification.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="queryOrder">The query order.</param>
        /// <param name="firstElement">The first element.</param>
        /// <param name="pageCount">The page count.</param>
        /// <param name="includes">The includes.</param>
        /// <param name="accessMode">The access mode.</param>
        /// <param name="queryMode">The query mode.</param>
        /// <param name="mapperMode">The mapper mode.</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [is read only mode].</param>
        /// <returns>All items.</returns>
        public new virtual async Task<IEnumerable<TDtoListItem>> GetAllAsync(
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            QueryOrder<TEntity> queryOrder = null,
            int firstElement = 0,
            int pageCount = 0,
            Expression<Func<TEntity, object>>[] includes = null,
            string accessMode = AccessMode.Read,
            string queryMode = null,
            string mapperMode = null,
            bool isReadOnlyMode = false)
        {
            return await this.GetAllAsync<TDtoListItem, TMapperListItem>(id: id, specification: specification, filter: filter, queryOrder: queryOrder, firstElement: firstElement, pageCount: pageCount, includes: includes, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode, isReadOnlyMode: isReadOnlyMode);
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="orderByExpression">The order by expression.</param>
        /// <param name="ascending">if set to <c>true</c> [ascending].</param>
        /// <param name="id">The identifier.</param>
        /// <param name="specification">The specification.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="firstElement">The first element.</param>
        /// <param name="pageCount">The page count.</param>
        /// <param name="includes">The includes.</param>
        /// <param name="accessMode">The access mode.</param>
        /// <param name="queryMode">The query mode.</param>
        /// <param name="mapperMode">The mapper mode.</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [is read only mode].</param>
        /// <returns>All items.</returns>
        public new virtual async Task<IEnumerable<TDtoListItem>> GetAllAsync(
            Expression<Func<TEntity, TKey>> orderByExpression,
            bool ascending,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            int firstElement = 0,
            int pageCount = 0,
            Expression<Func<TEntity, object>>[] includes = null,
            string accessMode = AccessMode.Read,
            string queryMode = null,
            string mapperMode = null,
            bool isReadOnlyMode = false)
        {
            return await this.GetAllAsync<TDtoListItem, TMapperListItem>(orderByExpression, ascending, id: id, specification: specification, filter: filter, firstElement: firstElement, pageCount: pageCount, includes: includes, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode, isReadOnlyMode: isReadOnlyMode);
        }

        /// <summary>
        /// Gets the CSV asynchronous.
        /// </summary>
        /// <typeparam name="TOtherFilter">The type of the other filter.</typeparam>
        /// <param name="filters">The filters.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="specification">The specification.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="accessMode">The access mode.</param>
        /// <param name="queryMode">The query mode.</param>
        /// <param name="mapperMode">The mapper mode.</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [is read only mode].</param>
        /// <returns>A cvs.</returns>
        public new virtual async Task<byte[]> GetCsvAsync<TOtherFilter>(
            TOtherFilter filters,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null,
            bool isReadOnlyMode = false)
            where TOtherFilter : LazyLoadDto, new()
        {
            return await this.GetCsvAsync<TDtoListItem, TMapperListItem, TOtherFilter>(filters: filters, id: id, specification: specification, filter: filter, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode, isReadOnlyMode: isReadOnlyMode);
        }

        /// <summary>
        /// Gets the CSV asynchronous.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="specification">The specification.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="accessMode">The access mode.</param>
        /// <param name="queryMode">The query mode.</param>
        /// <param name="mapperMode">The mapper mode.</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [is read only mode].</param>
        /// <returns>A csv.</returns>
        protected new virtual async Task<byte[]> GetCsvAsync(
            TFilterDto filters = null,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null,
            bool isReadOnlyMode = false)
        {
            return await this.GetCsvAsync<TDtoListItem, TMapperListItem, TFilterDto>(filters: filters, id: id, specification: specification, filter: filter, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode, isReadOnlyMode: isReadOnlyMode);
        }
    }
}