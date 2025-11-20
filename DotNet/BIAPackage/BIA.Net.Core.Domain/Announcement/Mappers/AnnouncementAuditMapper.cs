// <copyright file="AnnouncementAuditMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Announcement.Mappers
{
    using System.Linq;
    using BIA.Net.Core.Domain.Announcement.Entities;
    using BIA.Net.Core.Domain.Mapper;

    /// <summary>
    /// Audit mapper for <see cref="Announcement"/>.
    /// </summary>
    public class AnnouncementAuditMapper : AuditMapper<Announcement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnnouncementAuditMapper"/> class.
        /// </summary>
        public AnnouncementAuditMapper()
        {
            this.AuditPropertyMappers =
                [
                    new AuditPropertyMapper<Announcement, AnnouncementType>()
                    {
                        EntityProperty = x => x.Type,
                        EntityPropertyIdentifier = x => x.TypeId,
                        LinkedEntityPropertyDisplay = x => x.Id,
                    }
                ];
        }
    }
}
