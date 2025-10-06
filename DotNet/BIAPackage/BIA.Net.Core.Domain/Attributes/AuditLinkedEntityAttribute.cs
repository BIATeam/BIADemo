namespace BIA.Net.Core.Domain.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Attribute used to add to an <see cref="Audit.IAuditEntity"/> a linked entity.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AuditLinkedEntityAttribute"/> class.
    /// </remarks>
    /// <param name="linkedEntityType">The linked entity type.</param>
    /// <param name="linkedEntityPropertyName">The related entity property that refers to the audit property.</param>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AuditLinkedEntityAttribute(Type linkedEntityType, string linkedEntityPropertyName) : Attribute
    {
        public Type LinkedEntityType { get; } = linkedEntityType;
        public string LinkedEntityPropertyName { get; } = linkedEntityPropertyName;
    }
}
