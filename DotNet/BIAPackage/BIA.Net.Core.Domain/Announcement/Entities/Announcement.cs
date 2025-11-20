namespace BIA.Net.Core.Domain.Announcement.Entities
{
    using System;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Entity;
    using global::Audit.EntityFramework;

    [AuditInclude]
    public sealed class Announcement : BaseEntityVersioned<int>
    {
        public AnnouncementType Type { get; set; }
        public BiaAnnouncementType TypeId { get; set; }
        public string RawContent { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
