// <copyright file="AnnouncementTypeOptionAppService.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Application.Announcement
{
    using BIA.Net.Core.Application.Services;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Announcement.Entities;
    using BIA.Net.Core.Domain.Announcement.Mappers;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.RepoContract;

    /// <summary>
    /// The application service used for announcement type option.
    /// </summary>
    public class AnnouncementTypeOptionAppService : OptionAppServiceBase<TOptionDto<BiaAnnouncementType>, AnnouncementType, BiaAnnouncementType, AnnouncementTypeOptionMapper>, IAnnouncementTypeOptionAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnnouncementTypeOptionAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public AnnouncementTypeOptionAppService(ITGenericRepository<AnnouncementType, BiaAnnouncementType> repository)
            : base(repository)
        {
        }
    }
}
