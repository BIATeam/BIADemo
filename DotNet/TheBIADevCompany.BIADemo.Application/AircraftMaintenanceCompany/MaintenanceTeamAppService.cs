// BIADemo only
// <copyright file="MaintenanceTeam.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.AircraftMaintenanceCompany
{
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Application.AircraftMaintenanceCompany;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompanyModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;

    /// <summary>
    /// The application service used for AircraftMaintenanceCompany.
    /// </summary>
    public class MaintenanceTeamAppService : CrudAppServiceBase<MaintenanceTeamDto, MaintenanceTeam, int, PagingFilterFormatDto, MaintenanceTeamMapper>, IMaintenanceTeamAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaintenanceTeamAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="MaintenanceTeamQueryCustomizer">The MaintenanceTeam query customizer for include.</param>
        public MaintenanceTeamAppService(ITGenericRepository<MaintenanceTeam, int> repository)
            : base(repository)
        {
        }
    }
}