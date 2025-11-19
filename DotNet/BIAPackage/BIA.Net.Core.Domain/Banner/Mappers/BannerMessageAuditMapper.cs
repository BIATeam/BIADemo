// <copyright file="BannerMessageAuditMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Banner.Mappers
{
    using System.Linq;
    using BIA.Net.Core.Domain.Banner.Entities;
    using BIA.Net.Core.Domain.Mapper;

    /// <summary>
    /// Audit mapper for <see cref="BannerMessage"/>.
    /// </summary>
    public class BannerMessageAuditMapper : AuditMapper<BannerMessage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BannerMessageAuditMapper"/> class.
        /// </summary>
        public BannerMessageAuditMapper()
        {
            this.AuditPropertyMappers =
                [
                    new AuditPropertyMapper<BannerMessage, BannerMessageType>()
                    {
                        EntityProperty = x => x.Type,
                        EntityPropertyIdentifier = x => x.TypeId,
                        LinkedEntityPropertyDisplay = x => x.Id,
                    }
                ];
        }
    }
}
