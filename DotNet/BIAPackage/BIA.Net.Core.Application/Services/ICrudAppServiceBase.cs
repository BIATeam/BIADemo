// <copyright file="ICrudAppServiceBase.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Base.Interface;
    using BIA.Net.Core.Domain.Dto.Historic;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;

    /// <summary>
    /// The interface defining the CRUD methods.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The primary key type.</typeparam>
    /// <typeparam name="TFilterDto">The filter DTO type.</typeparam>
    public interface ICrudAppServiceBase<TDto, TEntity, TKey, TFilterDto>
        where TDto : BaseDto<TKey>, new()
        where TEntity : class, IEntity<TKey>, new()
        where TFilterDto : class, IPagingFilterFormatDto, new()
    {
        /// <summary>
        /// Get the DTO list with paging and sorting.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <param name="id">The id.</param>
        /// <param name="specification">Specification Used to filter query.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [This improves performance and enables parallel querying]. (optionnal, false by default).</param>
        /// <returns>The list of DTO.</returns>
        Task<(IEnumerable<TDto> Results, int Total)> GetRangeAsync(
            TFilterDto filters = null,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null,
            bool isReadOnlyMode = false);

        /// <summary>
        /// Get the csv with other filter.
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
        Task<byte[]> GetCsvAsync(
            TFilterDto filters = null,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null,
            bool isReadOnlyMode = false);

        /// <summary>
        /// Get the DTO list. (with a queryOrder).
        /// </summary>
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
        /// <param name="isReadOnlyMode">if set to <c>true</c> [This improves performance and enables parallel querying]. (optionnal, false by default).</param>
        /// <returns>The list of DTO.</returns>
        Task<IEnumerable<TDto>> GetAllAsync(
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
            bool isReadOnlyMode = false);

        /// <summary>
        /// Get the DTO list. (with an order By Expression and direction).
        /// </summary>
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
        /// <param name="isReadOnlyMode">if set to <c>true</c> [This improves performance and enables parallel querying]. (optionnal, false by default).</param>
        /// <returns>Data in csv format.</returns>
        Task<IEnumerable<TDto>> GetAllAsync(
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
            bool isReadOnlyMode = false);

        /// <summary>
        /// Return a DTO for a given identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="includes">The list of includes.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [This improves performance and enables parallel querying]. (optionnal, false by default).</param>
        /// <returns>The DTO.</returns>
        Task<TDto> GetAsync(
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>>[] includes = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.Read,
            string mapperMode = MapperMode.Item,
            bool isReadOnlyMode = false);

        /// <summary>
        /// Transform the DTO into the corresponding entity and add it to the DB.
        /// </summary>
        /// <param name="dto">The DTO.</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <returns>The DTO with id updated.</returns>
        Task<TDto> AddAsync(
            TDto dto,
            string mapperMode = null);

        /// <summary>
        /// Update an entity in DB with the DTO values.
        /// </summary>
        /// <param name="dto">The DTO.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <returns>The DTO updated.</returns>
        Task<TDto> UpdateAsync(TDto dto, string accessMode = AccessMode.Update, string queryMode = QueryMode.Update, string mapperMode = null);

        /// <summary>
        /// Remove an entity with its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="bypassFixed">Indicates weither the fixed security should be bypassed or not.</param>
        /// <returns>The deleted DTO.</returns>
        Task<TDto> RemoveAsync(TKey id, string accessMode = AccessMode.Delete, string queryMode = QueryMode.Delete, string mapperMode = null, bool bypassFixed = false);

        /// <summary>
        /// Remove an entity with its identifier.
        /// </summary>
        /// <param name="ids">The List of identifiers.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="bypassFixed">Indicates weither the fixed security should be bypassed or not.</param>
        /// <returns>The deleted DTO.</returns>
        Task<List<TDto>> RemoveAsync(List<TKey> ids, string accessMode = AccessMode.Delete, string queryMode = QueryMode.Delete, string mapperMode = null, bool bypassFixed = false);

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
        /// <param name="accessMode">The access mode.</param>
        /// <param name="queryMode">The query mode.</param>
        /// <param name="mapperMode">The mapper mode.</param>
        /// <returns>SaveSafeReturn struct.</returns>
        Task<List<TDto>> SaveSafeAsync(
            IEnumerable<TDto> dtos,
            BiaClaimsPrincipal principal,
            string rightAdd,
            string rightUpdate,
            string rightDelete,
            string accessMode = null,
            string queryMode = null,
            string mapperMode = null);

        /// <summary>
        /// Save the DTO in DB regarding to theirs state.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <param name="accessMode">The access mode.</param>
        /// <param name="queryMode">The query mode.</param>
        /// <param name="mapperMode">The mapper mode.</param>
        /// <returns>
        /// The saved DTO.
        /// </returns>
        Task<TDto> SaveAsync(
            TDto dto,
            string accessMode = null,
            string queryMode = null,
            string mapperMode = null);

        /// <summary>
        /// Save the list of DTO in DB regarding to theirs state.
        /// </summary>
        /// <param name="dtos">The list of DTO to save.</param>
        /// <param name="accessMode">The access mode.</param>
        /// <param name="queryMode">The query mode.</param>
        /// <param name="mapperMode">The mapper mode.</param>
        /// <returns>The saved list of DTOs.</returns>
        Task<IEnumerable<TDto>> SaveAsync(
            IEnumerable<TDto> dtos,
            string accessMode = null,
            string queryMode = null,
            string mapperMode = null);

        /// <summary>
        /// Transform the DTO into the corresponding entities and add these to the DB.
        /// </summary>
        /// <param name="dtos">The DTO for all items.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
#pragma warning disable S1133 // Deprecated code should be removed
        [Obsolete(message: "AddBulkAsync is deprecated, You can create your own method and call the this.Repository.UnitOfWork.AddBulkAsync method inside it", error: true)]
#pragma warning restore S1133 // Deprecated code should be removed
        Task AddBulkAsync(IEnumerable<TDto> dtos);

        /// <summary>
        /// Update the fixed status of an <see cref="IEntityFixable{TKey}"/>.
        /// </summary>
        /// <param name="id">ID of the entity.</param>
        /// <param name="isFixed">Fixed status.</param>
        /// <returns>Updated DTO.</returns>
        Task<TDto> UpdateFixedAsync(TKey id, bool isFixed);

        /// <summary>
        /// Get the historical of an item by its <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The item ID.</param>
        /// <returns>Collection of <see cref="EntityHistoricalEntryDto>"/>.</returns>
        Task<List<EntityHistoricalEntryDto>> GetHistoricalAsync(TKey id);
    }
}