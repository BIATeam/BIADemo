// BIADemo only
// <copyright file="PlaneAppService.cs" company="TheBIADevCompany">
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
    using TheBIADevCompany.BIADemo.Domain.Dto.Maintenance;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Mappers;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// The application service used for plane.
    /// </summary>
    public class PlaneAppService : CrudAppServiceBase<PlaneDto, Plane, int, PagingFilterFormatDto, PlaneMapper>, IPlaneAppService
    {
        /// <summary>
        /// The current AncestorTeamId.
        /// </summary>
        private readonly int currentAncestorTeamId;

        // BIAToolKit - Begin FixedChildrenRepositoryDefinitionPlane
        // Begin BIAToolKit Generation Ignore
        // BIAToolKit - Begin Partial FixedChildrenRepositoryDefinitionPlane Engine

        /// <summary>
        /// The engine app repository.
        /// </summary>
        private readonly IEngineRepository engineRepository;

        // BIAToolKit - End Partial FixedChildrenRepositoryDefinitionPlane Engine
        // End BIAToolKit Generation Ignore
        // BIAToolKit - End FixedChildrenRepositoryDefinitionPlane
#pragma warning disable SA1515 // Single-line comment should be preceded by blank line
#pragma warning disable SA1611 // Element parameters should be documented
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        // BIAToolKit - Begin FixedChildrenRepositoryConstructorParamPlane
        // Begin BIAToolKit Generation Ignore
        // BIAToolKit - Begin Partial FixedChildrenRepositoryConstructorParamPlane Engine
        /// <param name="engineRepository">The engine app service.</param>
        // BIAToolKit - End Partial FixedChildrenRepositoryConstructorParamPlane Engine
        // End BIAToolKit Generation Ignore
        // BIAToolKit - End FixedChildrenRepositoryConstructorParamPlane
        /// <param name="principal">The claims principal.</param>
        public PlaneAppService(
            ITGenericRepository<Plane, int> repository,
            // BIAToolKit - Begin FixedChildrenRepositoryInjectionPlane
            // Begin BIAToolKit Generation Ignore
            // BIAToolKit - Begin Partial FixedChildrenRepositoryInjectionPlane Engine
            IEngineRepository engineRepository,
            // BIAToolKit - End Partial FixedChildrenRepositoryInjectionPlane Engine
            // End BIAToolKit Generation Ignore
            // BIAToolKit - End FixedChildrenRepositoryInjectionPlane
            IPrincipal principal)
            : base(repository)
        {
            var userData = (principal as BiaClaimsPrincipal).GetUserData<UserDataDto>();
            this.currentAncestorTeamId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.Site) : 0;

            // For child : set the TeamId of the Ancestor that contain a team Parent
            this.FiltersContext.Add(AccessMode.Read, new DirectSpecification<Plane>(x => x.SiteId == this.currentAncestorTeamId));

            // BIAToolKit - Begin FixedChildrenRepositorySetPlane
            // Begin BIAToolKit Generation Ignore
            // BIAToolKit - Begin Partial FixedChildrenRepositorySetPlane Engine
            this.engineRepository = engineRepository;
            // BIAToolKit - End Partial FixedChildrenRepositorySetPlane Engine
            // End BIAToolKit Generation Ignore
            // BIAToolKit - End FixedChildrenRepositorySetPlane
        }
#pragma warning restore SA1611 // Element parameters should be documented
#pragma warning restore SA1515 // Single-line comment should be preceded by blank line

        /// <inheritdoc/>
        public override async Task<PlaneDto> UpdateFixedAsync(int id, bool isFixed)
        {
            return await this.ExecuteWithFrontUserExceptionHandlingAsync(async () =>
            {
                // Update entity fixed status
                var entity = await this.Repository.GetEntityAsync(id) ?? throw new ElementNotFoundException();
                this.Repository.UpdateFixedAsync(entity, isFixed);

                // BIAToolKit - Begin UpdateFixedChildrenPlane
                // Begin BIAToolKit Generation Ignore
                // BIAToolKit - Begin Partial UpdateFixedChildrenPlane Engine
                var engines = await this.engineRepository.GetAllEntityAsync(filter: x => x.PlaneId == id);
                foreach (var engine in engines)
                {
                    this.engineRepository.UpdateFixedAsync(engine, isFixed);
                }

                // BIAToolKit - End Partial UpdateFixedChildrenPlane Engine
                // End BIAToolKit Generation Ignore
                // BIAToolKit - End UpdateFixedChildrenPlane
                await this.Repository.UnitOfWork.CommitAsync();
                return await this.GetAsync(id);
            });
        }

        /// <inheritdoc/>
        public override async Task<PlaneDto> AddAsync(PlaneDto dto, string mapperMode = null)
        {
            if (dto.SiteId != this.currentAncestorTeamId)
            {
                throw new ForbiddenException("Can only add Plane on current parent Team.");
            }

            return await base.AddAsync(dto, mapperMode);
        }
    }
}