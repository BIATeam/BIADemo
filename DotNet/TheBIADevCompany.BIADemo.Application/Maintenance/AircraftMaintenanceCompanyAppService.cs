// BIADemo only
// <copyright file="AircraftMaintenanceCompanyAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Maintenance
{
    using System.Linq.Expressions;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using BIA.Net.Core.Domain.User.Specifications;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.Maintenance;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.Maintenance.Entities;
    using TheBIADevCompany.BIADemo.Domain.Maintenance.Mappers;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.User;

    /// <summary>
    /// The application service used for aircraftMaintenanceCompany.
    /// </summary>
    public class AircraftMaintenanceCompanyAppService : CrudAppServiceBase<AircraftMaintenanceCompanyDto, AircraftMaintenanceCompany, int, PagingFilterFormatDto<TeamAdvancedFilterDto>, AircraftMaintenanceCompanyMapper>, IAircraftMaintenanceCompanyAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftMaintenanceCompanyAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public AircraftMaintenanceCompanyAppService(
            ITGenericRepository<AircraftMaintenanceCompany, int> repository,
            IPrincipal principal)
            : base(repository)
        {
            this.FiltersContext.Add(
                AccessMode.Read,
                TeamAppService.ReadSpecification<AircraftMaintenanceCompany>(TeamTypeId.AircraftMaintenanceCompany, principal, TeamConfig.Config));

            this.FiltersContext.Add(
                AccessMode.Update,
                TeamAppService.UpdateSpecification<AircraftMaintenanceCompany, UserDataDto>(TeamTypeId.AircraftMaintenanceCompany, principal));
        }
    }
}