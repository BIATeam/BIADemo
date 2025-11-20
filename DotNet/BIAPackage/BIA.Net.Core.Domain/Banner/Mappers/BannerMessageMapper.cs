// <copyright file="BannerMessageMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Banner.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Banner.Entities;
    using BIA.Net.Core.Domain.Dto.Banner;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Translation.Entities;

    /// <summary>
    /// The mapper used for BannerMessage.
    /// </summary>
    public class BannerMessageMapper : BaseMapper<BannerMessageDto, BannerMessage, int>
    {
        private readonly UserContext userContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerMessageMapper"/> class.
        /// </summary>
        /// <param name="auditMappers">The injected collection of <see cref="IAuditMapper"/>.</param>
        public BannerMessageMapper(IEnumerable<IAuditMapper> auditMappers, UserContext userContext)
        {
            this.AuditMapper = auditMappers.FirstOrDefault(x => x.EntityType == typeof(BannerMessage));
            this.userContext = userContext;
        }

        /// <inheritdoc />
        public override ExpressionCollection<BannerMessage> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<BannerMessage>(base.ExpressionCollection)
                {
                    { HeaderName.End, bannerMessage => bannerMessage.End },
                    { HeaderName.RawContent, bannerMessage => bannerMessage.RawContent },
                    { HeaderName.Start, bannerMessage => bannerMessage.Start },
                    { HeaderName.Type, bannerMessage => bannerMessage.Type.BannerMessageTypeTranslations.Single(x => x.LanguageId == this.userContext.LanguageId).Label },
                };
            }
        }

        /// <inheritdoc />
        public override void DtoToEntity(BannerMessageDto dto, ref BannerMessage entity)
        {
            base.DtoToEntity(dto, ref entity);
            entity.End = dto.End.UtcDateTime;
            entity.RawContent = dto.RawContent;
            entity.Start = dto.Start.UtcDateTime;
            entity.TypeId = dto.Type.Id;
        }

        /// <inheritdoc />
        public override Expression<Func<BannerMessage, BannerMessageDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new BannerMessageDto
            {
                End = entity.End,
                RawContent = entity.RawContent,
                Start = entity.Start,
                Type = new TOptionDto<BiaBannerMessageType>
                {
                    Id = entity.Type.Id,
                    Display = entity.Type.BannerMessageTypeTranslations.Single(x => x.LanguageId == this.userContext.LanguageId).Label,
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
