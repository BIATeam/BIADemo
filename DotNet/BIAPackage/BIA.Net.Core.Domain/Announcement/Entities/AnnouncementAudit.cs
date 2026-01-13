// <copyright file="AnnouncementAudit.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Announcement.Entities
{
    using BIA.Net.Core.Domain.Audit;

    /// <summary>
    /// Audit entity for <see cref="Announcement"/>.
    /// </summary>
    public sealed class AnnouncementAudit : AuditKeyedEntity<Announcement, int, int>
    {
    }
}
