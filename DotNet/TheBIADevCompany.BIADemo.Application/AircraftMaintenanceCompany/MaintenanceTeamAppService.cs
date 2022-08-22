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
        /// The current SiteId.
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
            this.currentAircraftMaintenanceCompanyId = (principal as BIAClaimsPrincipal).GetUserData<UserDataDto>().GetCurrentTeamId((int)TeamTypeId.AircraftMaintenanceCompany);

            IEnumerable<string> currentUserPermissions = (principal as BIAClaimsPrincipal).GetUserPermissions();
            bool accessAll = currentUserPermissions?.Any(x => x == Rights.MaintenanceTeams.ListViewAll) == true;
            int userId = (principal as BIAClaimsPrincipal).GetUserId();

            this.filtersContext.Add(
                AccessMode.Read,
                new DirectSpecification<MaintenanceTeam>(p => p.AircraftMaintenanceCompanyId == this.currentAircraftMaintenanceCompanyId && (accessAll || p.Members.Any(m => m.UserId == userId || p.AircraftMaintenanceCompany.Members.Any(m => m.UserId == userId))))
            );
            this.filtersContext.Add(
                AccessMode.Update,
                new DirectSpecification<MaintenanceTeam>(p => p.AircraftMaintenanceCompanyId == this.currentAircraftMaintenanceCompanyId && p.Id == this.currentAircraftMaintenanceCompanyId)
            );

        }

        /// <inheritdoc/>
        public override Task<MaintenanceTeamDto> AddAsync(MaintenanceTeamDto dto, string mapperMode = null)
        {
            dto.AircraftMaintenanceCompanyId = this.currentAircraftMaintenanceCompanyId;
            return base.AddAsync(dto, mapperMode);
        }
    }
}