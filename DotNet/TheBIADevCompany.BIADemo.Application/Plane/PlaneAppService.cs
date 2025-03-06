// BIADemo only
// <copyright file="PlaneAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using System.Collections.Generic;
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
    using Microsoft.AspNetCore.Http;
    using TheBIADevCompany.BIADemo.Crosscutting.Common;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;
    using TheBIADevCompany.BIADemo.Domain.Plane.Mappers;

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
        private readonly ITGenericRepository<Engine, int> enginesRepository;

        // BIAToolKit - End AncestorTeam Site

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public PlaneAppService(ITGenericRepository<Plane, int> repository, ITGenericRepository<Engine, int> enginesRepository, IPrincipal principal)
            : base(repository)
        {
            // BIAToolKit - Begin AncestorTeam Site
            var userData = (principal as BiaClaimsPrincipal).GetUserData<UserDataDto>();
            this.currentTeamId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.Site) : 0;

            // For child : set the TeamId of the Ancestor that contain a team Parent
            this.FiltersContext.Add(AccessMode.Read, new DirectSpecification<Plane>(p => p.SiteId == this.currentTeamId));

            // BIAToolKit - End AncestorTeam Site
            if (!(principal as BiaClaimsPrincipal).IsInRole(Rights.Planes.Fix))
            {
                var specification = new DirectSpecification<Plane>(p => !p.IsFixed);
                this.FiltersContext.Add(AccessMode.Update, specification);
                this.FiltersContext.Add(AccessMode.Delete, specification);
            }

            this.enginesRepository = enginesRepository;
        }

        public override async Task<PlaneDto> UpdateAsync(PlaneDto dto, string accessMode = "Update", string queryMode = "Update", string mapperMode = null)
        {
            var updatedDto = await base.UpdateAsync(dto, accessMode, queryMode, mapperMode);
            var engines = await this.enginesRepository.GetAllEntityAsync(filter: x => x.PlaneId == updatedDto.Id);
            foreach (var engine in engines)
            {
                engine.IsFixed = updatedDto.IsFixed;
                engine.FixedDate = updatedDto.FixedDate;
                this.enginesRepository.SetModified(engine);
            }

            await this.enginesRepository.UnitOfWork.CommitAsync();

            return updatedDto;
        }
    }
}