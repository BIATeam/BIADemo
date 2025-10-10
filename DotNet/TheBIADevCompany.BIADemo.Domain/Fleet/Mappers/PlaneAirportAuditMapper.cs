namespace TheBIADevCompany.BIADemo.Domain.Fleet.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BIA.Net.Core.Domain.Mapper;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    public class PlaneAirportAuditMapper : AuditMapper<PlaneAirport>
    {
        public PlaneAirportAuditMapper()
        {
            this.AuditPropertyMappers =
            [
                new AuditPropertyMapper<PlaneAirport, Airport>
                {
                    ReferenceEntityProperty = planeAirport => planeAirport.Airport,
                    ReferenceEntityPropertyIdentifier = planeAirport => planeAirport.AirportId,
                    LinkedEntityPropertyDisplay = airport => airport.Name,
                },
                new AuditPropertyMapper<PlaneAirport, Plane>
                {
                    ReferenceEntityProperty = planeAirport => planeAirport.Plane,
                    ReferenceEntityPropertyIdentifier = planeAirport => planeAirport.PlaneId,
                    LinkedEntityPropertyDisplay = plane => plane.Msn,
                },
            ];
        }
    }
}
