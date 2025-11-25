// BIADemo only
// <copyright file="MaintenanceTeamAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Maintenance
{
    using System.Linq.Expressions;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Authentication;
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
    /// The application service used for maintenanceTeam.
    /// </summary>
    public class MaintenanceTeamAppService : CrudAppServiceBase<MaintenanceTeamDto, MaintenanceTeam, int, PagingFilterFormatDto<TeamAdvancedFilterDto>, MaintenanceTeamMapper>, IMaintenanceTeamAppService
    {
        /// <summary>
        /// The current AncestorTeamId.
        /// </summary>
        private readonly int currentAncestorTeamId;

        // BIAToolKit - Begin FixedChildrenRepositoryDefinitionMaintenanceTeam
        // BIAToolKit - End FixedChildrenRepositoryDefinitionMaintenanceTeam
#pragma warning disable SA1515 // Single-line comment should be preceded by blank line
#pragma warning disable SA1611 // Element parameters should be documented
        /// <summary>
        /// Initializes a new instance of the <see cref="MaintenanceTeamAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        // BIAToolKit - Begin FixedChildrenRepositoryConstructorParamMaintenanceTeam
        // BIAToolKit - End FixedChildrenRepositoryConstructorParamMaintenanceTeam
        /// <param name="principal">The claims principal.</param>
        public MaintenanceTeamAppService(
            ITGenericRepository<MaintenanceTeam, int> repository,
            // BIAToolKit - Begin FixedChildrenRepositoryInjectionMaintenanceTeam
            // BIAToolKit - End FixedChildrenRepositoryInjectionMaintenanceTeam
            IPrincipal principal)
            : base(repository)
        {
            this.FiltersContext.Add(
                AccessMode.Read,
                TeamAppService.ReadSpecification<MaintenanceTeam>(TeamTypeId.MaintenanceTeam, principal, TeamConfig.Config));

            this.FiltersContext.Add(
                AccessMode.Update,
                TeamAppService.UpdateSpecification<MaintenanceTeam, UserDataDto>(TeamTypeId.MaintenanceTeam, principal));
            var userData = (principal as BiaClaimsPrincipal).GetUserData<UserDataDto>();
            this.currentAncestorTeamId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.AircraftMaintenanceCompany) : 0;

            // BIAToolKit - Begin FixedChildrenRepositorySetMaintenanceTeam
            // BIAToolKit - End FixedChildrenRepositorySetMaintenanceTeam
        }
#pragma warning restore SA1611 // Element parameters should be documented
#pragma warning restore SA1515 // Single-line comment should be preceded by blank line

        /// <inheritdoc/>
        public override async Task<MaintenanceTeamDto> UpdateFixedAsync(int id, bool isFixed)
        {
            return await this.ExecuteWithFrontUserExceptionHandlingAsync(async () =>
            {
                // Update entity fixed status
                var entity = await this.Repository.GetEntityAsync(id) ?? throw new ElementNotFoundException();
                this.Repository.UpdateFixedAsync(entity, isFixed);

                // BIAToolKit - Begin UpdateFixedChildrenMaintenanceTeam
                // BIAToolKit - End UpdateFixedChildrenMaintenanceTeam
                await this.Repository.UnitOfWork.CommitAsync();
                return await this.GetAsync(id);
            });
        }

        /// <inheritdoc/>
        public override async Task<MaintenanceTeamDto> AddAsync(MaintenanceTeamDto dto, string mapperMode = null)
        {
            if (dto.AircraftMaintenanceCompanyId != this.currentAncestorTeamId)
            {
                throw new ForbiddenException("Can only add MaintenanceTeam on current parent Team.");
            }

            return await base.AddAsync(dto, mapperMode);
        }

        /// <inheritdoc/>
#pragma warning disable S1006 // Method overrides should not change parameter defaults
        public override async Task<(IEnumerable<MaintenanceTeamDto> Results, int Total)> GetRangeAsync(PagingFilterFormatDto<TeamAdvancedFilterDto> filters = null, int id = default, Specification<MaintenanceTeam> specification = null, Expression<Func<MaintenanceTeam, bool>> filter = null, string accessMode = "Read", string queryMode = "ReadList", string mapperMode = null, bool isReadOnlyMode = false)
#pragma warning restore S1006 // Method overrides should not change parameter defaults
        {
            specification ??= TeamAdvancedFilterSpecification<MaintenanceTeam>.Filter(filters);
            return await base.GetRangeAsync(filters, id, specification, filter, accessMode, queryMode, mapperMode, isReadOnlyMode);
        }
    }
}