// <copyright file="TeamAdvancedFilterSpecification.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Specifications
{
    using System.Linq;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Base.Interface;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.Specification;
    using Newtonsoft.Json;

    /// <summary>
    /// The specifications of the site entity.
    /// </summary>
    /// <typeparam name="TTeam">The type of team.</typeparam>
    public static class TeamAdvancedFilterSpecification<TTeam>
        where TTeam : class, IEntity<int>, IEntityTeam
    {
        /// <summary>
        /// Search site using the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// The specification.
        /// </returns>
        public static Specification<TTeam> Filter(IPagingFilterFormatDto<TeamAdvancedFilterDto> filter)
        {
            Specification<TTeam> specification = new TrueSpecification<TTeam>();
            if (filter.AdvancedFilter != null && filter.AdvancedFilter.UserId > 0)
            {
                specification &= new DirectSpecification<TTeam>(s =>
                    s.Members.Any(a => a.UserId == filter.AdvancedFilter.UserId));
            }

            return specification;
        }
    }
}