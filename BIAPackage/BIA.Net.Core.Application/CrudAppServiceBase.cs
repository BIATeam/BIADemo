// <copyright file="CrudAppServiceBase.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Transactions;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Dto;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.Specification;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using System.Linq.Expressions;
    using System;


    /// <summary>
    /// The base class for all CRUD application service.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TFilterDto">The filter DTO type.</typeparam>
    /// <typeparam name="TMapper">The mapper used between entity and DTO.</typeparam>
    public abstract class CrudAppServiceBase<TDto, TEntity, TFilterDto, TMapper> : FilteredServiceBase<TEntity>, ICrudAppServiceBase<TDto, TEntity, TFilterDto>
        where TDto : BaseDto, new()
        where TEntity : class, IEntity, new()
        where TFilterDto : LazyLoadDto, new()
        where TMapper : BaseMapper<TDto, TEntity>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="CrudAppServiceBase{TDto,TEntity,TFilterDto,TMapper}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        protected CrudAppServiceBase(ITGenericRepository<TEntity> repository)
            : base(repository)
        {
        }

        /// <inheritdoc cref="ICrudAppServiceBase{TDto,TFilterDto}.GetRangeAsync"/>
        public virtual async Task<(IEnumerable<TDto> Results, int Total)> GetRangeAsync(
            TFilterDto filters = null,
            int id = 0,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null)
        {
            return await this.GetRangeAsync<TDto, TMapper, TFilterDto>(filters:filters, id:id, specification: specification, filter: filter, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode);
        }


        public async Task<IEnumerable<TDto>> GetAllAsync(
            int id = 0,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            QueryOrder<TEntity> queryOrder = null,
            int firstElement = 0,
            int pageCount = 0,
            Expression<Func<TEntity, object>>[] includes = null,
            string accessMode = AccessMode.Read,
            string queryMode = null,
            string mapperMode = null)
        {
            return await this.GetAllAsync<TDto, TMapper>(id: id, specification: specification, filter: filter, queryOrder: queryOrder, firstElement: firstElement, pageCount:pageCount,includes:includes, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode);
        }

        public async Task<IEnumerable<TDto>> GetAllAsync<TKey>(Expression<Func<TEntity, TKey>> orderByExpression, bool ascending,
            int id = 0,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            int firstElement = 0,
            int pageCount = 0,
            Expression<Func<TEntity, object>>[] includes = null,
            string accessMode = AccessMode.Read,
            string queryMode = null,
            string mapperMode = null)
        {
            return await this.GetAllAsync<TDto, TMapper, TKey>(orderByExpression, ascending, id: id, specification: specification, filter: filter, firstElement: firstElement, pageCount: pageCount, includes: includes, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode);
        }

        public virtual async Task<byte[]> GetCsvAsync(
            TFilterDto filters = null,
            int id = 0,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null
            )
        {
            return await this.GetCsvAsync<TDto, TMapper, TFilterDto>(filters: filters, id: id, specification: specification, filter: filter, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode);
        }

        public virtual async Task<byte[]> GetCsvAsync<TOtherFilter>(
            TOtherFilter filters,
            int id = 0,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null
            )
            where TOtherFilter : LazyLoadDto, new()
        {
            return await this.GetCsvAsync<TDto, TMapper, TOtherFilter>(filters: filters, id: id, specification: specification, filter: filter, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode);
        }

        /// <inheritdoc cref="ICrudAppServiceBase{TDto,TFilterDto}.GetAsync"/>
        public virtual async Task<TDto> GetAsync(int id = 0,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>>[] includes = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.Read,
            string mapperMode = null)
        {
            return await this.GetAsync<TDto, TMapper>(id: id, specification: specification, filter:filter, includes:includes, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode) ;
        }


        /// <inheritdoc cref="ICrudAppServiceBase{TDto,TFilterDto}.AddAsync"/>
        public virtual async Task<TDto> AddAsync(TDto dto, string mapperMode = null)
        {
            return await this.AddAsync<TDto, TMapper>(dto, mapperMode: mapperMode);
        }

        /// <inheritdoc cref="ICrudAppServiceBase{TDto,TFilterDto}.UpdateAsync"/>
        public virtual async Task<TDto> UpdateAsync(
            TDto dto, 
            string accessMode = AccessMode.Update, 
            string queryMode = QueryMode.Update,
            string mapperMode = null)
        {
            return await this.UpdateAsync<TDto, TMapper>(dto, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode);
        }

        public virtual async Task SaveAsync(TDto dto,
            string accessMode = null,
            string queryMode = null,
            string mapperMode = null)
        {
            await this.SaveAsync<TDto, TMapper>(dto, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode);
        }

        /// <inheritdoc cref="ICrudAppServiceBase{TDto,TFilterDto}.SaveAsync"/>
        public virtual async Task SaveAsync(IEnumerable<TDto> dtos,
            string accessMode = null,
            string queryMode = null,
            string mapperMode = null)
        {
            await this.SaveAsync<TDto, TMapper>(dtos, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode);
        }
    }
}