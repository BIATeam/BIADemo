namespace BIA.Net.Core.Domain.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AuditLinkedEntityPropertyAttribute : Attribute
    {
        public AuditLinkedEntityPropertyAttribute(Type linkedEntityType, string linkedEntityPropertyDisplay, string linkedEntityPropertyIdentifier)
        {
            LinkedEntityType = linkedEntityType;
            LinkedEntityPropertyDisplay = linkedEntityPropertyDisplay;
            LinkedEntityPropertyIdentifier = linkedEntityPropertyIdentifier;
        }

        public Type LinkedEntityType { get; }
        public string LinkedEntityPropertyDisplay { get; }
        public string LinkedEntityPropertyIdentifier { get; }
    }
}
