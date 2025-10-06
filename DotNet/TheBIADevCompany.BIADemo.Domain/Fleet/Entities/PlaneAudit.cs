namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Attributes;
    using BIA.Net.Core.Domain.Audit;

    public class PlaneAudit : AuditEntity
    {
        [AuditLinkedEntityProperty(typeof(Airport), nameof(Airport.Name), nameof(Plane.CurrentAirportId), nameof(Plane.CurrentAirport))]
        public string CurrentAirportName { get; set; }
    }
}
