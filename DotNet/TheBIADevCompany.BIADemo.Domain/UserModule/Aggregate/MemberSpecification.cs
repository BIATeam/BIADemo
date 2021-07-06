// <copyright file="MemberSpecification.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate
{
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
        public static Specification<Member> SearchGetAll(MemberFilterDto filter)
        {
            Specification<Member> specification = new TrueSpecification<Member>();

            if (filter.SiteId != 0)
            {
                specification &= new DirectSpecification<Member>(s =>
                    s.SiteId == filter.SiteId);
            }

            return specification;
        }

        /// <summary>
        /// Search member for login.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns>The specification.</returns>
        public static Specification<Member> SearchForSid(string sid)
        {
            Specification<Member> specification = new TrueSpecification<Member>();

            if (!string.IsNullOrWhiteSpace(sid))
            {
                specification &= new DirectSpecification<Member>(s =>
                    s.User.Sid == sid);
            }

            return specification;
        }

        /// <summary>
        /// Searches for login and site.
        /// </summary>
        /// <param name="sid">The sid.</param>
        /// <param name="siteId">The site identifier.</param>
        /// <returns>The specification.</returns>
        public static Specification<Member> SearchForSidAndSite(string sid, int siteId)
        {
            Specification<Member> specification = new TrueSpecification<Member>();

            if (!string.IsNullOrWhiteSpace(sid))
            {
                specification &= new DirectSpecification<Member>(s =>
                    s.User.Sid == sid && s.Site.Id == siteId);
            }

            return specification;
        }
    }
}