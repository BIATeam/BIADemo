namespace BIA.Net.Core.Domain.Banner.Entities
{
    using System;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Entity;
    using global::Audit.EntityFramework;

    [AuditInclude]
    public sealed class BannerMessage : BaseEntityVersioned<int>
    {
        public string Name { get; set; }
        public BannerMessageType Type { get; set; }
        public BiaBannerMessageType TypeId { get; set; }
        public string RawContent { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
