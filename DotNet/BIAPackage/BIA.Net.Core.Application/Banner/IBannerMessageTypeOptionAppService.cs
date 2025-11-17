// <copyright file="IBannerMessageTypeOptionAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Banner
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The interface defining the application service for banner message type option.
    /// </summary>
    public interface IBannerMessageTypeOptionAppService : IOptionAppServiceBase<TOptionDto<BiaBannerMessageType>, BiaBannerMessageType>
    {
    }
}
