// <copyright file="AnnouncementMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Announcement.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Announcement.Entities;
    using BIA.Net.Core.Domain.Dto.Announcement;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Translation.Entities;

    /// <summary>
    /// The mapper used for Announcement.
    /// </summary>
    public class AnnouncementMapper : BaseMapper<AnnouncementDto, Announcement, int>
    {
        private readonly UserContext userContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnouncementMapper"/> class.
        /// </summary>
        /// <param name="auditMappers">The injected collection of <see cref="IAuditMapper"/>.</param>
        /// <param name="userContext">The user context.</param>
        public AnnouncementMapper(IEnumerable<IAuditMapper> auditMappers, UserContext userContext)
        {
            this.AuditMapper = auditMappers.FirstOrDefault(x => x.EntityType == typeof(Announcement));
            this.userContext = userContext;
        }

        /// <inheritdoc />
        public override ExpressionCollection<Announcement> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Announcement>(base.ExpressionCollection)
                {
                    { HeaderName.End, announcement => announcement.End },
                    { HeaderName.RawContent, announcement => announcement.RawContent },
                    { HeaderName.Start, announcement => announcement.Start },
                    { HeaderName.Type, announcement => announcement.Type.AnnouncementTypeTranslations.Single(x => x.LanguageId == this.userContext.LanguageId).Label },
                };
            }
        }

        /// <inheritdoc />
        public override void DtoToEntity(AnnouncementDto dto, ref Announcement entity)
        {
            base.DtoToEntity(dto, ref entity);
            entity.End = dto.End;
            entity.RawContent = dto.RawContent;
            entity.Start = dto.Start;
            entity.TypeId = dto.Type.Id;
        }

        /// <inheritdoc />
        public override Expression<Func<Announcement, AnnouncementDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new AnnouncementDto
            {
                End = entity.End,
                RawContent = entity.RawContent,
                Start = entity.Start,
                Type = new TOptionDto<BiaAnnouncementType>
                {
                    Id = entity.Type.Id,
                    Display = entity.Type.AnnouncementTypeTranslations.Single(x => x.LanguageId == this.userContext.LanguageId).Label,
                },
            });
        }

        /// <summary>
        /// Header names.
        /// </summary>
        public struct HeaderName
        {
            /// <summary>
            /// Header name for end.
            /// </summary>
            public const string End = "end";

            /// <summary>
            /// Header name for raw content.
            /// </summary>
            public const string RawContent = "rawContent";

            /// <summary>
            /// Header name for start.
            /// </summary>
            public const string Start = "start";

            /// <summary>
            /// Header name for type.
            /// </summary>
            public const string Type = "type";
        }
    }
}
