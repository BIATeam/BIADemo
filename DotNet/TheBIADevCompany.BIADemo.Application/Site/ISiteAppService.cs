// <copyright file="ISiteAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Site
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;
    using TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate;

    /// <summary>
    /// The interface defining the application service for site.
    /// </summary>
    public interface ISiteAppService : ICrudAppServiceBase<SiteDto, Site, int, SiteFilterDto>
    {
        /// <summary>
        /// Get the list of SiteInfoDto with paging and sorting.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of SiteInfoDto.</returns>
        Task<(IEnumerable<SiteInfoDto> Sites, int Total)> GetRangeWithMembersAsync(SiteFilterDto filters);

        /// <summary>
        /// Get Site with list of admins.
        /// </summary>
        /// <param name="id">id of the site.</param>
        /// <returns>The site.</returns>
        Task<SiteInfoDto> GetWithMembersAsync(int id);
    }
}