namespace BIA.Net.Core.Domain.Annoucement.Entities
{
    using System;
    using BIA.Net.Core.Common.Enum;
    using BIA.Net.Core.Domain.Entity;
    using global::Audit.EntityFramework;

    [AuditInclude]
    public sealed class Annoucement : BaseEntityVersioned<int>
    {
        public AnnoucementType Type { get; set; }
        public BiaAnnoucementType TypeId { get; set; }
        public string RawContent { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
