// BIADemo only
// <copyright file="MaintenanceTeamAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.AircraftMaintenanceCompany
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompanyModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;

    /// <summary>
    /// The application service used for AircraftMaintenanceCompany.
    /// </summary>
    public class MaintenanceTeamAppService : CrudAppServiceBase<MaintenanceTeamDto, MaintenanceTeam, int, PagingFilterFormatDto, MaintenanceTeamMapper>, IMaintenanceTeamAppService
    {
        /// <summary>
        /// The current Aircraft Maintenance Company Id.
        /// </summary>
        private readonly int currentAircraftMaintenanceCompanyId;


        /// <summary>
        /// Initializes a new instance of the <see cref="MaintenanceTeamAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public MaintenanceTeamAppService(ITGenericRepository<MaintenanceTeam, int> repository, IPrincipal principal)
            : base(repository)
        {
            var userData = (principal as BIAClaimsPrincipal).GetUserData<UserDataDto>();
            this.currentAircraftMaintenanceCompanyId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.AircraftMaintenanceCompany) : 0;
            var currentMaintenanceTeamyId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.MaintenanceTeam) : 0;

            IEnumerable<string> currentUserPermissions = (principal as BIAClaimsPrincipal).GetUserPermissions();
            bool accessAll = currentUserPermissions?.Any(x => x == Rights.MaintenanceTeams.ListViewAll) == true;
            int userId = (principal as BIAClaimsPrincipal).GetUserId();

            // You can see every team if your are member
            // For MaintenanceTeam we add
            //          - filter on current AircraftMaintenanceCompany to see only MaintenanceTeam of the current AircraftMaintenanceCompany
            //          - right for privilate acces (ListViewAll) = Admin and Supervisor of the Parent team (AircraftMaintenanceCompany)
            //          - right for member of the current AircraftMaintenanceCompany
            this.filtersContext.Add(
                AccessMode.Read,
                new DirectSpecification<MaintenanceTeam>(p => p.AircraftMaintenanceCompanyId == this.currentAircraftMaintenanceCompanyId && (accessAll || p.Members.Any(m => m.UserId == userId || p.AircraftMaintenanceCompany.Members.Any(m => m.UserId == userId)))));

            // In teams the right in jwt depends on current teams. So you should ensure that you are working on current team.
            this.filtersContext.Add(
                AccessMode.Update,
                new DirectSpecification<MaintenanceTeam>(p => p.Id == currentMaintenanceTeamyId));
        }

        /// <inheritdoc/>
        public override Task<MaintenanceTeamDto> AddAsync(MaintenanceTeamDto dto, string mapperMode = null)
        {
            dto.AircraftMaintenanceCompanyId = this.currentAircraftMaintenanceCompanyId;
            return base.AddAsync(dto, mapperMode);
        }
    }
}