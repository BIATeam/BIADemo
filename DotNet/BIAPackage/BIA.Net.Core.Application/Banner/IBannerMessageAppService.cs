// <copyright file="IBannerMessageAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Banner
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Banner.Entities;
    using BIA.Net.Core.Domain.Dto.Banner;
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The interface defining the application service for bannerMessage.
    /// </summary>
    public interface IBannerMessageAppService : ICrudAppServiceBase<BannerMessageDto, BannerMessage, int, PagingFilterFormatDto>
    {
    }
}
