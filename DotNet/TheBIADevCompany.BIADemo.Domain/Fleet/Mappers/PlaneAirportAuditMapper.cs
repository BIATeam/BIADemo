// <copyright file="PlaneAirportAuditMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Mappers
{
    using BIA.Net.Core.Domain.Mapper;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    /// <summary>
    /// Audit mapper for <see cref="PlaneAirport"/>.
    /// </summary>
    public class PlaneAirportAuditMapper : AuditMapper<PlaneAirport>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneAirportAuditMapper"/> class.
        /// </summary>
        public PlaneAirportAuditMapper()
        {
            this.AuditPropertyMappers =
            [
                new AuditPropertyMapper<PlaneAirport, Airport>
                {
                    EntityProperty = planeAirport => planeAirport.Airport,
                    EntityPropertyIdentifier = planeAirport => planeAirport.AirportId,
                    LinkedEntityPropertyDisplay = airport => airport.Name,
                },
                new AuditPropertyMapper<PlaneAirport, Plane>
                {
                    EntityProperty = planeAirport => planeAirport.Plane,
                    EntityPropertyIdentifier = planeAirport => planeAirport.PlaneId,
                    LinkedEntityPropertyDisplay = plane => plane.Msn,
                },
            ];
        }
    }
}
