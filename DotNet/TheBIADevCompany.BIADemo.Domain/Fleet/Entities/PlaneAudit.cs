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
    public class PlaneAudit : AuditEntity
    {
        /// <summary>
        /// Gets or sets the CurrentAirportName.
        /// </summary>
        [AuditLinkedEntityProperty(typeof(Airport), nameof(Airport.Name), nameof(Plane.CurrentAirportId), nameof(Plane.CurrentAirport))]
        public string CurrentAirportName { get; set; }

        /// <inheritdoc/>
        public override void FillSpecificProperties<TEntity>(TEntity entity)
        {
            if (entity is not Plane plane)
            {
                return;
            }

            this.CurrentAirportName = plane.CurrentAirport?.Name;
        }
    }
}
