// BIADemo only
// <copyright file="PlaneAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
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
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;
    using TheBIADevCompany.BIADemo.Domain.Plane.Mappers;
    using TheBIADevCompany.BIADemo.Domain.RepoContract;

    /// <summary>
    /// The application service used for plane.
    /// </summary>
    public class PlaneAppService : CrudAppServiceBase<PlaneDto, Plane, int, PagingFilterFormatDto, PlaneMapper>, IPlaneAppService
    {
        // BIAToolKit - Begin AncestorTeam Site

        /// <summary>
        /// The current TeamId.
        /// </summary>
        private readonly int currentTeamId;

        // BIAToolKit - End AncestorTeam Site

        // Begin BIADemo

        /// <summary>
        /// The engine app repository.
        /// </summary>
        private readonly IEngineRepository engineRepository;

#pragma warning disable SA1515 // Single-line comment should be preceded by blank line
#pragma warning disable SA1611 // Element parameters should be documented
        // End BIADemo

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        // Begin BIADemo
        /// <param name="engineRepository">The engine app service.</param>
        // End BIADemo
        /// <param name="principal">The claims principal.</param>
        public PlaneAppService(
            ITGenericRepository<Plane, int> repository,
            // Begin BIADemo
            IEngineRepository engineRepository,
            // End BIADemo
            IPrincipal principal)
            : base(repository)
        {
            // BIAToolKit - Begin AncestorTeam Site
            var userData = (principal as BiaClaimsPrincipal).GetUserData<UserDataDto>();
            this.currentTeamId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.Site) : 0;

            // For child : set the TeamId of the Ancestor that contain a team Parent
            this.FiltersContext.Add(AccessMode.Read, new DirectSpecification<Plane>(p => p.SiteId == this.currentTeamId));

            // BIAToolKit - End AncestorTeam Site

            // Begin BIADemo
            this.engineRepository = engineRepository;
            // End BIADemo
        }

        // Begin BIADemo
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

                // Update children fixed status
                var engines = await this.engineRepository.GetAllEntityAsync(filter: x => x.PlaneId == id);
                foreach (var engine in engines)
                {
                    this.engineRepository.UpdateFixedAsync(engine, isFixed);
                }

                await this.Repository.UnitOfWork.CommitAsync();
                return await this.GetAsync(id);
            });
        }

        // End BIADemo
    }
}