namespace TheBIADevCompany.BIADemo.Domain.Fleet.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Mapper;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    public class PlaneAuditMapper : AuditMapper<Plane>
    {
        public PlaneAuditMapper()
        {
            this.LinkedAuditMappers =
            [
                new LinkedAuditMapper<Plane, EngineAudit>
                {
                    ReferenceEntityProperty = plane => plane.Engines,
                    LinkedAuditEntityDisplayProperty = audit => audit.Reference,
                },
                new LinkedAuditMapper<Plane, PlaneAirportAudit>
                {
                    ReferenceEntityProperty = plane => plane.ConnectingAirports,
                    LinkedAuditEntityDisplayProperty = audit => audit.AirportName,
                },
            ];
            this.AuditPropertyMappers =
            [
                new AuditPropertyMapper<Plane, Airport>
                {
                    ReferenceEntityProperty = plane => plane.CurrentAirport,
                    ReferenceEntityPropertyIdentifier = plane => plane.CurrentAirportId,
                    LinkedEntityPropertyDisplay = airport => airport.Name,
                },
            ];
        }
    }
}
