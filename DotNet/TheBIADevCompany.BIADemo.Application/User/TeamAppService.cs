// <copyright file="TeamAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.User
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The application service used for team.
    /// </summary>
    public class TeamAppService : FilteredServiceBase<Team, int>, ITeamAppService
    {
        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly BIAClaimsPrincipal principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public TeamAppService(ITGenericRepository<Team, int> repository, IPrincipal principal)
            : base(repository)
        {
            this.principal = principal as BIAClaimsPrincipal;
        }

        /// <inheritdoc cref="ITeamAppService.GetAllAsync"/>
        public async Task<IEnumerable<TeamDto>> GetAllAsync(int teamTypeId, int userId = 0, IEnumerable<string> userPermissions = null)
        {
            userPermissions = userPermissions != null ? userPermissions : this.principal.GetUserPermissions();
            userId = userId > 0 ? userId : this.principal.GetUserId();

            TeamMapper mapper = this.InitMapper<TeamDto, TeamMapper>();
            if (userPermissions?.Any(x => x == Rights.Teams.AccessAll) == true)
            {
                return await this.Repository.GetAllResultAsync(mapper.EntityToDto(userId), specification: new DirectSpecification<Team>(team => team.TeamTypeId == teamTypeId));
            }
            else
            {
                return await this.Repository.GetAllResultAsync(mapper.EntityToDto(userId), specification: new DirectSpecification<Team>(team => team.TeamTypeId == teamTypeId && team.Members.Any(member => member.UserId == userId)));
            }
        }
    }
}