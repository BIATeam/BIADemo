// <copyright file="AuditPropertyMapper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Mapper
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Audit property mapper for a <typeparamref name="TEntity"/> to a <typeparamref name="TLinkedEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    /// <typeparam name="TLinkedEntity">Type of linked entity.</typeparam>
    public class AuditPropertyMapper<TEntity, TLinkedEntity> : IAuditPropertyMapper
    {
        /// <inheritdoc/>
        public Type LinkedEntityType => typeof(TLinkedEntity);

        /// <inheritdoc/>
        public string EntityPropertyName => Common.Helpers.PropertyMapper.GetPropertyName(this.EntityProperty);

        /// <inheritdoc/>
        public string EntityPropertyIdentifierName => Common.Helpers.PropertyMapper.GetPropertyName(this.EntityPropertyIdentifier);

        /// <inheritdoc/>
        public string LinkedEntityPropertyDisplayName => Common.Helpers.PropertyMapper.GetPropertyName(this.LinkedEntityPropertyDisplay);

        /// <summary>
        /// Selector of the <typeparamref name="TEntity"/> property that have reference to the <typeparamref name="TLinkedEntity"/>.
        /// </summary>
        public required Expression<Func<TEntity, object>> EntityProperty { get; init; }

        /// <summary>
        /// Selector of the <typeparamref name="TEntity"/> property used as reference identifier to the <typeparamref name="TLinkedEntity"/>.
        /// </summary>
        public required Expression<Func<TEntity, object>> EntityPropertyIdentifier { get; init; }

        /// <summary>
        /// Selector of the <typeparamref name="TLinkedEntity"/> property used as display of the <see cref="EntityProperty"/>.
        /// </summary>
        public required Expression<Func<TLinkedEntity, object>> LinkedEntityPropertyDisplay { get; init; }
    }
}
