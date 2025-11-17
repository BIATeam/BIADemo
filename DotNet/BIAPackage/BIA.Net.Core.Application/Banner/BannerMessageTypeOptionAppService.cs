// <copyright file="BannerMessageTypeOptionAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Banner
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Banner.Entities;
    using BIA.Net.Core.Domain.Banner.Mappers;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.RepoContract;

    /// <summary>
    /// The application service used for banner message type option.
    /// </summary>
    public class BannerMessageTypeOptionAppService : OptionAppServiceBase<TOptionDto<BiaBannerMessageType>, BannerMessageType, BiaBannerMessageType, BannerMessageTypeOptionMapper>, IBannerMessageTypeOptionAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BannerMessageTypeOptionAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public BannerMessageTypeOptionAppService(ITGenericRepository<BannerMessageType, BiaBannerMessageType> repository)
            : base(repository)
        {
        }
    }
}
