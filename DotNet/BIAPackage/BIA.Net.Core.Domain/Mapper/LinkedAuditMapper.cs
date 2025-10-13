// <copyright file="LinkedAuditMapper.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Mapper
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Audit mapper for a <typeparamref name="TLinkedAuditEntity"/> to a <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    /// <typeparam name="TLinkedAuditEntity">Type of linked audit entity.</typeparam>
    public class LinkedAuditMapper<TEntity, TLinkedAuditEntity> : ILinkedAuditMapper
    {
        /// <inheritdoc/>
        public Type LinkedAuditEntityType => typeof(TLinkedAuditEntity);

        /// <inheritdoc/>
        public string EntityPropertyName => Common.Helpers.PropertyMapper.GetPropertyName(this.EntityProperty);

        /// <inheritdoc/>
        public string LinkedAuditEntityDisplayPropertyName => Common.Helpers.PropertyMapper.GetPropertyName(this.LinkedAuditEntityDisplayProperty);

        /// <inheritdoc/>
        public string LinkedAuditEntityIdentifierPropertyName => Common.Helpers.PropertyMapper.GetPropertyName(this.LinkedAuditEntityIdentifierProperty);

        /// <summary>
        /// Selector of the <typeparamref name="TEntity"/> property that have reference to the audited entity of the <typeparamref name="TLinkedAuditEntity"/>.
        /// </summary>
        public required Expression<Func<TEntity, object>> EntityProperty { get; init; }

        /// <summary>
        /// Selector of the <typeparamref name="TLinkedAuditEntity"/> property used for display.
        /// </summary>
        public required Expression<Func<TLinkedAuditEntity, object>> LinkedAuditEntityDisplayProperty { get; init; }

        /// <summary>
        /// Selector of the <typeparamref name="TLinkedAuditEntity"/> property used as identifier to the <typeparamref name="TEntity"/>.
        /// </summary>
        public required Expression<Func<TLinkedAuditEntity, object>> LinkedAuditEntityIdentifierProperty { get; init; }
    }
}
