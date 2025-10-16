// BIADemo only
// <copyright file="PlaneAirportAudit.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using BIA.Net.Core.Domain.Audit;

    /// <summary>
    /// Audit entity for <see cref="PlaneAirport"/>.
    /// </summary>
    public class PlaneAirportAudit : AuditEntity<PlaneAirport, int>
    {
        /// <summary>
        /// Gets or sets the AirportId.
        /// </summary>
        public int AirportId { get; set; }

        /// <summary>
        /// Gets or sets the PlaneId.
        /// </summary>
        public int PlaneId { get; set; }

        /// <summary>
        /// Gets or sets the AirportName.
        /// </summary>
        public string AirportName { get; set; }

        /// <inheritdoc/>
        protected override void FillSpecificProperties(PlaneAirport entity)
        {
            this.AirportName = entity.Airport.Name;
        }
    }
}
