// BIADemo only
// <copyright file="MaintenanceContractAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.AircraftMaintenanceCompany
{
    using System.Security.Principal;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Entities;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Mappers;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;

    /// <summary>
    /// The application service used for maintenanceContract.
    /// </summary>
    public class MaintenanceContractAppService : CrudAppServiceBase<MaintenanceContractDto, MaintenanceContract, int, PagingFilterFormatDto, MaintenanceContractMapper>, IMaintenanceContractAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaintenanceContractAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public MaintenanceContractAppService(ITGenericRepository<MaintenanceContract, int> repository, IPrincipal principal)
            : base(repository)
        {
        }
    }
}
