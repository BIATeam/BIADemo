// <copyright file="TeamOperationalDomainServiceBase.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Service
{
    using System.Linq;
    using BIA.Net.Core.Domain.Dto.Base.Interface;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Specification;

    /// <summary>
    /// Extends <see cref="OperationalDomainServiceBase{TDto, TDtoListItem, TEntity, TKey, TFilterDto, TMapper, TMapperListItem}"/>
    /// with team-specific filtering based on <see cref="TeamAdvancedFilterDto"/>.
    /// Use this base class for services whose entity implements <see cref="IEntityTeam"/>.
    /// </summary>
    /// <typeparam name="TDto">DTO type for item.</typeparam>
    /// <typeparam name="TDtoListItem">DTO type for list items.</typeparam>
    /// <typeparam name="TEntity">The entity type, must implement <see cref="IEntityTeam"/>.</typeparam>
    /// <typeparam name="TKey">The primary key of the entity type.</typeparam>
    /// <typeparam name="TFilterDto">The filter DTO type used for paging/filtering.</typeparam>
    /// <typeparam name="TMapper">The mapper type from DTO to entity.</typeparam>
    /// <typeparam name="TMapperListItem">The mapper type from DTO for list item to entity.</typeparam>
    public abstract class TeamOperationalDomainServiceBase<TDto, TDtoListItem, TEntity, TKey, TFilterDto, TMapper, TMapperListItem>
        : OperationalDomainServiceBase<TDto, TDtoListItem, TEntity, TKey, TFilterDto, TMapper, TMapperListItem>
        where TDto : BaseDto<TKey>, new()
        where TDtoListItem : BaseDto<TKey>, new()
        where TEntity : class, IEntity<TKey>, IEntityTeam, new()
        where TFilterDto : class, IPagingFilterFormatDto, new()
        where TMapper : BiaBaseMapper<TDto, TEntity, TKey>
        where TMapperListItem : BiaBaseMapper<TDtoListItem, TEntity, TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamOperationalDomainServiceBase{TDto, TDtoListItem, TEntity, TKey, TFilterDto, TMapper, TMapperListItem}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        protected TeamOperationalDomainServiceBase(ITGenericRepository<TEntity, TKey> repository)
            : base(repository)
        {
        }

        /// <inheritdoc />
        protected override void SetGetRangeFilterSpecifications(ref Specification<TEntity> specification, IPagingFilterFormatDto filters)
        {
            base.SetGetRangeFilterSpecifications(ref specification, filters);

            if (filters is IPagingFilterFormatDto<TeamAdvancedFilterDto> teamFilters)
            {
                specification &= this.GetTeamAdvancedFilterSpecification(teamFilters);
            }
        }

        /// <summary>
        /// Returns a specification based on the team advanced filter (e.g. filter by member user ID).
        /// </summary>
        /// <param name="filters">The team-aware filter.</param>
        /// <returns>The specification.</returns>
        protected virtual Specification<TEntity> GetTeamAdvancedFilterSpecification(IPagingFilterFormatDto<TeamAdvancedFilterDto> filters)
        {
            Specification<TEntity> specification = new TrueSpecification<TEntity>();

            if (filters.AdvancedFilter is not null && filters.AdvancedFilter.UserId > 0)
            {
                specification &= new DirectSpecification<TEntity>(entity => entity.Members.Any(a => a.UserId == filters.AdvancedFilter.UserId));
            }

            return specification;
        }
    }
}
