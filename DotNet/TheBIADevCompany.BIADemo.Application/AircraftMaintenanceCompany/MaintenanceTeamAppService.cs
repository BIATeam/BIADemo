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
        // BIAToolKit - Begin AncestorTeam AircraftMaintenanceCompany

        /// <summary>
        /// The current Ancestor TeamId.
        /// </summary>
        private readonly int currentAncestorTeamId;

        // BIAToolKit - End AncestorTeam AircraftMaintenanceCompany

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintenanceTeamAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public MaintenanceTeamAppService(ITGenericRepository<MaintenanceTeam, int> repository, IPrincipal principal)
            : base(repository)
        {
            // BIAToolKit - Begin AncestorTeam AircraftMaintenanceCompany
            var userData = (principal as BiaClaimsPrincipal).GetUserData<UserDataDto>();
            this.currentAncestorTeamId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.AircraftMaintenanceCompany) : 0;

            // BIAToolKit - End AncestorTeam AircraftMaintenanceCompany
            this.FiltersContext.Add(
                AccessMode.Read,
                TeamAppService.ReadSpecification<MaintenanceTeam>(TeamTypeId.MaintenanceTeam, principal));

            this.FiltersContext.Add(
                AccessMode.Update,
                TeamAppService.UpdateSpecification<MaintenanceTeam>(TeamTypeId.MaintenanceTeam, principal));
        }

        // BIAToolKit - Begin AncestorTeam AircraftMaintenanceCompany

        /// <inheritdoc/>
        public override Task<MaintenanceTeamDto> AddAsync(MaintenanceTeamDto dto, string mapperMode = null)
        {
            dto.AircraftMaintenanceCompanyId = this.currentAncestorTeamId;
            return base.AddAsync(dto, mapperMode);
        }

        // BIAToolKit - End AncestorTeam AircraftMaintenanceCompany
    }
}