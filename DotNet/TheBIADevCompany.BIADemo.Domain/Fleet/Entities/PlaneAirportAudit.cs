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
    public class PlaneAirportAudit : AuditEntity
    {
        /// <summary>
        /// Gets or sets the AirportId.
        /// </summary>
        [AuditLinkedEntityPropertyIdentifier(linkedEntityType: typeof(Airport))]
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
        public override void FillSpecificProperties<TEntity>(TEntity entity)
        {
            if (entity is not PlaneAirport planeAirport)
            {
                return;
            }

            this.AirportName = planeAirport.Airport.Name;
            this.PlaneName = planeAirport.Plane.Msn;
        }
    }
}
