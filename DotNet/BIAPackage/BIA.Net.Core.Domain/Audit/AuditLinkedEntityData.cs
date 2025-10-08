// <copyright file="AuditLinkedEntityData.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Audit
{
    /// <summary>
    /// Record of linked entity data.
    /// </summary>
    /// <param name="EntityType">The linked entity type.</param>
    /// <param name="IndexPropertyName">The index property name that refers to the linked entity.</param>
    /// <param name="IndexPropertyValue">The index property value that refers to the linked entity.</param>
    public record class AuditLinkedEntityData(string EntityType, string IndexPropertyName, string IndexPropertyValue);
}
