// BIADemo only
// <copyright file="AircraftMaintenanceCompanyOptionAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Maintenance
{
    using System.Security.Principal;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Maintenance.Entities;
    using TheBIADevCompany.BIADemo.Domain.Maintenance.Mappers;

    /// <summary>
    /// The application service used for aircraftMaintenanceCompany option.
    /// </summary>
    public class AircraftMaintenanceCompanyOptionAppService : OptionAppServiceBase<OptionDto, AircraftMaintenanceCompany, int, AircraftMaintenanceCompanyOptionMapper>, IAircraftMaintenanceCompanyOptionAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftMaintenanceCompanyOptionAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The principal.</param>
        public AircraftMaintenanceCompanyOptionAppService(ITGenericRepository<AircraftMaintenanceCompany, int> repository, IPrincipal principal)
            : base(repository)
        {
            this.FiltersContext.Add(
                AccessMode.Read,
                TeamAppService.ReadSpecification<AircraftMaintenanceCompany>(TeamTypeId.AircraftMaintenanceCompany, principal));

            this.FiltersContext.Add(
                AccessMode.Update,
                TeamAppService.UpdateSpecification<AircraftMaintenanceCompany>(TeamTypeId.AircraftMaintenanceCompany, principal));
        }
    }
}
