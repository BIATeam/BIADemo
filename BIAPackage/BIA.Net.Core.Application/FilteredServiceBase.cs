// <copyright file="MapperServiceBase.cs" company="BIA">
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
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public abstract class FilteredServiceBase<TEntity> : AppServiceBase<TEntity>
        where TEntity : class, IEntity, new()
    {
        /// <summary>
        /// The filters
        /// </summary>
        protected Dictionary<string, Specification<TEntity>> filtersContext;

        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="FilteredServiceBase{TEntity}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        protected FilteredServiceBase(ITGenericRepository<TEntity> repository)
            : base(repository)
        {
            this.filtersContext = new Dictionary<string, Specification<TEntity>>();
        }

        public virtual async Task<(IEnumerable<TOtherDto> results, int total)> GetRangeAsync<TOtherDto, TOtherMapper, TOtherFilterDto>(
            TOtherFilterDto filters = null,
            int id = 0,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read, 
            string queryMode = QueryMode.ReadList,
            string mapperMode = null)
            where TOtherMapper : BaseMapper<TOtherDto, TEntity>, new()
            where TOtherDto : BaseDto, new()
            where TOtherFilterDto : LazyLoadDto, new()
        {
            var mapper = new TOtherMapper();

            var spec = SpecificationHelper.GetLazyLoad(
                GetFilterSpecification(accessMode, filtersContext) & specification,
                mapper,
                filters);

            var queryOrder = this.GetQueryOrder(mapper.ExpressionCollection, filters?.SortField, filters?.SortOrder == 1);

            var results = await this.Repository.GetRangeResultAsync(
                mapper.EntityToDto(mapperMode),
                id: id,
                specification: spec,
                filter: filter,
                queryOrder: queryOrder,
                firstElement: filters?.First ?? 0,
                pageCount: filters?.Rows ?? 0, 
                queryMode: queryMode);

            return (results.Item1.ToList(), results.Item2);
        }


        public async Task<IEnumerable<TOtherDto>> GetAllAsync<TOtherDto, TOtherMapper>(
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
            where TOtherMapper : BaseMapper<TOtherDto, TEntity>, new()
            where TOtherDto : BaseDto, new()
        {
            return await this.Repository.GetAllResultAsync(
                selectResult: new TOtherMapper().EntityToDto(mapperMode),
                id: id,
                specification: GetFilterSpecification(accessMode, filtersContext) & specification, 
                filter: filter, 
                queryOrder: queryOrder,
                firstElement: firstElement,
                pageCount: pageCount,
                includes: includes,
                queryMode: queryMode
                );
        }


        public async Task<IEnumerable<TOtherDto>> GetAllAsync<TOtherDto, TOtherMapper, TKey>(Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, 
            int id = 0,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            int firstElement = 0,
            int pageCount = 0,
            Expression<Func<TEntity, object>>[] includes = null,
            string accessMode = AccessMode.Read,
            string queryMode = null,
            string mapperMode = null)
            where TOtherMapper : BaseMapper<TOtherDto, TEntity>, new()
            where TOtherDto : BaseDto, new()
        {
            return await this.Repository.GetAllResultAsync(
                 new TOtherMapper().EntityToDto(mapperMode),
                orderByExpression,
                ascending,
                id: id,
                specification: GetFilterSpecification(accessMode, filtersContext) & specification,
                filter: filter,
                firstElement: firstElement,
                pageCount: pageCount,
                includes: includes,
                queryMode: queryMode
                );
        }

        public virtual async Task<byte[]> GetCsvAsync<TOtherDto, TOtherMapper, TOtherFilterDto>(
            TOtherFilterDto filters = null,
            int id = 0,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null
            )
            where TOtherMapper : BaseMapper<TOtherDto, TEntity>, new()
            where TOtherDto : BaseDto, new()
            where TOtherFilterDto : LazyLoadDto, new()
        {
            List<string> columnHeaders = null;
            if (filters is FileFiltersDto fileFilters)
            {
                columnHeaders = fileFilters.Columns.Select(x => x.Value).ToList();
            }

            // We reset these parameters, used for paging, in order to recover the totality of the data.
            filters.First = 0;
            filters.Rows = 0;

            IEnumerable<TOtherDto> results = (await this.GetRangeAsync<TOtherDto, TOtherMapper, TOtherFilterDto>(filters: filters, id:id, specification: specification, filter:filter, accessMode: accessMode, queryMode: queryMode)).results;

            List<object[]> records = results.Select(new TOtherMapper().DtoToRecord(mapperMode)).ToList();

            StringBuilder csv = new ();
            records.ForEach(line =>
            {
                csv.AppendLine(string.Join(BIAConstants.Csv.Separator, line));
            });

            string csvSep = $"sep={BIAConstants.Csv.Separator}\n";
            return Encoding.GetEncoding("iso-8859-1").GetBytes($"{csvSep}{string.Join(BIAConstants.Csv.Separator, columnHeaders ?? new List<string>())}\r\n{csv}");
        }

        public virtual async Task<TOtherDto> GetAsync<TOtherDto, TOtherMapper>(
            int id = 0,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>>[] includes = null,
            string accessMode = AccessMode.Read, 
            string queryMode = QueryMode.Read,
            string mapperMode = null)

            where TOtherMapper : BaseMapper<TOtherDto, TEntity>, new()
            where TOtherDto : BaseDto, new()
        {
            var mapper = new TOtherMapper();
            var result = await this.Repository.GetResultAsync(mapper.EntityToDto(mapperMode), 
                id: id, 
                specification: GetFilterSpecification(accessMode, filtersContext) & specification, 
                filter: filter,
                includes: includes,
                queryMode: queryMode);
            if (result == null)
            {
                throw new ElementNotFoundException();
            }

            return result;
        }

        public virtual async Task<TOtherDto> AddAsync<TOtherDto, TOtherMapper>(TOtherDto dto,
            string mapperMode = null)
            where TOtherMapper : BaseMapper<TOtherDto, TEntity>, new()
            where TOtherDto : BaseDto, new()
        {
            if (dto != null)
            {
                var entity = new TEntity();
                new TOtherMapper().DtoToEntity(dto, entity, mapperMode);
                this.Repository.Add(entity);
                await this.Repository.UnitOfWork.CommitAsync();
                dto.Id = entity.Id;
            }

            return dto;
        }

        public virtual async Task<TOtherDto> UpdateAsync<TOtherDto, TOtherMapper>(
            TOtherDto dto, 
            string accessMode = AccessMode.Update, 
            string queryMode = QueryMode.Update,
            string mapperMode = null)
            where TOtherMapper : BaseMapper<TOtherDto, TEntity>, new()
            where TOtherDto : BaseDto, new()
        {
            if (dto != null)
            {
                var mapper = new TOtherMapper();

                var entity = await this.Repository.GetEntityAsync(id: dto.Id, specification: GetFilterSpecification(accessMode, filtersContext), includes: mapper.IncludesForUpdate(mapperMode), queryMode: queryMode);
                if (entity == null)
                {
                    throw new ElementNotFoundException();
                }

                mapper.DtoToEntity(dto, entity, mapperMode);
                this.Repository.Update(entity);
                await this.Repository.UnitOfWork.CommitAsync();
                dto.DtoState = DtoState.Unchanged;
            }

            return dto;
        }


        public virtual async Task RemoveAsync(
            int id, 
            string accessMode = AccessMode.Delete, 
            string queryMode = QueryMode.Delete)
        {
            var entity = await this.Repository.GetEntityAsync(id: id, specification: GetFilterSpecification(accessMode, filtersContext), queryMode: queryMode);
            if (entity == null)
            {
                throw new ElementNotFoundException();
            }

            this.Repository.Remove(entity);
            await this.Repository.UnitOfWork.CommitAsync();
        }

        public virtual async Task SaveAsync<TOtherDto, TOtherMapper>(IEnumerable<TOtherDto> dtos,
            string accessMode = null,
            string queryMode = null,
            string mapperMode = null)
            where TOtherMapper : BaseMapper<TOtherDto, TEntity>, new()
            where TOtherDto : BaseDto, new()
        {
            var dtoList = dtos.ToList();
            if (!dtoList.Any())
            {
                return;
            }

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (var dto in dtoList)
                {
                    await this.SaveAsync<TOtherDto, TOtherMapper>(dto, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode);
                }

                transaction.Complete();
            }
        }

        /// <summary>
        /// Save a DTO in DB regarding to its state.
        /// </summary>
        /// <param name="dto">The dto to save.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual async Task SaveAsync<TOtherDto, TOtherMapper>(TOtherDto dto,
            string accessMode = null,
            string queryMode = null,
            string mapperMode = null)
            where TOtherMapper : BaseMapper<TOtherDto, TEntity>, new()
            where TOtherDto : BaseDto, new()
        {
            switch (dto.DtoState)
            {
                case DtoState.Added:
                    await this.AddAsync<TOtherDto, TOtherMapper>(dto,
                        mapperMode: mapperMode);
                    break;

                case DtoState.Modified:
                    await this.UpdateAsync<TOtherDto, TOtherMapper>(dto,
                        accessMode: accessMode ?? AccessMode.Update,
                        queryMode: queryMode ?? QueryMode.Update,
                        mapperMode: mapperMode);
                    break;

                case DtoState.Deleted:
                    await this.RemoveAsync(dto.Id,
                        accessMode: accessMode ?? AccessMode.Delete,
                        queryMode: queryMode ?? QueryMode.Delete);
                    break;

                default:
                    return;
            }
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
        /// Returns the filter to apply to the context for specify acces mode
        /// </summary>
        /// <param name="mode">Precise the usage (All/Read/Write)</param>
        /// <returns>The result mapped to the specified type</returns>
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

        public void AddBulk<TOtherDto, TOtherMapper>(IEnumerable<TOtherDto> dtoList)
            where TOtherMapper : BaseMapper<TOtherDto, TEntity>, new()
            where TOtherDto : BaseDto, new()
        {
            if (dtoList != null)
            {
                var entity = new List<TEntity>();

                foreach (var item in dtoList)
                {
                    var converted = new TEntity();
                    new TOtherMapper().DtoToEntity(item, converted);
                    entity.Add(converted);
                }

                this.Repository.UnitOfWork.AddBulk(entity);
            }
        }

        public void UpdateBulk<TOtherDto, TOtherMapper>(IEnumerable<TOtherDto> dtoList)
            where TOtherMapper : BaseMapper<TOtherDto, TEntity>, new()
            where TOtherDto : BaseDto, new()
        {
            if (dtoList != null)
            {
                var entity = new List<TEntity>();

                foreach (var item in dtoList)
                {
                    var converted = new TEntity();
                    new TOtherMapper().DtoToEntity(item, converted);
                    entity.Add(converted);
                }

                this.Repository.UnitOfWork.UpdateBulk(entity);
            }
        }

        public void RemoveBulk<TOtherDto, TOtherMapper>(IEnumerable<TOtherDto> dtoList)
            where TOtherMapper : BaseMapper<TOtherDto, TEntity>, new()
            where TOtherDto : BaseDto, new()
        {
            if (dtoList != null)
            {
                var entity = new List<TEntity>();

                foreach (var item in dtoList)
                {
                    var converted = new TEntity();
                    new TOtherMapper().DtoToEntity(item, converted);
                    entity.Add(converted);
                }

                this.Repository.UnitOfWork.RemoveBulk(entity);
            }
        }

        public async Task RemoveBulkAsync(IEnumerable<int> idList, string accessMode = AccessMode.Delete, string queryMode = QueryMode.Delete)
        {
            var entity = await this.Repository.GetAllEntityAsync(specification: GetFilterSpecification(accessMode, filtersContext), filter: x => idList.Contains(x.Id), queryMode: queryMode);
            if (entity == null)
            {
                throw new ElementNotFoundException();
            }

            this.Repository.UnitOfWork.RemoveBulk(entity);
        }
    }
}