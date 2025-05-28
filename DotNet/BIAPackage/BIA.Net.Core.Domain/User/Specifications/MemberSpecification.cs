// <copyright file="MemberSpecification.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.User.Specifications
{
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Specification;
    using BIA.Net.Core.Domain.User.Entities;

    /// <summary>
    /// The specifications of the member entity.
    /// </summary>
    public static class MemberSpecification
    {
        /// <summary>
        /// Search member using the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The specification.</returns>
        public static Specification<Member> SearchGetAll(PagingFilterFormatDto filter)
        {
            Specification<Member> specification = new TrueSpecification<Member>();

            if (filter.ParentIds != null && filter.ParentIds.Length > 0)
            {
                specification &= new DirectSpecification<Member>(s =>
                    s.TeamId == int.Parse(filter.ParentIds[0]));
            }

            return specification;
        }
    }
}