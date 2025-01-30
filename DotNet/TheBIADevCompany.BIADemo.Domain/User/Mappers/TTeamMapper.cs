// <copyright file="TTeamMapper.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.User.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Principal;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using Microsoft.VisualBasic;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.User;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

    /// <summary>
    /// The mapper used for site.
    /// </summary>
    /// <typeparam name="TTeamDto">Type for the Dto.</typeparam>
    /// <typeparam name="TTeam">Type for the Team.</typeparam>
    public class TTeamMapper<TTeamDto, TTeam> : BaseMapper<TTeamDto, TTeam, int>
        where TTeamDto : TeamDto, new()
        where TTeam : Team, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TTeamMapper{TTeamDto, TTeam}"/> class.
        /// </summary>
        /// <param name="principal">The principal.</param>
        public TTeamMapper(IPrincipal principal)
        {
            this.UserRoleIds = (principal as BiaClaimsPrincipal).GetRoleIds();
            this.UserId = (principal as BiaClaimsPrincipal).GetUserId();
            this.AdminRoleIds = TeamConfig.Config.Where(tc => tc.TeamTypeId == this.TeamType).Select(tc => tc.AdminRoleIds).FirstOrDefault() ?? Array.Empty<int>();
        }

        /// <summary>
        /// Gets or sets the collection used for expressions to access fields.
        /// </summary>
        public override ExpressionCollection<TTeam> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<TTeam>
                {
                    { "Id", site => site.Id },
                    { "Title", site => site.Title },
                    {
                        "Admins", site =>
                        site.Members.Where(w => w.MemberRoles.Any(a => this.AdminRoleIds.Contains(a.RoleId))).Select(s => s.User.LastName + " " + s.User.FirstName + " (" + s.User.Login + ")").OrderBy(x => x)
                    },
                };
            }
        }

        /// <summary>
        /// Precise the Id of the type of team.
        /// </summary>
        public virtual int TeamType
        {
            get { return 0; }
        }

        /// <summary>
        /// the user id.
        /// </summary>
        protected int UserId { get; set; }

        /// <summary>
        /// the admin role Ids.
        /// </summary>
        protected int[] AdminRoleIds { get; }

        /// <summary>
        /// the user roles.
        /// </summary>
        protected IEnumerable<int> UserRoleIds { get; set; }

        /// <summary>
        /// Create a site DTO from a entity.
        /// </summary>
        /// <returns>The site DTO.</returns>
        public override Expression<Func<TTeam, TTeamDto>> EntityToDto()
        {
            return entity => new TTeamDto
            {
                Id = entity.Id,
                Title = entity.Title,
                RowVersion = Convert.ToBase64String(entity.RowVersion),

                Admins = entity.Members
                    .Where(w => w.MemberRoles.Any(a => this.AdminRoleIds.Contains(a.RoleId)))
                    .Select(s => new OptionDto { Id = s.User.Id, Display = s.User.DisplayShort() }),

                // Should correspond to TTeam_Update permission (but without use the roles *_Member that is not determined at list display)
                CanUpdate =
                    this.UserRoleIds.Contains((int)RoleId.Admin),

                // Should correspond to TTeam_Member_List_Access (but without use the roles *_Member that is not determined at list display)
                CanMemberListAccess =
                    this.UserRoleIds.Contains((int)RoleId.Admin) ||
                    entity.Members.Any(m => m.UserId == this.UserId),
            };
        }

        /// <summary>
        /// Create a site entity from a DTO.
        /// </summary>
        /// <param name="dto">The site DTO.</param>
        /// <param name="entity">The entity to update.</param>
        public override void DtoToEntity(TTeamDto dto, TTeam entity)
        {
            if (entity == null)
            {
                entity = new TTeam();
            }

            entity.Id = dto.Id;
            entity.Title = dto.Title;
            entity.TeamTypeId = this.TeamType;
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Func<TTeamDto, object[]> DtoToRecord(List<string> headerNames = null)
        {
            return x => (new object[]
            {
                CSVString(x.Title),
                CSVList(x.Admins?.ToList()),
            });
        }
    }
}