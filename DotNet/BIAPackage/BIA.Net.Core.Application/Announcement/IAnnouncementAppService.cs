// <copyright file="IAnnouncementAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Announcement
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Announcement.Entities;
    using BIA.Net.Core.Domain.Dto.Announcement;
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The interface defining the application service for announcement.
    /// </summary>
    public interface IAnnouncementAppService : ICrudAppServiceBase<AnnouncementDto, Announcement, int, PagingFilterFormatDto>
    {
        /// <summary>
        /// Return actives announcements.
        /// </summary>
        /// <returns><see cref="List{T}"/> of <see cref="AnnouncementDto"/>.</returns>
        Task<List<AnnouncementDto>> GetActives();
    }
}
