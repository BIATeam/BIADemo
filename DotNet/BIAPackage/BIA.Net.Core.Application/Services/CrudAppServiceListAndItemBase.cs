// <copyright file="CrudAppServiceListAndItemBase.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
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
    /// The base class for all CRUD application service.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TDtoListItem">The DTO of item in list type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TFilterDto">The filter DTO type.</typeparam>
    /// <typeparam name="TMapper">The mapper used between entity and DTO.</typeparam>
    /// <typeparam name="TMapperListItem">The mapper used between entity and DTO for item in list.</typeparam>
    public abstract class CrudAppServiceListAndItemBase<TDto, TDtoListItem, TEntity, TKey, TFilterDto, TMapper, TMapperListItem> : OperationalDomainServiceBase<TDto, TDtoListItem, TEntity, TKey, TFilterDto, TMapper, TMapperListItem>
        where TDto : BaseDto<TKey>, new()
        where TDtoListItem : BaseDto<TKey>, new()
        where TEntity : class, IEntity<TKey>, new()
        where TFilterDto : class, IPagingFilterFormatDto, new()
        where TMapper : BiaBaseMapper<TDto, TEntity, TKey>
        where TMapperListItem : BiaBaseMapper<TDtoListItem, TEntity, TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CrudAppServiceListAndItemBase{TDto, TDtoListItem, TEntity, TKey, TFilterDto, TMapper, TMapperListItem}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        protected CrudAppServiceListAndItemBase(ITGenericRepository<TEntity, TKey> repository)
            : base(repository)
        {
        }
    }
}