// <copyright file="TeamCrudAppServiceBase.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Services
{
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Base.Interface;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;

    /// <summary>
    /// Base CRUD application service for entities that implement <see cref="IEntityTeam"/>.
    /// Extends <see cref="TeamOperationalDomainServiceBase{TDto, TDtoListItem, TEntity, TKey, TFilterDto, TMapper, TMapperListItem}"/>
    /// and implements <see cref="ICrudAppServiceBase{TDto, TEntity, TKey, TFilterDto}"/>.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type, must implement <see cref="IEntityTeam"/>.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TFilterDto">The filter DTO type.</typeparam>
    /// <typeparam name="TMapper">The mapper used between entity and DTO.</typeparam>
    public abstract class TeamCrudAppServiceBase<TDto, TEntity, TKey, TFilterDto, TMapper>
        : TeamOperationalDomainServiceBase<TDto, TDto, TEntity, TKey, TFilterDto, TMapper, TMapper>,
          ICrudAppServiceBase<TDto, TEntity, TKey, TFilterDto>
        where TDto : BaseDto<TKey>, new()
        where TEntity : class, IEntity<TKey>, IEntityTeam, new()
        where TFilterDto : class, IPagingFilterFormatDto, new()
        where TMapper : BiaBaseMapper<TDto, TEntity, TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamCrudAppServiceBase{TDto, TEntity, TKey, TFilterDto, TMapper}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        protected TeamCrudAppServiceBase(ITGenericRepository<TEntity, TKey> repository)
            : base(repository)
        {
        }
    }
}
