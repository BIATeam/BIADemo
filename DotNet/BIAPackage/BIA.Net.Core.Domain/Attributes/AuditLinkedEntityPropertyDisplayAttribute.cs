namespace BIA.Net.Core.Domain.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Attribute used in an <see cref="Audit.IAuditEntity"/> to identify a property that will be used as the display value of a linked entity.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AuditLinkedEntityPropertyDisplayAttribute"/> class.
    /// </remarks>
    /// <param name="linkedEntityType">The linked entity type.</param>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class AuditLinkedEntityPropertyDisplayAttribute(Type linkedEntityType) : Attribute
    {
        public Type LinkedEntityType { get; } = linkedEntityType;
    }
}
