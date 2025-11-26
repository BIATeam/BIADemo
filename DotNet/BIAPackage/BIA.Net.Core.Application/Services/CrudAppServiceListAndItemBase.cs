// <copyright file="CrudAppServiceListAndItemBase.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Base.Interface;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.Mapper;
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
        where TFilterDto : class, IPagingFilterFormatDto, new()
        where TMapper : BiaBaseMapper<TDto, TEntity, TKey>
        where TMapperListItem : BiaBaseMapper<TDtoListItem, TEntity, TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CrudAppServiceListAndItemBase{TDto, TDtoListItem, TEntity, TKey, TFilterDto, TMapper, TMapperListItem}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        protected CrudAppServiceListAndItemBase(ITGenericRepository<TEntity, TKey> repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="filters">The filter.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="specification">The specification.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="accessMode">The access mode.</param>
        /// <param name="queryMode">The query mode.</param>
        /// <param name="mapperMode">The mapper mode.</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [is read only mode].</param>
        /// <returns>All items.</returns>
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
            this.SetGetRangeSpecifications(ref specification, filters);
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
        /// Get the csv with filter.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <param name="id">The id.</param>
        /// <param name="specification">Specification Used to filter query.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [This improves performance and enables parallel querying]. (optionnal, false by default).</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public new virtual async Task<byte[]> GetCsvAsync(
            TFilterDto filters = null,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null,
            bool isReadOnlyMode = false)
        {
            return await this.GetCsvAsync<TDtoListItem, TMapperListItem>(filters, id, specification, filter, accessMode, queryMode, mapperMode, isReadOnlyMode);
        }
    }
}