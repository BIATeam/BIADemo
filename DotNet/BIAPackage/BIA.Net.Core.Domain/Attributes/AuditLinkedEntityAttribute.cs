namespace BIA.Net.Core.Domain.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AuditLinkedEntityAttribute : Attribute
    {
        public AuditLinkedEntityAttribute(Type linkedEntityType, string linkedEntityPropertyName)
        {
            LinkedEntityType = linkedEntityType;
            LinkedEntityPropertyName = linkedEntityPropertyName;
        }

        public Type LinkedEntityType { get; }
        public string LinkedEntityPropertyName { get; }
    }
}
