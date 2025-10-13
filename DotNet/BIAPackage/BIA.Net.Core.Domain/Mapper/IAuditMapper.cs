// <copyright file="IAuditMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Mapper
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface for audit mappers.
    /// </summary>
    public interface IAuditMapper
    {
        /// <summary>
        /// Entity type to audit.
        /// </summary>
        Type EntityType { get; }

        /// <summary>
        /// Collection of linked audit mappers.
        /// </summary>
        IReadOnlyList<ILinkedAuditMapper> LinkedAuditMappers { get; }

        /// <summary>
        /// Collection of audit property mappers.
        /// </summary>
        IReadOnlyList<IAuditPropertyMapper> AuditPropertyMappers { get; }
    }
}
