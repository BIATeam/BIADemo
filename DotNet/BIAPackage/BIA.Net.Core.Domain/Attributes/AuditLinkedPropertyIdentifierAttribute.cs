namespace BIA.Net.Core.Domain.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AuditLinkedPropertyIdentifierAttribute : Attribute
    {
        public AuditLinkedPropertyIdentifierAttribute(Type linkedEntityType)
        {
            LinkedEntityType = linkedEntityType;
        }

        public Type LinkedEntityType { get; }
    }
}
