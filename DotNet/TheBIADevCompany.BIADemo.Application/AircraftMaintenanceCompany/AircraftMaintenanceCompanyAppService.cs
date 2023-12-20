// BIADemo only
// <copyright file="AircraftMaintenanceCompanyAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.AircraftMaintenanceCompany
{
    using System.Collections.Generic;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using Microsoft.AspNetCore.Http;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompanyModule.Aggregate;
    using TheBIADevCompany.BIADemo.Domain.Dto.AircraftMaintenanceCompany;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// The application service used for AircraftMaintenanceCompany.
    /// </summary>
    public class AircraftMaintenanceCompanyAppService : CrudAppServiceBase<AircraftMaintenanceCompanyDto, AircraftMaintenanceCompany, int, PagingFilterFormatDto, AircraftMaintenanceCompanyMapper>, IAircraftMaintenanceCompanyAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftMaintenanceCompanyAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public AircraftMaintenanceCompanyAppService(ITGenericRepository<AircraftMaintenanceCompany, int> repository, IPrincipal principal)
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