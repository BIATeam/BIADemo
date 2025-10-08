// <copyright file="AuditChange.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Audit
{
    /// <summary>
    /// Record of an entity's audit change.
    /// </summary>
    /// <param name="ColumnName">Audited entity column's name of current change.</param>
    /// <param name="OriginalValue">Previous audited entity column's raw value.</param>
    /// <param name="OriginalDisplay">Previous audited entity column's displayed value.</param>
    /// <param name="NewValue">New audited entity column's raw value.</param>
    /// <param name="NewDisplay">New audited entity column's displayed value.</param>
    public record class AuditChange(
        string ColumnName,
        object OriginalValue,
        string OriginalDisplay,
        object NewValue,
        string NewDisplay)
    {
    }
}
