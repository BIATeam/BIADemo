// BIADemo only
// <copyright file="PlaneAuditMapper.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Mappers
{
    using BIA.Net.Core.Domain.Mapper;
    using TheBIADevCompany.BIADemo.Domain.Fleet.Entities;

    /// <summary>
    /// Audit mapper for <see cref="Plane"/>.
    /// </summary>
    public class PlaneAuditMapper : AuditMapper<Plane>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneAuditMapper"/> class.
        /// </summary>
        public PlaneAuditMapper()
        {
            // Begin BIADemo
            this.LinkedAuditMappers =
            [
                new LinkedAuditMapper<Plane, EngineAudit>
                {
                    EntityProperty = plane => plane.Engines,
                    LinkedAuditEntityDisplayProperty = audit => audit.Reference,
                    LinkedAuditEntityIdentifierProperty = audit => audit.PlaneId,
                },
                new LinkedAuditMapper<Plane, PlaneAirportAudit>
                {
                    EntityProperty = plane => plane.ConnectingAirports,
                    LinkedAuditEntityDisplayProperty = audit => audit.AirportName,
                    LinkedAuditEntityIdentifierProperty = audit => audit.PlaneId,
                },
                new LinkedAuditMapper<Plane, PlanePlaneTypeAudit>
                {
                    EntityProperty = plane => plane.SimilarPlaneTypes,
                    LinkedAuditEntityDisplayProperty = audit => audit.PlaneTypeTitle,
                    LinkedAuditEntityIdentifierProperty = audit => audit.PlaneId,
                },
            ];
            this.AuditPropertyMappers =
            [
                new AuditPropertyMapper<Plane, Airport>
                {
                    EntityProperty = plane => plane.CurrentAirport,
                    EntityPropertyIdentifier = plane => plane.CurrentAirportId,
                    LinkedEntityPropertyDisplay = airport => airport.Name,
                },
                new AuditPropertyMapper<Plane, PlaneType>
                {
                    EntityProperty = plane => plane.PlaneType,
                    EntityPropertyIdentifier = plane => plane.PlaneTypeId,
                    LinkedEntityPropertyDisplay = planeType => planeType.Title,
                },
            ];

            // End BIADemo
        }
    }
}
