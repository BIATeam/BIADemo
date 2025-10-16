// BIADemo only
// <copyright file="EngineAudit.cs" company="TheBIADevCompany">
// Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using BIA.Net.Core.Domain.Audit;

    /// <summary>
    /// Audit entity for <see cref="Engine"/>.
    /// </summary>
    public class EngineAudit : AuditKeyedEntity<Engine, int, int>
    {
        /// <summary>
        /// Gets or sets the PlaneId.
        /// </summary>
        public int PlaneId { get; set; }

        /// <summary>
        /// Gets or sets the Reference.
        /// </summary>
        public string Reference { get; set; }
    }
}
