// <copyright file="PlaneAudit.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using BIA.Net.Core.Domain.Attributes;
    using BIA.Net.Core.Domain.Audit;

    /// <summary>
    /// Audit entity for <see cref="Plane"/>.
    /// </summary>
    [AuditLinkedEntityProperty(
            linkedEntityType: typeof(Airport),
            linkedEntityPropertyDisplay: nameof(Airport.Name),
            entityReferencePropertyIdentifier: nameof(Plane.CurrentAirportId),
            entityPropertyName: nameof(Plane.CurrentAirport))]
    public class PlaneAudit : AuditEntity<Plane>
    {
        public string CurrentAirportName { get; set; }

        protected override void FillSpecificProperties(Plane entity)
        {
            this.CurrentAirportName = entity.CurrentAirport?.Name;
        }
    }
}
