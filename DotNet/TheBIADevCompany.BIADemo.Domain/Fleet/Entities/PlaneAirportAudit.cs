namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Attributes;
    using BIA.Net.Core.Domain.Audit;

    [AuditLinkedEntity(linkedEntityType: typeof(Plane), linkedEntityPropertyName: nameof(Plane.ConnectingAirports))]
    public class PlaneAirportAudit : AuditEntity
    {
        [AuditLinkedEntityPropertyIdentifier(linkedEntityType: typeof(Airport))]
        public int AirportId { get; set; }
        [AuditLinkedEntityPropertyIdentifier(linkedEntityType: typeof(Plane))]
        public int PlaneId { get; set; }
        [AuditLinkedEntityPropertyDisplay(linkedEntityType: typeof(Plane))]
        public string AirportName { get; set; }
        public string PlaneName { get; set; }

        public override void FillSpecificProperties<TEntity>(TEntity entity)
        {
            if (entity is not PlaneAirport planeAirport)
            {
                return;
            }

            AirportName = planeAirport.Airport.Name;
            PlaneName = planeAirport.Plane.Msn;
        }
    }
}
