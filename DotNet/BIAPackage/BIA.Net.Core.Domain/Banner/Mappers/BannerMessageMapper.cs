// <copyright file="BannerMessageMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Banner.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Banner.Entities;
    using BIA.Net.Core.Domain.Dto.Banner;
    using BIA.Net.Core.Domain.Mapper;

    /// <summary>
    /// The mapper used for BannerMessage.
    /// </summary>
    public class BannerMessageMapper : BaseMapper<BannerMessageDto, BannerMessage, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BannerMessageMapper"/> class.
        /// </summary>
        /// <param name="auditMappers">The injected collection of <see cref="IAuditMapper"/>.</param>
        public BannerMessageMapper(IEnumerable<IAuditMapper> auditMappers)
        {
            this.AuditMapper = auditMappers.FirstOrDefault(x => x.EntityType == typeof(BannerMessage));
        }

        /// <inheritdoc />
        public override ExpressionCollection<BannerMessage> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<BannerMessage>(base.ExpressionCollection)
                {
                    { HeaderName.End, bannerMessage => bannerMessage.End },
                    { HeaderName.Name, bannerMessage => bannerMessage.Name },
                    { HeaderName.RawContent, bannerMessage => bannerMessage.RawContent },
                    { HeaderName.Start, bannerMessage => bannerMessage.Start },
                    { HeaderName.Type, bannerMessage => bannerMessage.Type },
                };
            }
        }

        /// <inheritdoc />
        public override void DtoToEntity(BannerMessageDto dto, ref BannerMessage entity)
        {
            base.DtoToEntity(dto, ref entity);
            entity.End = dto.End;
            entity.Name = dto.Name;
            entity.RawContent = dto.RawContent;
            entity.Start = dto.Start;
            entity.Type = dto.Type;
        }

        /// <inheritdoc />
        public override Expression<Func<BannerMessage, BannerMessageDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new BannerMessageDto
            {
                End = entity.End,
                Name = entity.Name,
                RawContent = entity.RawContent,
                Start = entity.Start,
                Type = entity.Type,
            });
        }

        /// <inheritdoc />
        public override Dictionary<string, Func<string>> DtoToCellMapping(BannerMessageDto dto)
        {
            return new Dictionary<string, Func<string>>(base.DtoToCellMapping(dto))
            {
                { HeaderName.End, () => CSVDateTime(dto.End) },
                { HeaderName.Name, () => CSVString(dto.Name) },
                { HeaderName.RawContent, () => CSVString(dto.RawContent) },
                { HeaderName.Start, () => CSVDateTime(dto.Start) },
                { HeaderName.Type, () => CSVString(dto.Type.ToString()) },
            };
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
            /// Header name for name.
            /// </summary>
            public const string Name = "name";

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
