// BIADemo only
// <copyright file="PlaneOptionAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Plane
{
    using System.Security.Principal;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Authentication;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Mappers;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;

    /// <summary>
    /// The application service used for plane option.
    /// </summary>
    public class PlaneOptionAppService : OptionAppServiceBase<OptionDto, Plane, int, PlaneOptionMapper>, IPlaneOptionAppService
    {
        /// <summary>
        /// The current TeamId.
        /// </summary>
        private readonly int currentTeamId;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneOptionAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The principal.</param>
        public PlaneOptionAppService(ITGenericRepository<Plane, int> repository, IPrincipal principal)
            : base(repository)
        {
            // BIAToolKit - Begin AncestorTeam Site
            var userData = (principal as BiaClaimsPrincipal).GetUserData<UserDataDto>();
            this.currentTeamId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.Site) : 0;

            // For child : set the TeamId of the Ancestor that contain a team Parent
            this.FiltersContext.Add(AccessMode.Read, new DirectSpecification<Plane>(p => p.SiteId == this.currentTeamId));

            // BIAToolKit - End AncestorTeam Site
        }
    }
}
