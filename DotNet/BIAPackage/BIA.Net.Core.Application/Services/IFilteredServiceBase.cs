// <copyright file="IFilteredServiceBase.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;

    /// <summary>
    /// IFilteredServiceBase.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IFilteredServiceBase<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
        /// <summary>
        /// Transform the DTO into the corresponding entity and add it to the DB.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="dto">The DTO.</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <returns>The DTO with id updated.</returns>
        Task<TOtherDto> AddAsync<TOtherDto, TOtherMapper>(TOtherDto dto, string mapperMode = null)
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>;

        /// <summary>
        /// Adds the bulk asynchronous.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of the other dto.</typeparam>
        /// <typeparam name="TOtherMapper">The type of the other mapper.</typeparam>
        /// <param name="dtoList">The dto list.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task AddBulkAsync<TOtherDto, TOtherMapper>(IEnumerable<TOtherDto> dtoList)
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>;

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
        /// <param name="isReadOnlyMode">if set to <c>true</c> [is read only mode].</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        Task<IEnumerable<TOtherDto>> GetAllAsync<TOtherDto, TOtherMapper>(TKey id = default, Specification<TEntity> specification = null, Expression<Func<TEntity, bool>> filter = null, QueryOrder<TEntity> queryOrder = null, int firstElement = 0, int pageCount = 0, Expression<Func<TEntity, object>>[] includes = null, string accessMode = "Read", string queryMode = null, string mapperMode = null, bool isReadOnlyMode = false)
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>;

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
        /// <param name="isReadOnlyMode">if set to <c>true</c> [is read only mode].</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        Task<IEnumerable<TOtherDto>> GetAllAsync<TOtherDto, TOtherMapper>(Expression<Func<TEntity, TKey>> orderByExpression, bool ascending, TKey id = default, Specification<TEntity> specification = null, Expression<Func<TEntity, bool>> filter = null, int firstElement = 0, int pageCount = 0, Expression<Func<TEntity, object>>[] includes = null, string accessMode = "Read", string queryMode = null, string mapperMode = null, bool isReadOnlyMode = false)
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>;

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
        /// <param name="isReadOnlyMode">if set to <c>true</c> [is read only mode].</param>
        /// <returns>
        /// The DTO.
        /// </returns>
        Task<TOtherDto> GetAsync<TOtherDto, TOtherMapper>(TKey id = default, Specification<TEntity> specification = null, Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, object>>[] includes = null, string accessMode = "Read", string queryMode = "Read", string mapperMode = "Item", bool isReadOnlyMode = false)
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>;

        /// <summary>
        /// Gets the CSV asynchronous.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of the other dto.</typeparam>
        /// <typeparam name="TOtherMapper">The type of the other mapper.</typeparam>
        /// <typeparam name="TOtherFilterDto">The type of the other filter dto.</typeparam>
        /// <param name="filters">The filters.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="specification">The specification.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="accessMode">The access mode.</param>
        /// <param name="queryMode">The query mode.</param>
        /// <param name="mapperMode">The mapper mode.</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [is read only mode].</param>
        /// <returns>A csv.</returns>
        Task<byte[]> GetCsvAsync<TOtherDto, TOtherMapper, TOtherFilterDto>(TOtherFilterDto filters = null, TKey id = default, Specification<TEntity> specification = null, Expression<Func<TEntity, bool>> filter = null, string accessMode = "Read", string queryMode = "ReadList", string mapperMode = "Csv", bool isReadOnlyMode = false)
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>
            where TOtherFilterDto : LazyLoadDto, new();

        /// <summary>
        /// Get the DTO list with paging and sorting.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <typeparam name="TOtherFilterDto">The type of the other filter dto.</typeparam>
        /// <param name="filters">The filters.</param>
        /// <param name="id">The id.</param>
        /// <param name="specification">Specification Used to filter query.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [is read only mode].</param>
        /// <returns>
        /// The list of DTO.
        /// </returns>
        Task<(IEnumerable<TOtherDto> results, int total)> GetRangeAsync<TOtherDto, TOtherMapper, TOtherFilterDto>(TOtherFilterDto filters = null, TKey id = default, Specification<TEntity> specification = null, Expression<Func<TEntity, bool>> filter = null, string accessMode = "Read", string queryMode = "ReadList", string mapperMode = null, bool isReadOnlyMode = false)
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>
            where TOtherFilterDto : LazyLoadDto, new();

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of the other dto.</typeparam>
        /// <typeparam name="TOtherMapper">The type of the other mapper.</typeparam>
        /// <param name="ids">The ids.</param>
        /// <param name="accessMode">The access mode.</param>
        /// <param name="queryMode">The query mode.</param>
        /// <param name="mapperMode">The mapper mode.</param>
        /// <returns>List of dto removed.</returns>
        Task<List<TOtherDto>> RemoveAsync<TOtherDto, TOtherMapper>(List<TKey> ids, string accessMode = "Delete", string queryMode = "Delete", string mapperMode = null)
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>;

        /// <summary>
        /// Remove an entity with its identifier.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <returns>The deleted DTO.</returns>
        Task<TOtherDto> RemoveAsync<TOtherDto, TOtherMapper>(TKey id, string accessMode = "Delete", string queryMode = "Delete", string mapperMode = null)
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>;

        /// <summary>
        /// Remove quickly hudge number of element. Obsolete in V3.9.0.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="dtoList">The list of element to remove.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
#pragma warning disable S1133 // Deprecated code should be removed
        [Obsolete(message: "RemoveBulkAsync is deprecated, please use a custom repository instead and use the Entity Framework's ExecuteDeleteAsync method (See the example with the EngineRepository in BIADemo).", error: true)]
#pragma warning restore S1133 // Deprecated code should be removed
        Task RemoveBulkAsync<TOtherDto, TOtherMapper>(IEnumerable<TOtherDto> dtoList)
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>;

        /// <summary>
        /// Delete quickly hudge number of element by id. Obsolete in V3.9.0.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="idList">The list of id of element to delete.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
#pragma warning disable S1133 // Deprecated code should be removed
        [Obsolete(message: "RemoveBulkAsync is deprecated, please use a custom repository instead and use the Entity Framework's ExecuteDeleteAsync method (See the example with the EngineRepository in BIADemo).", error: true)]
#pragma warning restore S1133 // Deprecated code should be removed
        Task RemoveBulkAsync(IEnumerable<TKey> idList, string accessMode = "Delete", string queryMode = "Delete");

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
        Task<SaveSafeReturn<TOtherDto>> SaveSafeAsync<TOtherDto, TOtherMapper>(
            IEnumerable<TOtherDto> dtos,
            BiaClaimsPrincipal principal,
            string rightAdd,
            string rightUpdate,
            string rightDelete,
            string accessMode = null,
            string queryMode = null,
            string mapperMode = null)
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new();

        /// <summary>
        /// Save several entity with its identifier.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="dtos">List of the dtos to save.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <returns>The saved DTOs.</returns>
        Task<IEnumerable<TOtherDto>> SaveAsync<TOtherDto, TOtherMapper>(IEnumerable<TOtherDto> dtos, string accessMode = null, string queryMode = null, string mapperMode = null)
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>;

        /// <summary>
        /// Save a DTO in DB regarding to its state.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="dto">The dto to save.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<TOtherDto> SaveAsync<TOtherDto, TOtherMapper>(TOtherDto dto, string accessMode = null, string queryMode = null, string mapperMode = null)
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>;

        /// <summary>
        /// Update an entity in DB with the DTO values.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="dto">The DTO.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <returns>The DTO updated.</returns>
        Task<TOtherDto> UpdateAsync<TOtherDto, TOtherMapper>(TOtherDto dto, string accessMode = "Update", string queryMode = "Update", string mapperMode = null)
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>;

        /// <summary>
        /// Update quickly hudge number of element. Obsolete in V3.9.0.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="dtoList">The list of element to update.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
#pragma warning disable S1133 // Deprecated code should be removed
        [Obsolete(message: "UpdateBulkAsync is deprecated, please use a custom repository instead and use the Entity Framework's ExecuteUpdateAsync method (See the example with the EngineRepository in BIADemo).", error: true)]
#pragma warning restore S1133 // Deprecated code should be removed
        Task UpdateBulkAsync<TOtherDto, TOtherMapper>(IEnumerable<TOtherDto> dtoList)
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherMapper : BaseMapper<TOtherDto, TEntity, TKey>;
    }
}