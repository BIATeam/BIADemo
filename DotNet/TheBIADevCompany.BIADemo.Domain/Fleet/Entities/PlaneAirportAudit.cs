namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Attributes;
    using BIA.Net.Core.Domain.Audit;

    public class PlaneAirportAudit : AuditEntity
    {
        [AuditParentIdProperty]
        public int AirportId { get; set; }
        [AuditParentIdProperty]
        public int PlaneId { get; set; }
    }
}
