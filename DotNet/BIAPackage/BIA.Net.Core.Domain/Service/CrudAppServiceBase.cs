// <copyright file="CrudAppServiceBase.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using System.Transactions;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using BIA.Net.Core.Domain.Specification;

    /// <summary>
    /// The base class for all CRUD application service.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TFilterDto">The filter DTO type.</typeparam>
    /// <typeparam name="TMapper">The mapper used between entity and DTO.</typeparam>
    public abstract class CrudAppServiceBase<TDto, TEntity, TKey, TFilterDto, TMapper> : AppServiceBase<TDto, TEntity, TKey, TMapper>, ICrudAppServiceBase<TDto, TEntity, TKey, TFilterDto>
        where TDto : BaseDto<TKey>, new()
        where TEntity : class, IEntity<TKey>, new()
        where TFilterDto : LazyLoadDto, new()
        where TMapper : BaseMapper<TDto, TEntity, TKey>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CrudAppServiceBase{TDto, TEntity, TKey, TFilterDto, TMapper}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        protected CrudAppServiceBase(ITGenericRepository<TEntity, TKey> repository)
            : base(repository)
        {
            this.FiltersContext = new Dictionary<string, Specification<TEntity>>();
        }

        /// <summary>
        /// The filters.
        /// </summary>
        protected Dictionary<string, Specification<TEntity>> FiltersContext { get; set; }

        /// <inheritdoc/>
        public virtual async Task<TDto> GetAsync(
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>>[] includes = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.Read,
            string mapperMode = MapperMode.Item,
            bool isReadOnlyMode = false)
        {
            TMapper mapper = this.InitMapper();
            var result = await this.Repository.GetResultAsync(
                mapper.EntityToDto(mapperMode),
                id: id,
                specification: this.GetFilterSpecification(accessMode, this.FiltersContext) & specification,
                filter: filter,
                includes: includes,
                queryMode: queryMode,
                isReadOnlyMode: isReadOnlyMode);
            return result ?? throw new ElementNotFoundException();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TDto>> GetAllAsync(
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
            TMapper mapper = this.InitMapper();
            return await this.Repository.GetAllResultAsync(
                selectResult: mapper.EntityToDto(mapperMode),
                id: id,
                specification: this.GetFilterSpecification(accessMode, this.FiltersContext) & specification,
                filter: filter,
                queryOrder: queryOrder,
                firstElement: firstElement,
                pageCount: pageCount,
                includes: includes,
                queryMode: queryMode,
                isReadOnlyMode: isReadOnlyMode);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TDto>> GetAllAsync(
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
            TMapper mapper = this.InitMapper();
            return await this.Repository.GetAllResultAsync(
                mapper.EntityToDto(mapperMode),
                orderByExpression,
                ascending,
                id: id,
                specification: this.GetFilterSpecification(accessMode, this.FiltersContext) & specification,
                filter: filter,
                firstElement: firstElement,
                pageCount: pageCount,
                includes: includes,
                queryMode: queryMode,
                isReadOnlyMode: isReadOnlyMode);
        }

        /// <inheritdoc/>
        public virtual async Task<(IEnumerable<TOtherDto> results, int total)> GetRangeAsync<TOtherDto, TOtherMapper, TOtherFilterDto>(
            TOtherFilterDto filters = null,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null,
            bool isReadOnlyMode = false,
            TOtherMapper mapper = null)
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>, new()
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherFilterDto : LazyLoadDto, new()
        {
            mapper ??= this.InitMapper<TOtherDto, TOtherMapper>();

            var spec = SpecificationHelper.GetLazyLoad<TEntity, TKey, TOtherMapper>(
                this.GetFilterSpecification(accessMode, this.FiltersContext) & specification,
                mapper,
                filters);

            var queryOrder = this.GetQueryOrder(mapper.ExpressionCollection, filters?.SortField, filters?.SortOrder == 1);

            (IEnumerable<TOtherDto> results, int total) = await this.Repository.GetRangeResultAsync(
                mapper.EntityToDto(mapperMode),
                id: id,
                specification: spec,
                filter: filter,
                queryOrder: queryOrder,
                firstElement: filters?.First ?? 0,
                pageCount: filters?.Rows ?? 0,
                queryMode: queryMode,
                isReadOnlyMode: isReadOnlyMode);

            return (results.ToList(), total);
        }

        /// <inheritdoc/>
        public virtual async Task<(IEnumerable<TDto> results, int total)> GetRangeAsync(
            TFilterDto filters = null,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null,
            bool isReadOnlyMode = false)
        {
            return await this.GetRangeAsync<TDto, TMapper, TFilterDto>(
                filters: filters,
                id: id,
                specification: specification,
                filter: filter,
                accessMode: accessMode,
                queryMode: queryMode,
                mapperMode: mapperMode,
                isReadOnlyMode: isReadOnlyMode);
        }

        /// <inheritdoc/>
        public virtual async Task<TDto> AddAsync(
            TDto dto,
            string mapperMode = null)
        {
            if (dto != null)
            {
                TMapper mapper = this.InitMapper();
                var entity = new TEntity();
                mapper.DtoToEntity(dto, entity, mapperMode, this.Repository.UnitOfWork);
                this.Repository.Add(entity);
                await this.Repository.UnitOfWork.CommitAsync();
                mapper.MapEntityKeysInDto(entity, dto);
            }

            return dto;
        }

        /// <inheritdoc/>
        public virtual async Task<TDto> UpdateAsync(
            TDto dto,
            string accessMode = AccessMode.Update,
            string queryMode = QueryMode.Update,
            string mapperMode = null)
        {
            if (dto != null)
            {
                TMapper mapper = this.InitMapper();

                TEntity entity = await this.Repository.GetEntityAsync(id: dto.Id, specification: this.GetFilterSpecification(accessMode, this.FiltersContext), includes: mapper.IncludesForUpdate(mapperMode), queryMode: queryMode) ?? throw new ElementNotFoundException();
                mapper.DtoToEntity(dto, entity, mapperMode, this.Repository.UnitOfWork);

                await this.Repository.UnitOfWork.CommitAsync();
                dto.DtoState = DtoState.Unchanged;
                mapper.MapEntityKeysInDto(entity, dto);
            }

            return dto;
        }

        /// <inheritdoc/>
        public virtual async Task<TDto> RemoveAsync(
            TKey id,
            string accessMode = AccessMode.Delete,
            string queryMode = QueryMode.Delete,
            string mapperMode = null)
        {
            TMapper mapper = this.InitMapper();

            var entity = await this.Repository.GetEntityAsync(id: id, specification: this.GetFilterSpecification(accessMode, this.FiltersContext), includes: mapper.IncludesBeforeDelete(mapperMode), queryMode: queryMode) ?? throw new ElementNotFoundException();
            var dto = new TDto();
            mapper.MapEntityKeysInDto(entity, dto);

            this.Repository.Remove(entity);
            await this.Repository.UnitOfWork.CommitAsync();
            return dto;
        }

        /// <inheritdoc/>
        public virtual async Task<List<TDto>> RemoveAsync(
            List<TKey> ids,
            string accessMode = AccessMode.Delete,
            string queryMode = QueryMode.Delete,
            string mapperMode = null)
        {
            var dtos = new List<TDto>();
            foreach (TKey id in ids)
            {
                dtos.Add(await this.RemoveAsync(id, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode));
            }

            return dtos;
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TDto>> SaveAsync(
            IEnumerable<TDto> dtos,
            string accessMode = null,
            string queryMode = null,
            string mapperMode = null)
        {
            var dtoList = dtos.ToList();
            List<TDto> returnDto = new();
            if (dtoList.Any())
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                foreach (var dto in dtoList)
                {
                    returnDto.Add(await this.SaveAsync(dto, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode));
                }

                transaction.Complete();
            }

            return returnDto;
        }

        /// <inheritdoc/>
        public virtual async Task<TDto> SaveAsync(
            TDto dto,
            string accessMode = null,
            string queryMode = null,
            string mapperMode = null)
        {
            TDto returnDto = dto;
            switch (dto.DtoState)
            {
                case DtoState.Added:
                    returnDto = await this.AddAsync(
                        dto,
                        mapperMode: mapperMode);
                    break;

                case DtoState.Modified:
                    returnDto = await this.UpdateAsync(
                        dto,
                        accessMode: accessMode ?? AccessMode.Update,
                        queryMode: queryMode ?? QueryMode.Update,
                        mapperMode: mapperMode);
                    break;

                case DtoState.Deleted:
                    returnDto = await this.RemoveAsync(
                        dto.Id,
                        accessMode: accessMode ?? AccessMode.Delete,
                        queryMode: queryMode ?? QueryMode.Delete,
                        mapperMode: mapperMode);
                    break;

                default:
                    return returnDto;
            }

            return returnDto;
        }

        /// <inheritdoc/>
        public virtual async Task<byte[]> GetCsvAsync(
            TFilterDto filters = null,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = "Csv",
            bool isReadOnlyMode = false)
        {
            List<string> columnHeaderKeys = null;
            List<string> columnHeaderValues = null;
            if (filters is PagingFilterFormatDto fileFilters)
            {
                columnHeaderKeys = fileFilters.Columns.Select(x => x.Key).ToList();
                columnHeaderValues = fileFilters.Columns.Select(x => x.Value).ToList();
            }

            // We reset these parameters, used for paging, in order to recover the totality of the data.
            filters.First = 0;
            filters.Rows = 0;

            IEnumerable<TDto> results = (await this.GetRangeAsync(filters: filters, id: id, specification: specification, filter: filter, accessMode: accessMode, queryMode: queryMode, isReadOnlyMode: isReadOnlyMode)).results;

            TMapper mapper = this.InitMapper();
            List<object[]> records = results.Select(mapper.DtoToRecord(mapperMode, columnHeaderKeys)).ToList();

            StringBuilder csv = new();
            records.ForEach(line =>
            {
                csv.AppendLine(string.Join(BIAConstants.Csv.Separator, line));
            });

            string csvSep = $"sep={BIAConstants.Csv.Separator}\n";
            return Encoding.GetEncoding("iso-8859-1").GetBytes($"{csvSep}{string.Join(BIAConstants.Csv.Separator, columnHeaderValues ?? new List<string>())}\r\n{csv}");
        }

        /// <inheritdoc/>
        public virtual async Task AddBulkAsync(IEnumerable<TDto> dtoList)
        {
            if (dtoList != null)
            {
                TMapper mapper = this.InitMapper();
                List<TEntity> entity = new();

                foreach (var item in dtoList)
                {
                    var converted = new TEntity();
                    mapper.DtoToEntity(item, converted);
                    entity.Add(converted);
                }

                await this.Repository.UnitOfWork.AddBulkAsync(entity);
            }
        }

        /// <inheritdoc/>
        public virtual async Task UpdateBulkAsync(IEnumerable<TDto> dtoList)
        {
            if (dtoList != null)
            {
                TMapper mapper = this.InitMapper();
                List<TEntity> entity = new();

                foreach (var item in dtoList)
                {
                    var converted = new TEntity();
                    mapper.DtoToEntity(item, converted);
                    entity.Add(converted);
                }

                await this.Repository.UnitOfWork.UpdateBulkAsync(entity);
            }
        }

        /// <inheritdoc/>
        public virtual async Task RemoveBulkAsync(IEnumerable<TDto> dtoList)
        {
            if (dtoList != null)
            {
                TMapper mapper = this.InitMapper();
                List<TEntity> entity = new();

                foreach (var item in dtoList)
                {
                    var converted = new TEntity();
                    mapper.DtoToEntity(item, converted);
                    entity.Add(converted);
                }

                await this.Repository.UnitOfWork.RemoveBulkAsync(entity);
            }
        }

        /// <inheritdoc/>
        public virtual async Task RemoveBulkAsync(IEnumerable<TKey> idList, string accessMode = AccessMode.Delete, string queryMode = QueryMode.Delete)
        {
            var entity = await this.Repository.GetAllEntityAsync(specification: this.GetFilterSpecification(accessMode, this.FiltersContext), filter: x => idList.Contains(x.Id), queryMode: queryMode) ?? throw new ElementNotFoundException();
            await this.Repository.UnitOfWork.RemoveBulkAsync(entity);
        }

        /// <summary>
        /// Get the paging order.
        /// </summary>
        /// <param name="collection">The expression collection of entity.</param>
        /// <param name="orderMember">The order member.</param>
        /// <param name="ascending">If set to <c>true</c> [ascending].</param>
        /// <returns>The paging order.</returns>
        protected virtual QueryOrder<TEntity> GetQueryOrder(ExpressionCollection<TEntity> collection, string orderMember, bool ascending)
        {
            if (string.IsNullOrWhiteSpace(orderMember) || !collection.ContainsKey(orderMember))
            {
                return new QueryOrder<TEntity>().OrderBy(entity => entity.Id);
            }

            var order = new QueryOrder<TEntity>();
            order.GetByExpression(collection[orderMember], ascending);
            return order;
        }

        /// <summary>
        /// Returns the filter to apply to the context for specify acces mode.
        /// </summary>
        /// <param name="mode">Precise the usage (All/Read/Write).</param>
        /// <param name="filtersContext">The filter for context.</param>
        /// <returns>The result mapped to the specified type.</returns>
        protected virtual Specification<TEntity> GetFilterSpecification(string mode, Dictionary<string, Specification<TEntity>> filtersContext)
        {
            if (filtersContext.ContainsKey(mode))
            {
                return filtersContext[mode];
            }
            else if (mode == AccessMode.Update)
            {
                if (filtersContext.ContainsKey(AccessMode.Read))
                {
                    return filtersContext[AccessMode.Read];
                }
            }
            else if (mode == AccessMode.Delete)
            {
                if (filtersContext.ContainsKey(AccessMode.Update))
                {
                    return filtersContext[AccessMode.Update];
                }

                if (filtersContext.ContainsKey(AccessMode.Read))
                {
                    return filtersContext[AccessMode.Read];
                }
            }

            return null;
        }
    }
}