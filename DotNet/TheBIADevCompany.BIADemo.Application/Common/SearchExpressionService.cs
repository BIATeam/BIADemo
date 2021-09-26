namespace TheBIADevCompany.BIADemo.Application.Common
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Principal;
    using BIA.Net.Core.Common;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

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

        public class SearchExpressionItem
        {
            public Type EntityType { get; set; }
            public Expression Expression { get; set; }
        }

        public BIADictionary<SearchExpressionItem> dictionary { get; private set; }

        public SearchExpressionService(IPrincipal principal)
        {
            this.userId = (principal as BIAClaimsPrincipal).GetUserId();
            this.userDataDto = (principal as BIAClaimsPrincipal).GetUserData<UserDataDto>();

            this.dictionary = new BIADictionary<SearchExpressionItem>
            {
                 { "[Me]", new SearchExpressionItem() { EntityType = typeof(User), Expression = (Expression<Func<User, bool>>)(t => t.Id == this.userId) } },
                 { "[MyRole]", new SearchExpressionItem() { EntityType = typeof(Role), Expression = (Expression<Func<Role, bool>>)(t => this.userDataDto.CurrentRoleIds.Any( id => t.Id == id)) } },
            };
        }
    }

}