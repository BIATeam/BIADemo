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
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.Fleet;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Mappers;

    // Begin BIAToolKit Generation Ignore
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    // End BIAToolKit Generation Ignore

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
        // BIAToolKit - End FixedChildrenRepositoryDefinitionPlane
        // Begin BIAToolKit Generation Ignore

        /// <summary>
        /// The engine app repository.
        /// </summary>
        private readonly IEngineRepository engineRepository;

        // End BIAToolKit Generation Ignore
#pragma warning disable SA1515 // Single-line comment should be preceded by blank line
#pragma warning disable SA1611 // Element parameters should be documented
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        // BIAToolKit - Begin FixedChildrenRepositoryConstructorParamPlane
        // BIAToolKit - End FixedChildrenRepositoryConstructorParamPlane
        // Begin BIAToolKit Generation Ignore
        /// <param name="engineRepository">The engine app service.</param>
        // End BIAToolKit Generation Ignore
        /// <param name="principal">The claims principal.</param>
        public PlaneAppService(
            ITGenericRepository<Plane, int> repository,
            // BIAToolKit - Begin FixedChildrenRepositoryInjectionPlane
            // BIAToolKit - End FixedChildrenRepositoryInjectionPlane
            // Begin BIAToolKit Generation Ignore
            IEngineRepository engineRepository,
            // End BIAToolKit Generation Ignore
            IPrincipal principal)
            : base(repository)
        {
            var userData = (principal as BiaClaimsPrincipal).GetUserData<UserDataDto>();
            this.currentAncestorTeamId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.Site) : 0;

            // For child : set the TeamId of the Ancestor that contain a team Parent
            this.FiltersContext.Add(AccessMode.Read, new DirectSpecification<Plane>(x => x.SiteId == this.currentAncestorTeamId));

            // BIAToolKit - Begin FixedChildrenRepositorySetPlane
            // BIAToolKit - End FixedChildrenRepositorySetPlane
            // Begin BIAToolKit Generation Ignore
            this.engineRepository = engineRepository;
            // End BIAToolKit Generation Ignore
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
                // BIAToolKit - End UpdateFixedChildrenPlane
                // Begin BIAToolKit Generation Ignore
                var engines = await this.engineRepository.GetAllEntityAsync(filter: x => x.PlaneId == id);
                foreach (var engine in engines)
                {
                    this.engineRepository.UpdateFixedAsync(engine, isFixed);
                }

                // End BIAToolKit Generation Ignore
                await this.Repository.UnitOfWork.CommitAsync();
                return await this.GetAsync(id);
            });
        }
    }
}