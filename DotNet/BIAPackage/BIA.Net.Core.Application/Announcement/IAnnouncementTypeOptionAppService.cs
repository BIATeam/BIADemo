// <copyright file="IAnnouncementTypeOptionAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Announcement
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Dto.Option;

    /// <summary>
    /// The interface defining the application service for announcement type option.
    /// </summary>
    public interface IAnnouncementTypeOptionAppService : IOptionAppServiceBase<TOptionDto<BiaAnnouncementType>, BiaAnnouncementType>
    {
    }
}
