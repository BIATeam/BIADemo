namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Attributes;
    using BIA.Net.Core.Domain.Audit;


    [AuditLinkedEntity(typeof(Plane), nameof(Plane.Engines))]
    public class EngineAudit : AuditEntity
    {
        [AuditLinkedEntityPropertyIdentifier(linkedEntityType: typeof(Plane))]
        public int PlaneId { get; set; }

        [AuditLinkedEntityPropertyDisplay(linkedEntityType: typeof(Plane))]
        public string Reference { get; set; }
    }
}
