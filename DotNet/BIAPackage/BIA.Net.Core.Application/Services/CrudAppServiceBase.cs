// <copyright file="CrudAppServiceBase.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;
    using Audit.EntityFramework;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Attributes;
    using BIA.Net.Core.Domain.Audit;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Historic;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using Newtonsoft.Json;

    /// <summary>
    /// The base class for all CRUD application service.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TFilterDto">The filter DTO type.</typeparam>
    /// <typeparam name="TMapper">The mapper used between entity and DTO.</typeparam>
    public abstract class CrudAppServiceBase<TDto, TEntity, TKey, TFilterDto, TMapper> : OperationalDomainServiceBase<TEntity, TKey>, ICrudAppServiceBase<TDto, TEntity, TKey, TFilterDto>
        where TDto : BaseDto<TKey>, new()
        where TEntity : class, IEntity<TKey>, new()
        where TFilterDto : LazyLoadDto, new()
        where TMapper : BiaBaseMapper<TDto, TEntity, TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CrudAppServiceBase{TDto, TEntity, TKey, TFilterDto, TMapper}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        protected CrudAppServiceBase(ITGenericRepository<TEntity, TKey> repository)
            : base(repository)
        {
        }

        /// <inheritdoc />
        public virtual async Task<(IEnumerable<TDto> Results, int Total)> GetRangeAsync(
            TFilterDto filters = null,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null,
            bool isReadOnlyMode = false)
        {
            return await this.GetRangeAsync<TDto, TMapper, TFilterDto>(filters: filters, id: id, specification: specification, filter: filter, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode, isReadOnlyMode: isReadOnlyMode);
        }

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
        /// <returns>
        /// The list of DTO.
        /// </returns>
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
            return await this.GetAllAsync<TDto, TMapper>(id: id, specification: specification, filter: filter, queryOrder: queryOrder, firstElement: firstElement, pageCount: pageCount, includes: includes, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode, isReadOnlyMode: isReadOnlyMode);
        }

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
        /// <returns>
        /// Data in csv format.
        /// </returns>
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
            return await this.GetAllAsync<TDto, TMapper>(orderByExpression, ascending, id: id, specification: specification, filter: filter, firstElement: firstElement, pageCount: pageCount, includes: includes, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode, isReadOnlyMode: isReadOnlyMode);
        }

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
        /// <returns>A csv.</returns>
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
            return await this.GetCsvAsync<TDto, TMapper, TFilterDto>(filters: filters, id: id, specification: specification, filter: filter, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode, isReadOnlyMode: isReadOnlyMode);
        }

        /// <summary>
        /// Get the csv with other filter.
        /// </summary>
        /// <typeparam name="TOtherFilter">type of the filters.</typeparam>
        /// <param name="filters">The filters.</param>
        /// <param name="id">The id.</param>
        /// <param name="specification">Specification Used to filter query.</param>
        /// <param name="filter">Filter Query.</param>
        /// <param name="accessMode">The acces Mode (Read, Write delete, all ...). It take the corresponding filter.</param>
        /// <param name="queryMode">The queryMode use to customize query (repository functions CustomizeQueryBefore and CustomizeQueryAfter).</param>
        /// <param name="mapperMode">A string to adapt the mapper function DtoToEntity.</param>
        /// <param name="isReadOnlyMode">if set to <c>true</c> [This improves performance and enables parallel querying]. (optionnal, false by default).</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> representing the asynchronous operation.
        /// </returns>
        public virtual async Task<byte[]> GetCsvAsync<TOtherFilter>(
            TOtherFilter filters,
            TKey id = default,
            Specification<TEntity> specification = null,
            Expression<Func<TEntity, bool>> filter = null,
            string accessMode = AccessMode.Read,
            string queryMode = QueryMode.ReadList,
            string mapperMode = null,
            bool isReadOnlyMode = false)
            where TOtherFilter : LazyLoadDto, new()
        {
            return await this.GetCsvAsync<TDto, TMapper, TOtherFilter>(filters: filters, id: id, specification: specification, filter: filter, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode, isReadOnlyMode: isReadOnlyMode);
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
            return await this.GetAsync<TDto, TMapper>(id: id, specification: specification, filter: filter, includes: includes, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode, isReadOnlyMode: isReadOnlyMode);
        }

        /// <inheritdoc />
        public virtual async Task<TDto> AddAsync(TDto dto, string mapperMode = null)
        {
            return await this.AddAsync<TDto, TMapper>(dto, mapperMode: mapperMode);
        }

        /// <inheritdoc />
        public virtual async Task<TDto> UpdateAsync(
            TDto dto,
            string accessMode = AccessMode.Update,
            string queryMode = QueryMode.Update,
            string mapperMode = null)
        {
            return await this.UpdateAsync<TDto, TMapper>(dto, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode);
        }

        /// <inheritdoc />
        public virtual async Task<TDto> RemoveAsync(
            TKey id,
            string accessMode = AccessMode.Delete,
            string queryMode = QueryMode.Delete,
            string mapperMode = null,
            bool bypassFixed = false)
        {
            return await this.RemoveAsync<TDto, TMapper>(id, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode, bypassFixed: bypassFixed);
        }

        /// <inheritdoc />
        public virtual async Task<List<TDto>> RemoveAsync(
            List<TKey> ids,
            string accessMode = AccessMode.Delete,
            string queryMode = QueryMode.Delete,
            string mapperMode = null,
            bool bypassFixed = false)
        {
            return await this.RemoveAsync<TDto, TMapper>(ids, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode, bypassFixed: bypassFixed);
        }

        /// <inheritdoc />
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task AddBulkAsync(IEnumerable<TDto> dtos)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            await this.AddBulkAsync<TDto, TMapper>(dtos);
        }

        /// <inheritdoc />  Obsolete in V3.9.0.
#pragma warning disable S1133 // Deprecated code should be removed
        [Obsolete(message: "UpdateBulkAsync is deprecated, please use a custom repository instead and use the Entity Framework's ExecuteUpdateAsync method (See the example with the EngineRepository in BIADemo).", error: true)]
#pragma warning restore S1133 // Deprecated code should be removed
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task UpdateBulkAsync(IEnumerable<TDto> dtos)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />  Obsolete in V3.9.0.
#pragma warning disable S1133 // Deprecated code should be removed
        [Obsolete(message: "RemoveBulkAsync is deprecated, please use a custom repository instead and use the Entity Framework's ExecuteDeleteAsync method (See the example with the EngineRepository in BIADemo).", error: true)]
#pragma warning restore S1133 // Deprecated code should be removed
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task RemoveBulkAsync(IEnumerable<TDto> dtos)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            throw new NotImplementedException();
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
            return await this.SaveSafeAsync<TDto, TMapper>(
                dtos: dtos,
                principal: principal,
                rightAdd: rightAdd,
                rightUpdate: rightUpdate,
                rightDelete: rightDelete,
                accessMode: accessMode,
                queryMode: queryMode,
                mapperMode: mapperMode);
        }

        /// <summary>
        /// Save the DTO in DB regarding to theirs state.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <param name="accessMode">The access mode.</param>
        /// <param name="queryMode">The query mode.</param>
        /// <param name="mapperMode">The mappar mode.</param>
        /// <returns>
        /// The saved DTO.
        /// </returns>
        public virtual async Task<TDto> SaveAsync(
            TDto dto,
            string accessMode = null,
            string queryMode = null,
            string mapperMode = null)
        {
            return await this.SaveAsync<TDto, TMapper>(dto, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode);
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TDto>> SaveAsync(
            IEnumerable<TDto> dtos,
            string accessMode = null,
            string queryMode = null,
            string mapperMode = null)
        {
            return await this.SaveAsync<TDto, TMapper>(dtos, accessMode: accessMode, queryMode: queryMode, mapperMode: mapperMode);
        }

        /// <inheritdoc/>
        public virtual async Task<TDto> UpdateFixedAsync(TKey id, bool isFixed)
        {
            return await this.ExecuteWithFrontUserExceptionHandlingAsync(async () =>
            {
                var entity = await this.Repository.GetEntityAsync(id) ?? throw new ElementNotFoundException();
                this.Repository.UpdateFixedAsync(entity, isFixed);
                await this.Repository.UnitOfWork.CommitAsync();
                return await this.GetAsync(id);
            });
        }

        /// <inheritdoc/>
        public virtual async Task<List<EntityHistoricalEntryDto>> GetHistoricalAsync(TKey id)
        {
            return await this.ExecuteWithFrontUserExceptionHandlingAsync(async () =>
            {
                var allAudits = await this.Repository.GetAuditsAsync(id);
                var auditsPerSeconds = allAudits.Aggregate(new List<List<AuditEntity>>(), (groups, audit) =>
                {
                    return GroupAuditPerSeconds(groups, audit);
                });

                var historical = new List<EntityHistoricalEntryDto>();
                foreach (var audits in auditsPerSeconds)
                {
                    var entry = new EntityHistoricalEntryDto
                    {
                        EntryDateTime = audits[0].AuditDate,
                        EntryUserLogin = audits[0].AuditUserLogin,
                    };

                    if (audits.Any(audit => audit.AuditAction == Core.Common.BiaConstants.Audit.InsertAction && !audit.GetType().GetCustomAttributes<AuditLinkedEntityAttribute>().Any(a => a.LinkedEntityType == typeof(TEntity))))
                    {
                        entry.EntryType = EntityHistoricEntryType.Create;
                    }
                    else if (audits.Any(audit => audit.AuditAction == Core.Common.BiaConstants.Audit.DeleteAction && !audit.GetType().GetCustomAttributes<AuditLinkedEntityAttribute>().Any(a => a.LinkedEntityType == typeof(TEntity))))
                    {
                        entry.EntryType = EntityHistoricEntryType.Delete;
                    }
                    else
                    {
                        entry.EntryType = EntityHistoricEntryType.Update;
                        FillHistoricalEntryModifications(entry, audits);
                    }

                    historical.Add(entry);
                }

                return historical;
            });
        }

        private static List<List<AuditEntity>> GroupAuditPerSeconds(List<List<AuditEntity>> groups, AuditEntity audit)
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
        }

        private static void FillHistoricalEntryModifications(EntityHistoricalEntryDto entry, List<AuditEntity> audits)
        {
            foreach (var audit in audits)
            {
                var auditLinkedEntityAttribute = audit.GetType().GetCustomAttributes<AuditLinkedEntityAttribute>().FirstOrDefault(a => a.LinkedEntityType == typeof(TEntity));
                if (auditLinkedEntityAttribute != null)
                {
                    FillHistoricalEntryModificationLinkedEntity(entry, audit, auditLinkedEntityAttribute);
                    continue;
                }

                var linkedEntityProperties = audit
                    .GetType()
                    .GetProperties()
                    .Where(p => p.GetCustomAttribute<AuditLinkedEntityPropertyAttribute>() is not null)
                    .ToList();
                var changes = JsonConvert.DeserializeObject<List<AuditChange>>(audit.AuditChanges);
                foreach (var change in changes)
                {
                    var linkedEntityProperty = linkedEntityProperties.FirstOrDefault(x => x.GetCustomAttribute<AuditLinkedEntityPropertyAttribute>().EntityReferencePropertyIdentifier.Equals(change.ColumnName));
                    if (linkedEntityProperty is null)
                    {
                        entry.EntryModifications.Add(new EntityHistoricalEntryModificationDto
                        {
                            PropertyName = change.ColumnName,
                            NewValue = change.NewDisplay ?? change.NewValue?.ToString(),
                            OldValue = change.OriginalDisplay ?? change.OriginalValue?.ToString(),
                        });
                        continue;
                    }

                    var linkedEntityPropertyAttribute = linkedEntityProperty.GetCustomAttribute<AuditLinkedEntityPropertyAttribute>();
                    entry.EntryModifications.Add(new EntityHistoricalEntryModificationDto
                    {
                        PropertyName = linkedEntityPropertyAttribute.EntityPropertyName,
                        NewValue = change.NewDisplay ?? change.NewValue?.ToString(),
                        OldValue = change.OriginalDisplay ?? change.OriginalValue?.ToString(),
                    });
                }
            }
        }

        private static void FillHistoricalEntryModificationLinkedEntity(EntityHistoricalEntryDto entry, AuditEntity audit, AuditLinkedEntityAttribute auditLinkedEntityAttribute)
        {
            var auditLinkedEntityPropertyDisplayValue = audit
                                    .GetType()
                                    .GetProperties()
                                    .FirstOrDefault(p => p.GetCustomAttributes<AuditLinkedEntityPropertyDisplayAttribute>().Any(a => a.LinkedEntityType == typeof(TEntity)))?
                                    .GetValue(audit, null);
            var linkedEntityDisplayValue = auditLinkedEntityPropertyDisplayValue?.ToString();
            var linkedEntityPropertyName = auditLinkedEntityAttribute?.LinkedEntityPropertyName;

            switch (audit.AuditAction)
            {
                case Core.Common.BiaConstants.Audit.InsertAction:
                    entry.EntryModifications.Add(new EntityHistoricalEntryModificationDto
                    {
                        IsLinkedProperty = true,
                        PropertyName = linkedEntityPropertyName,
                        NewValue = linkedEntityDisplayValue,
                    });
                    break;
                case Core.Common.BiaConstants.Audit.DeleteAction:
                    entry.EntryModifications.Add(new EntityHistoricalEntryModificationDto
                    {
                        IsLinkedProperty = true,
                        PropertyName = linkedEntityPropertyName,
                        OldValue = linkedEntityDisplayValue,
                    });
                    break;
            }
        }
    }
}