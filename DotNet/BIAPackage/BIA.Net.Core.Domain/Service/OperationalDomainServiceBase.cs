// <copyright file="OperationalDomainServiceBase.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection.Metadata.Ecma335;
    using System.Text;
    using System.Threading.Tasks;
    using System.Transactions;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Error;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Common.Helpers;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Audit;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Base.Interface;
    using BIA.Net.Core.Domain.Dto.Historic;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using BIA.Net.Core.Domain.Specification;
    using BIA.Net.Core.Domain.User.Entities;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using static BIA.Net.Core.Common.BiaConstants;
    using static BIA.Net.Core.Common.BiaRights;

    /// <summary>
    /// Base class for a service that need an implementation of a <see cref="DomainServiceBase{TEntity, TKey}"/> with operations.
    /// </summary>
    /// <typeparam name="TDto">DTO type for item.</typeparam>
    /// <typeparam name="TDtoListItem">DTO type for list items.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The primary key of the entity type.</typeparam>
    /// <typeparam name="TFilterDto">The filter DTO type used for paging/filtering.</typeparam>
    /// <typeparam name="TMapper">The mapper type from DTO to entity.</typeparam>
    /// <typeparam name="TMapperListItem">The mapper type from DTO for list item to entity.</typeparam>
    public abstract class OperationalDomainServiceBase<TDto, TDtoListItem, TEntity, TKey, TFilterDto, TMapper, TMapperListItem> : DomainServiceBase<TEntity, TKey>, IOperationalDomainServiceBase<TDto, TDtoListItem, TEntity, TKey, TFilterDto>
        where TDto : BaseDto<TKey>, new()
        where TDtoListItem : BaseDto<TKey>, new()
        where TEntity : class, IEntity<TKey>, new()
        where TFilterDto : class, IPagingFilterFormatDto, new()
        where TMapper : BiaBaseMapper<TDto, TEntity, TKey>
        where TMapperListItem : BiaBaseMapper<TDtoListItem, TEntity, TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationalDomainServiceBase{TDto, TDtoListItem, TEntity, TKey, TFilterDto, TMapper, TMapperListItem}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        protected OperationalDomainServiceBase(ITGenericRepository<TEntity, TKey> repository)
            : base(repository)
        {
            this.FiltersContext = new Dictionary<string, Specification<TEntity>>();
        }

        /// <summary>
        /// Delegate for <see cref="GetGenericAsync{TOtherDto, TOtherMapper}(TKey, Specification{TEntity}, Expression{Func{TEntity, bool}}, Expression{Func{TEntity, object}}[], string, string, string, bool)"/>.
        /// </summary>
        /// <typeparam name="TOtherDto">The DTO type for item.</typeparam>
        /// <typeparam name="TOtherMapper">The mapper type from DTO to entity.</typeparam>
        /// <typeparam name="TOtherFilterDto">The filter DTO type used for paging/filtering.</typeparam>
        /// <param name="filters">The filters.</param>
        /// <param name="id">The id.</param>
        /// <param name="specification">Specification Used to filter query.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="isReadOnlyMode">Readonly mode to use readOnly context.</param>
        /// <returns>A tuple containing the results and the total count.</returns>
        protected delegate Task<(IEnumerable<TOtherDto> Results, int Total)> GetRangeGenericAsyncDelegate<TOtherDto, TOtherMapper, TOtherFilterDto>(
           TOtherFilterDto filters = null,
           TKey id = default,
           Specification<TEntity> specification = null,
           Expression<Func<TEntity, bool>> filter = null,
           string accessMode = AccessMode.Read,
           string queryMode = QueryMode.ReadList,
           string mapperMode = null,
           bool isReadOnlyMode = false)
           where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
           where TOtherDto : BaseDto<TKey>, new()
           where TOtherFilterDto : class, IPagingFilterFormatDto, new();

        /// <summary>
        /// Delegate for <see cref="GetGenericAsync{TOtherDto, TOtherMapper}(TKey, Specification{TEntity}, Expression{Func{TEntity, bool}}, Expression{Func{TEntity, object}}[], string, string, string, bool)"/>.
        /// </summary>
        /// <typeparam name="TOtherDto">The DTO type for item.</typeparam>
        /// <typeparam name="TOtherMapper">The mapper type from DTO to entity.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="includes">The list of includes.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="isReadOnlyMode">Readonly mode to use readOnly context.</param>
        /// <returns>The DTO.</returns>
        protected delegate Task<TOtherDto> GetGenericAsyncDelegate<TOtherDto, TOtherMapper>(
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>>[] includes = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.Read,
            string mapperMode = MapperMode.Item,
            bool isReadOnlyMode = false)
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new();

        /// <summary>
        /// Delegate for <see cref="AddGenericAsync{TOtherDto, TOtherMapper}(TOtherDto, string)" />.
        /// </summary>
        /// <typeparam name="TOtherDto">The DTO type for item.</typeparam>
        /// <typeparam name="TOtherMapper">The mapper type from DTO to entity.</typeparam>
        /// <param name="dto">The DTO to add.</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="autoCommit">if set to <c>true</c> [automatic commit].</param>
        /// <returns>The DTO with id updated.</returns>
        protected delegate Task<TOtherDto> AddGenericAsyncDelegate<TOtherDto, TOtherMapper>(
            TOtherDto dto,
            string mapperMode = null,
            bool autoCommit = true)
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new();

        /// <summary>
        /// Delegate for <see cref="UpdateGenericAsync{TOtherDto, TOtherMapper}(TOtherDto, string, string, string)" />.
        /// </summary>
        /// <typeparam name="TOtherDto">The DTO type for item.</typeparam>
        /// <typeparam name="TOtherMapper">The mapper type from DTO to entity.</typeparam>
        /// <param name="dto">The DTO to update.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="autoCommit">if set to <c>true</c> [automatic commit].</param>+
        /// <returns>The DTO updated.</returns>
        protected delegate Task<TOtherDto> UpdateGenericAsyncDelegate<TOtherDto, TOtherMapper>(
            TOtherDto dto,
            string accessMode = AccessMode.Update,
            string queryMode = QueryMode.Update,
            string mapperMode = null,
            bool autoCommit = true)
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new();

        /// <summary>
        /// Delegate for <see cref="RemoveGenericAsync{TOtherDto, TOtherMapper}(TKey, string, string, string, bool)" />.
        /// </summary>
        /// <typeparam name="TOtherDto">The DTO type for item.</typeparam>
        /// <typeparam name="TOtherMapper">The mapper type from DTO to entity.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="bypassFixed">Indicates whether the fixed security should be bypassed or not.</param>
        /// <param name="autoCommit">if set to <c>true</c> [automatic commit].</param>
        /// <returns>The deleted DTO.</returns>
        protected delegate Task<TOtherDto> RemoveGenericAsyncDelegate<TOtherDto, TOtherMapper>(
            TKey id,
            string accessMode = AccessMode.Delete,
            string queryMode = QueryMode.Delete,
            string mapperMode = null,
            bool bypassFixed = false,
            bool autoCommit = true);

        /// <summary>
        /// Delegate for <see cref="SaveGenericAsync{TOtherDto, TOtherMapper}(TOtherDto, AddGenericAsyncDelegate{TOtherDto,TOtherMapper}, UpdateGenericAsyncDelegate{TOtherDto,TOtherMapper}, RemoveGenericAsyncDelegate{TOtherDto,TOtherMapper}, string, string, string)"/>.
        /// </summary>
        /// <typeparam name="TOtherDto">The DTO type for item.</typeparam>
        /// <typeparam name="TOtherMapper">The mapper type from DTO to entity.</typeparam>
        /// <param name="dto">The dto to save.</param>
        /// <param name="addGenericAsync">Delegate used to add the dto.</param>
        /// <param name="updateGenericAsync">Delegate used to update the dto.</param>
        /// <param name="removeGenericAsync">Delegate used to remove the dto.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <returns>The saved DTO.</returns>
        protected delegate Task<TOtherDto> SaveGenericAsyncDelegate<TOtherDto, TOtherMapper>(
            TOtherDto dto,
            AddGenericAsyncDelegate<TOtherDto, TOtherMapper> addGenericAsync,
            UpdateGenericAsyncDelegate<TOtherDto, TOtherMapper> updateGenericAsync,
            RemoveGenericAsyncDelegate<TOtherDto, TOtherMapper> removeGenericAsync,
            string accessMode = null,
            string queryMode = null,
            string mapperMode = null)
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new();

        /// <summary>
        /// The filters.
        /// </summary>
        protected Dictionary<string, Specification<TEntity>> FiltersContext { get; set; }

        /// <summary>
        /// Escape a string for CSV output.
        /// </summary>
        /// <param name="x">The string value.</param>
        /// <returns>A string formatted for CSV cell.</returns>
        public static string CSVString(string x)
        {
            return "\"" + x?.Replace("\"", "\"\"") + "\"";
        }

        /// <inheritdoc />
        public virtual async Task<(IEnumerable<TDtoListItem> Results, int Total)> GetRangeAsync(
            TFilterDto filters = null,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null,
            bool isReadOnlyMode = false)
        {
            return await this.GetRangeGenericAsync<TDtoListItem, TMapperListItem, TFilterDto>(filters, id, specification, filter, accessMode, queryMode, mapperMode, isReadOnlyMode);
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TDtoListItem>> GetAllAsync(
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
            return await this.GetAllGenericAsync<TDtoListItem, TMapperListItem>(id, specification, filter, queryOrder, firstElement, pageCount, includes, accessMode, queryMode, mapperMode, isReadOnlyMode);
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TDtoListItem>> GetAllAsync(
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
            return await this.GetAllGenericAsync<TDtoListItem, TMapperListItem>(orderByExpression, ascending, id, specification, filter, firstElement, pageCount, includes, accessMode, queryMode, mapperMode, isReadOnlyMode);
        }

        /// <inheritdoc />
        public virtual async Task<byte[]> GetCsvAsync(
            TFilterDto filters = null,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null,
            bool isReadOnlyMode = false)
        {
            return await this.GetCsvGenericAsync<TDtoListItem, TMapperListItem, TFilterDto>(this.GetRangeAsync, filters, id, specification, filter, accessMode, queryMode, mapperMode, isReadOnlyMode);
        }

        /// <inheritdoc />
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
            return await this.GetGenericAsync<TDto, TMapper>(id, specification, filter, includes, accessMode, queryMode, mapperMode, isReadOnlyMode);
        }

        /// <inheritdoc />
        public virtual async Task<TDto> AddAsync(
            TDto dto,
            string mapperMode = null,
            bool autoCommit = true)
        {
            return await this.AddGenericAsync<TDto, TMapper>(dto: dto, mapperMode: mapperMode, autoCommit: true);
        }

        /// <inheritdoc />
        public virtual async Task<TDto> UpdateAsync(
            TDto dto,
            string accessMode = AccessMode.Update,
            string queryMode = QueryMode.Update,
            string mapperMode = null,
            bool autoCommit = true)
        {
            return await this.UpdateGenericAsync<TDto, TMapper>(dto: dto, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode, autoCommit: true);
        }

        /// <inheritdoc />
        public virtual async Task<TDto> RemoveAsync(
            TKey id,
            string accessMode = AccessMode.Delete,
            string queryMode = QueryMode.Delete,
            string mapperMode = null,
            bool bypassFixed = false,
            bool autoCommit = true)
        {
            return await this.RemoveGenericAsync<TDto, TMapper>(id, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode, bypassFixed: bypassFixed, autoCommit: true);
        }

        /// <inheritdoc />
        public virtual async Task<List<TDto>> RemoveAsync(
            List<TKey> ids,
            string accessMode = AccessMode.Delete,
            string queryMode = QueryMode.Delete,
            string mapperMode = null,
            bool bypassFixed = false,
            bool autoCommit = true)
        {
            return await this.RemoveGenericAsync<TDto, TMapper>(ids, this.RemoveAsync, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode, bypassFixed: bypassFixed, autoCommit: true);
        }

        /// <inheritdoc />
        public virtual async Task<List<TDto>> SaveSafeAsync(
            IEnumerable<TDto> dtos,
            BiaClaimsPrincipal principal,
            string rightAdd,
            string rightUpdate,
            string rightDelete,
            string accessMode = null,
            string queryMode = null,
            string mapperMode = null)
        {
            return await this.SaveSafeGenericAsync<TDto, TMapper>(dtos, principal, rightAdd, rightUpdate, rightDelete, this.AddAsync, this.UpdateAsync, this.RemoveAsync, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode);
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TDto>> SaveAsync(
            IEnumerable<TDto> dtos,
            string accessMode = null,
            string queryMode = null,
            string mapperMode = null)
        {
            return await this.SaveGenericAsync<TDto, TMapper>(
                dtos,
                (dto, _, _, _, _, _, _) => this.SaveAsync(dto, accessMode, queryMode, mapperMode),
                this.AddAsync,
                this.UpdateAsync,
                this.RemoveAsync,
                accessMode: accessMode,
                queryMode: queryMode,
                mapperMode: mapperMode);
        }

        /// <inheritdoc />
        public virtual async Task<TDto> SaveAsync(
            TDto dto,
            string accessMode = null,
            string queryMode = null,
            string mapperMode = null)
        {
            return await this.SaveGenericAsync<TDto, TMapper>(dto, this.AddAsync, this.UpdateAsync, this.RemoveAsync, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode);
        }

        /// <inheritdoc />
        public virtual async Task<TDto> UpdateFixedAsync(TKey id, bool isFixed)
        {
            return await this.UpdateFixedGenericAsync<TDto, TMapper>(id, isFixed, this.GetAsync);
        }

        /// <inheritdoc />
        public virtual async Task<List<EntityHistoricalEntryDto>> GetHistoricalAsync<TUserEntity>(TKey id)
            where TUserEntity : BaseEntityUser
        {
            return await this.GetHistoricalGenericAsync<TDto, TMapper, TUserEntity>(id);
        }

        /// <summary>
        /// Get the DTO list with paging and sorting.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <typeparam name="TOtherFilterDto">The type of the filter.</typeparam>
        /// <param name="filters">The filters.</param>
        /// <param name="id">The id.</param>
        /// <param name="specification">Specification Used to filter query.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="isReadOnlyMode">Readonly mode to use readOnly context.</param>
        /// <returns>The list of DTO.</returns>
        protected virtual async Task<(IEnumerable<TOtherDto> Results, int Total)> GetRangeGenericAsync<TOtherDto, TOtherMapper, TOtherFilterDto>(
            TOtherFilterDto filters = null,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null,
            bool isReadOnlyMode = false)
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherFilterDto : class, IPagingFilterFormatDto, new()
        {
            return await this.ExecuteWithFrontUserExceptionHandlingAsync(async () =>
            {
                TOtherMapper mapper = this.InitMapper<TOtherDto, TOtherMapper>();

                this.SetGetRangeFilterSpecifications(ref specification, filters);
                var spec = SpecificationHelper.GetLazyLoad<TEntity, TKey, TOtherMapper>(
                    this.GetFilterSpecification(accessMode, this.FiltersContext) & specification,
                    mapper,
                    filters);

                var queryOrder = this.GetQueryOrder(mapper.ExpressionCollectionOrder, filters?.SortField, filters?.SortOrder == 1, filters?.MultiSortMeta);

                var results = await this.Repository.GetRangeResultAsync(
                    mapper.EntityToDto(mapperMode),
                    id: id,
                    specification: spec,
                    filter: filter,
                    queryOrder: queryOrder,
                    firstElement: filters?.First ?? 0,
                    pageCount: filters?.Rows ?? 0,
                    queryMode: queryMode,
                    isReadOnlyMode: isReadOnlyMode);

                return (results.Item1.ToList(), results.Item2);
            });
        }

        /// <summary>
        /// Get the DTO list. (with a queryOrder).
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="specification">Specification Used to filter query.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="queryOrder">Order the Query.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <param name="includes">The list of includes.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="isReadOnlyMode">Readonly mode to use readOnly context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual async Task<IEnumerable<TOtherDto>> GetAllGenericAsync<TOtherDto, TOtherMapper>(
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
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new()
        {
            return await this.ExecuteWithFrontUserExceptionHandlingAsync(async () =>
            {
                TOtherMapper mapper = this.InitMapper<TOtherDto, TOtherMapper>();
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
            });
        }

        /// <summary>
        /// Get the DTO list. (with an order By Expression and direction).
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="orderByExpression">Lambda Expression for Ordering Query.</param>
        /// <param name="ascending">Direction of Ordering.</param>
        /// <param name="id">The id.</param>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="firstElement">First element to take.</param>
        /// <param name="pageCount">Number of elements in each page.</param>
        /// <param name="includes">The list of includes.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="isReadOnlyMode">Readonly mode to use readOnly context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual async Task<IEnumerable<TOtherDto>> GetAllGenericAsync<TOtherDto, TOtherMapper>(
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
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new()
        {
            return await this.ExecuteWithFrontUserExceptionHandlingAsync(async () =>
            {
                TOtherMapper mapper = this.InitMapper<TOtherDto, TOtherMapper>();
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
            });
        }

        /// <summary>
        /// Get the csv with filter for <typeparamref name="TOtherDto"/> and <typeparamref name="TOtherMapper"/>.
        /// </summary>
        /// <param name="getRangeAsync">Delegate used to get range of dto.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="id">The id.</param>
        /// <param name="specification">Specification Used to filter query.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [This improves performance and enables parallel querying]. (optionnal, false by default).</param>
        /// <typeparam name="TOtherDto">Other DTO type.</typeparam>
        /// <typeparam name="TOtherMapper">Other mapper type.</typeparam>
        /// <typeparam name="TOtherFilterDto">Other filter DTO type.</typeparam>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        protected virtual async Task<byte[]> GetCsvGenericAsync<TOtherDto, TOtherMapper, TOtherFilterDto>(
            GetRangeGenericAsyncDelegate<TOtherDto, TOtherMapper, TOtherFilterDto> getRangeAsync,
            TOtherFilterDto filters = null,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null,
            bool isReadOnlyMode = false)
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherFilterDto : class, IPagingFilterFormatDto, new()
        {
            filters.First = 0;
            filters.Rows = 0;

            IEnumerable<TOtherDto> results = (await getRangeAsync(filters: filters, id: id, specification: specification, filter: filter, accessMode: accessMode, queryMode: queryMode, isReadOnlyMode: isReadOnlyMode)).Results;

            var columnHeaderKeys = new List<string>();
            var columnHeaderValues = new List<string>();

            if (filters?.Columns is not null)
            {
                columnHeaderKeys.AddRange(filters.Columns.Select(x => x.Key));
                columnHeaderValues.AddRange(filters.Columns.Select(x => x.Value));
            }

            TOtherMapper mapper = this.InitMapper<TOtherDto, TOtherMapper>();
            List<object[]> records = results.Select(mapper.DtoToRecord(mapperMode, columnHeaderKeys)).ToList();

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine($"sep={BiaConstants.Csv.Separator}");
            foreach (string line in this.BiaNetSection.CsvAdditionalContent?.Headers ?? new List<string>())
            {
                csvBuilder.AppendLine(CSVString(line));
            }

            csvBuilder.AppendLine(string.Join(BiaConstants.Csv.Separator, columnHeaderValues));
            records.ForEach(line =>
            {
                csvBuilder.AppendLine(string.Join(BiaConstants.Csv.Separator, line));
            });

            foreach (string line in this.BiaNetSection.CsvAdditionalContent?.Footers ?? new List<string>())
            {
                csvBuilder.AppendLine(CSVString(line));
            }

            return Encoding.GetEncoding(1252).GetBytes(csvBuilder.ToString());
        }

        /// <summary>
        /// Return a DTO for a given identifier.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="includes">The list of includes.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="isReadOnlyMode">Readonly mode to use readOnly context.</param>
        /// <returns>The DTO.</returns>
        protected virtual async Task<TOtherDto> GetGenericAsync<TOtherDto, TOtherMapper>(
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>>[] includes = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.Read,
            string mapperMode = MapperMode.Item,
            bool isReadOnlyMode = false)

            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new()
        {
            return await this.ExecuteWithFrontUserExceptionHandlingAsync(async () =>
            {
                TOtherMapper mapper = this.InitMapper<TOtherDto, TOtherMapper>();
                var result = await this.Repository.GetResultAsync(
                    mapper.EntityToDto(mapperMode),
                    id: id,
                    specification: this.GetFilterSpecification(accessMode, this.FiltersContext) & specification,
                    filter: filter,
                    includes: includes,
                    queryMode: queryMode,
                    isReadOnlyMode: isReadOnlyMode);
                if (result == null)
                {
                    throw new ElementNotFoundException();
                }

                return result;
            });
        }

        /// <summary>
        /// Transform the DTO into the corresponding entity and add it to the DB.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="dto">The DTO.</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="autoCommit">if set to <c>true</c> [automatic commit].</param>+
        /// <returns>The DTO with id updated.</returns>
        protected virtual async Task<TOtherDto> AddGenericAsync<TOtherDto, TOtherMapper>(
            TOtherDto dto,
            string mapperMode = null,
            bool autoCommit = true)
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new()
        {
            return await this.ExecuteWithFrontUserExceptionHandlingAsync(async () =>
            {
                if (dto != null)
                {
                    TOtherMapper mapper = this.InitMapper<TOtherDto, TOtherMapper>();
                    var entity = default(TEntity);
                    mapper.DtoToEntity(dto, ref entity, mapperMode, this.Repository.UnitOfWork);
                    this.Repository.Add(entity);

                    if (autoCommit)
                    {
                        await this.Repository.UnitOfWork.CommitAsync();
                    }

                    mapper.MapEntityKeysInDto(entity, dto);
                }

                return dto;
            });
        }

        /// <summary>
        /// Update an entity in DB with the DTO values.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="dto">The DTO.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="autoCommit">Is auto commit.</param>
        /// <returns>The DTO updated.</returns>
        protected virtual async Task<TOtherDto> UpdateGenericAsync<TOtherDto, TOtherMapper>(
            TOtherDto dto,
            string accessMode = AccessMode.Update,
            string queryMode = QueryMode.Update,
            string mapperMode = null,
            bool autoCommit = true)
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new()
        {
            return await this.ExecuteWithFrontUserExceptionHandlingAsync(async () =>
            {
                if (dto != null)
                {
                    TOtherMapper mapper = this.InitMapper<TOtherDto, TOtherMapper>();

                    var entity = await this.Repository.GetEntityAsync(id: dto.Id, specification: this.GetFilterSpecification(accessMode, this.FiltersContext), includes: mapper.IncludesForUpdate(mapperMode), queryMode: queryMode)
                        ?? throw new ElementNotFoundException();

                    if (entity is IEntityFixable entityFixable && entityFixable.IsFixed)
                    {
                        throw new FrontUserException("Item is fixed and cannot be edited.");
                    }

                    if (entity is IEntityVersioned versionedEntity && dto is IDtoVersioned dtoVersioned
                    && !string.IsNullOrWhiteSpace(dtoVersioned.RowVersion)
                    && !Convert.ToBase64String(versionedEntity.RowVersion).SequenceEqual(dtoVersioned.RowVersion))
                    {
                        throw new OutdateException();
                    }

                    mapper.DtoToEntity(dto, ref entity, mapperMode, this.Repository.UnitOfWork);

                    if (autoCommit)
                    {
                        await this.Repository.UnitOfWork.CommitAsync();
                    }

                    dto.DtoState = DtoState.Unchanged;
                    mapper.MapEntityKeysInDto(entity, dto);
                }

                return dto;
            });
        }

        /// <summary>
        /// Remove an entity with its identifier.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="bypassFixed">Indicates weither the fixed security should be bypassed or not.</param>
        /// <param name="autoCommit">if set to <c>true</c> [automatic commit].</param>+
        /// <returns>The deleted DTO.</returns>
        protected virtual async Task<TOtherDto> RemoveGenericAsync<TOtherDto, TOtherMapper>(
            TKey id,
            string accessMode = AccessMode.Delete,
            string queryMode = QueryMode.Delete,
            string mapperMode = null,
            bool bypassFixed = false,
            bool autoCommit = true)
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new()
        {
            return await this.ExecuteWithFrontUserExceptionHandlingAsync(async () =>
            {
                TOtherMapper mapper = this.InitMapper<TOtherDto, TOtherMapper>();

                var entity = await this.Repository.GetEntityAsync(id: id, specification: this.GetFilterSpecification(accessMode, this.FiltersContext), includes: mapper.IncludesBeforeDelete(mapperMode), queryMode: queryMode);
                if (entity == null)
                {
                    throw new ElementNotFoundException();
                }

                if (!bypassFixed && entity is IEntityFixable entityFixable && entityFixable.IsFixed)
                {
                    throw new FrontUserException("Item is fixed and cannot be deleted.");
                }

                var dto = new TOtherDto();
                mapper.MapEntityKeysInDto(entity, dto);

                this.Repository.Remove(entity);

                if (autoCommit)
                {
                    await this.Repository.UnitOfWork.CommitAsync();
                }

                return dto;
            });
        }

        /// <summary>
        /// Remove several entity with its identifier.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="ids">List of the identifiers.</param>
        /// <param name="removeGenericAsync">Delegate used to remove the dto.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="bypassFixed">Indicates weither the fixed security should be bypassed or not.</param>
        /// <param name="autoCommit">if set to <c>true</c> [automatic commit].</param>+
        /// <returns>The deleted DTOs.</returns>
        protected virtual async Task<List<TOtherDto>> RemoveGenericAsync<TOtherDto, TOtherMapper>(
            List<TKey> ids,
            RemoveGenericAsyncDelegate<TOtherDto, TOtherMapper> removeGenericAsync,
            string accessMode = AccessMode.Delete,
            string queryMode = QueryMode.Delete,
            string mapperMode = null,
            bool bypassFixed = false,
            bool autoCommit = true)
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new()
        {
            var dtos = new List<TOtherDto>();
            foreach (TKey id in ids)
            {
                dtos.Add(await removeGenericAsync(id, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode, autoCommit: autoCommit));
            }

            return dtos;
        }

        /// <summary>
        /// Save several entity with its identifier safe asynchronous.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of the other dto.</typeparam>
        /// <typeparam name="TOtherMapper">The type of the other mapper.</typeparam>
        /// <param name="dtos">The dtos.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="rightAdd">The right add.</param>
        /// <param name="rightUpdate">The right update.</param>
        /// <param name="rightDelete">The right delete.</param>
        /// <param name="addGenericAsync">Delegate used to add the dto.</param>
        /// <param name="updateGenericAsync">Delegate used to update the dto.</param>
        /// <param name="removeGenericAsync">Delegate used to remove the dto.</param>
        /// <param name="accessMode">The access mode.</param>
        /// <param name="queryMode">The query mode.</param>
        /// <param name="mapperMode">The mapper mode.</param>
        /// <returns>SaveSafeReturn struct.</returns>
        protected virtual async Task<List<TOtherDto>> SaveSafeGenericAsync<TOtherDto, TOtherMapper>(
            IEnumerable<TOtherDto> dtos,
            BiaClaimsPrincipal principal,
            string rightAdd,
            string rightUpdate,
            string rightDelete,
            AddGenericAsyncDelegate<TOtherDto, TOtherMapper> addGenericAsync,
            UpdateGenericAsyncDelegate<TOtherDto, TOtherMapper> updateGenericAsync,
            RemoveGenericAsyncDelegate<TOtherDto, TOtherMapper> removeGenericAsync,
            string accessMode = null,
            string queryMode = null,
            string mapperMode = null)
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new()
        {
            StringBuilder strBldr = new StringBuilder();
            List<Exception> exceptions = new List<Exception>();

            int nbAdded = 0;
            int nbUpdated = 0;
            int nbDeleted = 0;
            int nbError = 0;

            bool canAdd = true;
            bool canUpdate = true;
            bool canDelete = true;
            if (principal != null)
            {
                IEnumerable<string> currentUserPermissions = principal.GetUserPermissions();
                canAdd = rightAdd == null || currentUserPermissions?.Any(x => x == rightAdd) == true;
                canUpdate = rightUpdate == null || currentUserPermissions?.Any(x => x == rightUpdate) == true;
                canDelete = rightUpdate == null || currentUserPermissions?.Any(x => x == rightDelete) == true;
            }

            if (!canAdd && dtos.Any(u => u.DtoState == DtoState.Added))
            {
                strBldr.AppendLine("No permission to add users.");
                nbError++;
            }

            if (!canUpdate && dtos.Any(u => u.DtoState == DtoState.Modified))
            {
                strBldr.AppendLine("No permission to update users.");
                nbError++;
            }

            if (!canDelete && dtos.Any(u => u.DtoState == DtoState.Deleted))
            {
                strBldr.AppendLine("No permission to delete users.");
                nbError++;
            }

            List<TOtherDto> savedDtos = new List<TOtherDto>();
            List<TOtherDto> dtoList = dtos.ToList();
            if (dtoList.Any())
            {
                foreach (TOtherDto dto in dtoList)
                {
                    try
                    {
                        TOtherDto returnDto = null;

                        switch (dto.DtoState)
                        {
                            case DtoState.Added:
                                if (canAdd)
                                {
                                    returnDto = await addGenericAsync(
                                        dto,
                                        mapperMode: mapperMode);
                                    this.Repository.UnitOfWork.Reset();
                                    nbAdded++;
                                }

                                break;

                            case DtoState.Modified:
                                if (canUpdate)
                                {
                                    returnDto = await updateGenericAsync(
                                        dto,
                                        accessMode: accessMode ?? AccessMode.Update,
                                        queryMode: queryMode ?? QueryMode.Update,
                                        mapperMode: mapperMode);
                                    this.Repository.UnitOfWork.Reset();
                                    nbUpdated++;
                                }

                                break;

                            case DtoState.Deleted:
                                if (canDelete)
                                {
                                    returnDto = await removeGenericAsync(
                                        dto.Id,
                                        accessMode: accessMode ?? AccessMode.Delete,
                                        queryMode: queryMode ?? QueryMode.Delete,
                                        mapperMode: mapperMode);
                                    this.Repository.UnitOfWork.Reset();
                                    nbDeleted++;
                                }

                                break;
                        }

                        if (returnDto != null)
                        {
                            savedDtos.Add(returnDto);
                        }
                    }
                    catch (ElementNotFoundException ex)
                    {
                        strBldr.AppendLine("Element " + dto.Id + " not exist or not authorized.");
                        exceptions.Add(ex);
                        this.Repository.UnitOfWork.Reset();
                        nbError++;
                    }
                    catch (FrontUserException ex)
                    {
                        strBldr.AppendLine(string.Format(ex.Message, ex.ErrorMessageParameters));
                        exceptions.Add(ex);
                        this.Repository.UnitOfWork.Reset();
                        nbError++;
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                        this.Repository.UnitOfWork.Reset();
                        nbError++;
                    }
                }
            }

            if (nbError > 0)
            {
                strBldr.Insert(0, $"Added: {nbAdded}, Updated: {nbUpdated}, Deleted: {nbDeleted}, Error{(nbError > 1 ? "s" : null)}: {nbError}{Environment.NewLine}");
                string errorMessage = strBldr.ToString();
                AggregateException aggregateException = new AggregateException(exceptions);
                throw new FrontUserException(errorMessage, aggregateException);
            }

            return savedDtos;
        }

        /// <summary>
        /// Save several entity with its identifier.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="dtos">List of the dtos to save.</param>
        /// <param name="saveGenericAsync">Delegate used to save the dto.</param>
        /// <param name="addGenericAsync">Delegate used to add the dto.</param>
        /// <param name="updateGenericAsync">Delegate used to update the dto.</param>
        /// <param name="removeGenericAsync">Delegate used to remove the dto.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <returns>
        /// The saved DTOs.
        /// </returns>
        protected virtual async Task<IEnumerable<TOtherDto>> SaveGenericAsync<TOtherDto, TOtherMapper>(
            IEnumerable<TOtherDto> dtos,
            SaveGenericAsyncDelegate<TOtherDto, TOtherMapper> saveGenericAsync,
            AddGenericAsyncDelegate<TOtherDto, TOtherMapper> addGenericAsync,
            UpdateGenericAsyncDelegate<TOtherDto, TOtherMapper> updateGenericAsync,
            RemoveGenericAsyncDelegate<TOtherDto, TOtherMapper> removeGenericAsync,
            string accessMode = null,
            string queryMode = null,
            string mapperMode = null)
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new()
        {
            var dtoList = dtos.ToList();
            List<TOtherDto> returnDto = new List<TOtherDto>();
            if (dtoList.Any())
            {
                foreach (var dto in dtoList)
                {
                    returnDto.Add(await saveGenericAsync(dto, addGenericAsync, updateGenericAsync, removeGenericAsync, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode));
                }

                await this.Repository.UnitOfWork.CommitAsync();
                this.Repository.UnitOfWork.Reset();
            }

            return returnDto;
        }

        /// <summary>
        /// Save a DTO in DB regarding to its state.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="dto">The dto to save.</param>
        /// <param name="addGenericAsync">Delegate used to add the dto.</param>
        /// <param name="updateGenericAsync">Delegate used to update the dto.</param>
        /// <param name="removeGenericAsync">Delegate used to remove the dto.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual async Task<TOtherDto> SaveGenericAsync<TOtherDto, TOtherMapper>(
            TOtherDto dto,
            AddGenericAsyncDelegate<TOtherDto, TOtherMapper> addGenericAsync,
            UpdateGenericAsyncDelegate<TOtherDto, TOtherMapper> updateGenericAsync,
            RemoveGenericAsyncDelegate<TOtherDto, TOtherMapper> removeGenericAsync,
            string accessMode = null,
            string queryMode = null,
            string mapperMode = null)
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new()
        {
            TOtherDto returnDto = dto;

            switch (dto.DtoState)
            {
                case DtoState.Added:
                    returnDto = await addGenericAsync(
                        dto,
                        mapperMode: mapperMode,
                        autoCommit: false);
                    break;

                case DtoState.Modified:
                    returnDto = await updateGenericAsync(
                        dto,
                        accessMode: accessMode ?? AccessMode.Update,
                        queryMode: queryMode ?? QueryMode.Update,
                        mapperMode: mapperMode,
                        autoCommit: false);
                    break;

                case DtoState.Deleted:
                    returnDto = await removeGenericAsync(
                        dto.Id,
                        accessMode: accessMode ?? AccessMode.Delete,
                        queryMode: queryMode ?? QueryMode.Delete,
                        mapperMode: mapperMode,
                        autoCommit: false);
                    break;

                default:
                    return returnDto;
            }

            return returnDto;
        }

        /// <summary>
        /// Update the fixed status of an <see cref="IEntityFixable{TKey}"/>.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="id">ID of the entity.</param>
        /// <param name="isFixed">Fixed status.</param>
        /// <param name="getGenericAsync">Delegate used to get the dto.</param>
        /// <returns>Updated DTO.</returns>
        protected virtual async Task<TOtherDto> UpdateFixedGenericAsync<TOtherDto, TOtherMapper>(
            TKey id,
            bool isFixed,
            GetGenericAsyncDelegate<TOtherDto, TOtherMapper> getGenericAsync)
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
        {
            return await this.ExecuteWithFrontUserExceptionHandlingAsync(async () =>
            {
                var entity = await this.Repository.GetEntityAsync(id) ?? throw new ElementNotFoundException();
                this.Repository.UpdateFixedAsync(entity, isFixed);
                await this.ExecuteActionsOnUpdateFixedAsync(id, isFixed);
                await this.Repository.UnitOfWork.CommitAsync();
                return await getGenericAsync(id);
            });
        }

        /// <summary>
        /// Execute actions in <see cref="UpdateFixedGenericAsync{TOtherDto, TOtherMapper}(TKey, bool)"/> before commit changes.
        /// </summary>
        /// <param name="entityUpdatedId">The updated entity ID.</param>
        /// <param name="isFixed">Fixed status.</param>
        /// <returns><see cref="Task"/>.</returns>
        protected virtual Task ExecuteActionsOnUpdateFixedAsync(TKey entityUpdatedId, bool isFixed)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Get the historical of an item by its <paramref name="id"/>.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <typeparam name="TUserEntity">The type of User Entity.</typeparam>
        /// <param name="id">The item ID.</param>
        /// <returns>Collection of <see cref="EntityHistoricalEntryDto>"/>.</returns>
        protected virtual async Task<List<EntityHistoricalEntryDto>> GetHistoricalGenericAsync<TOtherDto, TOtherMapper, TUserEntity>(TKey id)
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TUserEntity : BaseEntityUser
        {
            return await this.ExecuteWithFrontUserExceptionHandlingAsync(async () =>
            {
                var allAudits = await this.Repository.GetAuditsAsync(id);
                var auditsPerSeconds = allAudits.Aggregate(new List<List<IAuditEntity>>(), (groups, audit) =>
                {
                    if (groups.Count == 0)
                    {
                        groups.Add([audit]);
                        return groups;
                    }

                    var lastGroup = groups[^1];
                    var lastItem = lastGroup[^1];

                    var delta = (lastItem.AuditDate - audit.AuditDate).TotalSeconds;
                    if (delta <= 1)
                    {
                        lastGroup.Add(audit);
                    }
                    else
                    {
                        groups.Add([audit]);
                    }

                    return groups;
                });

                var mapper = this.InitMapper<TOtherDto, TOtherMapper>();
                var historical = new List<EntityHistoricalEntryDto>();
                foreach (var audits in auditsPerSeconds)
                {
                    var entry = new EntityHistoricalEntryDto
                    {
                        EntryDateTime = audits[0].AuditDate,
                        EntryUser = audits[0].AuditUserLogin,
                    };

                    // Single audit with no Update Action and no Linked Audit
                    var createOrDeleteAudit = audits.SingleOrDefault(audit => audit.AuditAction != Audit.UpdateAction && (mapper.AuditMapper is null || (!mapper.AuditMapper.LinkedAuditMappers.Any(mapper => mapper.LinkedAuditEntityType == audit.GetType()))));
                    if (createOrDeleteAudit is not null)
                    {
                        entry.EntryType = createOrDeleteAudit.AuditAction == Audit.InsertAction ? EntityHistoricEntryType.Create : EntityHistoricEntryType.Delete;
                    }
                    else
                    {
                        entry.EntryType = EntityHistoricEntryType.Update;
                        this.FillHistoricalEntryModifications<TOtherDto, TOtherMapper>(entry, audits, mapper);
                    }

                    historical.Add(entry);
                }

                await this.SetHistoricalEntriesUserDisplay<TUserEntity>(historical);

                return historical;
            });
        }

        protected virtual async Task SetHistoricalEntriesUserDisplay<TUserEntity>(List<EntityHistoricalEntryDto> entries)
            where TUserEntity : BaseEntityUser
        {
            var historicalUserDisplay = this.BiaNetSection?.CommonFeatures?.AuditConfiguration?.HistoricalUserDisplay;

            Dictionary<string, string> userFullNamesPerLogin = [];
            if (historicalUserDisplay == Audit.HistoricalUserDisplayFullName)
            {
                var auditLogins = entries.Select(entry => entry.EntryUser).Distinct().ToList();
                var auditsUsers = await this.Repository.ServiceProvider.GetRequiredService<ITGenericRepository<TUserEntity, int>>().GetAllResultAsync(x => new { x.FirstName, x.LastName, x.Login }, filter: user => auditLogins.Contains(user.Login), isReadOnlyMode: true);
                userFullNamesPerLogin = auditsUsers.ToDictionary(user => user.Login, user => $"{user.LastName} {user.FirstName}");
            }

            foreach (var entriesGroup in entries.GroupBy(x => x.EntryUser))
            {
                foreach (var entry in entriesGroup)
                {
                    entry.EntryUser = historicalUserDisplay switch
                    {
                        Audit.HistoricalUserDisplayFullName => userFullNamesPerLogin.TryGetValue(entry.EntryUser, out string auditUserFullName) ? auditUserFullName : entry.EntryUser,
                        Audit.HistoricalUserDisplayLogin => entry.EntryUser,
                        _ => string.Empty,
                    };
                }
            }
        }

        /// <summary>
        /// Get the historical user display from audit user login.
        /// </summary>
        /// <param name="auditUserLogin">The audit user login.</param>
        /// <param name="auditUsersFullNamePerLogin">The audits users full name per login.</param>
        /// <returns><see cref="string"/> of the historical user to display.</returns>
        protected virtual string GetHistoricalUserDisplayFromAuditUserLogin(string auditUserLogin, Dictionary<string, string> auditUsersFullNamePerLogin)
        {
            return this.BiaNetSection?.CommonFeatures?.AuditConfiguration?.HistoricalUserDisplay switch
            {
                Audit.HistoricalUserDisplayFullName => auditUsersFullNamePerLogin.TryGetValue(auditUserLogin, out string auditUserFullName) ? auditUserFullName : auditUserLogin,
                Audit.HistoricalUserDisplayLogin => auditUserLogin,
                _ => string.Empty,
            };
        }

        /// <summary>
        /// Get the paging order used in <see cref="GetRangeGenericAsync{TOtherDto, TOtherMapper, TOtherFilterDto}(TOtherFilterDto, TKey, Specification{TEntity}, Expression{Func{TEntity, bool}}, string, string, string, bool)"/>.
        /// </summary>
        /// <param name="collection">The expression collection of entity.</param>
        /// <param name="orderMember">The order member.</param>
        /// <param name="ascending">If set to <c>true</c> [ascending].</param>
        /// <param name="multiSortMeta">multi Sort Meta.</param>
        /// <returns>The paging order.</returns>
        protected virtual QueryOrder<TEntity> GetQueryOrder(ExpressionCollection<TEntity> collection, string orderMember, bool ascending, List<SortMeta> multiSortMeta = null)
        {
            if (multiSortMeta?.Any() == true)
            {
                bool multiSort = false;
                var multiOrder = new QueryOrder<TEntity>();
                foreach (var sortMeta in from sortMeta in multiSortMeta
                                         where !string.IsNullOrWhiteSpace(sortMeta.Field) && collection.ContainsKey(sortMeta.Field)
                                         select sortMeta)
                {
                    multiSort = true;
                    multiOrder.GetByExpression(collection[sortMeta.Field], sortMeta.Order == 1);
                }

                if (multiSort)
                {
                    return multiOrder;
                }
            }

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
            else if (mode == AccessMode.Read)
            {
                return this.GetFilterSpecification(AccessMode.All, filtersContext);
            }
            else if (mode == AccessMode.Update)
            {
                return this.GetFilterSpecification(AccessMode.Read, filtersContext);
            }
            else if (mode == AccessMode.Delete)
            {
                return this.GetFilterSpecification(AccessMode.Update, filtersContext);
            }

            return null;
        }

        /// <summary>
        /// Return specification based on the given <paramref name="filters"/> used in <see cref="GetRangeAsync(TFilterDto, TKey, Specification{TEntity}, Expression{Func{TEntity, bool}}, string, string, string, bool)"/>.
        /// </summary>
        /// <param name="filters">The filter.</param>
        /// <returns>
        /// The specification.
        /// </returns>
        protected virtual Specification<TEntity> GetFilterSpecification(IPagingFilterFormatDto filters)
        {
            return new TrueSpecification<TEntity>();
        }

        /// <summary>
        /// Set the specification used in <see cref="GetRangeAsync(TFilterDto, TKey, Specification{TEntity}, Expression{Func{TEntity, bool}}, string, string, string, bool)"/>.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="filters">The filters.</param>
        protected virtual void SetGetRangeFilterSpecifications(ref Specification<TEntity> specification, IPagingFilterFormatDto filters)
        {
            specification ??= this.GetFilterSpecification(filters);
            if (typeof(IEntityTeam).IsAssignableFrom(typeof(TEntity)) && filters is IPagingFilterFormatDto<TeamAdvancedFilterDto> teamFilters)
            {
                specification &= this.GetTeamAdvancedFilterSpecification(teamFilters);
            }
        }

        /// <summary>
        /// Return specification based on the given team advanced <paramref name="filters"/> used in <see cref="GetRangeAsync(TFilterDto, TKey, Specification{TEntity}, Expression{Func{TEntity, bool}}, string, string, string, bool)"/>.
        /// </summary>
        /// <param name="filters">The filter.</param>
        /// <returns>
        /// The specification.
        /// </returns>
        protected virtual Specification<TEntity> GetTeamAdvancedFilterSpecification(IPagingFilterFormatDto<TeamAdvancedFilterDto> filters)
        {
            Specification<TEntity> specification = new TrueSpecification<TEntity>();
            if (!typeof(IEntityTeam).IsAssignableFrom(typeof(TEntity)))
            {
                return specification;
            }

            if (filters.AdvancedFilter is not null && filters.AdvancedFilter.UserId > 0)
            {
                specification &= new DirectSpecification<TEntity>(entity => ((IEntityTeam)entity).Members.Any(a => a.UserId == filters.AdvancedFilter.UserId));
            }

            return specification;
        }

        /// <summary>
        /// Execute the <paramref name="action"/> and handling <see cref="FrontUserException"/>.
        /// </summary>
        /// <typeparam name="T">Generic return <typeparamref name="T"/> type of the executed <paramref name="action"/>.</typeparam>
        /// <param name="action">The action to execute.</param>
        /// <returns><typeparamref name="T"/>.</returns>
        /// <exception cref="FrontUserException">Throw into <see cref="FrontUserException"/> when raised.</exception>
        protected virtual async Task<T> ExecuteWithFrontUserExceptionHandlingAsync<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch (FrontUserException ex)
            {
                var handledException = this.HandleFrontUserException(ex);
                if (handledException != null)
                {
                    throw handledException;
                }

                return default;
            }
        }

        /// <summary>
        /// Handle the <paramref name="frontUserException"/>.
        /// </summary>
        /// <param name="frontUserException">The <see cref="FrontUserException"/> to handle.</param>
        /// <returns><see cref="Exception"/>.</returns>
        protected virtual Exception HandleFrontUserException(FrontUserException frontUserException)
        {
            return frontUserException.ErrorId switch
            {
                (int)BiaErrorId.DatabaseForeignKeyConstraint => new FrontUserException(frontUserException.ErrorId, frontUserException, typeof(TEntity).Name),
                (int)BiaErrorId.DatabaseUniqueConstraint => new FrontUserException(frontUserException.ErrorId, frontUserException, typeof(TEntity).Name),
                (int)BiaErrorId.DatabaseDuplicateKey => new FrontUserException(frontUserException.ErrorId, frontUserException, typeof(TEntity).Name),
                (int)BiaErrorId.DatabaseNullValueInsert => new FrontUserException(
                    frontUserException.ErrorId,
                    frontUserException,
                    [.. frontUserException.ErrorMessageParameters, typeof(TEntity).Name]),
                (int)BiaErrorId.DatabaseObjectNotFound => new FrontUserException(frontUserException.ErrorId, frontUserException, typeof(TEntity).Name),
                (int)BiaErrorId.DatabaseLoginUser => new FrontUserException(frontUserException.ErrorId, frontUserException, typeof(TEntity).Name),
                (int)BiaErrorId.DatabaseOpen => new FrontUserException(frontUserException.ErrorId, frontUserException, typeof(TEntity).Name),
                _ => frontUserException,
            };
        }

        private void FillHistoricalEntryModifications<TOtherDto, TOtherMapper>(EntityHistoricalEntryDto entry, List<IAuditEntity> audits, TOtherMapper mapper)
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
        {
            var auditMapper = mapper.AuditMapper
                ?? throw new BadBiaFrameworkUsageException($"Missing declaration of {nameof(IAuditMapper)} into {typeof(TOtherMapper)}");

            var userContext = this.Repository.ServiceProvider.GetRequiredService<UserContext>();
            var userCulture = new CultureInfo(userContext.Culture);
            foreach (var audit in audits)
            {
                // Linked Audit Mapper case
                var linkedAuditMapper = auditMapper.LinkedAuditMappers.FirstOrDefault(x => x.LinkedAuditEntityType == audit.GetType());
                if (linkedAuditMapper is not null)
                {
                    var linkedAuditEntityDisplayProperty = audit.GetType().GetProperty(linkedAuditMapper.LinkedAuditEntityDisplayPropertyName) ??
                        throw new BadBiaFrameworkUsageException($"Unable to find display property {linkedAuditMapper.LinkedAuditEntityDisplayPropertyName} into linked audit entity {linkedAuditMapper.LinkedAuditEntityType.Name}");
                    var linkedAuditEntityDisplayPropertyValue = linkedAuditEntityDisplayProperty.PropertyType.IsDateType() ?
                        DateHelper.FormatWithCulture(linkedAuditEntityDisplayProperty, linkedAuditEntityDisplayProperty.GetValue(audit), userCulture) :
                        linkedAuditEntityDisplayProperty.GetValue(audit)?.ToString();

                    var entryModification = new EntityHistoricalEntryModificationDto
                    {
                        IsLinkedProperty = true,
                        PropertyName = linkedAuditMapper.EntityPropertyName,
                    };

                    switch (audit.AuditAction)
                    {
                        case Audit.InsertAction:
                            entryModification.NewValue = linkedAuditEntityDisplayPropertyValue;
                            break;
                        case Audit.DeleteAction:
                            entryModification.OldValue = linkedAuditEntityDisplayPropertyValue;
                            break;
                    }

                    entry.EntryModifications.Add(entryModification);
                    continue;
                }

                var changes = JsonConvert.DeserializeObject<List<AuditChange>>(audit.AuditChanges);

                // Fixable change case
                if (typeof(IEntityFixable).IsAssignableFrom(typeof(TEntity)))
                {
                    var isFixedChange = changes.FirstOrDefault(c => c.ColumnName == nameof(IEntityFixable.IsFixed));
                    if (isFixedChange is not null)
                    {
                        entry.EntryType = bool.Parse(isFixedChange.NewValue.ToString()) ? EntityHistoricEntryType.Fixed : EntityHistoricEntryType.Unfixed;
                        return;
                    }
                }

                // Audit Property Mapper case
                foreach (var auditPropertyMapper in auditMapper.AuditPropertyMappers)
                {
                    var propertyChange = changes.FirstOrDefault(x => x.ColumnName == auditPropertyMapper.EntityPropertyIdentifierName);
                    if (propertyChange is null)
                    {
                        continue;
                    }

                    var changeProperty = typeof(TEntity).GetProperty(auditPropertyMapper.EntityPropertyName);
                    if (changeProperty is null)
                    {
                        continue;
                    }

                    var newValue = changeProperty.PropertyType.IsDateType() ?
                        DateHelper.FormatWithCulture(changeProperty, propertyChange.NewValue, userCulture) :
                        propertyChange.NewDisplay ?? propertyChange.NewValue?.ToString();
                    var originalValue = DateHelper.IsDateType(changeProperty.PropertyType) ?
                        DateHelper.FormatWithCulture(changeProperty, propertyChange.OriginalValue, userCulture) :
                        propertyChange.OriginalDisplay ?? propertyChange.OriginalValue?.ToString();

                    entry.EntryModifications.Add(new EntityHistoricalEntryModificationDto
                    {
                        PropertyName = auditPropertyMapper.EntityPropertyName,
                        NewValue = newValue,
                        OldValue = originalValue,
                    });

                    changes.Remove(propertyChange);
                }

                // General case
                foreach (var change in changes)
                {
                    var changeProperty = typeof(TEntity).GetProperty(change.ColumnName);
                    if (changeProperty is null)
                    {
                        continue;
                    }

                    var newValue = changeProperty.PropertyType.IsDateType() ?
                        DateHelper.FormatWithCulture(changeProperty, change.NewValue, userCulture) :
                        change.NewValue?.ToString();
                    var originalValue = DateHelper.IsDateType(changeProperty.PropertyType) ?
                        DateHelper.FormatWithCulture(changeProperty, change.OriginalValue, userCulture) :
                        change.OriginalValue?.ToString();

                    entry.EntryModifications.Add(new EntityHistoricalEntryModificationDto
                    {
                        PropertyName = change.ColumnName,
                        NewValue = newValue,
                        OldValue = originalValue,
                    });
                }
            }
        }
    }
}