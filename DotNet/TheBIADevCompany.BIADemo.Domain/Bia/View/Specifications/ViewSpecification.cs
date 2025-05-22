// <copyright file="ViewSpecification.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Bia.View.Specifications
{
    using System.Collections.Generic;
    using System.Linq;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Domain.Bia.View.Entities;

    /// <summary>
    /// The specifications of the view entity.
    /// </summary>
    public static class ViewSpecification
    {
        /// <summary>
        /// Search view using the filter.
        /// </summary>
        /// <param name="teamIds">The list of team id.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The specification.</returns>
        public static Specification<View> SearchGetAll(IEnumerable<int> teamIds, int userId)
        {
            Specification<View> specification = new DirectSpecification<View>(s => s.ViewUsers.Any(a => a.UserId == userId));

            var teams = teamIds.ToList();
            if (teams.Any())
            {
                specification |= new DirectSpecification<View>(s => s.ViewTeams.Any(a => teams.Contains(a.TeamId)));
            }

            return specification;
        }
    }
}