namespace BIA.Net.Core.Domain.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Attribute used in an <see cref="Audit.IAuditEntity"/> to identify a property that will be mapped to a specific property of a linked entity.
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
        public Type LinkedEntityType { get; } = linkedEntityType;
        public string LinkedEntityPropertyDisplay { get; } = linkedEntityPropertyDisplay;
        public string EntityReferencePropertyIdentifier { get; } = entityReferencePropertyIdentifier;
        public string EntityPropertyName { get; } = entityPropertyName;
    }
}
