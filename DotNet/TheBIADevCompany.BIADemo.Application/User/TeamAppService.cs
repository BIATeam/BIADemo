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
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;

    // Begin BIADemo
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompanyModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;

    // End BIADemo
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The application service used for team.
    /// </summary>
    public class TeamAppService : CrudAppServiceBase<TeamDto, Team, int, PagingFilterFormatDto, TeamMapper>, ITeamAppService
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
        /// <param name="userContext">The user context.</param>
        public TeamAppService(ITGenericRepository<Team, int> repository, IPrincipal principal, UserContext userContext)
            : base(repository)
        {
            this.principal = principal as BIAClaimsPrincipal;
            this.userContext = userContext;
        }

        /// <summary>
        /// Return options.
        /// </summary>
        /// <returns>List of OptionDto.</returns>
        /// <param name="teamTypeId">The team type id.</param>
        public Task<IEnumerable<OptionDto>> GetAllOptionsAsync()
        {
            return this.Repository.GetAllResultAsync(selectResult: this.InitMapper<OptionDto, TeamOptionMapper>().EntityToDto());
        }

        /// <inheritdoc cref="ITeamAppService.GetAllAsync"/>
        public async Task<IEnumerable<TeamDto>> GetAllAsync(int userId = 0, IEnumerable<string> userPermissions = null)
        {
            userPermissions ??= this.principal.GetUserPermissions();
            userId = userId > 0 ? userId : this.principal.GetUserId();

            TeamMapper mapper = this.InitMapper();
            if (userPermissions?.Any(x => x == Rights.Teams.AccessAll) == true)
            {
                return await this.Repository.GetAllResultAsync(mapper.EntityToDto(userId));
            }
            else
            {
                return await this.Repository.GetAllResultAsync(
                    mapper.EntityToDto(userId),
                    specification: new DirectSpecification<Team>(team =>
                        team.Members.Any(member =>

                        // Begin BIADemo
                        (team is AircraftMaintenanceCompany && (team as AircraftMaintenanceCompany).MaintenanceTeams.Any(a => a.Members.Any(b => b.UserId == userId))) ||

                        // End BIADemo
                        member.UserId == userId)));
            }
        }
    }
}