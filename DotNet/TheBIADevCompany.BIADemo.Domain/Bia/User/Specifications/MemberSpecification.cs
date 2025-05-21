// <copyright file="MemberSpecification.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Bia.User.Specifications
{
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Domain.Bia.User.Entities;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;

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