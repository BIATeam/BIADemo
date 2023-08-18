// <copyright file="ICrudAppServiceBase.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
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
        where TFilterDto : LazyLoadDto, new()
    {
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
        /// <param name="isReadOnlyMode">Readonly mode to use readOnly context.</param>
        /// <returns>The DTO.</returns>
        Task<TDto> GetAsync(TKey id = default, Specification<TEntity> specification = null, Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, object>>[] includes = null, string accessMode = "Read", string queryMode = "Read", string mapperMode = "Item", bool isReadOnlyMode = false);

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
        /// <param name="isReadOnlyMode">Readonly mode to use readOnly context.</param>
        /// <returns>The list of DTO.</returns>
        Task<IEnumerable<TDto>> GetAllAsync(TKey id = default, Specification<TEntity> specification = null, Expression<Func<TEntity, bool>> filter = null, QueryOrder<TEntity> queryOrder = null, int firstElement = 0, int pageCount = 0, Expression<Func<TEntity, object>>[] includes = null, string accessMode = "Read", string queryMode = null, string mapperMode = null, bool isReadOnlyMode = false);

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
        /// <param name="isReadOnlyMode">Readonly mode to use readOnly context.</param>
        /// <returns>The list of DTO.</returns>
        Task<IEnumerable<TDto>> GetAllAsync(Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, TKey id = default, Specification<TEntity> specification = null, Expression<Func<TEntity, bool>> filter = null, int firstElement = 0, int pageCount = 0, Expression<Func<TEntity, object>>[] includes = null, string accessMode = "Read", string queryMode = null, string mapperMode = null, bool isReadOnlyMode = false);

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
        /// <param name="isReadOnlyMode">Readonly mode to use readOnly context.</param>
        /// <param name="mapper">The mapper to use (will be automatically created if null).</param>
        /// <typeparam name="TOtherDto">The DTO type.</typeparam>
        /// <typeparam name="TOtherMapper">The mapper used between entity and DTO.</typeparam>
        /// <typeparam name="TOtherFilterDto">The filter DTO type.</typeparam>
        /// <returns>The list of DTO.</returns>
        Task<(IEnumerable<TOtherDto> results, int total)> GetRangeAsync<TOtherDto, TOtherMapper, TOtherFilterDto>(TOtherFilterDto filters = null, TKey id = default, Specification<TEntity> specification = null, Expression<Func<TEntity, bool>> filter = null, string accessMode = "Read", string queryMode = "ReadList", string mapperMode = null, bool isReadOnlyMode = false, TOtherMapper mapper = null)
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>, new()
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherFilterDto : LazyLoadDto, new();

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
        /// <param name="isReadOnlyMode">Readonly mode to use readOnly context.</param>
        /// <returns>The list of DTO.</returns>
        Task<(IEnumerable<TDto> results, int total)> GetRangeAsync(TFilterDto filters = null, TKey id = default, Specification<TEntity> specification = null, Expression<Func<TEntity, bool>> filter = null, string accessMode = "Read", string queryMode = "ReadList", string mapperMode = null, bool isReadOnlyMode = false);

        /// <summary>
        /// Transform the DTO into the corresponding entity and add it to the DB.
        /// </summary>
        /// <param name="dto">The DTO.</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <returns>The DTO with id updated.</returns>
        Task<TDto> AddAsync(TDto dto, string mapperMode = null);

        /// <summary>
        /// Save a DTO in DB regarding to its state.
        /// </summary>
        /// <param name="dto">The dto to save.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <returns>The saved DTO.</returns>
        Task<TDto> SaveAsync(TDto dto, string accessMode = null, string queryMode = null, string mapperMode = null);

        /// <summary>
        /// Save several entity with its identifier.
        /// </summary>
        /// <param name="dtos">List of the dtos to save.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <returns>The saved DTOs.</returns>
        Task<IEnumerable<TDto>> SaveAsync(IEnumerable<TDto> dtos, string accessMode = null, string queryMode = null, string mapperMode = null);

        /// <summary>
        /// Update an entity in DB with the DTO values.
        /// </summary>
        /// <param name="dto">The DTO.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <returns>The DTO updated.</returns>
        Task<TDto> UpdateAsync(TDto dto, string accessMode = "Update", string queryMode = "Update", string mapperMode = null);

        /// <summary>
        /// Remove several entity with its identifier.
        /// </summary>
        /// <param name="ids">List of the identifiers.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <returns>The deleted DTOs.</returns>
        Task<List<TDto>> RemoveAsync(List<TKey> ids, string accessMode = "Delete", string queryMode = "Delete", string mapperMode = null);

        /// <summary>
        /// Remove an entity with its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <returns>The deleted DTO.</returns>
        Task<TDto> RemoveAsync(TKey id, string accessMode = "Delete", string queryMode = "Delete", string mapperMode = null);

        /// <summary>
        /// Get a csv encoding file.
        /// </summary>
        /// <param name="filters">Other Filters Query.</param>
        /// <param name="id">The id.</param>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="isReadOnlyMode">Readonly mode to use readOnly context.</param>
        /// <returns>An array of  <see cref="byte"/> representing the csv file.</returns>
        Task<byte[]> GetCsvAsync(TFilterDto filters = null, TKey id = default, Specification<TEntity> specification = null, Expression<Func<TEntity, bool>> filter = null, string accessMode = "Read", string queryMode = "ReadList", string mapperMode = "Csv", bool isReadOnlyMode = false);

        /// <summary>
        /// Add quickly hudge number of element.
        /// </summary>
        /// <param name="dtoList">The list of element to add.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task AddBulkAsync(IEnumerable<TDto> dtoList);

        /// <summary>
        /// Remove quickly hudge number of element.
        /// </summary>
        /// <param name="dtoList">The list of element to remove.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RemoveBulkAsync(IEnumerable<TDto> dtoList);

        /// <summary>
        /// Delete quickly hudge number of element by id.
        /// </summary>
        /// <param name="idList">The list of id of element to delete.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RemoveBulkByIdsAsync(IEnumerable<TKey> idList, string accessMode = AccessMode.Delete, string queryMode = QueryMode.Delete);

        /// <summary>
        /// Update quickly hudge number of element.
        /// </summary>
        /// <param name="dtoList">The list of element to update.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task UpdateBulkAsync(IEnumerable<TDto> dtoList);
    }
}