// <copyright file="OperationalDomainServiceBase.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
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
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto;
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
    /// Base class for a service that need an implementation of a <see cref="DomainServiceBase{TEntity, TKey}"/> with operations.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The primary key of the entity type.</typeparam>
    public abstract class OperationalDomainServiceBase<TEntity, TKey> : DomainServiceBase<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationalDomainServiceBase{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        protected OperationalDomainServiceBase(ITGenericRepository<TEntity, TKey> repository)
            : base(repository)
        {
            this.FiltersContext = new Dictionary<string, Specification<TEntity>>();
        }

        /// <summary>
        /// The filters.
        /// </summary>
        protected Dictionary<string, Specification<TEntity>> FiltersContext { get; set; }

        /// <summary>
        /// CSVs the string.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>A string for a string cell.</returns>
        public static string CSVString(string x)
        {
            return "\"" + x?.Replace("\"", "\"\"") + "\"";
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
        protected virtual async Task<(IEnumerable<TOtherDto> Results, int Total)> GetRangeAsync<TOtherDto, TOtherMapper, TOtherFilterDto>(
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
            where TOtherFilterDto : LazyLoadDto, new()
        {
            return await this.ExecuteWithFrontUserExceptionHandlingAsync(async () =>
            {
                TOtherMapper mapper = this.InitMapper<TOtherDto, TOtherMapper>();

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
        protected async Task<IEnumerable<TOtherDto>> GetAllAsync<TOtherDto, TOtherMapper>(
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
        protected async Task<IEnumerable<TOtherDto>> GetAllAsync<TOtherDto, TOtherMapper>(
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
        /// Get a csv encoding file.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <typeparam name="TOtherFilterDto">Other Filters type.</typeparam>
        /// <param name="filters">Other Filters Query.</param>
        /// <param name="id">The id.</param>
        /// <param name="specification">Specification Used for Filtering Query.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="isReadOnlyMode">Readonly mode to use readOnly context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual async Task<byte[]> GetCsvAsync<TOtherDto, TOtherMapper, TOtherFilterDto>(
            TOtherFilterDto filters = null,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = "Csv",
            bool isReadOnlyMode = false)
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new()
            where TOtherFilterDto : LazyLoadDto, new()
        {
            return await this.ExecuteWithFrontUserExceptionHandlingAsync(async () =>
            {
                var columnHeaderKeys = new List<string>();
                var columnHeaderValues = new List<string>();
                if (filters is PagingFilterFormatDto fileFilters)
                {
                    columnHeaderKeys.AddRange(fileFilters.Columns.Select(x => x.Key));
                    columnHeaderValues.AddRange(fileFilters.Columns.Select(x => x.Value));
                }

                // We reset these parameters, used for paging, in order to recover the totality of the data.
                filters.First = 0;
                filters.Rows = 0;

                IEnumerable<TOtherDto> results = (await this.GetRangeAsync<TOtherDto, TOtherMapper, TOtherFilterDto>(filters: filters, id: id, specification: specification, filter: filter, accessMode: accessMode, queryMode: queryMode, isReadOnlyMode: isReadOnlyMode)).Results;

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

                return Encoding.GetEncoding("iso-8859-15").GetBytes(csvBuilder.ToString());
            });
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
        protected virtual async Task<TOtherDto> GetAsync<TOtherDto, TOtherMapper>(
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
        /// <returns>The DTO with id updated.</returns>
        protected virtual async Task<TOtherDto> AddAsync<TOtherDto, TOtherMapper>(
            TOtherDto dto,
            string mapperMode = null)
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
                    await this.Repository.UnitOfWork.CommitAsync();
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
        /// <returns>The DTO updated.</returns>
        protected virtual async Task<TOtherDto> UpdateAsync<TOtherDto, TOtherMapper>(
            TOtherDto dto,
            string accessMode = AccessMode.Update,
            string queryMode = QueryMode.Update,
            string mapperMode = null)
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

                    await this.Repository.UnitOfWork.CommitAsync();
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
        /// <returns>The deleted DTO.</returns>
        protected virtual async Task<TOtherDto> RemoveAsync<TOtherDto, TOtherMapper>(
            TKey id,
            string accessMode = AccessMode.Delete,
            string queryMode = QueryMode.Delete,
            string mapperMode = null,
            bool bypassFixed = false)
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
                await this.Repository.UnitOfWork.CommitAsync();
                return dto;
            });
        }

        /// <summary>
        /// Remove several entity with its identifier.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="ids">List of the identifiers.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="bypassFixed">Indicates weither the fixed security should be bypassed or not.</param>
        /// <returns>The deleted DTOs.</returns>
        protected virtual async Task<List<TOtherDto>> RemoveAsync<TOtherDto, TOtherMapper>(
            List<TKey> ids,
            string accessMode = AccessMode.Delete,
            string queryMode = QueryMode.Delete,
            string mapperMode = null,
            bool bypassFixed = false)
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new()
        {
            var dtos = new List<TOtherDto>();
            foreach (TKey id in ids)
            {
                dtos.Add(await this.RemoveAsync<TOtherDto, TOtherMapper>(id, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode));
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
        /// <param name="accessMode">The access mode.</param>
        /// <param name="queryMode">The query mode.</param>
        /// <param name="mapperMode">The mapper mode.</param>
        /// <returns>SaveSafeReturn struct.</returns>
        protected virtual async Task<List<TOtherDto>> SaveSafeAsync<TOtherDto, TOtherMapper>(
            IEnumerable<TOtherDto> dtos,
            BiaClaimsPrincipal principal,
            string rightAdd,
            string rightUpdate,
            string rightDelete,
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
                                    returnDto = await this.AddAsync<TOtherDto, TOtherMapper>(
                                        dto,
                                        mapperMode: mapperMode);
                                    this.Repository.UnitOfWork.Reset();
                                    nbAdded++;
                                }

                                break;

                            case DtoState.Modified:
                                if (canUpdate)
                                {
                                    returnDto = await this.UpdateAsync<TOtherDto, TOtherMapper>(
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
                                    returnDto = await this.RemoveAsync<TOtherDto, TOtherMapper>(
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
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <returns>
        /// The saved DTOs.
        /// </returns>
        protected virtual async Task<IEnumerable<TOtherDto>> SaveAsync<TOtherDto, TOtherMapper>(
            IEnumerable<TOtherDto> dtos,
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
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var dto in dtoList)
                    {
                        returnDto.Add(await this.SaveAsync<TOtherDto, TOtherMapper>(dto, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode));
                    }

                    transaction.Complete();
                }
            }

            return returnDto;
        }

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
        protected virtual async Task<TOtherDto> SaveAsync<TOtherDto, TOtherMapper>(
            TOtherDto dto,
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
                    returnDto = await this.AddAsync<TOtherDto, TOtherMapper>(
                        dto,
                        mapperMode: mapperMode);
                    break;

                case DtoState.Modified:
                    returnDto = await this.UpdateAsync<TOtherDto, TOtherMapper>(
                        dto,
                        accessMode: accessMode ?? AccessMode.Update,
                        queryMode: queryMode ?? QueryMode.Update,
                        mapperMode: mapperMode);
                    break;

                case DtoState.Deleted:
                    returnDto = await this.RemoveAsync<TOtherDto, TOtherMapper>(
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

        /// <summary>
        /// Add quickly hudge number of element.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="dtoList">The list of element to add.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual async Task AddBulkAsync<TOtherDto, TOtherMapper>(IEnumerable<TOtherDto> dtoList)
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new()
        {
            if (dtoList != null)
            {
                List<TEntity> entities = dtoList.AsParallel().Select(item =>
                    {
                        var converted = default(TEntity);
                        TOtherMapper mapper = this.InitMapper<TOtherDto, TOtherMapper>();
                        mapper.DtoToEntity(item, ref converted);
                        return converted;
                    }).ToList();

                await this.Repository.UnitOfWork.AddBulkAsync(entities);
            }
        }

        /// <summary>
        /// Update quickly hudge number of element. Obsolete in V3.9.0.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="dtoList">The list of element to update.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual async Task UpdateBulkAsync<TOtherDto, TOtherMapper>(IEnumerable<TOtherDto> dtoList)
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new()
        {
            if (dtoList != null)
            {
                List<TEntity> entities = dtoList.AsParallel().Select(item =>
                {
                    var converted = default(TEntity);
                    TOtherMapper mapper = this.InitMapper<TOtherDto, TOtherMapper>();
                    mapper.DtoToEntity(item, ref converted);
                    return converted;
                }).ToList();

                await this.Repository.UnitOfWork.UpdateBulkAsync(entities);
            }
        }

        /// <summary>
        /// Remove quickly hudge number of element. Obsolete in V3.9.0.
        /// </summary>
        /// <typeparam name="TOtherDto">The type of DTO.</typeparam>
        /// <typeparam name="TOtherMapper">The type of Mapper entity to Dto.</typeparam>
        /// <param name="dtoList">The list of element to remove.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual async Task RemoveBulkAsync<TOtherDto, TOtherMapper>(IEnumerable<TOtherDto> dtoList)
            where TOtherMapper : BiaBaseMapper<TOtherDto, TEntity, TKey>
            where TOtherDto : BaseDto<TKey>, new()
        {
            if (dtoList != null)
            {
                List<TEntity> entities = dtoList.AsParallel().Select(item => new TEntity() { Id = item.Id }).ToList();
                await this.Repository.UnitOfWork.RemoveBulkAsync(entities);
            }
        }

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
        [Obsolete("RemoveBulkAsync is deprecated, please use a custom repository instead and use the Entity Framework's ExecuteDeleteAsync method (See the example with the EngineRepository in BIADemo).")]
#pragma warning restore S1133 // Deprecated code should be removed
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected virtual async Task RemoveBulkAsync(IEnumerable<TKey> idList, string accessMode = AccessMode.Delete, string queryMode = QueryMode.Delete)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the paging order.
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
            return frontUserException.ErrorMessageKey switch
            {
                Common.Enum.FrontUserExceptionErrorMessageKey.DatabaseForeignKeyConstraint => new FrontUserException(frontUserException.ErrorMessageKey, frontUserException, typeof(TEntity).Name),
                Common.Enum.FrontUserExceptionErrorMessageKey.DatabaseUniqueConstraint => new FrontUserException(frontUserException.ErrorMessageKey, frontUserException, typeof(TEntity).Name),
                Common.Enum.FrontUserExceptionErrorMessageKey.DatabaseDuplicateKey => new FrontUserException(frontUserException.ErrorMessageKey, frontUserException, typeof(TEntity).Name),
                Common.Enum.FrontUserExceptionErrorMessageKey.DatabaseNullValueInsert => new FrontUserException(
                    frontUserException.ErrorMessageKey,
                    frontUserException,
                    [.. frontUserException.ErrorMessageParameters, typeof(TEntity).Name]),
                Common.Enum.FrontUserExceptionErrorMessageKey.DatabaseObjectNotFound => new FrontUserException(frontUserException.ErrorMessageKey, frontUserException, typeof(TEntity).Name),
                Common.Enum.FrontUserExceptionErrorMessageKey.DatabaseLoginUser => new FrontUserException(frontUserException.ErrorMessageKey, frontUserException, typeof(TEntity).Name),
                Common.Enum.FrontUserExceptionErrorMessageKey.DatabaseOpen => new FrontUserException(frontUserException.ErrorMessageKey, frontUserException, typeof(TEntity).Name),
                _ => frontUserException,
            };
        }
    }
}