// BIADemo only
// <copyright file="PilotAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Fleet
{
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common.Exceptions;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.Fleet;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Mappers;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// The application service used for pilot.
    /// </summary>
    public class PilotAppService : CrudAppServiceBase<PilotDto, Pilot, Guid, PagingFilterFormatDto, PilotMapper>, IPilotAppService
    {
        /// <summary>
        /// The current AncestorTeamId.
        /// </summary>
        private readonly int currentAncestorTeamId;

        // BIAToolKit - Begin FixedChildrenRepositoryDefinitionPilot
        // BIAToolKit - End FixedChildrenRepositoryDefinitionPilot
#pragma warning disable SA1515 // Single-line comment should be preceded by blank line
#pragma warning disable SA1611 // Element parameters should be documented
        /// <summary>
        /// Initializes a new instance of the <see cref="PilotAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        // BIAToolKit - Begin FixedChildrenRepositoryConstructorParamPilot
        // BIAToolKit - End FixedChildrenRepositoryConstructorParamPilot
        /// <param name="principal">The claims principal.</param>
        public PilotAppService(
            ITGenericRepository<Pilot, Guid> repository,
            // BIAToolKit - Begin FixedChildrenRepositoryInjectionPilot
            // BIAToolKit - End FixedChildrenRepositoryInjectionPilot
            IPrincipal principal)
            : base(repository)
        {
            var userData = (principal as BiaClaimsPrincipal).GetUserData<UserDataDto>();
            this.currentAncestorTeamId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.Site) : 0;

            // For child : set the TeamId of the Ancestor that contain a team Parent
            this.FiltersContext.Add(AccessMode.Read, new DirectSpecification<Pilot>(x => x.SiteId == this.currentAncestorTeamId));

            // BIAToolKit - Begin FixedChildrenRepositorySetPilot
            // BIAToolKit - End FixedChildrenRepositorySetPilot
        }
#pragma warning restore SA1611 // Element parameters should be documented
#pragma warning restore SA1515 // Single-line comment should be preceded by blank line

        /// <inheritdoc/>
        public override async Task<PilotDto> AddAsync(PilotDto dto, string mapperMode = null)
        {
            if (dto.SiteId != this.currentAncestorTeamId)
            {
                throw new ForbiddenException("Can only add Pilot on current parent Team.");
            }

            return await base.AddAsync(dto, mapperMode);
        }

        /// <inheritdoc/>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task ExecuteActionsOnUpdateFixedAsync(Guid entityUpdatedId, bool isFixed)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            // BIAToolKit - Begin UpdateFixedChildrenPilot
            // BIAToolKit - End UpdateFixedChildrenPilot
        }
    }
}