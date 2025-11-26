// <copyright file="SiteAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Site
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Specification;
    using BIA.Net.Core.Domain.User.Specifications;
    using TheBIADevCompany.BIADemo.Application.User;
    using TheBIADevCompany.BIADemo.Crosscutting.Common.Enum;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;
    using TheBIADevCompany.BIADemo.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.Site.Entities;
    using TheBIADevCompany.BIADemo.Domain.Site.Mappers;
    using TheBIADevCompany.BIADemo.Domain.User;

    /// <summary>
    /// The application service used for site.
    /// </summary>
    public class SiteAppService : CrudAppServiceBase<SiteDto, Site, int, PagingFilterFormatDto<TeamAdvancedFilterDto>, SiteMapper>, ISiteAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public SiteAppService(ITGenericRepository<Site, int> repository, IPrincipal principal)
            : base(repository)
        {
            this.FiltersContext.Add(
                AccessMode.Read,
                TeamAppService.ReadSpecification<Site>(TeamTypeId.Site, principal, TeamConfig.Config));

            this.FiltersContext.Add(
                AccessMode.Update,
                TeamAppService.UpdateSpecification<Site, UserDataDto>(TeamTypeId.Site, principal));
        }
    }
}