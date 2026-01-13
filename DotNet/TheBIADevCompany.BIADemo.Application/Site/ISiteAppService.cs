// <copyright file="ISiteAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Application.Site
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using TheBIADevCompany.BIADemo.Domain.Dto.Site;
    using TheBIADevCompany.BIADemo.Domain.Site.Entities;

    /// <summary>
    /// The interface defining the application service for site.
    /// </summary>
    public interface ISiteAppService : ICrudAppServiceBase<SiteDto, Site, int, PagingFilterFormatDto<TeamAdvancedFilterDto>>
    {
    }
}