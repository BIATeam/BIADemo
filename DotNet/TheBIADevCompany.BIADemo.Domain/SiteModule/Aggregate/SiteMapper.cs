// <copyright file="SiteMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Principal;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Option;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;

    /// <summary>
    /// The mapper used for site.
    /// </summary>
    public class SiteMapper : BaseMapper<SiteDto, Site, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapper"/> class.
        /// </summary>
        /// <param name="principal">The principal.</param>
        public SiteMapper(IPrincipal principal)
        {
            this.UserRoleIds = (principal as BIAClaimsPrincipal).GetRoleIds();
            this.UserId = (principal as BIAClaimsPrincipal).GetUserId();
        }

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
                        "Admins", site =>
                        site.Members.Where(w => w.MemberRoles.Any(a => a.RoleId == (int)RoleId.SiteAdmin)).Select(s => s.User.FirstName + " " + s.User.LastName + " (" + s.User.Login + ")").OrderBy(x => x)
                    },
                };
            }
        }

        /// <summary>
        /// the user id.
        /// </summary>
        private int UserId { get; set; }

        /// <summary>
        /// the user roles.
        /// </summary>
        private IEnumerable<int> UserRoleIds { get; set; }

        /// <summary>
        /// Create a site DTO from a entity.
        /// </summary>
        /// <returns>The site DTO.</returns>
        public override Expression<Func<Site, SiteDto>> EntityToDto()
        {
            return entity => new SiteDto
            {
                Id = entity.Id,
                Title = entity.Title,

                Admins = entity.Members
                    .Where(w => w.MemberRoles.Any(a => a.RoleId == (int)RoleId.SiteAdmin))
                    .Select(s => new OptionDto { Id = s.User.Id, Display = s.User.LastName + " " + s.User.FirstName }),

                // Should correspond to Site_Update permission (but without use the roles *_Member that is not determined at list display)
                CanUpdate =
                    this.UserRoleIds.Contains((int)RoleId.Admin),

                // Should correspond to Site_Member_List_Access (but without use the roles *_Member that is not determined at list display)
                CanMemberListAccess =
                    this.UserRoleIds.Contains((int)RoleId.Admin) ||
                    entity.Members.Any(m => m.UserId == this.UserId),
            };
        }

        /// <summary>
        /// Create a site DTO from a entity.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// The site DTO.
        /// </returns>
        public Expression<Func<Site, SiteDto>> EntityToDto(int userId)
        {
            return entity => new SiteDto { Id = entity.Id, Title = entity.Title, IsDefault = entity.Members.Any(member => member.UserId == userId && member.IsDefault) };
        }

        /// <summary>
        /// Create a site entity from a DTO.
        /// </summary>
        /// <param name="dto">The site DTO.</param>
        /// <param name="entity">The entity to update.</param>
        public override void DtoToEntity(SiteDto dto, Site entity)
        {
            if (entity == null)
            {
                entity = new Site();
            }

            entity.Id = dto.Id;
            entity.Title = dto.Title;
            entity.TeamTypeId = (int)TeamTypeId.Site;
        }
    }
}