// <copyright file="PlaneAudit.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIADemo.Domain.Fleet.Entities
{
    using BIA.Net.Core.Domain.Audit;

    /// <summary>
    /// Audit entity for <see cref="Plane"/>.
    /// </summary>
    public class PlaneAudit : AuditKeyedEntity<Plane, int, int>
    {
    }
}
