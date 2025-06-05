// BIADemo only
// <copyright file="MaintenanceContractAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Maintenance
{
    using System.Security.Principal;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.Maintenance;
    using TheBIADevCompany.BIADemo.Domain.Maintenance.Entities;
    using TheBIADevCompany.BIADemo.Domain.Maintenance.Mappers;

    /// <summary>
    /// The application service used for maintenanceContract.
    /// </summary>
    public class MaintenanceContractAppService : CrudAppServiceBase<MaintenanceContractDto, MaintenanceContract, int, PagingFilterFormatDto, MaintenanceContractMapper>, IMaintenanceContractAppService
    {
        /// <summary>
        /// The current SiteId.
        /// </summary>
        private readonly int currentSiteId;

        /// <summary>
        /// The current currentAircraftMaintenanceCompanyId.
        /// </summary>
        private readonly int currentAircraftMaintenanceCompanyId;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaintenanceContractAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public MaintenanceContractAppService(ITGenericRepository<MaintenanceContract, int> repository, IPrincipal principal)
            : base(repository)
        {
            var userData = (principal as BiaClaimsPrincipal).GetUserData<UserDataDto>();
            this.currentSiteId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.Site) : 0;
            this.currentAircraftMaintenanceCompanyId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.AircraftMaintenanceCompany) : 0;

            // For child : set the TeamId of the Ancestor that contain a team Parent
            this.FiltersContext.Add(AccessMode.Read, new DirectSpecification<MaintenanceContract>(
                    x => x.SiteId == this.currentSiteId && x.AircraftMaintenanceCompanyId == this.currentAircraftMaintenanceCompanyId));
        }

        /// <inheritdoc/>
        public override async Task<MaintenanceContractDto> AddAsync(MaintenanceContractDto dto, string mapperMode = null)
        {
            dto.SiteId = this.currentSiteId;
            dto.AircraftMaintenanceCompanyId = this.currentAircraftMaintenanceCompanyId;

            return await base.AddAsync(dto, mapperMode);
        }

        /// <inheritdoc/>
        public override async Task<MaintenanceContractDto> UpdateAsync(MaintenanceContractDto dto, string accessMode = AccessMode.Update, string queryMode = QueryMode.Update, string mapperMode = null)
        {
            if (dto.AircraftMaintenanceCompanyId != this.currentAircraftMaintenanceCompanyId && dto.SiteId != this.currentSiteId)
            {
                throw new ForbiddenException("Can only add MaintenanceContract on current parent Teams.");
            }

            return await base.UpdateAsync(dto, accessMode, queryMode, mapperMode);
        }
    }
}
