// BIADemo only
// <copyright file="EngineSpecification.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Plane.Specifications
{
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;

    /// <summary>
    /// The specifications of the member entity.
    /// </summary>
    public static class EngineSpecification
    {
        /// <summary>
        /// Search member using the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The specification.</returns>
        public static Specification<Engine> SearchGetAll(PagingFilterFormatDto filter)
        {
            Specification<Engine> specification = new TrueSpecification<Engine>();

            if (filter.ParentIds != null && filter.ParentIds.Length > 0)
            {
                specification &= new DirectSpecification<Engine>(s =>

                    // BIAToolKit - Begin Parent PlaneId
                    s.PlaneId == int.Parse(filter.ParentIds[0]));

                // BIAToolKit - End Parent PlaneId
            }

            return specification;
        }
    }
}