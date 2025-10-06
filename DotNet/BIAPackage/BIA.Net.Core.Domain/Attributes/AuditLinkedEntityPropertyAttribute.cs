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
    /// <param name="linkedEntityPropertyIdentifier">The identifier value from the linked entity.</param>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AuditLinkedEntityPropertyAttribute(Type linkedEntityType, string linkedEntityPropertyDisplay, string linkedEntityPropertyIdentifier) : Attribute
    {
        public Type LinkedEntityType { get; } = linkedEntityType;
        public string LinkedEntityPropertyDisplay { get; } = linkedEntityPropertyDisplay;
        public string LinkedEntityPropertyIdentifier { get; } = linkedEntityPropertyIdentifier;
    }
}
