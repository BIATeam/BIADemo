// <copyright file="PlaneAirportAudit.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using BIA.Net.Core.Domain.Attributes;
    using BIA.Net.Core.Domain.Audit;

    /// <summary>
    /// Audit entity for <see cref="PlaneAirport"/>.
    /// </summary>
    [AuditLinkedEntity(linkedEntityType: typeof(Plane), linkedEntityPropertyName: nameof(Plane.ConnectingAirports))]
    public class PlaneAirportAudit : AuditEntity<PlaneAirport>
    {
        /// <summary>
        /// Gets or sets the AirportId.
        /// </summary>
        public int AirportId { get; set; }

        /// <summary>
        /// Gets or sets the PlaneId.
        /// </summary>
        [AuditLinkedEntityPropertyIdentifier(linkedEntityType: typeof(Plane))]
        public int PlaneId { get; set; }

        /// <summary>
        /// Gets or sets the AirportName.
        /// </summary>
        [AuditLinkedEntityPropertyDisplay(linkedEntityType: typeof(Plane))]
        public string AirportName { get; set; }

        /// <summary>
        /// Gets or sets the Plane Name.
        /// </summary>
        public string PlaneName { get; set; }

        /// <inheritdoc/>
        protected override void FillSpecificProperties(PlaneAirport entity)
        {
            this.AirportName = entity.Airport.Name;
            this.PlaneName = entity.Plane.Msn;
        }
    }
}
