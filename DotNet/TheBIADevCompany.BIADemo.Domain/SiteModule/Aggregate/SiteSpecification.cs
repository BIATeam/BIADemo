// <copyright file="SiteSpecification.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate
{
    using System.Linq;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;

    /// <summary>
    /// The specifications of the site entity.
    /// </summary>
    public static class SiteSpecification
    {
        /// <summary>
        /// Search site using the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="siteId">The site identifier.</param>
        /// <returns>
        /// The specification.
        /// </returns>
        public static Specification<Site> SearchGetAll(SiteFilterDto filter, int siteId)
        {
            Specification<Site> specification = new TrueSpecification<Site>();

            if (filter.UserId > 0)
            {
                specification &= new DirectSpecification<Site>(s =>
                    s.Members.Any(a => a.UserId == filter.UserId));
            }

            if (siteId > 0)
            {
                specification &= new DirectSpecification<Site>(s =>
                    s.Members.Any(a => a.SiteId == siteId));
            }

            return specification;
        }
    }
}