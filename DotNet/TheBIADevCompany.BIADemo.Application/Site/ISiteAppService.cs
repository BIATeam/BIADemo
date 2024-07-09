// <copyright file="ISiteAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Site
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Service;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;
    using TheBIADevCompany.BIADemo.Domain.SiteModule.Aggregate;

    /// <summary>
    /// The interface defining the application service for site.
    /// </summary>
    public interface ISiteAppService : ICrudAppServiceBase<SiteDto, Site, int, PagingFilterFormatDto>
    {
    }
}