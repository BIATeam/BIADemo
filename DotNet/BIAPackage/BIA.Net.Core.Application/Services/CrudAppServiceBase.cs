// <copyright file="CrudAppServiceBase.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Audit.EntityFramework;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Common.Helpers;
    using BIA.Net.Core.Domain.Audit;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Historic;
    using BIA.Net.Core.Domain.Entity;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.QueryOrder;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using Microsoft.Extensions.DependencyInjection;
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
#pragma warning disable S1133 // Deprecated code should be removed
        [Obsolete(message: "AddBulkAsync is deprecated, You can create your own method and call the this.Repository.UnitOfWork.AddBulkAsync method inside it", error: true)]
#pragma warning restore S1133 // Deprecated code should be removed
        public virtual async Task AddBulkAsync(IEnumerable<TDto> dtos)
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
                var auditsPerSeconds = allAudits.Aggregate(new List<List<IAuditEntity>>(), (groups, audit) =>
                {
                    return GroupAuditPerSeconds(groups, audit);
                });

                static List<List<IAuditEntity>> GroupAuditPerSeconds(List<List<IAuditEntity>> groups, IAuditEntity audit)
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

                var mapper = this.InitMapper<TDto, TMapper>();
                var historical = new List<EntityHistoricalEntryDto>();
                foreach (var audits in auditsPerSeconds)
                {
                    var entry = new EntityHistoricalEntryDto
                    {
                        EntryDateTime = audits[0].AuditDate,
                        EntryUserLogin = audits[0].AuditUserLogin,
                    };

                    // Single audit with no Update Action and no Linked Audit
                    var createOrDeleteAudit = audits.SingleOrDefault(audit => audit.AuditAction != Core.Common.BiaConstants.Audit.UpdateAction && (mapper.AuditMapper is null || (!mapper.AuditMapper.LinkedAuditMappers.Any(mapper => mapper.LinkedAuditEntityType == audit.GetType()))));
                    if (createOrDeleteAudit is not null)
                    {
                        entry.EntryType = createOrDeleteAudit.AuditAction == Core.Common.BiaConstants.Audit.InsertAction ? EntityHistoricEntryType.Create : EntityHistoricEntryType.Delete;
                    }
                    else
                    {
                        entry.EntryType = EntityHistoricEntryType.Update;
                        this.FillHistoricalEntryModifications(entry, audits, mapper);
                    }

                    historical.Add(entry);
                }

                return historical;
            });
        }

        private void FillHistoricalEntryModifications(EntityHistoricalEntryDto entry, List<IAuditEntity> audits, TMapper mapper)
        {
            var auditMapper = mapper.AuditMapper
                ?? throw new BadBiaFrameworkUsageException($"Missing declaration of {nameof(IAuditMapper)} into {typeof(TMapper)}");

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
                        case Core.Common.BiaConstants.Audit.InsertAction:
                            entryModification.NewValue = linkedAuditEntityDisplayPropertyValue;
                            break;
                        case Core.Common.BiaConstants.Audit.DeleteAction:
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