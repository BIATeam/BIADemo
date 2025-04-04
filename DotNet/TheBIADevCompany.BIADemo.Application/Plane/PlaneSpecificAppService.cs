// BIADemo only
// <copyright file="PlaneSpecificAppService.cs" company="TheBIADevCompany">
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
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;
    using TheBIADevCompany.BIADemo.Domain.Plane.Mappers;

    /// <summary>
    /// The application service used for plane.
    /// </summary>
    public class PlaneSpecificAppService :
        CrudAppServiceListAndItemBase<PlaneSpecificDto, PlaneDto, Plane, int, PagingFilterFormatDto, PlaneSpecificMapper, PlaneMapper>,
        IPlaneSpecificAppService
    {
        // BIAToolKit - Begin Parent

        /// <summary>
        /// The current SiteId.
        /// </summary>
        private readonly int currentSiteId;

        // BIAToolKit - End Parent

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneSpecificAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public PlaneSpecificAppService(ITGenericRepository<Plane, int> repository, IPrincipal principal)
            : base(repository)
        {
            // BIAToolKit - Begin Parent siteId
            var userData = (principal as BiaClaimsPrincipal).GetUserData<UserDataDto>();
            this.currentSiteId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.Site) : 0;

            // For child : set the TeamId of the Ancestor that contain a team Parent
            this.FiltersContext.Add(AccessMode.Read, new DirectSpecification<Plane>(p => p.SiteId == this.currentSiteId));

            // BIAToolKit - End Parent siteId
        }
    }
}