// <copyright file="SearchExpressionService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Application.Common
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Principal;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.Bia.User.Entities;
    using TheBIADevCompany.BIADemo.Domain.User.Entities;

    /// <summary>
    /// The search expression service.
    /// </summary>
    public class SearchExpressionService
    {
        /// <summary>
        /// The user id.
        /// </summary>
        private readonly int userId;

        /// <summary>
        /// The user data.
        /// </summary>
        private readonly UserDataDto userDataDto;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchExpressionService"/> class.
        /// The search expression service.
        /// </summary>
        /// <param name="principal">The user principal.</param>
        public SearchExpressionService(IPrincipal principal)
        {
            this.userId = (principal as BiaClaimsPrincipal).GetUserId();
            this.userDataDto = (principal as BiaClaimsPrincipal).GetUserData<UserDataDto>();

            this.Dictionary = new BiaDictionary<SearchExpressionItem>
            {
                 { "[Me]", new SearchExpressionItem() { EntityType = typeof(User), Expression = (Expression<Func<User, bool>>)(t => t.Id == this.userId) } },
                 { "[MyRole]", new SearchExpressionItem() { EntityType = typeof(Role), Expression = (Expression<Func<Role, bool>>)(t => this.userDataDto.CurrentTeams.Any(team => team.CurrentRoleIds.Any(id => t.Id == id))) } },
            };
        }

        /// <summary>
        /// Dictionnary of the expression.
        /// </summary>
        public BiaDictionary<SearchExpressionItem> Dictionary { get; private set; }

        /// <summary>
        /// Search Expression item.
        /// </summary>
        public class SearchExpressionItem
        {
            /// <summary>
            /// Type of entity.
            /// </summary>
            public Type EntityType { get; set; }

            /// <summary>
            /// Expression.
            /// </summary>
            public Expression Expression { get; set; }
        }
    }
}