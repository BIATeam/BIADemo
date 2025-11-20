// <copyright file="AnnouncementAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Announcement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Domain.Announcement.Entities;
    using BIA.Net.Core.Domain.Announcement.Mappers;
    using BIA.Net.Core.Domain.Dto.Announcement;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.RepoContract;

    /// <summary>
    /// The application service used for announcement.
    /// </summary>
    public class AnnouncementAppService : CrudAppServiceBase<AnnouncementDto, Announcement, int, PagingFilterFormatDto, AnnouncementMapper>, IAnnouncementAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnnouncementAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        public AnnouncementAppService(
            ITGenericRepository<Announcement, int> repository,
            IPrincipal principal)
            : base(repository)
        {
        }

        /// <inheritdoc/>
        public async Task<List<AnnouncementDto>> GetActives()
        {
            var currentDatetime = DateTime.UtcNow;
            var actives = await this.GetAllAsync(filter: x => x.End > currentDatetime && x.Start <= currentDatetime);
            return [.. actives.OrderBy(x => x.Start)];
        }
    }
}
