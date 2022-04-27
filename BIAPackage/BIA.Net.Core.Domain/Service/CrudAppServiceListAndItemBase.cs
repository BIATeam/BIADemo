// <copyright file="CrudAppServiceBase.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using BIA.Net.Core.Domain.Specification;
    using System.Linq.Expressions;
    using System;

    /// <summary>
    /// The base class for all CRUD application service.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TFilterDto">The filter DTO type.</typeparam>
    /// <typeparam name="TMapper">The mapper used between entity and DTO.</typeparam>
    public abstract class CrudAppServiceListAndItemBase<TDto, TDtoListItem, TEntity, TKey, TFilterDto, TMapper, TMapperListItem> : CrudAppServiceBase<TDto, TEntity, TKey, TFilterDto, TMapper>
        where TDto : BaseDto<TKey>, new()
        where TDtoListItem : BaseDto<TKey>, new()
        where TEntity : class, IEntity<TKey>, new()
        where TFilterDto : LazyLoadDto, new()
        where TMapper : BaseMapper<TDto, TEntity, TKey>, new()
        where TMapperListItem : BaseMapper<TDtoListItem, TEntity, TKey>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="CrudAppServiceBase{TDto,TEntity,TFilterDto,TMapper}"/> class.
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

        public new virtual async Task<IEnumerable<TDtoListItem>> GetAllAsync(Expression<Func<TEntity, TKey>> orderByExpression, bool ascending,
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

        protected new virtual async Task<byte[]> GetCsvAsync(
            TFilterDto filters = null,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null,
            bool isReadOnlyMode = false
            )
        {
            return await this.GetCsvAsync<TDtoListItem, TMapperListItem, TFilterDto>(filters: filters, id: id, specification: specification, filter: filter, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode, isReadOnlyMode: isReadOnlyMode);
        }

        public new virtual async Task<byte[]> GetCsvAsync<TOtherFilter>(
            TOtherFilter filters,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null,
            bool isReadOnlyMode = false
            )
            where TOtherFilter : LazyLoadDto, new()
        {
            return await this.GetCsvAsync<TDtoListItem, TMapperListItem, TOtherFilter>(filters: filters, id: id, specification: specification, filter: filter, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode, isReadOnlyMode: isReadOnlyMode);
        }
    }
}