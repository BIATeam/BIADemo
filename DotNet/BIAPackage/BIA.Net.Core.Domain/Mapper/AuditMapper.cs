// <copyright file="AuditMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Mapper
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Audit mapper class for a generic <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity to audit.</typeparam>
    public abstract class AuditMapper<TEntity> : IAuditMapper
    {
        /// <inheritdoc/>
        public Type EntityType => typeof(TEntity);

        /// <inheritdoc/>
        public IReadOnlyList<ILinkedAuditMapper> LinkedAuditMappers { get; init; } = [];

        /// <inheritdoc/>
        public IReadOnlyList<IAuditPropertyMapper> AuditPropertyMappers { get; init; } = [];
    }
}
