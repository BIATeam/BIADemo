// <copyright file="AuditLinkedEntityPropertyAttribute.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Attributes
{
    using System;

    /// <summary>
    /// Attribute used in an <see cref="Audit.AuditEntity"/> to identify a property that will be mapped to a specific property of a linked entity.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AuditLinkedEntityPropertyAttribute"/> class.
    /// </remarks>
    /// <param name="linkedEntityType">The linked entity type.</param>
    /// <param name="linkedEntityPropertyDisplay">The property name of the display value to use from the linked entity.</param>
    /// <param name="entityReferencePropertyIdentifier">The identifier reference value from the audited entity.</param>
    /// <param name="entityPropertyName">The property name from the audited entity that corresponds to the linked entity property value.</param>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AuditLinkedEntityPropertyAttribute(Type linkedEntityType, string linkedEntityPropertyDisplay, string entityReferencePropertyIdentifier, string entityPropertyName) : Attribute
    {
        /// <summary>
        /// The linked entity type.
        /// </summary>
        public Type LinkedEntityType { get; } = linkedEntityType;

        /// <summary>
        /// The property name of the display value to use from the linked entity.
        /// </summary>
        public string LinkedEntityPropertyDisplay { get; } = linkedEntityPropertyDisplay;

        /// <summary>
        /// The identifier reference value from the audited entity.
        /// </summary>
        public string EntityReferencePropertyIdentifier { get; } = entityReferencePropertyIdentifier;

        /// <summary>
        /// The property name from the audited entity that corresponds to the linked entity property value.
        /// </summary>
        public string EntityPropertyName { get; } = entityPropertyName;
    }
}
