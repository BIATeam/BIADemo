// <copyright file="SiteInfoMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;

    /// <summary>
    /// The mapper used for site.
    /// </summary>
    public class SiteInfoMapper : BaseMapper<SiteInfoDto, Site, int>
    {
        /// <summary>
        /// Gets or sets the collection used for expressions to access fields.
        /// </summary>
        public override ExpressionCollection<Site> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Site>
                {
                    { "Id", site => site.Id },
                    { "Title", site => site.Title },
                    {
                        "SiteAdmins", site =>
                        site.Members.Where(w => w.MemberRoles.Any(a => a.RoleId == (int)RoleId.SiteAdmin)).Select(s => s.User.FirstName + " " + s.User.LastName + " (" + s.User.Login + ")").OrderBy(x => x)
                    },
                };
            }
        }

        /// <summary>
        /// Create a site DTO from a entity.
        /// </summary>
        /// <returns>The site DTO.</returns>
        public override Expression<Func<Site, SiteInfoDto>> EntityToDto()
        {
            return entity => new SiteInfoDto
            {
                Id = entity.Id,
                Title = entity.Title,
                SiteAdmins = entity.Members
                    .Where(w => w.MemberRoles.Any(a => a.RoleId == (int)RoleId.SiteAdmin))
                    .Select(s => new OptionDto { Id = s.User.Id, Display = s.User.LastName + " " + s.User.FirstName }),
            };
        }
    }
}