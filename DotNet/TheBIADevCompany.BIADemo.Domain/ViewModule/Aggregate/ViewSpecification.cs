// <copyright file="ViewSpecification.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.ViewModule.Aggregate
{
    using System.Collections.Generic;
    using System.Linq;
    using BIA.Net.Core.Domain.Specification;

    /// <summary>
    /// The specifications of the view entity.
    /// </summary>
    public static class ViewSpecification
    {
        /// <summary>
        /// Search view using the filter.
        /// </summary>
        /// <param name="siteIds">The list of site id.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The specification.</returns>
        public static Specification<View> SearchGetAll(IEnumerable<int> siteIds, int userId)
        {
            Specification<View> specification = new DirectSpecification<View>(s => s.ViewUsers.Any(a => a.UserId == userId));

            var sites = siteIds.ToList();
            if (sites.Any())
            {
                specification |= new DirectSpecification<View>(s => s.ViewSites.Any(a => sites.Contains(a.SiteId)));
            }

            return specification;
        }
    }
}