// <copyright file="BannerMessageAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Banner
{
    using System.Security.Principal;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Banner.Entities;
    using BIA.Net.Core.Domain.Banner.Mappers;
    using BIA.Net.Core.Domain.Dto.Banner;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;

    /// <summary>
    /// The application service used for bannerMessage.
    /// </summary>
    public class BannerMessageAppService : CrudAppServiceBase<BannerMessageDto, BannerMessage, int, PagingFilterFormatDto, BannerMessageMapper>, IBannerMessageAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BannerMessageAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public BannerMessageAppService(
            ITGenericRepository<BannerMessage, int> repository,
            IPrincipal principal)
            : base(repository)
        {
        }
    }
}
