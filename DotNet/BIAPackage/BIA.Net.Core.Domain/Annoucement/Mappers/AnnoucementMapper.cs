// <copyright file="AnnoucementMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Annoucement.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Common.Extensions;
    using BIA.Net.Core.Domain;
    using BIA.Net.Core.Domain.Annoucement.Entities;
    using BIA.Net.Core.Domain.Dto.Annoucement;
    using BIA.Net.Core.Domain.Dto.Option;
    using BIA.Net.Core.Domain.Mapper;
    using BIA.Net.Core.Domain.Service;
    using BIA.Net.Core.Domain.Translation.Entities;

    /// <summary>
    /// The mapper used for Annoucement.
    /// </summary>
    public class AnnoucementMapper : BaseMapper<AnnoucementDto, Annoucement, int>
    {
        private readonly UserContext userContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnoucementMapper"/> class.
        /// </summary>
        /// <param name="auditMappers">The injected collection of <see cref="IAuditMapper"/>.</param>
        public AnnoucementMapper(IEnumerable<IAuditMapper> auditMappers, UserContext userContext)
        {
            this.AuditMapper = auditMappers.FirstOrDefault(x => x.EntityType == typeof(Annoucement));
            this.userContext = userContext;
        }

        /// <inheritdoc />
        public override ExpressionCollection<Annoucement> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Annoucement>(base.ExpressionCollection)
                {
                    { HeaderName.End, annoucement => annoucement.End },
                    { HeaderName.RawContent, annoucement => annoucement.RawContent },
                    { HeaderName.Start, annoucement => annoucement.Start },
                    { HeaderName.Type, annoucement => annoucement.Type.AnnoucementTypeTranslations.Single(x => x.LanguageId == this.userContext.LanguageId).Label },
                };
            }
        }

        /// <inheritdoc />
        public override void DtoToEntity(AnnoucementDto dto, ref Annoucement entity)
        {
            base.DtoToEntity(dto, ref entity);
            entity.End = dto.End.UtcDateTime;
            entity.RawContent = dto.RawContent;
            entity.Start = dto.Start.UtcDateTime;
            entity.TypeId = dto.Type.Id;
        }

        /// <inheritdoc />
        public override Expression<Func<Annoucement, AnnoucementDto>> EntityToDto()
        {
            return base.EntityToDto().CombineMapping(entity => new AnnoucementDto
            {
                End = entity.End,
                RawContent = entity.RawContent,
                Start = entity.Start,
                Type = new TOptionDto<BiaAnnoucementType>
                {
                    Id = entity.Type.Id,
                    Display = entity.Type.AnnoucementTypeTranslations.Single(x => x.LanguageId == this.userContext.LanguageId).Label,
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
