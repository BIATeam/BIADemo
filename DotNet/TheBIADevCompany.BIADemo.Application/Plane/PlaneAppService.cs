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
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.Plane;
    using TheBIADevCompany.BIADemo.Domain.Plane.Entities;
    using TheBIADevCompany.BIADemo.Domain.PlaneModule.Aggregate;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public PlaneAppService(ITGenericRepository<Plane, int> repository, IPrincipal principal)
            : base(repository)
        {
            // BIAToolKit - Begin AncestorTeam Site
            var userData = (principal as BiaClaimsPrincipal).GetUserData<UserDataDto>();
            this.currentTeamId = userData != null ? userData.GetCurrentTeamId((int)TeamTypeId.Site) : 0;

            // For child : set the TeamId of the Ancestor that contain a team Parent
            this.FiltersContext.Add(AccessMode.Read, new DirectSpecification<Plane>(p => p.SiteId == this.currentTeamId));

            // BIAToolKit - End AncestorTeam Site
        }

        /// <inheritdoc/>
        async Task<List<PlaneDto>> IPlaneAppService.SaveSafeAsync(IEnumerable<PlaneDto> dtos, BiaClaimsPrincipal principal, string rightAdd, string rightUpdate, string rightDelete, string accessMode, string queryMode, string mapperMode)
        {
            var saveSafeReturn = await this.SaveSafeAsync(dtos, principal, rightAdd, rightUpdate, rightDelete, accessMode, queryMode, mapperMode);

            if (saveSafeReturn.AggregateException != null)
            {
                throw new FrontUserException(saveSafeReturn.AggregateException);
            }

            if (!string.IsNullOrEmpty(saveSafeReturn.ErrorMessage))
            {
                throw new FrontUserException(saveSafeReturn.ErrorMessage);
            }

            return saveSafeReturn.DtosSaved;
        }
    }
}