// <copyright file="BaseTeamMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Bia.Base.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Formats.Asn1;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Principal;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Base.Interface;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Entity.Interface;
    using BIA.Net.Core.Domain.Mapper;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Bia.User;
    using TheBIADevCompany.BIADemo.Domain.User;

    /// <summary>
    /// The mapper used for site.
    /// </summary>
    /// <typeparam name="TTeamDto">Type for the Dto.</typeparam>
    /// <typeparam name="TTeam">Type for the Team.</typeparam>
    public class BaseTeamMapper<TTeamDto, TTeam> : BasePrincipalMapper<TTeamDto, TTeam, int>
        where TTeamDto : BaseDtoVersionedTeam, new()
        where TTeam : class, IEntity<int>, IEntityTeam, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTeamMapper{TTeamDto, TTeam}"/> class.
        /// </summary>
        /// <param name="principal">The principal.</param>
        public BaseTeamMapper(IPrincipal principal)
            : base(principal)
        {
            Debug.Assert(typeof(IEntityTeam).IsAssignableFrom(typeof(TTeam)), typeof(TTeam) + " should imùplement IEntityTeam");
            Debug.Assert(typeof(IDtoTeam).IsAssignableFrom(typeof(TTeamDto)), typeof(TTeamDto) + " should imùplement IDtoTeam");

            this.UserRoleIds = (principal as BiaClaimsPrincipal).GetRoleIds();
            this.AdminRoleIds = TeamConfig.Config.Where(tc => tc.TeamTypeId == this.TeamType).Select(tc => tc.AdminRoleIds).FirstOrDefault() ?? [];
        }

        /// <summary>
        /// Gets or sets the collection used for expressions to access fields.
        /// </summary>
        public override ExpressionCollection<TTeam> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<TTeam>(base.ExpressionCollection)
                {
                    { TeamHeaderName.Title, team => team.Title },
                    {
                        TeamHeaderName.Admins, team =>
                        team.Members.Where(w => w.MemberRoles.Any(a => this.AdminRoleIds.Contains(a.RoleId))).Select(s => s.User.LastName + " " + s.User.FirstName + " (" + s.User.Login + ")").OrderBy(x => x)
                    },
                };
            }
        }

        /// <summary>
        /// Precise the Id of the type of team.
        /// </summary>
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
        public virtual int TeamType => throw new NotImplementedException("Implementation of TeamType is missing.");
#pragma warning restore CA1065 // Do not raise exceptions in unexpected locations

        /// <summary>
        /// the admin role Ids.
        /// </summary>
        protected int[] AdminRoleIds { get; }

        /// <summary>
        /// the user roles.
        /// </summary>
        protected IEnumerable<int> UserRoleIds { get; set; }

        /// <summary>
        /// Create a site entity from a DTO.
        /// </summary>
        /// <param name="dto">The site DTO.</param>
        /// <param name="entity">The entity to update.</param>
        public override void DtoToEntity(TTeamDto dto, ref TTeam entity)
        {
            base.DtoToEntity(dto, ref entity);

            entity.Title = dto.Title;
            entity.TeamTypeId = this.TeamType;
        }

        /// <summary>
        /// Create a site DTO from a entity.
        /// </summary>
        /// <returns>The site DTO.</returns>
        public override Expression<Func<TTeam, TTeamDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new TTeamDto
            {
                Title = entity.Title,
                TeamTypeId = this.TeamType,

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
            });
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToCellMapping"/>
        public override Dictionary<string, Func<string>> DtoToCellMapping(TTeamDto dto)
        {
            return new Dictionary<string, Func<string>>(base.DtoToCellMapping(dto))
            {
                { TeamHeaderName.Title, () => CSVString(dto.Title) },
                { TeamHeaderName.Admins, () => CSVList(dto.Admins?.ToList()) },
            };
        }

        /// <summary>
        /// Header names.
        /// </summary>
        public struct TeamHeaderName
        {
            /// <summary>
            /// Header name for site id.
            /// </summary>
            public const string Title = "title";

            /// <summary>
            /// Header name for msn.
            /// </summary>
            public const string Admins = "admins";
        }
    }
}