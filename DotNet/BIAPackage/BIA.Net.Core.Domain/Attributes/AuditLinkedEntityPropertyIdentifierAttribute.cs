// <copyright file="AuditLinkedEntityPropertyIdentifierAttribute.cs" company="BIA">
// Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain.Attributes
{
    using System;

    /// <summary>
    /// Attribute used in an <see cref="Audit.AuditEntity"/> to identify a property that will be used as the identifier value of a linked entity.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AuditLinkedEntityPropertyDisplayAttribute"/> class.
    /// </remarks>
    /// <param name="linkedEntityType">The linked entity type.</param>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AuditLinkedEntityPropertyIdentifierAttribute(Type linkedEntityType) : Attribute
    {
        /// <summary>
        /// The linked entity type.
        /// </summary>
        public Type LinkedEntityType { get; } = linkedEntityType;
    }
}
