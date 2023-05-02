// <copyright file="MemberSpecification.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate
{
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Specification;
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
                    s.SiteId == int.Parse(filter.ParentIds[0]));
            }

            return specification;
        }

        /// <summary>
        /// Search member for login.
        /// </summary>
        /// <param name="userId">The userid.</param>
        /// <returns>The specification.</returns>
        public static Specification<Member> SearchById(int userId)
        {
            Specification<Member> specification = new TrueSpecification<Member>();

            if (userId > 0)
            {
                specification &= new DirectSpecification<Member>(s =>
                    s.User.Id == userId);
            }

            return specification;
        }

        /// <summary>
        /// Searches for login and site.
        /// </summary>
        /// <param name="userId">The userid.</param>
        /// <param name="siteId">The site identifier.</param>
        /// <returns>The specification.</returns>
        public static Specification<Member> SearchByIdAndSite(int userId, int siteId)
        {
            Specification<Member> specification = new TrueSpecification<Member>();

            if (userId > 0)
            {
                specification &= new DirectSpecification<Member>(s =>
                    s.User.Id == userId && s.Site.Id == siteId);
            }

            return specification;
        }
    }
}