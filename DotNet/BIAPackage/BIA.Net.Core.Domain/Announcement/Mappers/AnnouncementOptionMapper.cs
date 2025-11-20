// <copyright file="AnnouncementTypeOptionMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Announcement.Mappers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain.Announcement.Entities;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.Service;

    /// <summary>
    /// The mapper used for announcement type option.
    /// </summary>
    public class AnnouncementOptionMapper : BaseMapper<TOptionDto<BiaAnnouncementType>, AnnouncementType, BiaAnnouncementType>
    {
        private readonly UserContext userContext;

        public AnnouncementOptionMapper(UserContext userContext)
        {
            this.userContext = userContext;
        }

        /// <inheritdoc />
        public override Expression<Func<AnnouncementType, TOptionDto<BiaAnnouncementType>>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new TOptionDto<BiaAnnouncementType>
            {
                Display = entity.AnnouncementTypeTranslations.Single(x => x.LanguageId == this.userContext.LanguageId).Label,
            });
        }
    }
}
