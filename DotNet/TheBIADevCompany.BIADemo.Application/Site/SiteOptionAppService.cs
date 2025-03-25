// BIADemo only
// <copyright file="SiteOptionAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Site
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.RepoContract;
    using TheBIADevCompany.BIADemo.Domain.AircraftMaintenanceCompany.Mappers;
    using TheBIADevCompany.BIADemo.Domain.Site.Entities;

    /// <summary>
    /// The application service used for site option.
    /// </summary>
    public class SiteOptionAppService : OptionAppServiceBase<OptionDto, Site, int, SiteOptionMapper>, ISiteOptionAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteOptionAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public SiteOptionAppService(ITGenericRepository<Site, int> repository)
            : base(repository)
        {
        }
    }
}
