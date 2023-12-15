// BIADemo only
// <copyright file="MaintenanceTeamAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.AircraftMaintenanceCompany
{
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompanyModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;

    /// <summary>
    /// The application service used for AircraftMaintenanceCompany.
    /// </summary>
    public class MaintenanceTeamAppService : CrudAppServiceBase<MaintenanceTeamDto, MaintenanceTeam, int, PagingFilterFormatDto, MaintenanceTeamMapper>, IMaintenanceTeamAppService
    {
        /* Begin Parent AircraftMaintenanceCompany */

        /// <summary>
        /// The current Aircraft Maintenance Company Id.
        /// </summary>
        private readonly int currentAircraftMaintenanceCompanyId;
        /* End Parent AircraftMaintenanceCompany */

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintenanceTeamAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public MaintenanceTeamAppService(ITGenericRepository<MaintenanceTeam, int> repository, IPrincipal principal)
            : base(repository)
        {
            /* Begin Parent AircraftMaintenanceCompany */
            var userData = (principal as BIAClaimsPrincipal).GetUserData<UserDataDto>();
            this.currentAircraftMaintenanceCompanyId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.AircraftMaintenanceCompany) : 0;
            /* End Parent AircraftMaintenanceCompany */

            this.FiltersContext.Add(
                AccessMode.Read,
                TeamAppService.ReadSpecification<MaintenanceTeam>(TeamTypeId.MaintenanceTeam, principal));

            // In teams the right in jwt depends on current teams. So you should ensure that you are working on current team.
            this.FiltersContext.Add(
                AccessMode.Update,
                TeamAppService.UpdateSpecification<MaintenanceTeam>(TeamTypeId.MaintenanceTeam, principal));
        }

        /* Begin Parent AircraftMaintenanceCompany */

        /// <inheritdoc/>
        public override Task<MaintenanceTeamDto> AddAsync(MaintenanceTeamDto dto, string mapperMode = null)
        {
            dto.AircraftMaintenanceCompanyId = this.currentAircraftMaintenanceCompanyId;
            return base.AddAsync(dto, mapperMode);
        }

        /* End Parent AircraftMaintenanceCompany */
    }
}