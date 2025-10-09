// <copyright file="EngineAudit.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using BIA.Net.Core.Domain.Attributes;
    using BIA.Net.Core.Domain.Audit;

    /// <summary>
    /// Audit entity for <see cref="Engine"/>.
    /// </summary>
    [AuditLinkedEntity(linkedEntityType: typeof(Plane), linkedEntityPropertyName: nameof(Plane.Engines))]
    public class EngineAudit : AuditEntity<Engine>
    {
        /// <summary>
        /// Gets or sets the PlaneId.
        /// </summary>
        [AuditLinkedEntityPropertyIdentifier(linkedEntityType: typeof(Plane))]
        public int PlaneId { get; set; }

        /// <summary>
        /// Gets or sets the Reference.
        /// </summary>
        [AuditLinkedEntityPropertyDisplay(linkedEntityType: typeof(Plane))]
        public string Reference { get; set; }
    }
}
